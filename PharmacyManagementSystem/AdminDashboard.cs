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
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            AdminUserManagement adminUMF = new AdminUserManagement();
            adminUMF.Dock = DockStyle.Fill;

            panelMain.Controls.Add(adminUMF);
            //AdminUserManagement adminUMF = new AdminUserManagement();
            //adminUMF.Show();  

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                
                Session.UserId = 0;
                Session.FullName = string.Empty;
                Session.RoleName = string.Empty;
                
                this.Hide(); 

                Login loginForm = new Login(); 
                loginForm.Show(); 
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            AdminReports adminUMF = new AdminReports();
            adminUMF.Dock = DockStyle.Fill;

            panelMain.Controls.Add(adminUMF);
        }
    }
}
