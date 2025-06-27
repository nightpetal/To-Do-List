using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class UserInterface : Form
    {
        public string uName { get; set; }
        public string dbName = "todolist.db";
        public UserInterface(string UserName)
        {
            InitializeComponent();
            uName = UserName;
        }
        private void UserInterface_Load(object sender, EventArgs e)
        {
            LoadDB();
        }

        private void create_DB_TABLE()
        {
            if (!File.Exists(dbName))
            {
                SQLiteConnection.CreateFile(dbName);
                var items = ToDoList.Items.Add($"{uName} add new task here!");
                checkedListBox1.Items.Add("Completed task", true);
            }

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbName))
            {
                conn.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS UserData (username varchar(200), task varchar(255), completed Boolean)";
                SQLiteCommand createCmd = new SQLiteCommand(createTableQuery, conn);
                createCmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void LoadDB()
        {
            create_DB_TABLE();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbName))
            {
                conn.Open();

                string selectQuery = "SELECT task, completed FROM UserData WHERE username = @username";
                SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conn);
                selectCmd.Parameters.AddWithValue("@username", uName);

                using (SQLiteDataReader reader = selectCmd.ExecuteReader())
                {
                    ToDoList.Items.Clear();

                    bool hasRows = false;
                    while (reader.Read())
                    {
                        hasRows = true;
                        string task = reader["task"].ToString();
                        bool completed = Convert.ToBoolean(reader["completed"]);
                        if (!completed)
                        {
                            ToDoList.Items.Add(task);
                        }
                        else
                        {
                            checkedListBox1.Items.Add(task, true);
                        }
                    }

                    if (!hasRows)
                    {
                        MessageBox.Show("No saved file found");
                    }
                }

                conn.Close();
            }
        }


        private void Logout_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (var login = new LoginForm())
            {
                var result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.Show();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void AddTaskBtn_Click(object sender, EventArgs e)
        {
            create_DB_TABLE();

            if (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.TextLength <= 255)
            {
                ToDoList.Items.Add(textBox1.Text);

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbName))
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO UserData VALUES (@username, @task, @completed)";
                    SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@username", uName);
                    insertCmd.Parameters.AddWithValue("@task", textBox1.Text);
                    insertCmd.Parameters.AddWithValue("@completed", false);
                    insertCmd.ExecuteNonQuery();

                    conn.Close();
                }
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Blank Entry or Large Input", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbName))
                {
                    conn.Open();

                    string updateQuery = "UPDATE UserData SET completed = 1 WHERE username = @username AND task = @task";
                    SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@username", uName);
                    updateCmd.Parameters.AddWithValue("@task", item);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    var i = (rowsAffected > 0) ? DialogResult.None : MessageBox.Show("Task not found.");

                    conn.Close();
                }
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

            DialogResult confirm = MessageBox.Show($"Are you sure you want to delete task?",
                                           "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            foreach (var item in list)
            {
                checkedListBox1.Items.Remove(item);

                if (confirm == DialogResult.Yes)
                {
                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbName))
                    {
                        conn.Open();

                        string deleteQuery = "DELETE FROM UserData WHERE username = @username AND task = @task";
                        SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, conn);
                        deleteCmd.Parameters.AddWithValue("@username", uName);
                        deleteCmd.Parameters.AddWithValue("@task", item);

                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        var i = (rowsAffected > 0) ? DialogResult.None : MessageBox.Show("Task not found in database.");
                        conn.Close();
                    }
                }
            }
            checkedListBox1.EndUpdate();
        }
    }
}
