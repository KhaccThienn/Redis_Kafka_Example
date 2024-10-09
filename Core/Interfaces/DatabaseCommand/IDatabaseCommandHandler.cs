namespace Remake_Kafka_Example_01.Core.Interfaces.DatabaseCommand
{
    public interface IDatabaseCommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
