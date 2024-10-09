namespace Remake_Kafka_Example_01.Core.Services
{
    public class ProductPersistanceService : IProductPersistanceService
    {
        private readonly IServiceScopeFactory                      _scopeFactory;
        private readonly ILogger<ProductPersistanceService>        _logger;

        public ProductPersistanceService(IServiceScopeFactory scopeFactory, ILogger<ProductPersistanceService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger       = logger;
        }

        public async Task<TableProduct_V2> InsertProduct(TableProduct_V2 p)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    dbContext.TableProducts.Add(p);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[ProductPersistanceService ~ InsertProduct] {ex.Message}");
                }
            }
            return p;
        }

        public async Task<TableProduct_V2> UpdatePrice(Guid productId, decimal price)
        {
            var product = new TableProduct_V2();
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                product = await dbContext.TableProducts.FirstOrDefaultAsync(p => p.Id == productId);

                bool flag = true;
                if (product != null)
                {
                    product.Price = price;

                    if (price < 0)
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        try
                        {
                            dbContext.TableProducts.Update(product);
                            await dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"[ProductPersistanceService ~ UpdatePrice] {ex.Message}");
                        }
                    }
                }
                else
                {
                    _logger.LogError($"[ProductPersistanceService ~ UpdatePrice] Product Not Found with Id {productId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ProductPersistanceService ~ UpdatePrice] {ex.Message}");
            }
            return product;
        }

        public async Task<TableProduct_V2> UpdateQuantity(Guid productId, decimal quantity, bool increase)
        {
            var product = new TableProduct_V2();
            using var scope = _scopeFactory.CreateScope();
            var dbContext   = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                product = await dbContext.TableProducts.FirstOrDefaultAsync(p => p.Id == productId);

                bool flag = true;
                if (product != null)
                {
                    if (increase)
                    {
                        product.Quantity += quantity;
                    }
                    else
                    {
                        if (quantity > product.Quantity)
                        {
                            flag = false;
                        }
                        else
                        {
                            product.Quantity -= quantity;
                        }
                    }
                    if (flag)
                    {
                        try
                        {
                            dbContext.TableProducts.Update(product);
                            await dbContext.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    _logger.LogError("[ProductPersistanceService ~ UpdateQuantity] Product Not Found");
                }
            } catch (Exception ex)
            {
                _logger.LogError($"[ProductPersistanceService ~ UpdateQuantity] {ex.Message}");
            }
            return product;
        }
    }
}
