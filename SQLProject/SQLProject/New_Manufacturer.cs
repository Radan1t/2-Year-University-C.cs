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

namespace SQLProject
{
    public partial class New_Manufacturer : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        DataBase dataBase = new DataBase();
        public New_Manufacturer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataBase.SetConnection(Login, Password);
                dataBase.OpenConnection();

                // Виклик збереженої процедури NEWType
                using (SqlCommand cmd = new SqlCommand("NEWManufactur", dataBase.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Додавання параметрів до команди
                    cmd.Parameters.AddWithValue("@Manufacture_name", textBox1.Text);

                    // Виконання команди
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Створено виробника");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при створенні нового типу товару: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
            this.Close();
        }
    }
}
