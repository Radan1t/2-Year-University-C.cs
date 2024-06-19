using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace SQLProject
{
    internal class DataBase
    {
        private SqlConnection connection;
        private string username;

        public void SetConnection(string user, string password)
        {
            string lineConn;
            using (StreamReader stream = new StreamReader("DBConnectString.txt"))
            {
                lineConn = stream.ReadLine();
            }
 

            connection = new SqlConnection($"{lineConn}User ID={user};Password={password}");
          
            username = user;
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
        }

        public void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
        }
    }
}
