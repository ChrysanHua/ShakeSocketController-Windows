using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller.Tests
{
    [TestClass()]
    public class MessageAdapterTests
    {
        [TestMethod()]
        public void BufferTest()
        {
            // TODO: 考虑给ByteUtil换成使用Buffer类
            byte[] buf = { 0, 0, 0, 0, 0, 0 };
            Buffer.SetByte(buf, 2, 0xAA);
            byte[] dst = new byte[3];
            Buffer.BlockCopy(buf, 0, dst, 0, 3);
            dst[0] = 0x01;
            Buffer.SetByte(dst, 1, 0x02);
            for (int i = 0; i < Buffer.ByteLength(buf); i++)
            {
                Console.Write(Buffer.GetByte(buf, i) + " ");
            }
            Console.WriteLine();
            Console.WriteLine($"buf:{StrUtil.ByteToHexStr(buf)}");
            Console.WriteLine($"dst:{StrUtil.ByteToHexStr(dst)}");
        }

        [TestMethod()]
        public void OtherTest()
        {

        }

        [TestMethod()]
        public void VerifyVerboseTest()
        {
            MessageAdapter.lastRecDateTime = DateTime.Now;
            MessageAdapter.lastRecPacket = new MsgPacket(new byte[] { 0, 0, 0, 0, 0, 0 });
            byte[] buf = { 0, 0, 1, 0, 0, 0 };
            MsgPacket packet = new MsgPacket(buf);
            Assert.IsTrue(MessageAdapter.VerifyVerbose(packet));
        }
    }
}