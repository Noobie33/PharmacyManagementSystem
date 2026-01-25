using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class InventoryDashboard : Form
    {
        public InventoryDashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CategoryForm ca = new CategoryForm();
            ca.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MedicineForm ma=new MedicineForm();
            ma.Show();
            this.Hide();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            SupplierForm ca = new SupplierForm();
            ca.Show(); 
            this.Hide();
        }

        private void btnBatchStockForm_Click(object sender, EventArgs e)
        {
            BatchStockForm ba = new BatchStockForm();
            ba.Show();
            this.Hide();
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PharmacistDashboard ph = new PharmacistDashboard();
            ph.Show();
            this.Hide();
        }
    }
}
