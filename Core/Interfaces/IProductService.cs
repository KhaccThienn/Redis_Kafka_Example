namespace Remake_Kafka_Example_01.Core.Interfaces
{
    public interface IProductService
    {
        Task<List<TableProduct_V2>> GetProducts();
        TableProduct_V2             InsertProduct(TableProduct_V2 p);
        TableProduct_V2             UpdatePrice(string key, Guid productId, decimal price);
        TableProduct_V2             UpdateQuantity(string key, Guid productId, decimal quantity, bool increase);
    }
}
