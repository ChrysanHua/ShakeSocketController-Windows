using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;

namespace ShakeSocketController.Handler
{
    class CmdMsgHandler : MsgHandler
    {
        public const string CMD_SHUTDOWN = "shutdown";
        public const string CMD_SCREENLOCK = "screenlock";
        public const string CMD_PPT = "ppt";
        public const string CMD_MUSIC = "music";
        public const string CMD_VOLUME = "volume";

        public CmdMsgHandler(string dataStr) : base(dataStr) { }

        public override void Handle(TransactionController controller, object state)
        {
            try
            {
                IMsgHandler msgHandler = CreateCmdHandler(
                    StrUtil.JsonToObject<CmdData>(dataStr));
                msgHandler.Handle(controller, state);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public override void Respond(TransactionController controller, object state)
        {
            throw new NotImplementedException();
        }

        private IMsgHandler CreateCmdHandler(CmdData cmdData)
        {
            IMsgHandler msgHandler = null;
            switch (cmdData.CmdType)
            {
                case CMD_SHUTDOWN:
                    msgHandler = new ShutdownCmd(cmdData.ParamStr);
                    break;
                case CMD_SCREENLOCK:
                    msgHandler = new ScreenLockCmd(cmdData.ParamStr);
                    break;
                case CMD_PPT:
                    msgHandler = new PptCmd(cmdData.ParamStr);
                    break;
                case CMD_MUSIC:
                    msgHandler = new MusicCmd(cmdData.ParamStr);
                    break;
                case CMD_VOLUME:
                    msgHandler = new VolumeCmd(cmdData.ParamStr);
                    break;
            }
            return msgHandler;
        }
    }
}
