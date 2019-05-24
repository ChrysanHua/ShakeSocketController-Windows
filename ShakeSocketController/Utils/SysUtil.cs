using ShakeSocketController.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShakeSocketController.Utils
{
    public static class SysUtil
    {
        public static string GetTempPath()
        {
            return Application.StartupPath;
        }

        public static string GetTempPath(string fileName)
        {
            return Path.Combine(GetTempPath(), fileName);
        }

        public static string GetDeviceName()
        {
            return Environment.MachineName;
        }

        public static string GetWinUserName()
        {
            return Environment.UserName;
        }

        public static IPAddress GetLocalIP()
        {
            //IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            //foreach (var item in ips)
            //{
            //    if (!IPAddress.IsLoopback(item) && item.AddressFamily == AddressFamily.InterNetwork)
            //        Console.WriteLine(item);
            //}
            //Console.WriteLine(NetworkInterface.GetIsNetworkAvailable());

            //string localIP;
            //using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            //{
            //    socket.Connect("8.8.8.8", 12000);
            //    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            //    localIP = endPoint.Address.ToString();
            //}
            //Console.WriteLine(localIP);

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up
                    && (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                    || nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    IPInterfaceProperties ipProperties = nic.GetIPProperties();
                    if (ipProperties.GetIPv4Properties().IsDhcpEnabled)
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
        }


    }
}
