namespace Remake_Kafka_Example_01.MemoryCommands.Handlers
{
    public class InsertProductToMemoryCommandHandler : IMemoryCommandHandler<InsertProductToMemoryCommand>
    {
        private readonly IProductService _productService;

        public InsertProductToMemoryCommandHandler(IProductService service)
        {
            _productService = service;
        }

        public void Handle(InsertProductToMemoryCommand command)
        {
            var product = new TableProduct_V2
            {
                Id       = command.Id,
                Name     = command.Name,
                Price    = command.Price,
                Quantity = command.Quantity
            };
            _productService.InsertProduct(product);
        }
    }
}
