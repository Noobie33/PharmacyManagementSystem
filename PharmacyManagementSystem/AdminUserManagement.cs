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
    public partial class AdminUserManagement : UserControl
    {
        private readonly DatabaseHelper db;
        public AdminUserManagement()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            //LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                string sql = @"SELECT u.UserId, u.Username, u.FullName, r.RoleName, u.IsActive
                       FROM dbo.Users u
                       INNER JOIN dbo.Roles r ON u.RoleId = r.RoleId";

                DataTable dt = db.Execute(sql);
                dgvUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdminUserManagement_Load_1(object sender, EventArgs e)
        {
            LoadUsers();

            string sql = "SELECT RoleId, RoleName FROM dbo.Roles";
            DataTable dt = db.Execute(sql);

            cmbRole.DataSource = dt;
            cmbRole.DisplayMember = "RoleName";
            cmbRole.ValueMember = "RoleId";
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }
            try
            {
                string sql = @"INSERT INTO dbo.Users 
                          (Username, [Password], FullName, RoleId, IsActive)
                           VALUES (@Username, @Password, @FullName, @RoleId, 1)";

                SqlCommand cmd = db.GetCommand(sql);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@RoleId", cmbRole.SelectedValue);

                db.ExecuteNonQuery(cmd);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);
            try
            {
                string sql = @"UPDATE dbo.Users
                           SET Username=@Username,
                               [Password]=@Password,
                               FullName=@FullName,
                               RoleId=@RoleId
                           WHERE UserId=@UserId";

                SqlCommand cmd = db.GetCommand(sql);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@RoleId", cmbRole.SelectedValue);
                cmd.Parameters.AddWithValue("@UserId", userId);

                db.ExecuteNonQuery(cmd);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);
            bool isActive = Convert.ToBoolean(dgvUsers.SelectedRows[0].Cells["IsActive"].Value);
            string sql;

            if (isActive)
            {
               
                sql = "UPDATE dbo.Users SET IsActive = 0 WHERE UserId = @UserId";
                btnDeactivateUser.Text = "Activate";
            }
            else
            {
                
                sql = "UPDATE dbo.Users SET IsActive = 1 WHERE UserId = @UserId";
                btnDeactivateUser.Text = "Deactivate";
            }

            SqlCommand cmd = db.GetCommand(sql);
            cmd.Parameters.AddWithValue("@UserId", userId);

            try
            {
                db.ExecuteNonQuery(cmd);
                LoadUsers(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

            txtUsername.Text = row.Cells["Username"].Value.ToString();
            txtFullName.Text = row.Cells["FullName"].Value.ToString();
            cmbRole.Text = row.Cells["RoleName"].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserId"].Value);

            try
            {
                string sql = "DELETE FROM dbo.Users WHERE UserId = @UserId";
                SqlCommand cmd = db.GetCommand(sql);
                cmd.Parameters.AddWithValue("@UserId", userId);
                db.ExecuteNonQuery(cmd);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cmbRole.SelectedIndex = -1;
        }
    }
}
