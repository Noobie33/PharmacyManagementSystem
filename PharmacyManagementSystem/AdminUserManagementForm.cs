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
    public partial class AdminUserManagementForm : Form
    {
        public AdminUserManagementForm()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            string sql = "SELECT UserId, Username, FullName, RoleName, IsActive FROM dbo.Users u INNER JOIN dbo.Roles r ON u.RoleId = r.RoleId";
            DataTable dt = DatabaseHelper.GetDataTable(sql);

            dgvUsers.DataSource = dt;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            int roleId = Convert.ToInt32(cmbRole.SelectedValue); // RoleId from ComboBox

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }

            string sql = "INSERT INTO dbo.Users (Username, [Password], FullName, RoleId, IsActive) VALUES (@Username, @Password, @FullName, @RoleId, 1)";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);  // Password should be hashed in real-world apps
            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@RoleId", roleId);

            DatabaseHelper.ExecuteNonQuery(cmd);

            // Refresh the DataGridView to show the new user
            LoadUsers();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            // Get the selected user
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string fullName = txtFullName.Text.Trim();
                int roleId = Convert.ToInt32(cmbRole.SelectedValue);

                string sql = "UPDATE dbo.Users SET Username = @Username, [Password] = @Password, FullName = @FullName, RoleId = @RoleId WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); // Hash the password in production
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                DatabaseHelper.ExecuteNonQuery(cmd);

                // Refresh the DataGridView to show updated data
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Please select a user to update.");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);

                string sql = "UPDATE dbo.Users SET IsActive = 0 WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@UserId", userId);

                DatabaseHelper.ExecuteNonQuery(cmd);

                // Refresh the DataGridView to show updated data
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Please select a user to deactivate.");
            }
        }

        private void AdminUserManagementForm_Load(object sender, EventArgs e)
        {
            string sql = "SELECT RoleId, RoleName FROM dbo.Roles";
            DataTable dt = DatabaseHelper.GetDataTable(sql);

            cmbRole.DataSource = dt;
            cmbRole.DisplayMember = "RoleName";
            cmbRole.ValueMember = "RoleId";
        }
    }
}
