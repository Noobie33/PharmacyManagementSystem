using System;
using System.Linq;
using System.Windows.Forms;
using PharmacyManagementSystem.Data;

namespace PharmacyManagementSystem.Forms
{
    public partial class TodaySalesForm : Form
    {
        private readonly ReportsRepository _repo = new ReportsRepository();

        public TodaySalesForm()
        {
            InitializeComponent();
            dtpDate.Value = DateTime.Today;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime selectedDate = dtpDate.Value.Date;

                var salesList = _repo.GetSalesByDate(selectedDate);

                dgvSales.AutoGenerateColumns = false;
                dgvSales.DataSource = salesList;

                txtTotalSales.Text = salesList.Count.ToString();
                txtNetTotalSum.Text = salesList.Sum(x => x.NetTotal).ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load report.\n" + ex.Message);
            }
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int saleId = Convert.ToInt32(
                dgvSales.Rows[e.RowIndex].Cells["colSaleId"].Value
            );

            using (var f = new SaleDetailsForm(saleId))
            {
                f.ShowDialog();
            }


        }
    }
}
