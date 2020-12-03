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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            userNameTextBox.Clear();
            passwordTextBox.Clear();
            userNameTextBox.Focus();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            // TO-DO: Check user name and password to login
            SqlConnection connection = new SqlConnection("Data Source = DEV-VM\\SQLSERVER2019;Initial Catalog = Stock;Integrated Security = True");
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(@"SELECT [UserName], [Password]
                                                                 FROM[Stock].[dbo].[Login]
                                                                 WHERE UserName = '"+ userNameTextBox.Text +"' AND Password = '"+ passwordTextBox.Text +"'", connection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count == 1)
            {
                // redirect to StockMain
                this.Hide(); // hides the login window
                StockMain main = new StockMain();
                main.Show(); 
            }
            else
            {
                MessageBox.Show("Invalid Username and Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clearButton_Click(sender, e); // clear the contents using the clear button's click event 
            }
                
        }
    }
}
