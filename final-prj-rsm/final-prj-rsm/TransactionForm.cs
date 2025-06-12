using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace final_prj_rsm
{
    public partial class TransactionForm : Form
    {
        private int selectedTransactionIndex = -1;

        public TransactionForm()
        {
            InitializeComponent();
            cmbType.SelectedIndex = 0;
            LoadCategories();
            RefreshTransactions();
            SetupButtons();
        }

        private void TransactionForm_Load(object sender, EventArgs e)
        {

        }
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            for (int i = 0; i < DataManager.CategoryCount; i++)
                cmbCategory.Items.Add(DataManager.Categories[i]);
            cmbCategory.SelectedIndex = 0;
        }
        private void AddNewTransaction()
        {
            DataManager.Transactions[DataManager.TransactionCount, 0] = cmbType.Text;
            DataManager.Transactions[DataManager.TransactionCount, 1] = txtAmount.Text;
            DataManager.Transactions[DataManager.TransactionCount, 2] =
                cmbType.Text == "درآمد" ? txtSource.Text : cmbCategory.Text;
            DataManager.Transactions[DataManager.TransactionCount, 3] = dtpDate.Value.ToString("yyyy-MM-dd");
            DataManager.Transactions[DataManager.TransactionCount, 4] = txtDescription.Text;
            DataManager.Transactions[DataManager.TransactionCount, 5] = DataManager.ActiveUserIndex.ToString();
            DataManager.TransactionCount++;

            MessageBox.Show("!تراکنش با موفقیت اضافه شد");
        }
        private void UpdateTransaction()
        {
            DataManager.Transactions[selectedTransactionIndex, 0] = cmbType.Text;
            DataManager.Transactions[selectedTransactionIndex, 1] = txtAmount.Text;
            DataManager.Transactions[selectedTransactionIndex, 2] =
                cmbType.Text == "درآمد" ? txtSource.Text : cmbCategory.Text;
            DataManager.Transactions[selectedTransactionIndex, 3] = dtpDate.Value.ToString("yyyy-MM-dd");
            DataManager.Transactions[selectedTransactionIndex, 4] = txtDescription.Text;

            MessageBox.Show("!تراکنش با موفقیت به‌روزرسانی شد");
            selectedTransactionIndex = -1;
        }
        private void ClearFields()
        {
            txtAmount.Clear();
            txtSource.Clear();
            txtDescription.Clear();
            dtpDate.Value = DateTime.Now;
            cmbType.SelectedIndex = 0;
            btnAdd.Text = "افزودن";
            btnDelete.Enabled = true;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSource.Visible = txtSource.Visible = cmbType.Text == "درآمد";
            lblCategory.Visible = cmbCategory.Visible = cmbType.Text == "هزینه";
        }

        private bool ValidateInputs()
        {
            if (!decimal.TryParse(txtAmount.Text, out _))
            {
                MessageBox.Show("!لطفا مبلغ معتبری وارد کنید");
                return false;
            }

            if (cmbType.Text == "درآمد" && string.IsNullOrWhiteSpace(txtSource.Text))
            {
                MessageBox.Show("!لطفا منبع درآمد را وارد کنید");
                return false;
            }

            return true;
        }

        private void RefreshTransactions()
        {
            listTransactions.Items.Clear();
            for (int i = 0; i < DataManager.TransactionCount; i++)
            {
                if (DataManager.Transactions[i, 5] == DataManager.ActiveUserIndex.ToString())
                {
                    listTransactions.Items.Add(
                        $"{DataManager.Transactions[i, 0]}: {DataManager.Transactions[i, 1]} " +
                        $"برای {DataManager.Transactions[i, 2]} در تاریخ {DataManager.Transactions[i, 3]}");
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            if (selectedTransactionIndex == -1)
            {
                AddNewTransaction();
            }
            else
            {
                UpdateTransaction();
            }

            RefreshTransactions();
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listTransactions.SelectedIndex == -1)
            {
                MessageBox.Show("!لطفا تراکنشی را برای حذف انتخاب کنید");
                return;
            }

            if (MessageBox.Show("آیا از حذف این تراکنش مطمئن هستید؟",
                "تأیید حذف", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int selectedIndex = listTransactions.SelectedIndex;
                int actualIndex = GetActualTransactionIndex(selectedIndex);

                for (int i = actualIndex; i < DataManager.TransactionCount - 1; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        DataManager.Transactions[i, j] = DataManager.Transactions[i + 1, j];
                    }
                }
                DataManager.TransactionCount--;

                MessageBox.Show("!تراکنش با موفقیت حذف شد");
                RefreshTransactions();
                ClearFields();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listTransactions.SelectedIndex == -1)
            {
                MessageBox.Show("!لطفا تراکنش مورد نظر را برای ویرایش انتخاب کنید");
                return;
            }

            int selectedIndex = listTransactions.SelectedIndex;
            selectedTransactionIndex = GetActualTransactionIndex(selectedIndex);

            cmbType.Text = DataManager.Transactions[selectedTransactionIndex, 0];
            txtAmount.Text = DataManager.Transactions[selectedTransactionIndex, 1];

            if (cmbType.Text == "درآمد")
            {
                txtSource.Text = DataManager.Transactions[selectedTransactionIndex, 2];
            }
            else
            {
                cmbCategory.Text = DataManager.Transactions[selectedTransactionIndex, 2];
            }

            dtpDate.Value = DateTime.Parse(DataManager.Transactions[selectedTransactionIndex, 3]);
            txtDescription.Text = DataManager.Transactions[selectedTransactionIndex, 4];

            btnAdd.Text = "به‌روزرسانی";
            btnDelete.Enabled = false;
        }
       
       
        private int GetActualTransactionIndex(int listIndex)
        {
            int count = 0;
            for (int i = 0; i < DataManager.TransactionCount; i++)
            {
                if (DataManager.Transactions[i, 5] == DataManager.ActiveUserIndex.ToString())
                {
                    if (count == listIndex) return i;
                    count++;
                }
            }
            return -1;
        }
        private void SetupButtons()
        {
            btnAdd.Text = "افزودن";
            btnDelete.Enabled = true;
        }
       

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
