﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();

            
            lblFullName.Text = Resource1.FullName;
            btnAdd.Text = Resource1.Add;
            btndelete.Text = Resource1.Delete;

            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = txtFullName.Text,
                
            };
            users.Add(u);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "csv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = saveFileDialog.FileName;
                    StreamWriter sw = new StreamWriter(path);
                    for (int i = 0; i < users.Count; i++)
                    {
                        sw.WriteLine("{0},{1}", users[i].ID, users[i].FullName);
                    }
                    sw.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {

            User userToRemove = (User)listBox1.SelectedItem;
            try
            {
                users.Remove(userToRemove);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
