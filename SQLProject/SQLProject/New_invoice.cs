using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class New_invoice : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string InvoiceId { get; set; }

        public New_invoice()
        {
            InitializeComponent();
        }

        private void New_invoice_Load(object sender, EventArgs e)
        {
            // Оновлюємо текст мітки після завантаження форми
            label1.Text = "Накладна № " + InvoiceId;
            LoadInvoiceItems();
        }

        private void LoadInvoiceItems()
        {
            DataBase dataBase = new DataBase();
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                using (SqlCommand cmd = new SqlCommand("SELECT ID_Provider, Article, Count, Price FROM Invoice_row WHERE ID_Invoice = @InvoiceId", dataBase.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }

                CalculateTotalPrice(); // Виклик методу для обрахунку загальної суми після завантаження даних
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
                if (row.Cells["Count"].Value != null && row.Cells["Price"].Value != null)
                {
                    int count = Convert.ToInt32(row.Cells["Count"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);

                    // Додавання до загальної суми
                    totalPrice += price * count;
                }
            }

            // Виведення загальної суми з символом гривні
            labelTotalPrice.Text = "Загальна сума: " + totalPrice.ToString() + "₴";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Privoz newInvoiceForm = new Privoz
            {
                Login = this.Login,
                Password = this.Password,
                InvoiceId = InvoiceId
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
    }
}
