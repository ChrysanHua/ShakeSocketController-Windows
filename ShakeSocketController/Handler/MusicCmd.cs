using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;

namespace ShakeSocketController.Handler
{
    class MusicCmd : IMsgHandler
    {
        private string paramStr;

        public MusicCmd(string paramStr)
        {
            this.paramStr = paramStr;
        }

        public void Handle(TransactionController controller, object state)
        {
            string exePath = null;
            if (paramStr == "play")
            {

            }
            else if (paramStr == "next")
            {

            }
            else if (paramStr == "front")
            {

            }
            if (exePath != null)
            {
                Process.Start(exePath);
                Respond(controller, state);
                Logging.Info("Handle 音乐控制");
            }
        }

        public void Respond(TransactionController controller, object state)
        {
            MessageAdapter.CreateAnsHandler(new AnsData()
            {
                AnsType = CmdMsgHandler.CMD_MUSIC,
                AnsStatus = true,
                AnsValue = paramStr,
                AnsMsg = "Play",
            })?.Respond(controller, state);
        }
    }
}
