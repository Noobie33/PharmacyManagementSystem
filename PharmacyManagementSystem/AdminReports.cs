using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class AdminReports : UserControl
    {
        private readonly DatabaseHelper db;
        public AdminReports()
        {
            InitializeComponent();
            db = new DatabaseHelper();
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            string sql = @"SELECT SaleDate, SUM(NetTotal) AS TotalSales, SUM(Discount) AS TotalDiscount
                   FROM dbo.Sales
                   WHERE SaleDate BETWEEN @StartDate AND @EndDate
                   GROUP BY SaleDate";

            SqlCommand cmd = db.GetCommand(sql);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);

            try
            {
                DataTable dt = db.Execute(cmd);
                dgvReport.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateLowStockReport_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT MedicineName, CurrentStockQty, ReorderLevel
                   FROM dbo.Medicines m
                   INNER JOIN dbo.MedicineBatches mb ON m.MedicineId = mb.MedicineId
                   WHERE mb.CurrentStockQty <= m.ReorderLevel";

            SqlCommand cmd = db.GetCommand(sql);

            try
            {
                DataTable dt = db.Execute(cmd);
                dgvReport.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateNearExpiryReport_Click(object sender, EventArgs e)
        {
            string sql = @"SELECT 
    m.MedicineName,
    mb.BatchNo,
    mb.ExpiryDate
FROM dbo.MedicineBatches mb
INNER JOIN dbo.Medicines m 
    ON m.MedicineId = mb.MedicineId
WHERE mb.ExpiryDate <= DATEADD(DAY, 30, GETDATE());
";

            SqlCommand cmd = db.GetCommand(sql);

            try
            {
                DataTable dt = db.Execute(cmd);
                dgvReport.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
