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
        SqlConnection con = new SqlConnection(
    ConfigurationManager.ConnectionStrings["PharmacyDB"].ConnectionString);
        public CategoryForm()
        {
            InitializeComponent();
        }
        private void CategoryForm_Load(object sender, EventArgs e)
        {
            //LoadCategories();
        }
    }
}
