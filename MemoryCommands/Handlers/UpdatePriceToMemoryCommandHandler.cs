namespace Remake_Kafka_Example_01.MemoryCommands.Handlers
{
    public class UpdatePriceToMemoryCommandHandler : IMemoryCommandHandler<UpdatePriceToMemoryCommand>
    {
        private readonly IProductService _productService;

        public UpdatePriceToMemoryCommandHandler(IProductService service)
        {
            _productService = service;
        }
        public void Handle(UpdatePriceToMemoryCommand command)
        {
            _productService.UpdatePrice(command.Key, command.Id, command.Price);
        }
    }
}
