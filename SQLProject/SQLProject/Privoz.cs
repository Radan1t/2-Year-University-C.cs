using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Privoz : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string InvoiceId { get; set; }

        public Privoz()
        {
            InitializeComponent();
        }

        private void Privoz_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
        }

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

                // Заповнення другого ComboBox
                using (SqlCommand cmd = new SqlCommand("SELECT ID_Provider, Provider_name FROM Provider", dataBase.GetConnection()))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        comboBox2.DataSource = dataTable;
                        comboBox2.DisplayMember = "Provider_name";
                        comboBox2.ValueMember = "ID_Provider";
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

                using (SqlCommand cmd = new SqlCommand("NEWINVOICE_ROW", dataBase.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Додавання параметрів до команди
                    cmd.Parameters.AddWithValue("@Invoice_ID", InvoiceId);
                    cmd.Parameters.AddWithValue("@ID_Provider", comboBox2.SelectedValue);
                    cmd.Parameters.AddWithValue("@Article", comboBox1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Count", decimal.Parse(textBox1.Text));
                    cmd.Parameters.AddWithValue("@Price", decimal.Parse(textBox2.Text));

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Товар додано до накладної");
                    New_invoice newInvoiceForm = new New_invoice
                    {
                        Login = this.Login,
                        Password = this.Password,
                        InvoiceId = InvoiceId
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
