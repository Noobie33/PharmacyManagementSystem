using System;
using System.Windows.Forms;

namespace PharmacyManagementSystem.Forms
{
    public partial class CashierDashboardForm : Form
    {
        private readonly int _userId;
        private readonly string _fullName;

        public CashierDashboardForm(int userId, string fullName)
        {
            InitializeComponent();

            _userId = userId;
            _fullName = fullName;

            lblWelcome.Text = $"Welcome, {_fullName}";
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            using (var f = new PosForm(_userId, _fullName))
            {
                f.ShowDialog();
            }
        }

        private void btnTodaySales_Click(object sender, EventArgs e)
        {
            using (var f = new TodaySalesForm())
            {
                f.ShowDialog();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
