using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Chek_List : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Chek_ID { get; set; }
        private DataBase dataBase = new DataBase();

        public Chek_List()
        {
            InitializeComponent();
            this.Load += new EventHandler(Chek_List_Load);
        }

        private void Chek_List_Load(object sender, EventArgs e)
        {
            label1.Text = "Чек № " + Chek_ID;
            LoadInvoiceItems();
            CalculateTotalPrice(); // Виклик методу для обрахунку загальної суми
        }

        private void LoadInvoiceItems()
        {
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                using (SqlCommand cmd = new SqlCommand("SELECT Article, Count_of_saled FROM Chek_row WHERE ID_Chek = @Chek_ID", dataBase.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Chek_ID", Chek_ID);

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
                MessageBox.Show("Помилка при завантаженні товарів: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }

        private void CalculateTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Article"].Value != null && row.Cells["Count_of_saled"].Value != null)
                {
                    string article = row.Cells["Article"].Value.ToString();
                    int count = Convert.ToInt32(row.Cells["Count_of_saled"].Value);

                    // Отримання ціни товару з таблиці складу (Storage)
                    decimal price = GetProductPriceFromStorage(article);

                    // Додавання до загальної суми
                    totalPrice += price * count;
                }
            }

            // Виведення загальної суми
            labelTotalPrice.Text = "Загальна сума: " + totalPrice.ToString()+ "₴";
        }

        private decimal GetProductPriceFromStorage(string article)
        {
            decimal price = 0;

            try
            {
                dataBase.OpenConnection();

                using (SqlCommand cmd = new SqlCommand("SELECT Sale_Price FROM Storage WHERE Article = @Article", dataBase.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Article", article);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        price = Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при отриманні ціни товару: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }

            return price;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main_Page main_Page = new Main_Page
            {
                Login = this.Login,
                Password = this.Password,
            };
            this.Hide();
            main_Page.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            New_Chek_Row newInvoiceForm = new New_Chek_Row
            {
                Login = this.Login,
                Password = this.Password,
                Chek_ID = Chek_ID
            };
            // Підписка на подію закриття форми New_invoice
            newInvoiceForm.FormClosed += NewInvoiceForm_FormClosed;
            this.Hide();
            newInvoiceForm.Show();
        }

        private void NewInvoiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Відображення форми Invoice_chek після закриття форми New_invoice
            this.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Main_Page main_Page = new Main_Page
            {
                Login = this.Login,
                Password = this.Password,
            };
            this.Hide();
            main_Page.Show();
        }
    }
}
