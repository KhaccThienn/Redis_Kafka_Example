namespace Remake_Kafka_Example_01.Core.Domains
{
    public partial class TableProduct_V2
    {
        public Guid    Id       { get; set; }
        public string  Name     { get; set; }
        public decimal Price    { get; set; }
        public decimal Quantity { get; set; }
    }
}
