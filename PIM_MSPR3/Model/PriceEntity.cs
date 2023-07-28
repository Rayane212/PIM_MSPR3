namespace PIM_MSPR3.Model
{
    public class PriceEntity
    {
        public int idPrice { get; set; }
        public string CodePrice { get; set; }
        public string Currency { get; set; }
        public float PriceWT { get; set; }
        public int QtyMinimal { get; set; }
    }
}