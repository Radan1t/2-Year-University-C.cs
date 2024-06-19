namespace SQLProject
{
    partial class Rep2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.marketDBDataSet1 = new SQLProject.MarketDBDataSet1();
            this.workerOperationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.workerOperationsTableAdapter = new SQLProject.MarketDBDataSet1TableAdapters.WorkerOperationsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.marketDBDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerOperationsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource2.Name = "DataSet1";
            reportDataSource2.Value = this.workerOperationsBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "SQLProject.Report2.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // marketDBDataSet1
            // 
            this.marketDBDataSet1.DataSetName = "MarketDBDataSet1";
            this.marketDBDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // workerOperationsBindingSource
            // 
            this.workerOperationsBindingSource.DataMember = "WorkerOperations";
            this.workerOperationsBindingSource.DataSource = this.marketDBDataSet1;
            // 
            // workerOperationsTableAdapter
            // 
            this.workerOperationsTableAdapter.ClearBeforeFill = true;
            // 
            // Rep2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Rep2";
            this.Text = "Rep2";
            this.Load += new System.EventHandler(this.Rep2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.marketDBDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerOperationsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private MarketDBDataSet1 marketDBDataSet1;
        private System.Windows.Forms.BindingSource workerOperationsBindingSource;
        private MarketDBDataSet1TableAdapters.WorkerOperationsTableAdapter workerOperationsTableAdapter;
    }
}