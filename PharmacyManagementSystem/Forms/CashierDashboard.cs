using System;
using System.Windows.Forms;

namespace PharmacyManagementSystem.Forms
{
    public partial class CashierDashboard : Form
    {
        private readonly int _userId;
        private readonly string _fullName;

        public CashierDashboard(int userId, string fullName)
        {
            InitializeComponent();

            _userId = userId;
            _fullName = fullName;

            lblWelcome.Text = $"Welcome, {_fullName}";
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            PosForm f = new PosForm(_userId, _fullName);
            f.Owner = this;     
            this.Hide();        
            f.Show();
        }

        private void btnTodaySales_Click(object sender, EventArgs e)
        {
            TodaySalesForm f = new TodaySalesForm();
            f.Owner = this;  
            this.Hide();      
            f.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();        
            Login login = new Login();
            login.Show();
        }


    }
}
