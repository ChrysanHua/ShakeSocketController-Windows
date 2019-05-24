using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;

namespace ShakeSocketController.Handler
{
    class IPMsgHandler : MsgHandler
    {
        public IPMsgHandler(string dataStr) : base(dataStr) { }

        public override void Handle(TransactionController controller, object state)
        {
            try
            {
                IPAddress targetIP = state as IPAddress;
                if (targetIP == null) return;
                DeviceInfo targetInfo = StrUtil.JsonToObject<DeviceInfo>(dataStr);
                if (!targetInfo.IsComplete()) return;
                controller.CheckInDevice(targetIP, targetInfo);
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
    }
}
