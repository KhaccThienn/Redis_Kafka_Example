namespace Remake_Kafka_Example_01.BackgroundTasks
{
    public class ProductConsumingTask : IConsumingTask<string, string>
    {
        private readonly ILogger<ProductConsumingTask>                        _logger;
        private readonly IMemoryCommandHandler<InsertProductToMemoryCommand>  _insertProductCommandHandler;
        private readonly IMemoryCommandHandler<UpdatePriceToMemoryCommand>    _updatePriceProductCommandHandler;
        private readonly IMemoryCommandHandler<UpdateQuantityToMemoryCommand> _updateQuantityProductCommandHandler;

        public ProductConsumingTask(
            ILogger<ProductConsumingTask>                        logger,
            IMemoryCommandHandler<InsertProductToMemoryCommand>  insertProductCommandHandler,
            IMemoryCommandHandler<UpdatePriceToMemoryCommand>    updatePriceProductCommandHandler,
            IMemoryCommandHandler<UpdateQuantityToMemoryCommand> updateQuantityProductCommandHandler
            )
        {
            _logger                              = logger;
            _insertProductCommandHandler         = insertProductCommandHandler;
            _updatePriceProductCommandHandler    = updatePriceProductCommandHandler;
            _updateQuantityProductCommandHandler = updateQuantityProductCommandHandler;
        }
        public async Task ExecuteAsync(ConsumeResult<string, string> result)
        {
            var productEvent = "";
            try
            {
                foreach (var header in result.Message.Headers)
                {
                    productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
                }

                _logger.LogInformation(
                    $"[ProductConsumingTask] Consuming message from topic: {result.Topic}, Partition: {result.Partition}, Offset: {result.Offset}, Key: {result.Message.Key} and Event {productEvent}");

                switch(productEvent)
                {
                    case "InsertProduct":
                        var product =  JsonSerializer.Deserialize<TableProduct_V2>(result.Message.Value);

                        Guid newGuid = Guid.NewGuid();
                        product.Id = newGuid;

                        _logger.LogInformation("[ProductConsumingTask ~ InsertProduct] Inserting new product: {@Product}", product);

                        _insertProductCommandHandler.Handle(new InsertProductToMemoryCommand(product.Id, product.Name, product.Price, product.Quantity));
                        break;

                    case "UpdateQuantity":
                        var prod = JsonSerializer.Deserialize<UpdateQuantityDTO>(result.Message.Value);
                        var key = result.Message.Key;

                        _logger.LogInformation("[ProductConsumingTask ~ UpdateQuantity] Updating quantity for product ID: {Id}, New Quantity: {Quantity}, Increase: {Increase}",
                            prod.Id, prod.Quantity, prod.Increase);

                        _updateQuantityProductCommandHandler.Handle(new UpdateQuantityToMemoryCommand(key, prod.Id, prod.Quantity, prod.Increase));
                        break;

                    case "UpdatePrice":
                        var p = JsonSerializer.Deserialize<UpdatePriceDTO>(result.Message.Value);
                        var k = result.Message.Key;
                        _logger.LogInformation("[ProductConsumingTask ~ UpdatePrice] Updating price for product ID: {Id}, New Price: {Price}", p.Id, p.Price);
                        _updatePriceProductCommandHandler.Handle(new UpdatePriceToMemoryCommand(k, p.Id, p.Price));
                        break;

                    default:
                        _logger.LogWarning("[ProductConsumingTask] Received unknown event: {Event}", productEvent);
                        break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"[ProductConsumingTask] Failed to deserialize message value: {result.Message.Value}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ProductConsumingTask] Error occurred while processing message with event {productEvent}");
            }
        }
    }
}
