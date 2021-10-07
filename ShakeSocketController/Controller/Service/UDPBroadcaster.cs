using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ShakeSocketController.Controller.Service
{
    public class UDPBroadcaster
    {
        private readonly int BROADCAST_INTERVAL;
        private readonly IPEndPoint bcEP;
        private Socket broadcaster;
        private byte[] bcBuf;

        private readonly ManualResetEvent endManualSignal;
        private readonly AutoResetEvent reloadAutoSignal;
        private readonly object bcStopLock = new object();
        private bool isBCStop = true;

        public event EventHandler BroadcasterException;
        public event EventHandler OnceBCSucceed;

        public bool IsBCStop
        {
            get { lock (bcStopLock) { return isBCStop; } }
            private set { lock (bcStopLock) { isBCStop = value; } }
        }


        public UDPBroadcaster(int bcPort, int bcInterval = 3000)
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
            bcEP = new IPEndPoint(IPAddress.Broadcast, bcPort);
            this.BROADCAST_INTERVAL = bcInterval;
            endManualSignal = new ManualResetEvent(false);
            reloadAutoSignal = new AutoResetEvent(false);
        }

        public void BeginBroadcast(byte[] msgDataBuf)
        {
            bcBuf = msgDataBuf;
            Reload();
        }

        public void EndBroadcast()
        {
            if (IsBCStop) return;
            //set as end signal
            endManualSignal.Set();
            //reset the reload signal(if it has)
            reloadAutoSignal.Reset();
        }

        public void Reload()
        {
            if (broadcaster == null)
                throw new InvalidOperationException("UDPBroadcaster has been closed!");
            if (bcBuf == null)
                throw new ArgumentNullException(nameof(bcBuf));

            try
            {
                if (IsBCStop)
                {
                    //flag start status
                    IsBCStop = false;
                    //set as BC signal
                    endManualSignal.Reset();
                    Logging.Debug(broadcaster.LocalEndPoint, bcEP, bcBuf.Length, "begin broadcast");
                    //start broadcasting
                    broadcaster.BeginSendTo(bcBuf, 0, bcBuf.Length, SocketFlags.None,
                        bcEP, new AsyncCallback(BroadcastCallback), bcBuf);
                }
                else if (!endManualSignal.WaitOne(0))
                {
                    //the BC is working, set as reload signal
                    reloadAutoSignal.Set();
                }
            }
            catch (Exception e)
            {
                Logging.Error(e);
                IsBCStop = true;
                BroadcasterException?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Close()
        {
            if (broadcaster == null) return;
            try
            {
                EndBroadcast();
                endManualSignal.Close();
                reloadAutoSignal.Close();

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
            if (broadcaster == null) return;

            try
            {
                int len = broadcaster.EndSendTo(ar);
                byte[] oldBcBuf = (byte[])ar.AsyncState;
                if (len != oldBcBuf.Length)
                    throw new Exception($"广播异常，操作系统未能正确地发送广播信息！ " +
                        $"(A/P len={len}/{oldBcBuf.Length})");

                OnceBCSucceed?.Invoke(this, EventArgs.Empty);

                //wait a moment
                if (endManualSignal.WaitOne(BROADCAST_INTERVAL))
                {
                    //get the endBC signal, stop broadcasting and flag stop status
                    IsBCStop = true;
                    Logging.Debug("end broadcast!");
                }
                else
                {
                    //there is no end signal, go ahead
                    byte[] nextBcBuf = oldBcBuf;
                    if (reloadAutoSignal.WaitOne(0))
                    {
                        //get the reload signal, use new buf
                        nextBcBuf = this.bcBuf;
                        Logging.Debug(broadcaster.LocalEndPoint, bcEP, nextBcBuf.Length, "reload broadcast");
                    }

                    //broadcast again
                    broadcaster?.BeginSendTo(nextBcBuf, 0, nextBcBuf.Length, SocketFlags.None,
                        bcEP, new AsyncCallback(BroadcastCallback), nextBcBuf);
                }
            }
            catch (Exception e)
            {
                Logging.Error(e);
                IsBCStop = true;
                BroadcasterException?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
