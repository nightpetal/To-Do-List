using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class UserInterface : Form
    {
        public string uName {  get; set; }
        public UserInterface(string UserName)
        {
            InitializeComponent();
            //label1.Text = $"Hello {UserName}";

            var items = ToDoList.Items.Add($"{UserName} add new task here!");
            checkedListBox1.Items.Add("Completed task", true);
        }
        private void UserInterface_Load(object sender, EventArgs e)
        {
            //MessageBox.Show($"Username from constructor: {label1.Text}\nC: {uName}");
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (var login = new LoginForm())
            {
                var result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //label1.Text = $"Hello {login.UserName}";
                    this.Show();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddTaskBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                ToDoList.Items.Add(textBox1.Text);
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Blank Entry!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            button3.Visible = true;
            groupBox2.Visible = false;
            button4.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            button3.Visible = false;
            groupBox2.Visible = true;
            button4.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ToDoList.BeginUpdate();
            checkedListBox1.BeginUpdate();

            List<object> list = new List<object>();

            foreach (var item in ToDoList.CheckedItems)
            {
                list.Add(item);
            }

            foreach (var item in list)
            {
                checkedListBox1.Items.Add(item, true);
                ToDoList.Items.Remove(item);
            }

            list.Clear();

            ToDoList.EndUpdate();
            checkedListBox1.EndUpdate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<object> list = new List<object>();
            checkedListBox1.BeginUpdate();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                list.Add(item);
            }

            foreach (var item in list)
            {
                checkedListBox1.Items.Remove(item);
            }
            checkedListBox1.EndUpdate();
        }
    }
}
