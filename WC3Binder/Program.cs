using System;
using System.Threading;
using System.Windows.Forms;

namespace WC3Binder
{
    static class Program
    {
        private static Mutex m_instance;
        private const string m_appName = "NameOfMyApp";
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool tryCreateNewApp;
            m_instance = new Mutex(true, m_appName,
                    out tryCreateNewApp);
            if (tryCreateNewApp)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
