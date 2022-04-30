using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;

namespace ShakeSocketController.Handler
{
    class ShutdownCmd : IMsgHandler
    {
        private int timeout = 0;
        private bool enable = false;
        private bool status = false;

        public ShutdownCmd(string paramStr)
        {
            if (int.TryParse(paramStr, out timeout))
                enable = true;
        }

        public void Handle(TransactionController controller, object state)
        {
            if (enable)
            {
                Respond(controller, state);
                Logging.Info("Handle 远程关机");
                //notice controller
                controller.Exit();
                //shutdown computer
                Process process = new Process();
                process.StartInfo.FileName = "shutdown.exe";
                string timeoutStr = timeout > 0 ? Convert.ToString(timeout) : null;
                process.StartInfo.Arguments = "-s -t " + timeoutStr;
                process.Start();
            }
        }

        public void Respond(TransactionController controller, object state)
        {
            MessageAdapter.CreateAnsHandler(new AnsData()
            {
                AnsType = CmdMsgHandler.CMD_SHUTDOWN,
                AnsStatus = status,
                AnsMsg = "电脑成功关机",
            })?.Respond(controller, state);
        }
    }
}
