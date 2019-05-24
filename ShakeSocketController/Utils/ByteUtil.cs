using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Utils
{
    public static class ByteUtil
    {
        public static byte[] ConcatByte(byte[] frontByte, byte[] laterByte)
        {
            byte[] allByte = new byte[frontByte.Length + laterByte.Length];
            frontByte.CopyTo(allByte, 0);
            laterByte.CopyTo(allByte, frontByte.Length);
            return allByte;
        }

        public static byte[] SubByte(byte[] dataByte, int startIndex, int count)
        {
            byte[] partByte = new byte[count];
            Array.Copy(dataByte, startIndex, partByte, 0, count);
            return partByte;
        }

        public static byte[] SplitByte(byte[] dataByte, int splitPoint, out byte[] laterByte)
        {
            //return the front part
            laterByte = SubByte(dataByte, splitPoint, dataByte.Length - splitPoint);
            return SubByte(dataByte, 0, splitPoint);
        }

        public static void ClearByte(byte[] dataByte)
        {
            Array.Clear(dataByte, 0, dataByte.Length);
        }

        public static byte[] FixLen(byte[] dataByte, int len)
        {
            byte[] result = new byte[len];
            Array.Copy(dataByte, 0, result, 0,
                dataByte.Length > len ? len : dataByte.Length);
            return result;
        }

    }
}
