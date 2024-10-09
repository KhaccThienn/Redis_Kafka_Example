namespace Remake_Kafka_Example_01.DatabaseCommands
{
    public record UpdateQuantityToDatabaseCommand(Guid Id, decimal Quantity, bool Increase) : ICommand;
}
