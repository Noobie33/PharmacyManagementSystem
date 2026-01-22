namespace PharmacyManagementSystem.Models
{
    public class SaleDraftItem
    {
        public int BatchId { get; set; }
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal LineTotal
        {
            get { return UnitPrice * Quantity; }
        }
    }
}
