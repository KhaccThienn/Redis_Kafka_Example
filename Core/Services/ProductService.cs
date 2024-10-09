namespace Remake_Kafka_Example_01.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IServiceScopeFactory                _scopeFactory;
        private readonly IKafkaProducerManager               _producerManager;
        private readonly ILogger<ProductService>             _logger;

        public ProductService(
            IServiceScopeFactory    scopeFactory,
            IKafkaProducerManager   producer, 
            ILogger<ProductService> logger
            )
        {
            _scopeFactory    = scopeFactory;
            _producerManager = producer;
            _logger          = logger;
        }
        public async Task<List<TableProduct_V2>> GetProducts()
        {
            List<TableProduct_V2> productList = new List<TableProduct_V2>();
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var dbContext   = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                productList = cacheService.GetData<List<TableProduct_V2>>("table_products");

                if (productList != null && productList.Count() > 0)
                {
                    return productList;
                }

                productList = await dbContext.TableProducts.ToListAsync();

                var expiryTime = DateTimeOffset.Now.AddSeconds(1);
                cacheService.SetData<IEnumerable<TableProduct_V2>>("table_products", productList, expiryTime);
            }
            catch (Exception e)
            {
                _logger.LogError($"[ProductService ~ GetProducts] {e.Message}");
                throw;
            }
            return productList;
        }

        public TableProduct_V2 InsertProduct(TableProduct_V2 product)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                cacheService.SetData($"product_{product.Id}", product, null);

                // Produce message to 'product-output' topic
                var kafkaProducer = _producerManager.GetProducer<string, string>("1");
                var message       = new Message<string, string>
                {
                    Value   = JsonSerializer.Serialize(product),
                    Headers = new Headers 
                    { 
                        { "eventname", Encoding.UTF8.GetBytes("InsertProduct") } 
                    }
                };
                kafkaProducer.Produce(message);

                _logger.LogInformation($"[ProductService ~ InsertProduct] Inserted new product with ID {product.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"[ProductService ~ InsertProduct] {e.Message}");
            }
            return product;
        }

        public TableProduct_V2 UpdatePrice(string key, Guid productId, decimal price)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var product = cacheService.GetData<TableProduct_V2>($"product_{productId}");
                if (product == null)
                {
                    _logger.LogError($"[ProductService ~ UpdatePrice] Product with ID {productId} not found in Redis.");
                    return null;
                }

                if (price < 0)
                {
                    _logger.LogError($"[ProductService ~ UpdatePrice] Invalid price for Product ID {productId}");
                    return null;
                }

                product.Price = price;
                cacheService.SetData($"product_{product.Id}", product, null);

                var prod = new UpdatePriceDTO
                {
                    Id = productId,
                    Price     = price
                };

                // Produce message to 'product-output' topic
                var kafkaProducer  = _producerManager.GetProducer<string, string>("1");
                var message        = new Message<string, string>
                {
                    Value   = JsonSerializer.Serialize(prod),
                    Headers = new Headers
                    {
                        { "eventname", Encoding.UTF8.GetBytes("UpdatePrice") }
                    }
                };
                kafkaProducer.Produce(message);

                _logger.LogInformation($"[ProductService ~ UpdatePrice] Updated price for Product ID {product.Id}");
                return product;
            }
            catch (Exception e)
            {
                _logger.LogError($"[ProductService ~ UpdatePrice] {e.Message}");
            }
            return null;
        }


        public TableProduct_V2 UpdateQuantity(string key, Guid productId, decimal quantity, bool increase)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var product = cacheService.GetData<TableProduct_V2>($"product_{productId}");
                if (product == null)
                {
                    _logger.LogError($"[ProductService ~ UpdateQuantity] Product with ID {productId} not found in Redis.");
                    return null;
                }

                if (increase)
                {
                    product.Quantity += quantity;
                }
                else
                {
                    if (product.Quantity - quantity < 0)
                    {
                        _logger.LogError($"[ProductService ~ UpdateQuantity] Insufficient quantity for Product ID {productId}");
                        return null;
                    }
                    product.Quantity -= quantity;
                }

                cacheService.SetData($"product_{product.Id}", product, null);

                var prod = new UpdateQuantityDTO
                {
                    Id  = productId,
                    Quantity   = quantity,
                    Increase   = increase
                };

                // Produce message to 'product-output' topic
                var kafkaProducer  = _producerManager.GetProducer<string, string>("1");
                var message        = new Message<string, string>
                {
                    Value   = JsonSerializer.Serialize(prod),
                    Headers = new Headers 
                    { 
                        { "eventname", Encoding.UTF8.GetBytes("UpdateQuantity") }
                    }
                };
                kafkaProducer.Produce(message);

                _logger.LogInformation($"[ProductService ~ UpdateQuantity] Updated quantity for Product ID {product.Id}");
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ProductService ~ UpdateQuantity] {ex.Message}");
            }
            return null;
        }
    }
}
