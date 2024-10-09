namespace Remake_Kafka_Example_01.Core.Request
{
    public class UpdateQuantityRequest
    {
        public string  Key       { get; set; }
        public Guid    ProductId { get; set; }
        public decimal Quantity  { get; set; }
        public bool    Increase  { get; set; }
    }
}
