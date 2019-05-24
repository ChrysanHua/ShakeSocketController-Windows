using ShakeSocketController.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller.Service
{
    class UDPBroadcaster
    {
        private const int BROADCAST_INTERVAL = 3000;

        private readonly IPEndPoint bcEP;
        private Socket broadcaster;
        private byte[] bcBuf;

        private readonly object bcStopLock = new object();
        private bool isBCStop = true;

        public UDPBroadcaster(int port)
        {
            broadcaster = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp)
            {
                EnableBroadcast = true
            };
            bcEP = new IPEndPoint(IPAddress.Broadcast, port);
            broadcaster.Bind(new IPEndPoint(Utils.SysUtil.GetLocalIP(), 0));
        }

        public bool GetIsBCStop()
        {
            lock (bcStopLock)
            {
                return isBCStop;
            }
        }

        public void BeginBroadcast(byte[] msgData)
        {
            bcBuf = msgData;
            Reload();
        }

        public void EndBroadcast()
        {
            lock (bcStopLock)
            {
                if (isBCStop) return;
                isBCStop = true;
            }
            Logging.Debug("end broadcast!");
        }

        public void Reload()
        {
            if (broadcaster == null) return;
            lock (bcStopLock)
            {
                if (!isBCStop) return;
                isBCStop = false;
            }
            try
            {
                Logging.Debug(broadcaster.LocalEndPoint, bcEP, bcBuf.Length, "begin broadcast");
                broadcaster.BeginSendTo(bcBuf, 0, bcBuf.Length, SocketFlags.None,
                    bcEP, new AsyncCallback(BroadcastCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public void Close()
        {
            if (broadcaster == null) return;
            EndBroadcast();
            try
            {
                broadcaster.Shutdown(SocketShutdown.Both);
                broadcaster.Close();
                broadcaster.Dispose();
                broadcaster = null;
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        private void BroadcastCallback(IAsyncResult ar)
        {
            try
            {
                int len = broadcaster.EndSendTo(ar);
                lock (bcStopLock)
                {
                    if (isBCStop) return;
                }
                if (len == bcBuf.Length)
                {
                    //wait a moment
                    Thread.Sleep(BROADCAST_INTERVAL);
                }
                broadcaster?.BeginSendTo(bcBuf, 0, bcBuf.Length, SocketFlags.None,
                    bcEP, new AsyncCallback(BroadcastCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }
    }
}
