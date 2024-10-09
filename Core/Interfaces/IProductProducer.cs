namespace Remake_Kafka_Example_01.Core.Interfaces
{
    public interface IProductProducer
    {
        Task ProduceInsertProductAsync(InsertProductRequest product);
        Task ProduceUpdateQuantityAsync(UpdateQuantityRequest request);
        Task ProduceUpdatePriceAsync(UpdatePriceRequest request);
    }
}
