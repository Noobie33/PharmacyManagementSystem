using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Forms
{
    public partial class PosForm : Form
    {
        private readonly int _userId;
        private readonly string _fullName;

        private readonly InventoryRepository _invRepo = new InventoryRepository();
        private readonly SalesRepository _salesRepo = new SalesRepository();

        private readonly List<SaleDraftItem> _cart = new List<SaleDraftItem>();

        public PosForm(int userId, string FullName)
        {
            InitializeComponent();

            _userId = userId;
            _fullName = FullName;

            
            lblCashier.Text = $"Cashier: {_fullName}";

            RecalcTotals();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Search clicked");
            var q = txtSearch.Text.Trim();
            var list = _invRepo.SearchAvailableBatches(q);

            MessageBox.Show("Rows: " + (list == null ? "null" : list.Count.ToString()));
            dgvBatches.AutoGenerateColumns = false;
            dgvBatches.DataSource = list;
        }

        
        private void dgvBatches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvBatches.Columns[e.ColumnIndex].Name != "colAdd") return;

            var row = dgvBatches.Rows[e.RowIndex];

            int batchId = Convert.ToInt32(row.Cells["colBatchId"].Value);
            string medName = row.Cells["MedicineName"].Value.ToString();
            string batchNo = row.Cells["BatchNo"].Value.ToString();
            decimal price = Convert.ToDecimal(row.Cells["SalePrice"].Value);

            AddToCart(batchId, medName, batchNo, price);
        }

        private void AddToCart(int batchId, string medName, string batchNo, decimal price)
        {
            var existing = _cart.FirstOrDefault(x => x.BatchId == batchId);

            if (existing != null)
                existing.Quantity += 1;
            else
                _cart.Add(new SaleDraftItem
                {
                    BatchId = batchId,
                    MedicineName = medName,
                    BatchNo = batchNo,
                    UnitPrice = price,
                    Quantity = 1
                });

            BindCart();
        }

        private void BindCart()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.DataSource = null;

           
            dgvCart.DataSource = _cart.Select(x => new
            {
                BatchId = x.BatchId,
                MedicineName = x.MedicineName,
                BatchNo = x.BatchNo,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                LineTotal = x.LineTotal
            }).ToList();

            RecalcTotals();
        }

       
        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCart.Columns[e.ColumnIndex].Name != "colRemove") return;

            int batchId = Convert.ToInt32(dgvCart.Rows[e.RowIndex].Cells["colBatchId"].Value);
            _cart.RemoveAll(x => x.BatchId == batchId);

            BindCart();
        }

        
        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCart.Columns[e.ColumnIndex].Name != "colQty") return;

            int batchId = Convert.ToInt32(dgvCart.Rows[e.RowIndex].Cells["colBatchId"].Value);

            int qty = 1;
            int.TryParse(Convert.ToString(dgvCart.Rows[e.RowIndex].Cells["colQty"].Value), out qty);
            if (qty <= 0) qty = 1;

            var item = _cart.First(x => x.BatchId == batchId);
            item.Quantity = qty;

            BindCart();
        }

    
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            RecalcTotals();
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            RecalcTotals();
        }

        private void RecalcTotals()
        {
            decimal sub = _cart.Sum(x => x.LineTotal);

            decimal.TryParse(txtDiscount.Text, out var dis);
            decimal.TryParse(txtVat.Text, out var vat);

            decimal net = sub - dis + vat;

            txtSubTotal.Text = sub.ToString("0.00");
            txtNetTotal.Text = net.ToString("0.00");
        }

        
        private void btnClear_Click(object sender, EventArgs e)
        {
            _cart.Clear();
            BindCart();
        }

      
        private void btnCompleteSale_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            try
            {
                decimal sub = decimal.Parse(txtSubTotal.Text);
                decimal.TryParse(txtDiscount.Text, out var dis);
                decimal.TryParse(txtVat.Text, out var vat);
                decimal net = decimal.Parse(txtNetTotal.Text);

                int saleId = _salesRepo.CreateSale(_userId, sub, dis, vat, net, _cart);

                MessageBox.Show($"Sale completed.\nInvoice No: {saleId}");

                _cart.Clear();
                BindCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sale Failed");
            }
        }
    }
}
