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
            productStatusComboBox.SelectedIndex = -1;
            productNameTextBox.Focus();

            LoadData();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                //SqlConnection connection = new SqlConnection("Data Source = DEV-VM\\SQLSERVER2019;Initial Catalog = Stock;Integrated Security = True");
                SqlConnection connection = DBConnection.GetConnection();
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

                ClearTextBoxes(); 
            }
        }

        private void ClearTextBoxes()
        {
            productCodeTextBox.Clear();
            productNameTextBox.Clear();
            productStatusComboBox.SelectedIndex = -1;
            productStatusComboBox.Text = "";
            addButton.Text = "Add";
            productNameTextBox.Focus();
        }

        private bool IfProductExists(SqlConnection connection ,string productCode)
        {
            if(productCode =="")
            {
                //MessageBox.Show("Please select a Product.");
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
            SqlConnection connection = DBConnection.GetConnection();

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
            //productStatusComboBox.SelectedIndex = int.Parse(productsDataGridView.SelectedRows[0].Cells[2].Value.ToString()); //productStatusComboBox.SelectedIndex;

            //MessageBox.Show(productsDataGridView.SelectedRows[0].Cells[2].Value.ToString());//true
            //MessageBox.Show(productsDataGridView.SelectedRows[0].Cells[2].ToString()); // row index....
            //MessageBox.Show(productsDataGridView.SelectedRows[0].Cells[2].Selected.ToString() );//true
            //MessageBox.Show(productsDataGridView.SelectedRows[0].Cells[2].State.ToString()); //none
            //MessageBox.Show(productsDataGridView.SelectedRows[0].Cells[2].);

            addButton.Text = "Update";
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete record?", "Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (Validation())
                {
                    SqlConnection connection = DBConnection.GetConnection();

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
                        MessageBox.Show("You must select a valid Product to delete.", "Error");
                    }
                    // Load data to DataGridView
                    LoadData();
                    ClearTextBoxes();
                } 
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private bool Validation()
        {
            bool result = false;

            errorProvider.Clear();

            //if (string.IsNullOrEmpty(productCodeTextBox.Text))
            //{
            //    errorProvider.Clear();
            //    errorProvider.SetError(productCodeTextBox, "Product Name Required");
            //}
            //else if (string.IsNullOrEmpty(productNameTextBox.Text))
            if (string.IsNullOrEmpty(productNameTextBox.Text))
            {
                errorProvider.Clear();
                errorProvider.SetError(productNameTextBox, "Product name required");
            }
            //else if (productStatusComboBox.SelectedIndex == -1)
            else if (productStatusComboBox.Text == "")
            {
                errorProvider.Clear();
                errorProvider.SetError(productStatusComboBox, "Pease select status");
            }
            else
                result = true;

           //if (string.IsNullOrEmpty(productCodeTextBox.Text) && string.IsNullOrEmpty(productNameTextBox.Text) && productStatusComboBox.SelectedIndex == -1)
           //result = false;
           
            return result;
        }
    }
}
