using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class New_Item : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        DataBase dataBase = new DataBase();

        public New_Item()
        {
            InitializeComponent();
            Load += New_Item_Load; // Додаємо обробник події Load форми
        }

        private void New_Item_Load(object sender, EventArgs e)
        {
            LoadComboBoxData(); // Викликаємо метод для завантаження даних у комбо бокси
        }

        // Метод для завантаження даних у комбо бокси
        private void LoadComboBoxData()
        {
            try
            {
                dataBase.SetConnection(Login, Password);
                dataBase.OpenConnection();

                // Запит до бази даних для отримання виробників
                string query1 = "SELECT ID_Manufactur, Manufactur_name FROM Manufacturer";
                using (SqlCommand cmd1 = new SqlCommand(query1, dataBase.GetConnection()))
                {
                    using (SqlDataAdapter adapter1 = new SqlDataAdapter(cmd1))
                    {
                        DataTable dataTable1 = new DataTable();
                        adapter1.Fill(dataTable1);

                        comboBox1.DataSource = dataTable1;
                        comboBox1.DisplayMember = "Manufactur_name";
                        comboBox1.ValueMember = "ID_Manufactur";
                    }
                }

                // Запит до бази даних для отримання типів
                string query2 = "SELECT ID_Type, Type_name FROM Type";
                using (SqlCommand cmd2 = new SqlCommand(query2, dataBase.GetConnection()))
                {
                    using (SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2))
                    {
                        DataTable dataTable2 = new DataTable();
                        adapter2.Fill(dataTable2);

                        comboBox2.DataSource = dataTable2;
                        comboBox2.DisplayMember = "Type_name";
                        comboBox2.ValueMember = "ID_Type";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних у комбо бокси: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataBase.SetConnection(Login, Password);
                dataBase.OpenConnection();

                // Виклик збереженої процедури NEWArticle
                // Виклик збереженої процедури NEWArticle
                using (SqlCommand cmd = new SqlCommand("NEWArticle", dataBase.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Додавання параметрів до команди
                    cmd.Parameters.AddWithValue("@ID_Manufactur", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Item_name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@ID_Type", comboBox2.SelectedValue);

                    // Додавання параметру OUTPUT для отримання значення Article
                    SqlParameter articleParam = new SqlParameter("@Article", SqlDbType.Char, 12);
                    articleParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(articleParam);

                    // Виконання команди
                    cmd.ExecuteNonQuery();

                    // Отримання значення Article
                    string article = articleParam.Value.ToString();

                    MessageBox.Show("Новий товар додано успішно");

                    // Отримання значень з текстових полів
                    string countTyper = textBox2.Text;
                    string salePrice = textBox3.Text;
                    string count = textBox4.Text;

                    // Перетворення значень Sale_Price та Count у тип float
                    float salePriceValue;
                    float countValue;

                    if (!float.TryParse(salePrice, out salePriceValue))
                    {
                        MessageBox.Show("Некоректне значення для Sale_Price");
                        return;
                    }

                    if (!float.TryParse(count, out countValue))
                    {
                        MessageBox.Show("Некоректне значення для Count");
                        return;
                    }

                    // Вставка значень в таблицю Storage
                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Storage (Article, Count, Count_Typer, Sale_Price) VALUES (@Article, @Count, @Count_Typer, @Sale_Price)", dataBase.GetConnection()))
                    {
                        insertCmd.Parameters.AddWithValue("@Article", article);
                        insertCmd.Parameters.AddWithValue("@Count", countValue);
                        insertCmd.Parameters.AddWithValue("@Count_Typer", countTyper);
                        insertCmd.Parameters.AddWithValue("@Sale_Price", salePriceValue);

                        insertCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні нового товару: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
            this.Close();
        }


    }
}
