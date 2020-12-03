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

namespace StockApp
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            productStatusComboBox.SelectedIndex = 1;
            LoadData();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source = DEV-VM\\SQLSERVER2019;Initial Catalog = Stock;Integrated Security = True");
            connection.Open();

            var sqlQuery = "";

            if (IfProductExists(connection, productCodeTextBox.Text))
            {
                //update record
                sqlQuery = @"UPDATE [dbo].[Products] 
                            SET [ProductName] = '" + productNameTextBox.Text + "', [ProductStatus] = " + productStatusComboBox.SelectedIndex + " WHERE [ProductCode] = " + productCodeTextBox.Text;
            }
            else
            {
                // insert new record - not exists
                sqlQuery = @"INSERT INTO [dbo].[Products] ([ProductName], [ProductStatus])
                            VALUES ('" + productNameTextBox.Text + "', '" + productStatusComboBox.SelectedIndex + "')";
            }

            SqlCommand cmd = new SqlCommand(sqlQuery, connection);
            cmd.ExecuteNonQuery();
            connection.Close();

            // Load data to DataGridView
            LoadData();
        }

        private bool IfProductExists(SqlConnection connection ,string productCode)
        {
            if(productCode =="")
            {
                MessageBox.Show("Please select a Product.");
                return false;
            }
            else
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT 1 FROM [dbo].[Products] WHERE [ProductCode] = " + productCode, connection);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            
        }
        private void LoadData()
        {
            SqlConnection connection = new SqlConnection("Data Source = DEV-VM\\SQLSERVER2019;Initial Catalog = Stock;Integrated Security = True");

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT * FROM [dbo].[Products]", connection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            productsDataGridView.Rows.Clear(); //////////////////// IMPORTANT ROW CLEAR BEFORE LOAD//////////////

            foreach (DataRow item in dataTable.Rows)
            {
                int n = productsDataGridView.Rows.Add();

                productsDataGridView.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                productsDataGridView.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                productsDataGridView.Rows[n].Cells[2].Value = item["ProductStatus"].ToString();

            }
        }

        private void productsDataGridView_DoubleClick(object sender, EventArgs e)
        {
            productCodeTextBox.Text = productsDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            productNameTextBox.Text = productsDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            productStatusComboBox.Text = productsDataGridView.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source = DEV-VM\\SQLSERVER2019;Initial Catalog = Stock;Integrated Security = True");

            var sqlQuery = "";

            if (IfProductExists(connection, productCodeTextBox.Text))
            {
                //delete record
                connection.Open();
                sqlQuery = @"DELETE FROM [dbo].[Products] WHERE [ProductCode] = " + productCodeTextBox.Text;

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                MessageBox.Show("You must select a valid Product to delete.","Error");
            }
            // Load data to DataGridView
            LoadData();
        }
    }
} // comment test for GitHub
