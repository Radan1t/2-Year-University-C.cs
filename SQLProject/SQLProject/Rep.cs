using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SQLProject
{
    public partial class Rep : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Rep()
        {
            InitializeComponent();
        }

        private void Rep_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "marketDBDataSet1.WorkerOperations". При необходимости она может быть перемещена или удалена.
            

            string quary = $"SELECT * FROM Storage_type_list";
            DataBase dataBase = new DataBase();
            dataBase.SetConnection(Login, Password);
            SqlCommand comand = new SqlCommand(quary, dataBase.GetConnection());
            SqlDataAdapter dataAdapter = new SqlDataAdapter(comand);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSet1", dataTable);
            reportViewer1.LocalReport.ReportEmbeddedResource = "SQLProject.Report1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            this.reportViewer1.RefreshReport();

           
        }
    }
}
