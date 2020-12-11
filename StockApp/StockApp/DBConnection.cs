using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    public static class DBConnection
    {

        // Get connection method
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection();
            // we need to read the xml file (App.config) containing the connection string so we use the "System.Configuration.ConfigurationManager."
            // Add Reference -> System.Configuration
            connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StockConnection"].ConnectionString;
            return connection;
        }
         
    }
}
