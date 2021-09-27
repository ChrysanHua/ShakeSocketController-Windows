using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;

namespace ShakeSocketController.Utils
{
    /// <summary>
    /// 自定义系统工具类
    /// </summary>
    public static class SysUtil
    {
        /// <summary>
        /// 返回临时（文件/文件夹）的路径
        /// </summary>
        /// <param name="filename">文件名（为空则返回文件夹路径，默认为空）</param>
        /// <param name="useSysTemp">是否使用系统临时文件夹（默认使用可执行文件的所在路径）</param>
        /// <returns>绝对路径字符串</returns>
        public static string GetTempPath(string filename = null, bool useSysTemp = false)
        {
            return Path.Combine(useSysTemp ? Path.GetDirectoryName(Path.GetTempPath()) : Application.StartupPath
                , string.IsNullOrWhiteSpace(filename) ? string.Empty : filename);
        }

        /// <summary>
        /// 返回程序可执行文件的绝对路径
        /// </summary>
        /// <returns>绝对路径字符串</returns>
        public static string GetExecutablePath()
        {
            // Don't use Application.ExecutablePath
            // see https://stackoverflow.com/questions/12945805/odd-c-sharp-path-issue
            return Assembly.GetEntryAssembly().Location;
        }

        /// <summary>
        /// 返回当前计算机的NetBIOS名称
        /// </summary>
        /// <returns>计算机名字符串</returns>
        public static string GetDeviceName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// 返回当前登录Windows的用户名
        /// </summary>
        /// <returns>用户名字符串</returns>
        public static string GetWinUserName()
        {
            return Environment.UserName;
        }

        /// <summary>
        /// 返回当前程序集标识的版本号
        /// </summary>
        /// <param name="fieldCount">指示要返回的版本号部分数（默认2，例如1.0）</param>
        /// <returns>版本号字符串</returns>
        public static string GetVersionStr(int fieldCount = 2)
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString(fieldCount);
        }

        /// <summary>
        /// 返回当前程序集标识的标题名
        /// </summary>
        /// <returns>标题名字符串</returns>
        public static string GetAssemblyTitle()
        {
            object[] attr = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attr.Length > 0)
            {
                AssemblyTitleAttribute title = (AssemblyTitleAttribute)attr[0];
                if (!string.IsNullOrWhiteSpace(title.Title))
                    return title.Title;
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回当前网络连接的本地IP地址
        /// </summary>
        /// <returns>本地IP地址</returns>
        public static IPAddress GetLocalIP()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                //because it is udp socket, it will perform an invalid connect operation
                socket.Connect("8.8.8.8", 12000);
                //but we can still get the IP from it
                return (socket.LocalEndPoint as IPEndPoint).Address;
            }

            /******the second effective method, but it's inefficient******
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up
                    && (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    || nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    IPInterfaceProperties ipProperties = nic.GetIPProperties();
                    if (ipProperties.GetIPv4Properties().IsDhcpEnabled
                        && ipProperties.GatewayAddresses.Count >= 1)
                    {
                        foreach (UnicastIPAddressInformation ipInfo in ipProperties.UnicastAddresses)
                        {
                            if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                //If there are more than one IP, return first
                                return ipInfo.Address;
                            }
                        }
                    }
                }
            }
            return null;
            */


            /******invalid code, just a usage note******
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var item in ips)
            {
                if (!IPAddress.IsLoopback(item) && item.AddressFamily == AddressFamily.InterNetwork)
                    Console.WriteLine(item);
            }
            Console.WriteLine(NetworkInterface.GetIsNetworkAvailable());
            */
        }

        /// <summary>
        /// 打开指定的注册表节点
        /// </summary>
        /// <param name="name">注册表节点路径</param>
        /// <param name="writable">是否需要编辑该注册表</param>
        /// <param name="hive">注册表顶级节点类型</param>
        /// <returns>打开后的注册表对象</returns>
        public static RegistryKey OpenRegKey(string name, bool writable,
            RegistryHive hive = RegistryHive.CurrentUser)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            try
            {
                RegistryKey userKey = RegistryKey.OpenBaseKey(hive,
                        Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32)
                    .OpenSubKey(name, writable);
                return userKey;
            }
            catch
            {
                return null;
            }
        }

    }
}
