using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Data
{
    public class InventoryRepository
    {
        public List<BatchSearchResult> SearchAvailableBatches(string q)
        {
            var list = new List<BatchSearchResult>();
            var db = new DatabaseHelper();

            string sql = @"
SELECT
    b.BatchId,
    m.MedicineName,
    b.BatchNo,
    b.ExpiryDate,
    b.CurrentStockQty AS StockQty,
    b.SalePrice
FROM MedicineBatches b
INNER JOIN Medicines m ON m.MedicineId = b.MedicineId
WHERE m.IsActive = 1
  AND b.CurrentStockQty > 0
  AND b.ExpiryDate >= CAST(GETDATE() AS DATE)
  AND (m.MedicineName LIKE '%' + @q + '%' OR b.BatchNo LIKE '%' + @q + '%')
ORDER BY m.MedicineName;
";

            SqlCommand cmd = db.GetCommand(sql);
            cmd.Parameters.AddWithValue("@q", q ?? "");

            DataTable dt = db.Execute(cmd);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BatchSearchResult
                {
                    BatchId = Convert.ToInt32(row["BatchId"]),
                    MedicineName = row["MedicineName"].ToString(),
                    BatchNo = row["BatchNo"].ToString(),
                    ExpiryDate = Convert.ToDateTime(row["ExpiryDate"]),
                    StockQty = Convert.ToInt32(row["StockQty"]),
                    SalePrice = Convert.ToDecimal(row["SalePrice"])
                });
            }

            return list;
        }
    }
}
