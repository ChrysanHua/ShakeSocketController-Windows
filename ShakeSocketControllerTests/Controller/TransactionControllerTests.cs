using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller.Tests
{
    [TestClass()]
    public class TransactionControllerTests
    {
        [TestMethod()]
        public void TransactionControllerTest()
        {
            IPAddress address1 = SysUtil.GetLocalIP();
            IPAddress address2 = IPAddress.Parse("192.168.18.1");

            SynchronizedCollection<DeviceInfo> deviceCol = new SynchronizedCollection<DeviceInfo>();
            deviceCol.Add(new DeviceInfo() { IP = address1 });
            List<DeviceInfo> deviceList = deviceCol.ToList();
            ReadOnlyCollection<DeviceInfo> readOnlyCol = deviceCol.ToList().AsReadOnly();

            deviceCol.Add(new DeviceInfo("UUID", "device", "user", address2));
            deviceList.Add(new DeviceInfo());
            Console.WriteLine("deviceCol:" + deviceCol.Count);
            Console.WriteLine("readOnlyCol:" + readOnlyCol.Count);
            Console.WriteLine("list:" + deviceList.Count);

            deviceList[0].ShakeMark = "ABCF";
            Console.WriteLine("==: " + (deviceList[0] == deviceCol[0]));
            Console.WriteLine("readOnlyCol:" + readOnlyCol[0].ShakeMark);
            Console.WriteLine("deviceCol:" + deviceCol[0].ShakeMark);
        }

    }
}