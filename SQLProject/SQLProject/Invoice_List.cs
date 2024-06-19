using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Invoice_List : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public Invoice_List()
        {
            InitializeComponent();
            Load += Chek_List_Info_Load; // Додаємо обробник події Load форми
        }

        // Метод, який буде викликаний при завантаженні форми
        private void Chek_List_Info_Load(object sender, EventArgs e)
        {
            LoadComboBoxData(); // Викликаємо метод загрузки даних у комбо бокси
        }

        // Метод для завантаження даних у комбо бокси
        private void LoadComboBoxData()
        {
            DataBase dataBase = new DataBase();
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                // Заповнення комбо бокса з Invoice_id та Invoice_name
                using (SqlCommand cmd = new SqlCommand("SELECT ID_Chek FROM Chek", dataBase.GetConnection()))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        comboBox1.DataSource = dataTable;
                        comboBox1.DisplayMember = "ID_Chek";
                        comboBox1.ValueMember = "ID_Chek";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Main_Page main_Page = new Main_Page
            {
                Login = this.Login,
                Password = this.Password,
            };
            this.Hide();
            main_Page.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Correct_Chek_Info main_Page = new Correct_Chek_Info
            {
                Login = this.Login,
                Password = this.Password,
                ID_Chek = comboBox1.SelectedValue.ToString(),
            };

            main_Page.Show();
        }
    }
}
