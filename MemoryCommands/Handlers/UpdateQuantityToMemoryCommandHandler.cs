namespace Remake_Kafka_Example_01.MemoryCommands.Handlers
{
    public class UpdateQuantityToMemoryCommandHandler : IMemoryCommandHandler<UpdateQuantityToMemoryCommand>
    {
        private readonly IProductService _productService;

        public UpdateQuantityToMemoryCommandHandler(IProductService service)
        {
            _productService = service;
        }
        public void Handle(UpdateQuantityToMemoryCommand command)
        {
            _productService.UpdateQuantity(command.Key, command.Id, command.Quantity, command.Increase);
        }
    }
}
