namespace Remake_Kafka_Example_01.DatabaseCommands.Handlers
{
    public class UpdateQuantityToDatabaseCommandHandler : IDatabaseCommandHandler<UpdateQuantityToDatabaseCommand>
    {
        private readonly IProductPersistanceService _service;
        public UpdateQuantityToDatabaseCommandHandler(IProductPersistanceService service)
        {
            _service = service;
        }
        public async Task HandleAsync(UpdateQuantityToDatabaseCommand command)
        {
            await _service.UpdateQuantity(command.Id, command.Quantity, command.Increase);
        }
    }
}
