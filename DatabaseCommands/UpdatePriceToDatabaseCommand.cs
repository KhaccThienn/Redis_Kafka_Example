namespace Remake_Kafka_Example_01.DatabaseCommands
{
    public record UpdatePriceToDatabaseCommand(Guid Id, decimal Price) : ICommand;
}
