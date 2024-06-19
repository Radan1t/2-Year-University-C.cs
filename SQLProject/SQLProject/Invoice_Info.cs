using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Invoice_Info : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ID_Invoice { get; set; }

        public Invoice_Info()
        {
            InitializeComponent();
            Load += Invoice_Info_Load; // Додаємо обробник події Load форми
        }

        // Метод, який буде викликаний при завантаженні форми
        private void Invoice_Info_Load(object sender, EventArgs e)
        {
            LoadDataGridView(); // Викликаємо метод для завантаження даних у DataGridView
        }

        // Метод для завантаження даних у DataGridView
        private void LoadDataGridView()
        {
            DataBase dataBase = new DataBase();
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                // Запит до бази даних для отримання даних для відображення у DataGridView
                string query = "SELECT * FROM Invoice_list WHERE ID_Invoice = @ID_Invoice"; // Припустимо, що ви фільтруєте дані за ID рахунку
                using (SqlCommand cmd = new SqlCommand(query, dataBase.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@ID_Invoice", ID_Invoice); // Передача параметру ID

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
