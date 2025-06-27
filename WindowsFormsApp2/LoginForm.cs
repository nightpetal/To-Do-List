using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class LoginForm : Form
    {
        public string UserName { get; private set; }
        public LoginForm()
        {
            InitializeComponent();
            textBox1.Text = "nightpetal";
            textBox2.Text = "nightpetal";
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;

            if (userName == "nightpetal" && password == "nightpetal")
            {
                UserName = userName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
        }
    }
}
