namespace PharmacyManagementSystem
{
    partial class InventoryDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSupplier = new System.Windows.Forms.Button();
            this.btnBatchStockForm = new System.Windows.Forms.Button();
            this.btnPurchase = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(652, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(267, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "CategoryForm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(652, 93);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(267, 55);
            this.button2.TabIndex = 1;
            this.button2.Text = "MedicineForm";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSupplier
            // 
            this.btnSupplier.Location = new System.Drawing.Point(652, 154);
            this.btnSupplier.Name = "btnSupplier";
            this.btnSupplier.Size = new System.Drawing.Size(267, 55);
            this.btnSupplier.TabIndex = 2;
            this.btnSupplier.Text = "SupplierForm";
            this.btnSupplier.UseVisualStyleBackColor = true;
            this.btnSupplier.Click += new System.EventHandler(this.btnSupplier_Click);
            // 
            // btnBatchStockForm
            // 
            this.btnBatchStockForm.Location = new System.Drawing.Point(652, 215);
            this.btnBatchStockForm.Name = "btnBatchStockForm";
            this.btnBatchStockForm.Size = new System.Drawing.Size(267, 55);
            this.btnBatchStockForm.TabIndex = 3;
            this.btnBatchStockForm.Text = "BatchStockForm";
            this.btnBatchStockForm.UseVisualStyleBackColor = true;
            this.btnBatchStockForm.Click += new System.EventHandler(this.btnBatchStockForm_Click);
            // 
            // btnPurchase
            // 
            this.btnPurchase.Location = new System.Drawing.Point(652, 276);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.Size = new System.Drawing.Size(267, 55);
            this.btnPurchase.TabIndex = 4;
            this.btnPurchase.Text = "PurchaseForm";
            this.btnPurchase.UseVisualStyleBackColor = true;
            this.btnPurchase.Click += new System.EventHandler(this.btnPurchase_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(652, 337);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(267, 55);
            this.button6.TabIndex = 5;
            this.button6.Text = "StockAlertForm";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // InventoryDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 514);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.btnPurchase);
            this.Controls.Add(this.btnBatchStockForm);
            this.Controls.Add(this.btnSupplier);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "InventoryDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InventoryDashboard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSupplier;
        private System.Windows.Forms.Button btnBatchStockForm;
        private System.Windows.Forms.Button btnPurchase;
        private System.Windows.Forms.Button button6;
    }
}