using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ShakeSocketController.Controller.Service
{
    class UDPBroadcaster
    {
        private const int BROADCAST_INTERVAL = 3000;

        private readonly IPEndPoint bcEP;
        private Socket broadcaster;
        private byte[] bcBuf;

        private readonly AutoResetEvent endAutoSignal;
        private readonly object bcStopLock = new object();
        private bool isBCStop = true;

        public bool IsBCStop
        {
            get { lock (bcStopLock) { return isBCStop; } }
            private set { lock (bcStopLock) { isBCStop = value; } }
        }


        public UDPBroadcaster(int port)
        {
            broadcaster = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp)
            {
                EnableBroadcast = true,
                MulticastLoopback = false,
                DontFragment = true
            };
            //socket must be bound with local IP to ensure effective broadcasting
            broadcaster.Bind(new IPEndPoint(Utils.SysUtil.GetLocalIP(), 0));
            bcEP = new IPEndPoint(IPAddress.Broadcast, port);
            endAutoSignal = new AutoResetEvent(false);
        }

        public void BeginBroadcast(byte[] msgData)
        {
            bcBuf = msgData;
            Reload();
        }

        public void EndBroadcast()
        {
            if (IsBCStop) return;
            //set the end signal
            endAutoSignal.Set();
        }

        public void Reload()
        {
            if (broadcaster == null) return;
            try
            {
                //stop broadcasting first
                EndBroadcast();

                while (endAutoSignal.WaitOne(0))
                {
                    //get the end signal, need to resend!
                    //be sure to wait for the old broadcast to stop
                    endAutoSignal.Set();
                }

                //flag start status
                IsBCStop = false;

                Logging.Debug(broadcaster.LocalEndPoint, bcEP, bcBuf.Length, "begin broadcast");
                //start broadcasting
                broadcaster.BeginSendTo(bcBuf, 0, bcBuf.Length, SocketFlags.None,
                    bcEP, new AsyncCallback(BroadcastCallback), bcBuf);
            }
            catch (Exception e)
            {
                Logging.Error(e);
                IsBCStop = true;
            }
        }

        public void Close()
        {
            if (broadcaster == null) return;
            try
            {
                EndBroadcast();
                endAutoSignal.Close();

                broadcaster.Shutdown(SocketShutdown.Both);
                broadcaster.Close();
                broadcaster.Dispose();
                broadcaster = null;
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            finally
            {
                IsBCStop = true;
            }
        }

        private void BroadcastCallback(IAsyncResult ar)
        {
            try
            {
                int len = broadcaster.EndSendTo(ar);
                byte[] oldBcBuf = (byte[])ar.AsyncState;
                if (len != oldBcBuf.Length)
                    throw new Exception("广播异常，操作系统未能正确地发送广播信息！");
                //wait a moment
                if (endAutoSignal.WaitOne(BROADCAST_INTERVAL))
                {
                    //get the endBC signal, stop broadcasting and flag stop status
                    IsBCStop = true;
                    Logging.Debug("end broadcast!");
                    return;
                }

                //there is no end signal, go ahead
                broadcaster?.BeginSendTo(oldBcBuf, 0, oldBcBuf.Length, SocketFlags.None,
                    bcEP, new AsyncCallback(BroadcastCallback), oldBcBuf);
            }
            catch (Exception e)
            {
                Logging.Error(e);
                IsBCStop = true;
            }
        }
    }
}
