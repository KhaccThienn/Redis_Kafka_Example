namespace Remake_Kafka_Example_01.DatabaseCommands.Handlers
{
    public class UpdatePriceToDatabaseCommandHandler : IDatabaseCommandHandler<UpdatePriceToDatabaseCommand>
    {
        private readonly IProductPersistanceService _service;
        public UpdatePriceToDatabaseCommandHandler(IProductPersistanceService service)
        {
            _service = service;
        }
        public async Task HandleAsync(UpdatePriceToDatabaseCommand command)
        {
            await _service.UpdatePrice(command.Id, command.Price);
        }
    }
}
