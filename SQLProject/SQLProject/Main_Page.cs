using System;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Main_Page : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public Main_Page()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 start = new Form1();
            this.Hide();
            start.ShowDialog();
            start.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Invoice_List list = new Invoice_List
            {
                Login = Login,
                Password = Password
            };
            this.Hide();
            list.ShowDialog();
          
        }

        private void Main_Page_Load(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Chek_List_Info list = new Chek_List_Info
            {
                Login = Login,
                Password = Password
            };
            this.Hide();
            list.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Invoice_chek list = new Invoice_chek
            {
                Login = Login,
                Password = Password
            };
            this.Hide();
            list.ShowDialog();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            New_Chek list = new New_Chek
            {
                Login = Login,
                Password = Password
            };
            this.Hide();
            list.ShowDialog();
            

            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Storage storage = new Storage();
            this.Hide();
            storage.ShowDialog();
            this.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Login == "Administrator")
            {
                Admin start = new Admin
                {
                    Login = Login,
                    Password = Password
                };
                start.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("У вас недостатньо прав");
            }
        }
    }
}
