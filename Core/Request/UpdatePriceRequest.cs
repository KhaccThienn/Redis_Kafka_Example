namespace Remake_Kafka_Example_01.Core.Request
{
    public class UpdatePriceRequest
    {
        public string  Key       { get; set; }
        public Guid    ProductId { get; set; }
        public decimal Price     { get; set; }
    }
}
