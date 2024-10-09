namespace Remake_Kafka_Example_01.BackgroundTasks
{
    public class ProductPersistanceConsumingTask : IConsumingTask<string, string>
    {
        private readonly ILogger<ProductPersistanceConsumingTask>                 _logger;
        private readonly IDatabaseCommandHandler<InsertProductToDatabaseCommand>  _insertProductCommandHandler;
        private readonly IDatabaseCommandHandler<UpdatePriceToDatabaseCommand>    _updatePriceProductCommandHandler;
        private readonly IDatabaseCommandHandler<UpdateQuantityToDatabaseCommand> _updateQuantityProductCommandHandler;

        public ProductPersistanceConsumingTask(
            ILogger<ProductPersistanceConsumingTask>                 logger,
            IDatabaseCommandHandler<InsertProductToDatabaseCommand>  insertProductCommandHandler,
            IDatabaseCommandHandler<UpdatePriceToDatabaseCommand>    updatePriceProductCommandHandler,
            IDatabaseCommandHandler<UpdateQuantityToDatabaseCommand> updateQuantityProductCommandHandler
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
                    var key = header.Key;
                    var value = Encoding.UTF8.GetString(header.GetValueBytes());

                    if (key == "eventname")
                    {
                        productEvent = value;
                        break;
                    }
                }

                _logger.LogInformation("[ProductPersistanceConsumingTask] Consumed message from topic {Topic}, partition {Partition}, offset {Offset} with event {Event}",
                        result.Topic, result.Partition, result.Offset, productEvent);

                switch (productEvent)
                {
                    case "InsertProduct":
                        var product = JsonSerializer.Deserialize<TableProduct_V2>(result.Message.Value);

                        TableProduct_V2 p = new TableProduct_V2();

                        p.Id       = product.Id;
                        p.Price    = product.Price;
                        p.Name     = product.Name;
                        p.Quantity = product.Quantity;

                        await _insertProductCommandHandler.HandleAsync(new InsertProductToDatabaseCommand(p.Id, p.Name, p.Price, p.Quantity));

                        _logger.LogInformation("[ProductPersistanceConsumingTask ~ InsertProduct] Inserted product: {@Product}", product);

                        break;

                    case "UpdateQuantity":
                        var prod = JsonSerializer.Deserialize<UpdateQuantityToDatabaseCommand>(result.Message.Value);

                        await _updateQuantityProductCommandHandler.HandleAsync(new UpdateQuantityToDatabaseCommand(prod.Id, prod.Quantity, prod.Increase));

                        _logger.LogInformation("[ProductPersistanceConsumingTask ~ UpdateQuantity] Updated quantity for product ID {Id} to {Quantity}, Increase {Increase}",
                                prod.Id, prod.Quantity, prod.Increase);

                        break;

                    case "UpdatePrice":
                        var prd = JsonSerializer.Deserialize<UpdatePriceToDatabaseCommand>(result.Message.Value);

                        await _updatePriceProductCommandHandler.HandleAsync(new UpdatePriceToDatabaseCommand(prd.Id, prd.Price));

                        _logger.LogInformation("[ProductPersistanceConsumingTask ~ UpdatePrice] Updated price for product ID {Id} to {Price}",
                                prd.Id, prd.Price);
                        break;
                    default:
                        _logger.LogWarning($"[ProductPersistanceConsumingTask] Received unknown event: {productEvent}");
                        break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"[ProductPersistanceConsumingTask] Failed to deserialize message value: {result.Message.Value}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ProductPersistanceConsumingTask] Error occurred while processing message with event {productEvent}");
            }
        }
    }
}
