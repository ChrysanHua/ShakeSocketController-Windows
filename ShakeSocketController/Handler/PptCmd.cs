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
    class PptCmd : IMsgHandler
    {
        private string paramStr;

        public PptCmd(string paramStr)
        {
            this.paramStr = paramStr;
        }

        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private void ClickOneNext()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void ClickOneFront()
        {
            //...
        }

        public void Handle(TransactionController controller, object state)
        {
            if (paramStr == "down")
            {
                ClickOneNext();
                Respond(controller, state);
            }
            Logging.Info("Handle PPT助手");
        }

        public void Respond(TransactionController controller, object state)
        {
            MessageAdapter.CreateAnsHandler(new AnsData()
            {
                AnsType = CmdMsgHandler.CMD_PPT,
                AnsStatus = true,
                AnsValue = paramStr,
                AnsMsg  = "下翻页",
            })?.Respond(controller, state);
        }
    }
}
