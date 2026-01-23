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
    public partial class MedicineForm : Form
    {
        string conString =
    ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;

        public MedicineForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InventoryDashboard ibd = new InventoryDashboard();
            ibd.Show();
            this.Hide();
        }

        private void MedicineForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadMedicines();
        }
        void LoadCategories()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT CategoryId, CategoryName FROM Categories", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
            con.Close();
        }
        void LoadMedicines()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            SqlCommand cmd = new SqlCommand(@"SELECT m.MedicineId, m.MedicineName, m.GenericName,c.CategoryName, m.ReorderLevel FROM Medicines m JOIN Categories c ON m.CategoryId = c.CategoryId", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvMedicines.DataSource = dt;
            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtMedicineName.Text == "" || txtReorderLevel.Text == "")
            {
                MessageBox.Show("Please fill all required fields");
                return;
            }

            SqlConnection con = new SqlConnection(conString);
            con.Open();

            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Medicines
                  (MedicineName, GenericName, CategoryId, ReorderLevel)
                  VALUES (@name, @generic, @catId, @reorder)", con);

            cmd.Parameters.AddWithValue("@name", txtMedicineName.Text);
            cmd.Parameters.AddWithValue("@generic", txtGenericName.Text);
            cmd.Parameters.AddWithValue("@catId", cmbCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@reorder",
                int.Parse(txtReorderLevel.Text));

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Medicine Added Successfully");

            ClearFields();
            LoadMedicines();
        }
        void ClearFields()
        {
            txtMedicineName.Clear();
            txtGenericName.Clear();
            txtReorderLevel.Clear();
            cmbCategory.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
