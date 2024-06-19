using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SQLProject
{
    public partial class New_Chek_Row : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Chek_ID { get; set; }

        public New_Chek_Row()
        {
            InitializeComponent();
            Load += New_Chek_Row_Load; // Додаємо обробник події Load форми
        }

        // Метод, який буде викликаний при завантаженні форми
        private void New_Chek_Row_Load(object sender, EventArgs e)
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

                // Заповнення першого ComboBox
                using (SqlCommand cmd = new SqlCommand("SELECT Article, Item_name FROM Item", dataBase.GetConnection()))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        comboBox1.DataSource = dataTable;
                        comboBox1.DisplayMember = "Item_name";
                        comboBox1.ValueMember = "Article";
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

            DataBase dataBase = new DataBase();
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                using (SqlCommand cmd = new SqlCommand("NEWChekRow", dataBase.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Додавання параметрів до команди
                    cmd.Parameters.AddWithValue("@ID_Chek", Chek_ID);
 
                    cmd.Parameters.AddWithValue("@Article", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Count_of_saled", decimal.Parse(textBox1.Text));
 

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Товар додано до чеку");
                    Chek_List newInvoiceForm = new Chek_List
                    {
                        Login = this.Login,
                        Password = this.Password,
                        Chek_ID = Chek_ID
                    };
                    this.Hide();
                    newInvoiceForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні товару: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }
    }
}
