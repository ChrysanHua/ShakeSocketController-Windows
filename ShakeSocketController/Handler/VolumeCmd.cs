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
    class VolumeCmd : IMsgHandler
    {
        private int value;
        private bool enable = false;
        private bool status = false;

        public VolumeCmd(string paramStr)
        {
            if (int.TryParse(paramStr,out value))
            {
                enable = true;
                if (value > 100) value = 100;
                if (value < 0) value = 0;
            }
        }

        public void Handle(TransactionController controller, object state)
        {
            if (enable)
            {
                SystemVolume.SetMasterVolume(value);
                if (value == (int)SystemVolume.GetMasterVolume())
                    status = true;
                Respond(controller, state);
            }
            Logging.Info("Handle 音量调节");
        }

        public void Respond(TransactionController controller, object state)
        {
            MessageAdapter.CreateAnsHandler(new AnsData()
            {
                AnsType = CmdMsgHandler.CMD_VOLUME,
                AnsStatus = status,
                AnsValue = Convert.ToString((int)SystemVolume.GetMasterVolume()),
                AnsMsg = "音量调节成功",
            })?.Respond(controller, state);
        }
    }
}
