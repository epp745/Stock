using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class StockMain : Form
    {
        public StockMain()
        {
            InitializeComponent();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var products = new Products();
            products.MdiParent = this; // to show the new window inside the main parent window StockMain
            products.StartPosition = FormStartPosition.CenterScreen; // in order to start the window centered
            products.Show();
        }

        bool close = true;
        private void StockMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes)
                {
                    close = false; // to avoid dual messageboxes
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                } 
            }
        }
    }
}
