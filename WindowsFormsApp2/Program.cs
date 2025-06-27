using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string userName = null;

            using (var login = new LoginForm())
            {
                userName = login.UserName;

                var result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    userName = login.UserName;
                }
                else
                {
                    return;
                }
            }

            Application.Run(new UserInterface(userName));

        }
    }
}
