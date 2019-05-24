using ShakeSocketController.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Handler
{
    public interface IMsgHandler
    {
        void Handle(TransactionController controller, object state);

        void Respond(TransactionController controller, object state);
    }

    public abstract class MsgHandler : IMsgHandler
    {
        protected string dataStr;

        protected MsgHandler(string dataStr)
        {
            this.dataStr = dataStr;
        }

        public abstract void Handle(TransactionController controller, object state);

        public abstract void Respond(TransactionController controller, object state);
    }
}
