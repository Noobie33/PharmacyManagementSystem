using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PharmacyManagementSystem.Data
{
    public class ReportsRepository
    {
        public List<TodaySaleRow> GetSalesByDate(DateTime date)
        {
            var list = new List<TodaySaleRow>();
            var db = new DatabaseHelper();

            string sql = @"
SELECT
    s.SaleId,
    s.SaleDate,
    u.FullName AS Cashier,
    s.SubTotal,
    s.Discount,
    s.VAT,
    s.NetTotal
FROM Sales s
INNER JOIN Users u ON u.UserId = s.SoldBy
WHERE CAST(s.SaleDate AS date) = @d
ORDER BY s.SaleDate DESC;
";

            SqlCommand cmd = db.GetCommand(sql);
            cmd.Parameters.AddWithValue("@d", date.Date);

            DataTable dt = db.Execute(cmd);

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new TodaySaleRow
                {
                    SaleId = Convert.ToInt32(r["SaleId"]),
                    SaleDate = Convert.ToDateTime(r["SaleDate"]),
                    Cashier = r["Cashier"].ToString(),
                    SubTotal = Convert.ToDecimal(r["SubTotal"]),
                    Discount = Convert.ToDecimal(r["Discount"]),
                    VAT = Convert.ToDecimal(r["VAT"]),
                    NetTotal = Convert.ToDecimal(r["NetTotal"])
                });
            }

            return list;
        }

        public List<SaleItemRow> GetSaleItems(int saleId)
        {
            var list = new List<SaleItemRow>();
            var db = new DatabaseHelper();

            string sql = @"
SELECT
    m.MedicineName,
    b.BatchNo,
    si.Quantity,
    si.UnitPrice,
    si.LineTotal
FROM SaleItems si
INNER JOIN MedicineBatches b ON b.BatchId = si.BatchId
INNER JOIN Medicines m ON m.MedicineId = b.MedicineId
WHERE si.SaleId = @saleId
ORDER BY m.MedicineName;
";

            SqlCommand cmd = db.GetCommand(sql);
            cmd.Parameters.AddWithValue("@saleId", saleId);

            DataTable dt = db.Execute(cmd);

            foreach (DataRow r in dt.Rows)
            {
                list.Add(new SaleItemRow
                {
                    MedicineName = r["MedicineName"].ToString(),
                    BatchNo = r["BatchNo"].ToString(),
                    Quantity = Convert.ToInt32(r["Quantity"]),
                    UnitPrice = Convert.ToDecimal(r["UnitPrice"]),
                    LineTotal = Convert.ToDecimal(r["LineTotal"])
                });
            }

            return list;
        }
    }

    public class TodaySaleRow
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string Cashier { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal VAT { get; set; }
        public decimal NetTotal { get; set; }
    }

    public class SaleItemRow
    {
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
