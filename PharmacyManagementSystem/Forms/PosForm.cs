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
        private int _lastSaleId = 0;

        private const decimal VAT_PERCENT = 10m; 

        public PosForm(int userId, string FullName)
        {
            InitializeComponent();

            _userId = userId;
            _fullName = FullName;

            lblCashier.Text = $"Cashier: {_fullName}";

           
            txtDiscount.Text = "0";

           
            txtVat.Text = VAT_PERCENT.ToString("0");
            txtVat.ReadOnly = true;

            RecalcTotals();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var q = txtSearch.Text.Trim();
            var list = _invRepo.SearchAvailableBatches(q);

            dgvBatches.AutoGenerateColumns = false;
            dgvBatches.DataSource = list;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var q = txtSearch.Text.Trim();
                var list = _invRepo.SearchAvailableBatches(q);

                dgvBatches.AutoGenerateColumns = false;
                dgvBatches.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message);
            }
        }

        private void dgvBatches_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvBatches.Columns[e.ColumnIndex].Name != "colAdd") return;

            var row = dgvBatches.Rows[e.RowIndex];

            int batchId = Convert.ToInt32(row.Cells["BatchId"].Value);
            string medName = Convert.ToString(row.Cells["MedicineName"].Value);
            string batchNo = Convert.ToString(row.Cells["BatchNo"].Value);
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
            dgvCart.DataSource = _cart;

            RecalcTotals();
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCart.Columns[e.ColumnIndex].Name != "colRemove") return;

            dynamic itemRow = dgvCart.Rows[e.RowIndex].DataBoundItem;
            int batchId = (int)itemRow.BatchId;

            _cart.RemoveAll(x => x.BatchId == batchId);
            BindCart();
        }

        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

           
            if (dgvCart.Columns[e.ColumnIndex].DataPropertyName != "Quantity") return;

            dynamic itemRow = dgvCart.Rows[e.RowIndex].DataBoundItem;

            int batchId = (int)itemRow.BatchId;

            int qty;
            if (!int.TryParse(Convert.ToString(itemRow.Quantity), out qty) || qty <= 0)
                qty = 1;

            var item = _cart.FirstOrDefault(x => x.BatchId == batchId);
            if (item == null) return;

            item.Quantity = qty;

            dgvCart.Refresh();
            RecalcTotals();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscount.Focused && string.IsNullOrWhiteSpace(txtDiscount.Text))
                return;

            RecalcTotals();
        }

        
        private decimal GetDiscountPercent()
        {
            decimal p = 0m;
            decimal.TryParse(txtDiscount.Text, out p);
            if (p < 0) p = 0;
            if (p > 100) p = 100;
            return p;
        }

        
        private void ComputeTotals(out decimal subTotal, out decimal discountAmount, out decimal vatAmount, out decimal netTotal)
        {
            subTotal = _cart.Sum(x => x.LineTotal);

            decimal discountPercent = GetDiscountPercent();
            decimal afterDiscount = subTotal - (subTotal * (discountPercent / 100m));
            discountAmount = subTotal - afterDiscount;

            vatAmount = afterDiscount * (VAT_PERCENT / 100m);
            netTotal = afterDiscount + vatAmount;
        }

        private void RecalcTotals()
        {
            ComputeTotals(out var sub, out var disAmt, out var vatAmt, out var net);

            txtSubTotal.Text = sub.ToString("0.00");
            txtNetTotal.Text = net.ToString("0.00");

           
            if (txtVat.Text != VAT_PERCENT.ToString("0"))
                txtVat.Text = VAT_PERCENT.ToString("0");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _cart.Clear();

            txtDiscount.Text = "0";
            txtVat.Text = VAT_PERCENT.ToString("0"); 

            _lastSaleId = 0;

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
               
                ComputeTotals(out var sub, out var discountAmount, out var vatAmount, out var net);

                int saleId = _salesRepo.CreateSale(_userId, sub, discountAmount, vatAmount, net, _cart);
                _lastSaleId = saleId;

                MessageBox.Show($"Sale completed.\nInvoice No: {saleId}");

               
                _cart.Clear();
                txtDiscount.Text = "0";
                txtVat.Text = VAT_PERCENT.ToString("0");

                BindCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sale Failed");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_lastSaleId <= 0)
            {
                MessageBox.Show("Please complete a sale first, then print the invoice.");
                return;
            }

            using (var f = new SaleDetailsForm(_lastSaleId))
            {
                f.ShowDialog();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
           
            if (this.Owner != null)
                this.Owner.Show();

            this.Close();
        }

        

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDiscount.Text))
                txtDiscount.Text = "0";
        }

        private void txtDiscount_Enter(object sender, EventArgs e)
        {
            if (txtDiscount.Text.Trim() == "0")
                txtDiscount.Clear();
        }
    }
}
