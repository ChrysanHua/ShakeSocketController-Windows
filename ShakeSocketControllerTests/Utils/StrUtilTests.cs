using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Utils.Tests
{
    [TestClass()]
    public class StrUtilTests
    {
        [TestMethod()]
        public void ObjectToJsonTest()
        {
            DeviceInfo info1 = new DeviceInfo("uuidTest", "MyDevice", "MyUser",
                IPAddress.Parse("192.168.15.16"));
            DeviceInfo info2 = new DeviceInfo("uuidTest", "MyDevice", "MyUser",
                IPAddress.Parse("192.168.15.16"));

            string json1 = StrUtil.ObjectToJson(info1);
            string json2 = StrUtil.ObjectToJson(info2);
            string json3 = StrUtil.ObjectToJson(info2, true, true);
            string json4 = StrUtil.ObjectToJson(info2, true, false);
            Console.WriteLine(json1);
            Console.WriteLine(json2);
            Console.WriteLine(json3);
            Console.WriteLine(json4);

            info2 = StrUtil.JsonToObject<DeviceInfo>(json1);

            Console.WriteLine(info2);
            Console.WriteLine("==: " + (info1 == info2));
            Console.WriteLine("equals: " + info1.Equals(info2));

            Assert.AreEqual(info1, info2);
        }

        [TestMethod()]
        public void JsonTest()
        {
            AppConfig config = null;
            Console.WriteLine(StrUtil.ObjectToJson(config));
            config = StrUtil.JsonToObject<AppConfig>("");
            Console.WriteLine("'':" + (config == null));
            config = StrUtil.JsonToObject<AppConfig>("null");
            Console.WriteLine("null:" + (config == null));

            config = StrUtil.JsonToObject<AppConfig>("{}");
            Console.WriteLine("{}:" + (config == null));
            Console.WriteLine(StrUtil.OverrideToString(config));

            config = StrUtil.JsonToObject<AppConfig>(" ");
            Console.WriteLine("' ':" + (config == null));
        }

        [TestMethod()]
        public void EqualsTest()
        {
            DeviceInfo info1 = new DeviceInfo("uuidTest", "MyDevice", "",
                IPAddress.Parse("192.168.15.16"));
            DeviceInfo info2 = new DeviceInfo("uuidTest", "MyDevice", null,
                IPAddress.Parse("192.168.99.16"));
            Console.WriteLine("==: " + (info1 == info2));
            Console.WriteLine("equals: " + info1.Equals(info2));
        }

        [TestMethod()]
        public void ConfigTest()
        {
            AppConfig config1 = AppConfig.GetDefaultConfig();
            string json = StrUtil.ObjectToJson(config1);
            Console.WriteLine(json);
            AppConfig config2 = StrUtil.JsonToObject<AppConfig>(json);
            Console.WriteLine(StrUtil.ObjectToJson(config2));
        }

        [TestMethod()]
        public void AddressTest()
        {
            Console.WriteLine(SysUtil.GetLocalIP());
        }
    }
}