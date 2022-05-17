using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Model
{
    /// <summary>
    /// 数据报文对象的数据类
    /// </summary>
    public class MsgPacket
    {
        /// <summary>
        /// 报文头数据的字节长度
        /// </summary>
        public const int HEADER_LENGTH = 6;
        public const string TYPE_CMD = "[cmd]"; // <
        public const string TYPE_IP = "[ipp]";   // <
        public const string TYPE_ANS = "[ans]"; // <>
        public const string TYPE_GUN = "[gun]"; // <>
        //private DeviceInfo targetInfo;

        /*
         * +----------+----------+----------------+------------------+----------+----------+
         * |                                    报文头                           | 数据单元 |
         * +----------+----------+----------------+------------------+----------+----------+
         * | 主功能码  | 次功能码  | 数据补充说明码  | 数据类型         | 数据长度  | 任意数据  |
         * +----------+----------+----------------+------------------+----------+----------+
         * | 1Byte    | 1Byte    | 1Byte          | 1Byte(仅用低4位)  | 2Byte    | nByte    |
         * +----------+----------+----------------+------------------+----------+----------+
         * | 0x01     | 0x00     | 0x09           | 0x01             | 0x1000   | …        |
         * +----------+----------+----------------+------------------+----------+----------+
         */


        /* 数据补充说明码：
         * +---------+------+----------+---+---+----------+------+------+------+
         * | 对应项  | 机位  | 保留备用  |   |   | 需要回复 | 应答  | 测试 | 冗余 |
         * +---------+------+----------+---+---+----------+------+------+------+
         * | Bit码位 | 8    | 7         | 6 | 5 | 4       | 3    | 2    | 1    |
         * +---------+------+----------+---+---+----------+------+------+------+
         */


        private readonly byte[] headerBuf;              //报文头
        private readonly byte[] dataBuf;                //数据单元

        /// <summary>
        /// 完整数据报文Buf
        /// </summary>
        public byte[] MsgData => ByteUtil.ConcatByte(headerBuf, dataBuf);

        /// <summary>
        /// 是否冗余重发报文
        /// </summary>
        public bool IsVerbose
        {
            get => GetHeaderNotesMask(1);
            set => SetHeaderNotesMask(value, 1);
        }
        /// <summary>
        /// 是否测试可用性报文
        /// </summary>
        public bool IsTestOP
        {
            get => GetHeaderNotesMask(2);
            set => SetHeaderNotesMask(value, 2);
        }
        /// <summary>
        /// 是否回复报文
        /// </summary>
        public bool IsRespond
        {
            get => GetHeaderNotesMask(3);
            set => SetHeaderNotesMask(value, 3);
        }
        /// <summary>
        /// 该报文是否需要得到回复
        /// </summary>
        public bool NeedRespond
        {
            get => GetHeaderNotesMask(4);
            set => SetHeaderNotesMask(value, 4);
        }
        /// <summary>
        /// 备用保留位的使能值
        /// </summary>
        public bool RemarksEnabled
        {
            get => GetHeaderNotesMask(7);
            set => SetHeaderNotesMask(value, 7);
        }
        /// <summary>
        /// 是否来源于上位机（即报文的来源机位）
        /// </summary>
        public bool IsUpperDevice
        {
            get => GetHeaderNotesMask(8);
            set => SetHeaderNotesMask(value, 8);
        }

        /// <summary>
        /// 数据单元的数据类型
        /// </summary>
        public PacketDataType DataType
        {
            get => Enum.IsDefined(typeof(PacketDataType), headerBuf[3] & 0x0F) ?
                (PacketDataType)(headerBuf[3] & 0x0F) : PacketDataType.Unknown;
            set => headerBuf[3] = Convert.ToByte((headerBuf[3] & 0xF0) | ((int)value));
        }

        /// <summary>
        /// 报文头中指示的数据单元字节长度
        /// </summary>
        public int DataLength
        {
            get => BitConverter.ToUInt16(headerBuf, 4);
            set
            {
                byte[] lenBuf = BitConverter.GetBytes(Convert.ToUInt16(value));
                headerBuf[4] = lenBuf[0];
                headerBuf[5] = lenBuf[1];
            }
        }


        public string TypeStr => StrUtil.ByteToStr(headerBuf).Substring(0, HEADER_LENGTH);
        public string DataStr => StrUtil.ByteToStr(dataBuf);


        public MsgPacket(byte[] msgData)
        {
            if (msgData == null)
                throw new ArgumentNullException(nameof(msgData));
            if (msgData.Length < 6)
                throw new ArgumentException("Array length is too short!", nameof(msgData));

            headerBuf = ByteUtil.SplitByte(msgData, HEADER_LENGTH, out dataBuf);
        }

        public MsgPacket(byte[] headerBuf, byte[] dataBuf)
        {
            if (headerBuf == null || dataBuf == null)
                throw new ArgumentNullException(headerBuf == null ?
                    nameof(dataBuf) : nameof(headerBuf));
            if (headerBuf.Length < 6)
                throw new ArgumentException("Array length is too short!", nameof(headerBuf));

            this.headerBuf = headerBuf;
            this.dataBuf = dataBuf;
        }

        public static MsgPacket Parse(string typeStr, string dataStr)
        {
            return new MsgPacket(ByteUtil.Resize(StrUtil.StrToByte(typeStr), HEADER_LENGTH),
                StrUtil.StrToByte(dataStr));
        }

        /// <summary>
        /// 获取报文头里数据补充说明码中指定码位的值
        /// </summary>
        /// <param name="bitLocation">要获取的从低位到高位的二进制码位（1~8）</param>
        /// <returns>返回指定二进制值码位对应的使能值</returns>
        public bool GetHeaderNotesMask(int bitLocation)
        {
            if (bitLocation < 1 || bitLocation > 8)
                throw new ArgumentOutOfRangeException(nameof(bitLocation));

            return Convert.ToBoolean((headerBuf[2] >> (bitLocation - 1)) & 1);
        }

        /// <summary>
        /// 设置报文头里数据补充说明码中指定码位的值
        /// </summary>
        /// <param name="enabled">要设置的使能值</param>
        /// <param name="bitLocation">要设置的从低位到高位的二进制码位（1~8）</param>
        /// <returns>返回是否实际上执行了设置操作</returns>
        public bool SetHeaderNotesMask(bool enabled, int bitLocation)
        {
            if (bitLocation < 1 || bitLocation > 8)
                throw new ArgumentOutOfRangeException(nameof(bitLocation));

            byte oldByte = headerBuf[2];
            bool oldVal = GetHeaderNotesMask(bitLocation);
            if (oldVal != enabled)
            {
                headerBuf[2] = Convert.ToByte(oldByte ^ (1 << (bitLocation - 1)));
                return true;
            }

            return false;
        }

    }
}
