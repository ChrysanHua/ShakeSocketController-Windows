using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using ShakeSocketController.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        /// 任务栏图标菜单控制器
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
            if (!SingleStartup.Register($"Global\\{appTitle}_{SysUtil.GetAssemblyGUID()}"))
            {
                //the app has been started
                MessageBox.Show($"{appTitle}已启动！", appTitle);
                return;
            }

            //unified the running path
            Directory.SetCurrentDirectory(Application.StartupPath);

            //register the log file
            if (!Logging.Init($"{appTitle}.log"))
            {
                MessageBox.Show("日志模块初始化失败！", appTitle);
                return;
            }

            //使跨线程操作UI报错，用来Debug
            Control.CheckForIllegalCrossThreadCalls = true;

            //initialize and start
            MainController = new TransactionController();
            MenuController = new MenuViewController(MainController);
            MainController.Run();

            Application.Run();
        }

        //测试用的方法
        public static List<DeviceInfo> CreateTestInfoList(int count)
        {
            List<DeviceInfo> list = new List<DeviceInfo>(count);
            for (int i = 1; i <= count; i++)
            {
                list.Add(new DeviceInfo($"UUID{i}", $"Device{i}", $"User{i}",
                    IPAddress.Parse($"192.168.9.{i}"))
                {
                    IsConnected = i == 2,
                });
            }

            return list;
        }

        /// <summary>
        /// 应用程序的退出事件
        /// </summary>
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //in any case, unsubscribe the static event handlers first
            Application.ApplicationExit -= Application_ApplicationExit;
            //unregister single instance startup
            SingleStartup.Unregister();
            //stop the controller (if need)
            if (MainController != null)
            {
                MainController.Exit();
                MainController = null;
                Logging.Info("ShakeSocketController stopped.");
                Logging.SplitLine();
            }
        }
    }
}
