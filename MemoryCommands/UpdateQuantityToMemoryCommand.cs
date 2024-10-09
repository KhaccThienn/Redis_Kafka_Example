namespace Remake_Kafka_Example_01.MemoryCommands
{
    public record UpdateQuantityToMemoryCommand(string Key, Guid Id, decimal Quantity, bool Increase) : ICommand;
}
