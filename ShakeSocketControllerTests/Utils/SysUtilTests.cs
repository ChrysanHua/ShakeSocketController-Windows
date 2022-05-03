using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Utils.Tests
{
    [TestClass()]
    public class SysUtilTests
    {
        [TestMethod()]
        public void GetVersionStrTest()
        {
            Console.WriteLine(SysUtil.GetVersionStr(4));
        }

        [TestMethod()]
        public void GuidTest()
        {
            Guid guid = Guid.NewGuid();
            Console.WriteLine(guid.ToString());
            Console.WriteLine(guid.ToString("D"));
            Console.WriteLine(guid.ToString("N"));
            Console.WriteLine(guid.ToString("B"));
            Console.WriteLine(guid.ToString("P"));
            Console.WriteLine(guid.ToString("X"));
            Console.WriteLine();
            Console.WriteLine(SysUtil.GetAssemblyGUID());
        }

        [TestMethod()]
        public void LocalInfoTest()
        {
            AppConfig config = AppConfig.GetDefaultConfig();
            string json = StrUtil.ObjectToJson(config);
            LocalInfo localInfo = config.GetLocalInfo();
            string bcJson = localInfo.BCJson;
            string connJson1 = localInfo.GetIntactJson(SysUtil.GenerateUUID());
            string connJson2 = localInfo.GetIntactJson(SysUtil.GenerateUUID());

            Console.WriteLine(json);
            Console.WriteLine(bcJson);
            Console.WriteLine(connJson1);
            Console.WriteLine(connJson2);
        }

        [TestMethod()]
        public void AppConfigFileTest()
        {
            AppConfig config = AppConfig.Load();
            Console.WriteLine(StrUtil.OverrideToString(config));
            AppConfig.Save(null);
            Console.WriteLine(StrUtil.OverrideToString(config));
        }
    }
}