namespace Remake_Kafka_Example_01.Core.Request
{
    public class InsertProductRequest
    {
        public string  Name     { get; set; }
        public decimal Price    { get; set; }
        public decimal Quantity { get; set; }
    }
}
