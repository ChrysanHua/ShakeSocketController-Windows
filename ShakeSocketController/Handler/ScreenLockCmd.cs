using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;

namespace ShakeSocketController.Handler
{
    class ScreenLockCmd : IMsgHandler
    {
        private bool enable = false;
        private bool status = false;

        public ScreenLockCmd(string paramStr)
        {
            if (!string.IsNullOrEmpty(paramStr))
                enable = true;
        }

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        public void Handle(TransactionController controller, object state)
        {
            if (enable)
            {
                //lock screen and send Ans back
                status = LockWorkStation();
                Respond(controller, state);
            }
            Logging.Info("Handle 远程锁屏");
        }

        public void Respond(TransactionController controller, object state)
        {
            MessageAdapter.CreateAnsHandler(new AnsData()
            {
                AnsType = CmdMsgHandler.CMD_SCREENLOCK,
                AnsStatus = status,
                AnsMsg = "电脑成功锁屏",
            })?.Respond(controller, state);
        }
    }
}
