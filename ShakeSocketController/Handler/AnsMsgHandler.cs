using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShakeSocketController.Controller;
using ShakeSocketController.Model;

namespace ShakeSocketController.Handler
{
    class AnsMsgHandler : MsgHandler
    {
        public AnsMsgHandler(string dataStr) : base(dataStr) { }

        public override void Handle(TransactionController controller, object state)
        {
            throw new NotImplementedException();
        }

        public override void Respond(TransactionController controller, object state)
        {
            controller.SendTCP(MsgPacket.Parse(MsgPacket.TYPE_ANS, dataStr).MsgData);
        }
    }
}
