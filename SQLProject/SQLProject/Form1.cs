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
    public partial class Form1 : Form
    {
        DataBase dataBase = new DataBase();
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {


        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataBase.SetConnection(textBox1.Text, textBox2.Text);
                dataBase.OpenConnection();
                dataBase.CloseConnection();


                Main_Page form2 = new Main_Page();
                form2.Login=textBox1.Text;
                form2.Password = textBox2.Text;
                form2.FormClosed += (s, args) => this.Close(); // Додаємо обробник події FormClosed для закриття поточної форми

                this.Hide();
                form2.Show();
                
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Невірний пароль або логін: {ex.Message}", "Помилка", MessageBoxButtons.OK);
            }
        }

    }
}
