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
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;

    namespace SQLProject
    {
        public partial class Invoice_chek : Form
        {
            DataBase dataBase = new DataBase();
            public string Login { get; set; }
            public string Password { get; set; }

            public Invoice_chek()
            {
                InitializeComponent();
            
            

            }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataBase.SetConnection(Login, Password);
                dataBase.OpenConnection();

                using (SqlCommand cmd = new SqlCommand("NEWINVOICE", dataBase.GetConnection()))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Додавання параметрів до команди
                    cmd.Parameters.AddWithValue("@Invoice_name", textBox1.Text);

                    // Отримання ID накладної
                    var invoiceId = cmd.ExecuteScalar().ToString();

                    MessageBox.Show("Накладну створено з ID: " + invoiceId);

                    OpenNewInvoiceForm(invoiceId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні накладної: " + Login + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }
        private void OpenNewInvoiceForm(string invoiceId)
        {
            New_invoice newInvoiceForm = new New_invoice
            {
                Login = this.Login,
                Password = this.Password,
                InvoiceId = invoiceId
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
    