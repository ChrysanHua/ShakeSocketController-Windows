using ShakeSocketController.Controller;
using ShakeSocketController.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShakeSocketController
{
    static class Program
    {
        public static TransactionController MainController { get; private set; }
        public static MenuViewController MenuController { get; private set; }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\ShakeSocketController"))
            {
                Application.ApplicationExit += Application_ApplicationExit;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("ShakeSocketController已启动！");
                    return;
                }

                Directory.SetCurrentDirectory(Application.StartupPath);
                Logging.Init();

                MainController = new TransactionController();
                MenuController = new MenuViewController(MainController);
                MainController.Start();

                Application.Run();
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Application.ApplicationExit -= Application_ApplicationExit;
            if (MainController != null)
            {
                MainController.Stop();
                MainController = null;
            }
        }
    }
}
