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
    public partial class Correct_Chek_Info : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ID_Chek { get; set; }

        public Correct_Chek_Info()
        {
            InitializeComponent();
            Load += Invoice_Info_Load;
        }
        private void Invoice_Info_Load(object sender, EventArgs e)
        {
            LoadDataGridView(); // Викликаємо метод для завантаження даних у DataGridView
        }
        private void LoadDataGridView()
        {
            DataBase dataBase = new DataBase();
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                // Запит до бази даних для отримання даних для відображення у DataGridView
                string query = "SELECT * FROM Chek_info WHERE ID_Chek = @ID_Chek"; // Припустимо, що ви фільтруєте дані за ID рахунку
                using (SqlCommand cmd = new SqlCommand(query, dataBase.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@ID_Chek", ID_Chek); // Передача параметру ID

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних у DataGridView: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }
    }
}
