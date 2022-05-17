using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShakeSocketController.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Model.Tests
{
    [TestClass()]
    public class MsgPacketTests
    {
        [TestMethod()]
        public void BitToBoolTest()
        {
            byte b = 0xAA;  //‭1010 1010‬
            b += 1;
            Console.WriteLine(b.ToString("X2"));
            Console.WriteLine(Convert.ToString(b, 2));
            for (int i = 0; i < 8; i++)
            {
                Console.Write((b >> i) & 1);
                Console.Write(Convert.ToBoolean((b >> i) & 1) + " ");
            }
            Console.WriteLine();
        }

        [TestMethod()]
        public void GetHeaderNotesMaskTest()
        {
            byte b = 0xAC;  //‭1010 1100‬
            byte[] buf = { 0, 0, b, 0, 0, 0 };
            MsgPacket packet = new MsgPacket(buf);
            Console.WriteLine(b.ToString("X2"));
            Console.WriteLine(Convert.ToString(b, 2));
            for (int i = 1; i <= 8; i++)
            {
                Console.Write(packet.GetHeaderNotesMask(i));
            }
            Console.WriteLine();
        }

        [TestMethod()]
        public void SetHeaderNotesMaskTest()
        {
            byte b = 0xAC;  //‭1010 1100‬
            byte[] buf = { 0, 0, b, 0, 0, 0 };
            MsgPacket packet = new MsgPacket(buf);
            Console.WriteLine(b.ToString("X2"));
            Console.WriteLine(Convert.ToString(b, 2));
            for (int i = 1; i <= 8; i++)
            {
                Console.Write(packet.GetHeaderNotesMask(i) + " ");
                packet.SetHeaderNotesMask(true, i);
                //packet.SetHeaderNotesMask(false, i);
                Console.WriteLine(packet.GetHeaderNotesMask(i));
            }
            //注意源buf是不变的
            Console.WriteLine(Convert.ToString(buf[2], 2));
            //实际是有变化的
            Console.WriteLine(Convert.ToString(packet.headerBuf[2], 2));
        }

        [TestMethod()]
        public void PacketDataTypeTest()
        {
            Array array = Enum.GetValues(typeof(PacketDataType));
            foreach (var item in array)
            {
                Console.WriteLine($"{item}:{(int)item}");
            }
            Console.WriteLine();

            byte b = 0xAD;
            PacketDataType p = (PacketDataType)(b & 0x0F);
            Console.WriteLine(Enum.IsDefined(typeof(PacketDataType), p));
            byte[] buf = { 0, 0, 0, b, 0, 0 };
            MsgPacket packet = new MsgPacket(buf);
            Console.WriteLine(b.ToString("X2"));
            Console.WriteLine(Convert.ToString(b, 2));
            Console.WriteLine(packet.DataType);
            packet.DataType = PacketDataType.Int;
            Console.WriteLine(packet.headerBuf[3].ToString("X2"));
            Console.WriteLine(Convert.ToString(packet.headerBuf[3], 2));
            Console.WriteLine(packet.DataType);
        }

    }
}