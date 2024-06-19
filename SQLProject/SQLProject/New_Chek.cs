using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class New_Chek : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        private DataBase dataBase = new DataBase();

        public New_Chek()
        {
            InitializeComponent();
            this.Load += new EventHandler(New_Chek_Load);
        }

        private void New_Chek_Load(object sender, EventArgs e)
        {
            LoadEmployeeNames();
        }

        private void LoadEmployeeNames()
        {
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                string query = "SELECT ID_Worker, Last_name FROM Workers";
                using (SqlCommand cmd = new SqlCommand(query, dataBase.GetConnection()))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Дані про працівників не знайдено.");
                        }
                        else
                        {
                            comboBox1.DataSource = dataTable;
                            comboBox1.DisplayMember = "Last_name";
                            comboBox1.ValueMember = "ID_Worker";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні фамілій працівників: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }

        private void AddCheck()
        {
            dataBase.SetConnection(Login, Password);
            try
            {
                dataBase.OpenConnection();

                // Отримуємо ідентифікатор обраного працівника
                if (comboBox1.SelectedValue != null)
                {
                    string selectedEmployeeId = comboBox1.SelectedValue.ToString();
                    using (SqlCommand cmd = new SqlCommand("NEWChek", dataBase.GetConnection()))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Worker", selectedEmployeeId);
                        var ID_Chek = cmd.ExecuteScalar().ToString();

                        Chek_List newInvoiceForm = new Chek_List
                        {
                            Login = this.Login,
                            Password = this.Password,
                            Chek_ID = ID_Chek
                        };
                        this.Hide();
                        newInvoiceForm.ShowDialog();
                      
                    }
                }
                else
                {
                    MessageBox.Show("Не вдалося отримати ідентифікатор обраного працівника.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні чеку: " + ex.Message);
            }
            finally
            {
                dataBase.CloseConnection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddCheck();
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
