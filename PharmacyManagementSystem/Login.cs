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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            try
            {
                string sql = @"
        SELECT u.UserId, u.FullName, r.RoleName
        FROM dbo.Users u
        INNER JOIN dbo.Roles r ON r.RoleId = u.RoleId
        WHERE u.Username = @Username
          AND u.[Password] = @Password
          AND u.IsActive = 1;
        ";

                
                SqlCommand cmd = DatabaseHelper.GetCommand(sql,
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password));

                
                DataTable dt = DatabaseHelper.Execute(cmd);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid username/password or inactive account.");
                    return;
                }

                
                Session.UserId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                Session.FullName = dt.Rows[0]["FullName"].ToString();
                Session.RoleName = dt.Rows[0]["RoleName"].ToString();

                
                OpenDashboard(Session.RoleName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message);
            }
        }

        private void OpenDashboard(string roleName)
        {
            Form dashboard;

            if (roleName == "Admin")
                dashboard = new AdminDashboard();
            else if (roleName == "Pharmacist")
                dashboard = new PharmacistDashboard();
            else if (roleName == "Cashier")
                dashboard = new CashierDashboard();
            else
            {
                MessageBox.Show("Unknown role: " + roleName);
                return;
            }

            this.Hide();
            dashboard.FormClosed += (s, e) => this.Close();
            dashboard.Show();
        }
    }
}
