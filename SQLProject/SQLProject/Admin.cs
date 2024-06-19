using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Admin : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Admin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Workers start = new Workers();
            this.Hide();
            start.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Storage_Change start = new Storage_Change();
            this.Hide();
            start.ShowDialog();
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            New_Type type = new New_Type { 
                Login = Login,
                Password = Password,
            };
            type.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            New_Provider type = new New_Provider
            {
                Login = Login,
                Password = Password,
            };
            type.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            New_Manufacturer type = new New_Manufacturer
            {
                Login = Login,
                Password = Password,
            };
            type.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            New_Item type = new New_Item
            {
                Login = Login,
                Password = Password,
            };
            type.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Rep report = new Rep
            {
                Login = Login,
                Password = Password,
            };
            report.ShowDialog();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Rep2 report = new Rep2
            {
                Login = Login,
                Password = Password,
            };
            report.ShowDialog();
        }
    }
}
