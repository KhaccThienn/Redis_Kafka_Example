namespace Remake_Kafka_Example_01.Core.Interfaces
{
    public interface IProductPersistanceService
    {
        Task<TableProduct_V2> InsertProduct(TableProduct_V2 p);
        Task<TableProduct_V2> UpdateQuantity(Guid productId, decimal quantity, bool increase);
        Task<TableProduct_V2> UpdatePrice(Guid productId, decimal price);
    }
}
