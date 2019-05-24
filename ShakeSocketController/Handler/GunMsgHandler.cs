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
    class GunMsgHandler : MsgHandler
    {
        public GunMsgHandler(string dataStr) : base(dataStr) { }

        public override void Handle(TransactionController controller, object state)
        {
            try
            {
                IPAddress targetIP = state as IPAddress;
                if (targetIP == null) return;
                DeviceInfo targetInfo = StrUtil.JsonToObject<DeviceInfo>(dataStr);
                if (!targetInfo.IsComplete()) return;
                //if (!controller.ShouldHandleMsg(targetIP, targetInfo)) return;
                controller.StartTCPHandler(targetInfo.IP);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public override void Respond(TransactionController controller, object state)
        {
            IPEndPoint targetEP = state as IPEndPoint;
            if (targetEP == null) return;
            controller.SendUDP(MsgPacket.Parse(MsgPacket.TYPE_GUN, dataStr).MsgData, targetEP);
        }
    }
}
