namespace Remake_Kafka_Example_01.DatabaseCommands
{
    public record InsertProductToDatabaseCommand(Guid Id, string Name, decimal Price, decimal Quantity) : ICommand;
}
