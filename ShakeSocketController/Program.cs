using ShakeSocketController.Controller;
using ShakeSocketController.Utils;
using ShakeSocketController.Views;
using System;
using System.IO;
using System.Windows.Forms;

namespace ShakeSocketController
{
    static class Program
    {
        /// <summary>
        /// 全局控制器
        /// </summary>
        public static TransactionController MainController { get; private set; }
        /// <summary>
        /// 任务栏菜单控制器
        /// </summary>
        public static MenuViewController MenuController { get; private set; }


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //first, subscribe to AppExitEvent handling
            Application.ApplicationExit += Application_ApplicationExit;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string appTitle = SysUtil.GetAssemblyTitle();
            //register single instance startup
            if (!SingleStartup.Register($"Global\\{appTitle}_{SysUtil.GetAssemblyGUID(8)}"))
            {
                //the app has been started
                MessageBox.Show($"{appTitle}已启动！");
                return;
            }

            //unified the running path
            Directory.SetCurrentDirectory(Application.StartupPath);

            //register the log file
            if (!Logging.Init($"{appTitle}.log"))
            {
                MessageBox.Show("日志模块初始化失败！");
                return;
            }

            //initialize and start
            MainController = new TransactionController();
            MenuController = new MenuViewController(MainController);
            MainController.Run();

            Application.Run();
        }

        /// <summary>
        /// 应用程序的退出事件
        /// </summary>
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //in any case, unsubscribe first
            Application.ApplicationExit -= Application_ApplicationExit;
            //unregister single instance startup
            SingleStartup.Unregister();
            //stop the controller (if need)
            if (MainController != null)
            {
                MainController.Exit();
                MainController = null;
            }
        }
    }
}
