using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PharmacyManagementSystem.Forms;   
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
                DatabaseHelper db = new DatabaseHelper();

                string sql = @"
SELECT u.UserId, u.FullName, r.RoleName
FROM dbo.Users u
INNER JOIN dbo.Roles r ON r.RoleId = u.RoleId
WHERE u.Username = @Username
  AND u.[Password] = @Password
  AND u.IsActive = 1;
";

                SqlCommand cmd = db.GetCommand(sql,
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password));

                DataTable dt = db.Execute(cmd);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid username/password or inactive account.");
                    return;
                }

                int userId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                string fullName = dt.Rows[0]["FullName"].ToString();
                string roleName = dt.Rows[0]["RoleName"].ToString();

                OpenDashboard(roleName, userId, fullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login failed: " + ex.Message);
            }
        }

        private void OpenDashboard(string roleName, int userId, string fullName)
        {
            Form dashboard;

            if (roleName == "Admin")
                dashboard = new AdminDashboard();
            else if (roleName == "Pharmacist")
                dashboard = new PharmacistDashboard();
            else if (roleName == "Cashier")
                dashboard = new CashierDashboard(userId, fullName);
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
