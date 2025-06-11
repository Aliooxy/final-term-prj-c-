using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace final_prj_rsm
{
    public partial class ReportsForm : Form
    {
       
        public ReportsForm()
        {
            InitializeComponent();
            

        }
        private void SetInitialValues()
        {
            cmbReportType.SelectedIndex = 0;
            dtpMonth.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            GenerateReport();
        }

        private void UpdateDateRange(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex == 0)
                dtpMonth.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            else if (cmbReportType.SelectedIndex == 2)
                dtpMonth.Value = DateTime.Now;
        }

        private void GenerateReport(object sender = null, EventArgs e = null)
        {
            switch (cmbReportType.SelectedIndex)
            {
                case 0: GenerateMonthlySummary(); break;
                case 1: GenerateCategoryReport(); break;
                case 2: GenerateDailyBalance(); break;
            }
        }

        private void GenerateMonthlySummary()
        {
            decimal totalIncome = 0, totalExpense = 0;
            DateTime selectedDate = dtpMonth.Value;

            for (int i = 0; i < DataManager.TransactionCount; i++)
            {
                if (DataManager.Transactions[i, 5] != DataManager.ActiveUserIndex.ToString())
                    continue;

                DateTime transDate = DateTime.Parse(DataManager.Transactions[i, 3]);
                if (transDate.Year == selectedDate.Year && transDate.Month == selectedDate.Month)
                {
                    decimal amount = decimal.Parse(DataManager.Transactions[i, 1]);
                    if (DataManager.Transactions[i, 0] == "درآمد")
                        totalIncome += amount;
                    else
                        totalExpense += amount;
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("عنوان");
            dt.Columns.Add("مبلغ", typeof(decimal));

            dt.Rows.Add("کل درآمدها", totalIncome);
            dt.Rows.Add("کل هزینه‌ها", totalExpense);
            dt.Rows.Add("مانده ماه", totalIncome - totalExpense);

            dgvReports.DataSource = dt;
            FormatDataGrid();
        }

        private void GenerateCategoryReport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("دسته‌بندی");
            dt.Columns.Add("درآمد", typeof(decimal));
            dt.Columns.Add("هزینه", typeof(decimal));
            dt.Columns.Add("مانده", typeof(decimal));

            decimal totalIncome = 0, totalExpense = 0;
            DateTime selectedDate = dtpMonth.Value;

            foreach (string category in DataManager.Categories)
                dt.Rows.Add(category, 0, 0, 0);

            for (int i = 0; i < DataManager.TransactionCount; i++)
            {
                if (DataManager.Transactions[i, 5] != DataManager.ActiveUserIndex.ToString())
                    continue;

                DateTime transDate = DateTime.Parse(DataManager.Transactions[i, 3]);
                if (transDate.Year == selectedDate.Year && transDate.Month == selectedDate.Month)
                {
                    decimal amount = decimal.Parse(DataManager.Transactions[i, 1]);
                    string category = DataManager.Transactions[i, 2];
                    int rowIndex = Array.IndexOf(DataManager.Categories, category);
                    if (rowIndex < 0) continue;

                    if (DataManager.Transactions[i, 0] == "درآمد")
                    {
                        dt.Rows[rowIndex]["درآمد"] = (decimal)dt.Rows[rowIndex]["درآمد"] + amount;
                        totalIncome += amount;
                    }
                    else
                    {
                        dt.Rows[rowIndex]["هزینه"] = (decimal)dt.Rows[rowIndex]["هزینه"] + amount;
                        totalExpense += amount;
                    }
                }
            }

            foreach (DataRow row in dt.Rows)
                row["مانده"] = (decimal)row["درآمد"] - (decimal)row["هزینه"];

            dt.Rows.Add("جمع کل", totalIncome, totalExpense, totalIncome - totalExpense);

            dgvReports.DataSource = dt;
            FormatDataGrid();
        }

        private void GenerateDailyBalance()
        {
            DateTime selectedDate = dtpMonth.Value;
            decimal income = 0, expense = 0;

            for (int i = 0; i < DataManager.TransactionCount; i++)
            {
                if (DataManager.Transactions[i, 5] != DataManager.ActiveUserIndex.ToString())
                    continue;

                DateTime transDate = DateTime.Parse(DataManager.Transactions[i, 3]);
                if (transDate.Date == selectedDate.Date)
                {
                    decimal amount = decimal.Parse(DataManager.Transactions[i, 1]);
                    if (DataManager.Transactions[i, 0] == "درآمد")
                        income += amount;
                    else
                        expense += amount;
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("تاریخ");
            dt.Columns.Add("درآمد", typeof(decimal));
            dt.Columns.Add("هزینه", typeof(decimal));
            dt.Columns.Add("مانده", typeof(decimal));

            dt.Rows.Add(
                selectedDate.ToString("yyyy/MM/dd"),
                income,
                expense,
                income - expense
            );

            dgvReports.DataSource = dt;
            FormatDataGrid();
        }

        private void FormatDataGrid()
        {
            foreach (DataGridViewColumn column in dgvReports.Columns)
            {
                if (column.ValueType == typeof(decimal))
                {
                    column.DefaultCellStyle.Format = "N0";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            if (dgvReports.Rows.Count > 0)
            {
                dgvReports.Rows[dgvReports.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
                dgvReports.Rows[dgvReports.Rows.Count - 1].DefaultCellStyle.Font =
                    new Font(dgvReports.Font, FontStyle.Bold);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
    }
}
