using System;
using System.Net;
using System.Net.Sockets;

namespace ShakeSocketController.Model
{
    public class AbnormalDataEventArgs : EventArgs
    {
        public IPEndPoint IPE { get; }
        public SocketError SocketErr { get; }

        public AbnormalDataEventArgs(EndPoint endPoint,
            SocketError socketErr = SocketError.SocketError)
        {
            this.IPE = endPoint as IPEndPoint;
            this.SocketErr = socketErr;
        }
    }
}
