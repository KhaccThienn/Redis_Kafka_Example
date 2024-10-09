namespace Remake_Kafka_Example_01.MemoryCommands
{
    public record InsertProductToMemoryCommand(Guid Id, string Name, decimal Price, decimal Quantity) : ICommand;
}
