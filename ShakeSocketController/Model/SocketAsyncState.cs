using System.Net;
using System.Net.Sockets;

namespace ShakeSocketController.Model
{
    public class SocketAsyncState
    {
        public Socket WorkSocket;
        public byte[] DataBuffer;
        public EndPoint RemoteEP;

        public SocketAsyncState(Socket workSocket, byte[] dataBuf, EndPoint remoteEP)
        {
            this.WorkSocket = workSocket;
            this.DataBuffer = dataBuf;
            this.RemoteEP = remoteEP;
        }

    }
}
