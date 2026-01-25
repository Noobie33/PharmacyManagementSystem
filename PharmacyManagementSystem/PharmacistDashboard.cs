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
    public partial class PharmacistDashboard : Form
    {
        public PharmacistDashboard()
        {
            InitializeComponent();
        }

        private void clkbtn_Click(object sender, EventArgs e)
        {
            InventoryDashboard idb = new InventoryDashboard();
            idb.Show();
            this.Hide();
        }
    }
}
