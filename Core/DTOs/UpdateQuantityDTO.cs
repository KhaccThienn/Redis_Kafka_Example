namespace Remake_Kafka_Example_01.Core.DTOs
{
    public class UpdateQuantityDTO
    {
        public Guid    Id        { get; set; }
        public decimal Quantity  { get; set; }
        public bool    Increase  { get; set; }
    }
}
