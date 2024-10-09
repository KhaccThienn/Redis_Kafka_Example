namespace Remake_Kafka_Example_01.MemoryCommands
{
    public record UpdatePriceToMemoryCommand(string Key, Guid Id, decimal Price) : ICommand;
}
