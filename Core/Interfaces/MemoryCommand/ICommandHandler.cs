namespace Remake_Kafka_Example_01.Core.Interfaces.MemoryCommand
{
    public interface IMemoryCommandHandler<in TCommand> where TCommand : class, ICommand
    {
        void Handle(TCommand command);
    }
}
