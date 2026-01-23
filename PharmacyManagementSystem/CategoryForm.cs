using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class CategoryForm : Form
    {
        string conString = "Data Source=THINKPAD-26B7VP\\SQLEXPRESS05;Initial Catalog=pharmacyDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";


        public CategoryForm()
        {
            InitializeComponent();
        }
        private void CategoryForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }
        void LoadCategories()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT CategoryId, CategoryName FROM Categories", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvCategories.DataSource = dt;

            con.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text == "")
            {
                MessageBox.Show("Category name required");
                return;
            }

            SqlConnection con = new SqlConnection(conString);
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Categories(CategoryName) VALUES(@name)", con);

            cmd.Parameters.AddWithValue("@name", txtCategoryName.Text);

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Category Inserted Successfully");

            txtCategoryName.Clear();
            LoadCategories();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCategoryName.Clear();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            InventoryDashboard ib = new InventoryDashboard();
            ib.Show();
            this.Hide();
        }
    }
}
