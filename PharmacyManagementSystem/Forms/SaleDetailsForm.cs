using System;
using System.Windows.Forms;
using PharmacyManagementSystem.Data;

namespace PharmacyManagementSystem.Forms
{
    public partial class SaleDetailsForm : Form
    {
        private readonly int _saleId;
        private readonly ReportsRepository _repo = new ReportsRepository();

        public SaleDetailsForm(int saleId)
        {
            InitializeComponent();
            _saleId = saleId;
        }

        private void SaleDetailsForm_Load(object sender, EventArgs e)
        {
            try
            {
                var items = _repo.GetSaleItems(_saleId);

                dgvItems.AutoGenerateColumns = false;
                dgvItems.DataSource = items;

                this.Text = $"Sale Details / Invoice (SaleId: {_saleId})";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load sale details.\n" + ex.Message);
            }
        }
    }
}
