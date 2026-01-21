using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class Login : Form
    {
        private readonly DatabaseHelper db;

        public Login()
        {
            InitializeComponent();
            db = new DatabaseHelper();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            string sql = @"
                SELECT u.UserId, u.FullName, r.RoleName
                FROM dbo.Users u
                INNER JOIN dbo.Roles r ON r.RoleId = u.RoleId
                WHERE u.Username = @Username
                  AND u.[Password] = @Password
                  AND u.IsActive = 1";

            SqlCommand cmd = db.GetCommand(sql);
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

            DataTable dt = db.Execute(cmd);

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
                MessageBox.Show("Unknown role.");
                return;
            }

            this.Hide();
            dashboard.FormClosed += (s, e) => this.Close();
            dashboard.Show();
        }
    }
}
