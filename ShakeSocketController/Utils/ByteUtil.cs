using System;

namespace ShakeSocketController.Utils
{
    public static class ByteUtil
    {
        public static byte[] IntToByte(int num)
        {
            return BitConverter.GetBytes(num);
        }

        public static int ByteToInt(byte[] numByte)
        {
            return BitConverter.ToInt32(numByte, 0);
        }

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

        public static byte[] Resize(byte[] dataByte, int len)
        {
            Array.Resize(ref dataByte, len);
            return dataByte;
        }

        public static void Reverse(byte[] dataByte)
        {
            Array.Reverse(dataByte);
        }

        /// <summary>
        /// Caution may cause unpredictable consequences!（慎用！）
        /// </summary>
        public static byte[] TrimByte(byte[] dataByte)
        {
            int front = -1, behind = -1;
            for (int i = 0; i < dataByte.Length; i++)
            {
                if (!dataByte[i].Equals(byte.MinValue))
                {
                    //not empty byte
                    if (front == -1)
                    {
                        front = i;
                        behind = front;
                    }
                    else
                    {
                        behind = i;
                    }
                }
            }

            front = (front == -1) ? 0 : front;  //empty buf
            return SubByte(dataByte, front, behind - front + 1);
        }

    }
}
