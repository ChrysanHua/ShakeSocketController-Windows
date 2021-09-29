using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Model
{
    public class MsgPacket
    {
        public const int TYPE_LENGTH = 5;
        public const string TYPE_CMD = "[cmd]"; // <
        public const string TYPE_IP = "[ipp]";   // <
        public const string TYPE_ANS = "[ans]"; // <>
        public const string TYPE_GUN = "[gun]"; // <>

        //private DeviceInfo targetInfo;
        private readonly byte[] typeBuf;
        private readonly byte[] dataBuf;

        public string TypeStr => StrUtil.ByteToStr(typeBuf).Substring(0, TYPE_LENGTH);

        public string DataStr => StrUtil.ByteToStr(dataBuf);

        public byte[] MsgData => ByteUtil.ConcatByte(typeBuf, dataBuf);

        

        public MsgPacket(byte[] msgData)
        {
            typeBuf = ByteUtil.SplitByte(msgData, TYPE_LENGTH, out dataBuf);
        }

        public MsgPacket(byte[] typeBuf, byte[] dataBuf)
        {
            this.typeBuf = typeBuf;
            this.dataBuf = dataBuf;
        }

        public static MsgPacket Parse(string typeStr, string dataStr)
        {
            return new MsgPacket(ByteUtil.Resize(StrUtil.StrToByte(typeStr), TYPE_LENGTH),
                StrUtil.StrToByte(dataStr));
        }

    }
}
