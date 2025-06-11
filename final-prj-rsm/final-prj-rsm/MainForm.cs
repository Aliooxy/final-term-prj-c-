using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace final_prj_rsm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnUsers_MouseClick(object sender, MouseEventArgs e)
        {
            UserManagementForm userForm = new UserManagementForm();
            userForm.ShowDialog();
        }
       
        private void btnReports_Click(object sender, EventArgs e)
        {
            if (DataManager.ActiveUserIndex == -1)
            {
                MessageBox.Show("!لطفا ابتدا یک کاربر فعال انتخاب کنید");
                return;
            }
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void btnTransactions_Click(object sender, EventArgs e)
        {
            if (DataManager.ActiveUserIndex == -1)
            {
                MessageBox.Show("!لطفا ابتدا یک کاربر فعال انتخاب کنید");
                return;
            }
            TransactionForm transForm = new TransactionForm();
            transForm.ShowDialog();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
