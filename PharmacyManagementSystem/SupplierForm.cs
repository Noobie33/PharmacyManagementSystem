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
    public partial class SupplierForm : Form
    {
        string conString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;
        public SupplierForm()
        {
            InitializeComponent();
        }
        private void SupplierForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }
        void LoadSuppliers()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT SupplierId, SupplierName, Phone, Address FROM Suppliers", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSuppliers.DataSource = dt;
            con.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            InventoryDashboard enb = new InventoryDashboard();
            enb.Show();
            this.Hide();
        }
        private void dgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSupplierId.Text = dgvSuppliers.Rows[e.RowIndex].Cells["SupplierId"].Value.ToString();
                txtSupplierName.Text = dgvSuppliers.Rows[e.RowIndex].Cells["SupplierName"].Value.ToString();
                txtPhone.Text = dgvSuppliers.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                txtAddress.Text = dgvSuppliers.Rows[e.RowIndex].Cells["Address"].Value.ToString();
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtSupplierName.Text == "")
            {
                MessageBox.Show("Supplier name required");
                return;
            }
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Suppliers (SupplierName, Phone, Address) VALUES (@name,@phone,@address)", con);
            cmd.Parameters.AddWithValue("@name", txtSupplierName.Text);
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Supplier Added Successfully");
            ClearFields();
            LoadSuppliers();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtSupplierId.Text == "")
            {
                MessageBox.Show("Select a supplier first");
                return;
            }
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Suppliers SET SupplierName=@name, Phone=@phone, Address=@address WHERE SupplierId=@id", con);
            cmd.Parameters.AddWithValue("@name", txtSupplierName.Text);
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@id", int.Parse(txtSupplierId.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Supplier Updated Successfully");
            ClearFields();
            LoadSuppliers();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtSupplierId.Text == "")
            {
                MessageBox.Show("Select a supplier first");
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete?","Confirm", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection(conString);
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Suppliers WHERE SupplierId=@id", con);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtSupplierId.Text));
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Supplier Deleted Successfully");
                ClearFields();
                LoadSuppliers();
            }
        }
        void ClearFields()
        {
            txtSupplierId.Clear();
            txtSupplierName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
