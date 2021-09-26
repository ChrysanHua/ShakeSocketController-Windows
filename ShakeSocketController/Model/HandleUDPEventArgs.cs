using System;
using System.Net;

namespace ShakeSocketController.Model
{
    public class HandleUDPEventArgs : EventArgs
    {
        public byte[] DataBuf { get; }
        public int Length { get; }
        public IPEndPoint IPE { get; }

        public HandleUDPEventArgs(byte[] dataBuf, EndPoint endPoint, int len = -1)
        {
            this.DataBuf = dataBuf;
            this.IPE = endPoint as IPEndPoint;
            this.Length = (len == -1) ? dataBuf.Length : len;
        }
    }
}
