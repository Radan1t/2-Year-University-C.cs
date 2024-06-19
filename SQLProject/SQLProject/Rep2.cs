using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace SQLProject
{
    public partial class Rep2 : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Rep2()
        {
            InitializeComponent();
        }

        private void Rep_Load(object sender, EventArgs e)
        {

            string quary = $"SELECT * FROM WorkerOperations";
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

        private void Rep2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "marketDBDataSet1.WorkerOperations". При необходимости она может быть перемещена или удалена.
            this.workerOperationsTableAdapter.Fill(this.marketDBDataSet1.WorkerOperations);

            this.reportViewer1.RefreshReport();
        }
    }
}
