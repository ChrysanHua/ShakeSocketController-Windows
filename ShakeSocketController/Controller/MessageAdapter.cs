using ShakeSocketController.Handler;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller
{
    public static class MessageAdapter
    {
        public static MsgPacket PackDataBuf(byte[] dataBuf, int len)
        {
            try
            {
                return new MsgPacket(ByteUtil.SubByte(dataBuf, 0, len));
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            return null;
        }

        public static IMsgHandler CreateMsgHandler(MsgPacket packet)
        {
            IMsgHandler msgHandler = null;
            switch (packet.TypeStr)
            {
                case MsgPacket.TYPE_IP:
                    msgHandler = new IPMsgHandler(packet.DataStr);
                    break;
                case MsgPacket.TYPE_CMD:
                    msgHandler = new CmdMsgHandler(packet.DataStr);
                    break;
                case MsgPacket.TYPE_ANS:
                    msgHandler = new AnsMsgHandler(packet.DataStr);
                    break;
                case MsgPacket.TYPE_GUN:
                    msgHandler = new GunMsgHandler(packet.DataStr);
                    break;
            }
            return msgHandler;
        }

        public static IMsgHandler CreateAnsHandler(AnsData ansData)
        {
            try
            {
                return new AnsMsgHandler(StrUtil.ObjectToJson(ansData));
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            return null;
        }

    }
}
