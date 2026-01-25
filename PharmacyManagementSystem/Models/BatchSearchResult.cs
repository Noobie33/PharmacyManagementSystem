using System;

namespace PharmacyManagementSystem.Models
{
    public class BatchSearchResult
    {
        public int BatchId { get; set; }
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StockQty { get; set; }
        public decimal SalePrice { get; set; }
    }
}
