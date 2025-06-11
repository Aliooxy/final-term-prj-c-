using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace final_prj_rsm
{
    public partial class UserManagementForm : Form
    {
        public UserManagementForm()
        {
            InitializeComponent();
            RefreshUsersList();
        }

        private void btnAddUser_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("!لطفا هم نام کاربری و هم ایمیل را وارد کنید");
                return;
            }

            DataManager.Users[DataManager.UserCount, 0] = txtUsername.Text;
            DataManager.Users[DataManager.UserCount, 1] = txtEmail.Text;
            DataManager.Users[DataManager.UserCount, 2] = "0";
            DataManager.UserCount++;

            MessageBox.Show("!کاربر با موفقیت اضافه شد");
            txtUsername.Clear();
            txtEmail.Clear();
            RefreshUsersList();
        }

        private void btnSetActive_Click(object sender, EventArgs e)
        {
            if (listUsers.SelectedIndex == -1)
            {
                MessageBox.Show("!لطفا ابتدا یک کاربر انتخاب کنید");
                return;
            }

            for (int i = 0; i < DataManager.UserCount; i++)
                DataManager.Users[i, 2] = "0";

            DataManager.Users[listUsers.SelectedIndex, 2] = "1";
            DataManager.ActiveUserIndex = listUsers.SelectedIndex;

            MessageBox.Show($" کاربر فعال تنظیم شده روی : {DataManager.Users[listUsers.SelectedIndex, 0]}");
           RefreshUsersList();
        }
        private void RefreshUsersList()
        {
            listUsers.Items.Clear();
            for (int i = 0; i < DataManager.UserCount; i++)
            {
                string status = DataManager.Users[i, 2] == "1" ? "(فعال)" : "";
                listUsers.Items.Add($"{DataManager.Users[i, 0]} - {DataManager.Users[i, 1]} {status}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {

        }
    }
}
