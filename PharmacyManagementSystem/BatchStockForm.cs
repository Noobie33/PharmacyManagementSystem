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
    public partial class BatchStockForm : Form
    {
        string conString = ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString;
        public BatchStockForm()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            InventoryDashboard inb = new InventoryDashboard();
            inb.Show();
            this.Hide();
        }
        private void BatchStockForm_Load(object sender, EventArgs e)
        {
            LoadMedicines();
            LoadBatches();
        }
        void LoadMedicines()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT MedicineId, MedicineName FROM Medicines", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbMedicine.DataSource = dt;
            cmbMedicine.DisplayMember = "MedicineName";
            cmbMedicine.ValueMember = "MedicineId";
            con.Close();
        }
        void LoadBatches()
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT b.BatchId, m.MedicineName, b.BatchNo, b.ExpiryDate, b.PurchasePrice, b.SalePrice, b.CurrentStockQty FROM MedicineBatches b JOIN Medicines m ON b.MedicineId = m.MedicineId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvBatches.DataSource = dt;
            con.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtBatchNo.Text == "" || txtStockQty.Text == "")
            {
                MessageBox.Show("Please fill required fields");
                return;
            }
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlCommand cmd = new SqlCommand(@"INSERT INTO MedicineBatches(MedicineId, BatchNo, ExpiryDate, PurchasePrice, SalePrice, CurrentStockQty) VALUES (@medId, @batch, @exp, @buy, @sell, @qty)", con);
            cmd.Parameters.AddWithValue("@medId", cmbMedicine.SelectedValue);
            cmd.Parameters.AddWithValue("@batch", txtBatchNo.Text);
            cmd.Parameters.AddWithValue("@exp", dtpExpiry.Value.Date);
            cmd.Parameters.AddWithValue("@buy",decimal.Parse(txtPurchasePrice.Text));
            cmd.Parameters.AddWithValue("@sell",decimal.Parse(txtSalePrice.Text));
            cmd.Parameters.AddWithValue("@qty",int.Parse(txtStockQty.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Batch Added Successfully");
            ClearFields();
            LoadBatches();
        }
        void ClearFields()
        {
            txtBatchNo.Clear();
            txtPurchasePrice.Clear();
            txtSalePrice.Clear();
            txtStockQty.Clear();
            cmbMedicine.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dtpExpiry_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
