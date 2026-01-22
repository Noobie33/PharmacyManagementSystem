using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Data
{
    public class SalesRepository
    {
        public int CreateSale(
            int cashierUserId,
            decimal subTotal,
            decimal discount,
            decimal vat,
            decimal netTotal,
            List<SaleDraftItem> items)
        {
            if (items == null || items.Count == 0)
                throw new Exception("Cart is empty.");

            var db = new DatabaseHelper();

            string saleSql = @"
INSERT INTO Sales (SoldBy, SubTotal, Discount, VAT, NetTotal)
OUTPUT INSERTED.SaleId
VALUES (@SoldBy, @Sub, @Dis, @Vat, @Net);
";

            SqlCommand saleCmd = db.GetCommand(
                saleSql,
                new SqlParameter("@SoldBy", cashierUserId),
                new SqlParameter("@Sub", subTotal),
                new SqlParameter("@Dis", discount),
                new SqlParameter("@Vat", vat),
                new SqlParameter("@Net", netTotal)
            );

            DataTable saleDt = db.Execute(saleCmd);
            if (saleDt.Rows.Count == 0)
                throw new Exception("Failed to create sale.");

            int saleId = Convert.ToInt32(saleDt.Rows[0][0]);

            
            foreach (var it in items)
            {
                if (it.Quantity <= 0) throw new Exception("Invalid quantity in cart.");

               
                string checkSql = @"
SELECT CurrentStockQty, ExpiryDate
FROM MedicineBatches
WHERE BatchId = @BatchId;
";
                SqlCommand chkCmd = db.GetCommand(checkSql,
                    new SqlParameter("@BatchId", it.BatchId));

                DataTable chkDt = db.Execute(chkCmd);
                if (chkDt.Rows.Count == 0)
                    throw new Exception("Batch not found: " + it.BatchId);

                int stock = Convert.ToInt32(chkDt.Rows[0]["CurrentStockQty"]);
                DateTime expiry = Convert.ToDateTime(chkDt.Rows[0]["ExpiryDate"]);

                if (expiry.Date < DateTime.Today)
                    throw new Exception("Expired batch: " + it.BatchNo);

                if (it.Quantity > stock)
                    throw new Exception($"Out of stock: {it.BatchNo}. Available: {stock}");

               
                string itemSql = @"
INSERT INTO SaleItems (SaleId, BatchId, Quantity, UnitPrice)
VALUES (@SaleId, @BatchId, @Qty, @Price);

UPDATE MedicineBatches
SET CurrentStockQty = CurrentStockQty - @Qty
WHERE BatchId = @BatchId;
";

                SqlCommand itemCmd = db.GetCommand(itemSql,
                    new SqlParameter("@SaleId", saleId),
                    new SqlParameter("@BatchId", it.BatchId),
                    new SqlParameter("@Qty", it.Quantity),
                    new SqlParameter("@Price", it.UnitPrice));

                db.Execute(itemCmd);
            }

            return saleId;
        }
    }
}
