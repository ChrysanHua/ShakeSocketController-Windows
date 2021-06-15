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


    }
}
