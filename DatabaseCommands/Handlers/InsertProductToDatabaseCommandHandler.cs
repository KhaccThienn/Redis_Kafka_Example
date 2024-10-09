namespace Remake_Kafka_Example_01.DatabaseCommands.Handlers
{
    public class InsertProductToDatabaseCommandHandler : IDatabaseCommandHandler<InsertProductToDatabaseCommand>
    {
        private readonly IProductPersistanceService _service;
        public InsertProductToDatabaseCommandHandler(IProductPersistanceService service)
        {
            _service = service;
        }
        public async Task HandleAsync(InsertProductToDatabaseCommand command)
        {
            var product = new TableProduct_V2
            {
                Id       = command.Id,
                Name     = command.Name,
                Price    = command.Price,
                Quantity = command.Quantity
            };
            await _service.InsertProduct(product);
        }
    }
}
