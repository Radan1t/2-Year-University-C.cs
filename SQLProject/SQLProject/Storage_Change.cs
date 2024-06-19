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
    public partial class Storage_Change : Form
    {
        public Storage_Change()
        {
            InitializeComponent();
        }

        private void Storage_Change_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "marketDBDataSet.Item_list". При необходимости она может быть перемещена или удалена.
            this.item_listTableAdapter.Fill(this.marketDBDataSet.Item_list);

        }
    }
}
