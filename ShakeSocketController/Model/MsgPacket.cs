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
        // TODO: 移除这些旧代码
        public const string TYPE_CMD = "[cmd]"; // <
        public const string TYPE_IP = "[ipp]";   // <
        public const string TYPE_ANS = "[ans]"; // <>
        public const string TYPE_GUN = "[gun]"; // <>

        /*
         * +----------+----------+----------------+------------------+----------+----------+
         * |                                    报文头                           | 数据单元 |
         * +----------+----------+----------------+------------------+----------+----------+
         * | 主功能码  | 次功能码  | 数据补充说明码  | 数据类型         | 数据长度  | 任意数据  |
         * +----------+----------+----------------+------------------+----------+----------+
         * | 1Byte    | 1Byte    | 1Byte          | 1Byte(仅用低4位)  | 2Byte    | nByte    |
         * +----------+----------+----------------+------------------+----------+----------+
         * | 0x01     | 0x00     | 0x01           | 0x01             | 0x1000   | …        |
         * +----------+----------+----------------+------------------+----------+----------+
         */


        /* 数据补充说明码：
         * +---------+------+----------+---+----------+----------+------+------+------+
         * | 对应项  | 机位  | 保留备用  |   | 优先回复 | 需要回复  | 应答  | 测试 | 冗余 |
         * +---------+------+----------+---+----------+----------+------+------+------+
         * | Bit码位 | 8    | 7         | 6 | 5       | 4        | 3     | 2    | 1   |
         * +---------+------+----------+---+----------+----------+------+------+------+
         */


        private readonly byte[] headerBuf;              //报文头
        private readonly byte[] dataBuf;                //数据单元

        /// <summary>
        /// 完整数据报文Buf
        /// </summary>
        public byte[] MsgData => ByteUtil.ConcatByte(headerBuf, dataBuf);

        /// <summary>
        /// 主功能码
        /// </summary>
        public byte MainFunCode => headerBuf[0];

        /// <summary>
        /// 次功能码
        /// </summary>
        public byte SubFunCode => headerBuf[1];

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
        /// 该报文是否需要先回复然后再执行具体操作
        /// </summary>
        public bool RespondFirst
        {
            get => GetHeaderNotesMask(5);
            set => SetHeaderNotesMask(value, 5);
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

        /// <summary>
        /// 报文头的十六进制易读字符串
        /// </summary>
        public string HeaderHexStr => StrUtil.ByteToHexStr(headerBuf, true);

        /// <summary>
        /// 数据单元的十六进制字符串
        /// </summary>
        public string DataHexStr => StrUtil.ByteToHexStr(dataBuf);


        public MsgPacket(byte[] msgData)
        {
            if (msgData == null)
                throw new ArgumentNullException(nameof(msgData));
            if (msgData.Length < HEADER_LENGTH)
                throw new ArgumentException("Buffer length is too short!", nameof(msgData));

            this.headerBuf = ByteUtil.SplitByte(msgData, HEADER_LENGTH, out this.dataBuf);

            if (this.DataLength != this.dataBuf.Length)
                throw new ArgumentException("Data buffer length mismatch!", nameof(msgData));
        }

        public MsgPacket(byte[] headerBuf, byte[] dataBuf)
        {
            if (headerBuf == null || dataBuf == null)
                throw new ArgumentNullException(headerBuf == null ?
                    nameof(dataBuf) : nameof(headerBuf));
            if (headerBuf.Length < HEADER_LENGTH)
                throw new ArgumentException("Buffer length is too short!", nameof(headerBuf));

            this.headerBuf = headerBuf;
            this.dataBuf = dataBuf;

            if (this.DataLength != this.dataBuf.Length)
                throw new ArgumentException("Data buffer length mismatch!", nameof(dataBuf));
        }

        public MsgPacket(byte mainFunCode, byte subFunCode, PacketDataType dataType, byte[] dataBuf)
        {
            if (dataBuf == null)
                throw new ArgumentNullException(nameof(dataBuf));

            byte[] lenBuf = BitConverter.GetBytes(Convert.ToUInt16(dataBuf.Length));
            this.headerBuf = new byte[] { mainFunCode, subFunCode, 0,
                (byte)dataType, lenBuf[0], lenBuf[1] };
            this.dataBuf = dataBuf;
        }

        public MsgPacket(byte mainFunCode, byte subFunCode)
            : this(mainFunCode, subFunCode, PacketDataType.None, new byte[0]) { }


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

        /// <summary>
        /// 判断与指定的数据报文对象是否使用相同的（主、次）功能码
        /// </summary>
        public bool FunCodeEquals(MsgPacket packet)
        {
            if (packet != null && MainFunCode.Equals(packet.MainFunCode)
                && SubFunCode.Equals(packet.SubFunCode))
                return true;

            return false;
        }

        /// <summary>
        /// 判断与指定的数据报文对象是否包含相似的报文头，即除了数据补充说明码以外报文头其余部分完全相同
        /// </summary>
        public bool WithSameHeader(MsgPacket packet)
        {
            for (int i = 0; i < HEADER_LENGTH; i++)
            {
                if (i != 2)
                {
                    if (!this.headerBuf[i].Equals(packet?.headerBuf[i]))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断与指定的数据报文对象是否为冗余意义上的相同
        /// </summary>
        public bool VerboseEquals(MsgPacket packet)
        {
            //除数据补充说明码中的冗余位以外，其余部分完全相同
            if (WithSameHeader(packet) && this.dataBuf.SequenceEqual(packet.dataBuf)
                && ((this.headerBuf[2] ^ packet.headerBuf[2]) & 0xFE) == 0)
                return true;

            return false;
        }

    }
}
