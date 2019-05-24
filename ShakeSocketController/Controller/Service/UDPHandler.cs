using ShakeSocketController.Utils;
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
    class UDPHandler
    {
        private readonly TransactionController _controller;

        private Socket udpSocket;
        private byte[] recBuf = new byte[4096];
        private EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        private readonly object recStopLock = new object();
        private bool isRecStop = true;

        public UDPHandler(TransactionController controller)
        {
            this._controller = controller;
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);
        }

        public void BeginHandle(IPEndPoint localEP)
        {
            try
            {
                udpSocket.Bind(localEP);
                Logging.Debug("UDPReceive bind localEP: " + localEP);
                Reload();
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public void EndHandle()
        {
            lock (recStopLock)
            {
                if (isRecStop) return;
                isRecStop = true;
            }
            Logging.Debug("end UDPReceive!");
        }

        public void Reload()
        {
            if (udpSocket == null) return;
            lock (recStopLock)
            {
                if (!isRecStop) return;
                isRecStop = false;
            }
            try
            {
                Logging.Debug("begin UDPReceive!");
                udpSocket.BeginReceiveFrom(recBuf, 0, recBuf.Length, SocketFlags.None,
                    ref remoteEP, new AsyncCallback(ReceiveFromCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public void SendTo(byte[] dataBuf, IPEndPoint targetEP)
        {
            try
            {
                udpSocket?.BeginSendTo(dataBuf, 0, dataBuf.Length, SocketFlags.None,
                    targetEP, new AsyncCallback(SendToCallback), targetEP);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public void Close()
        {
            if (udpSocket == null) return;
            EndHandle();
            try
            {
                udpSocket.Shutdown(SocketShutdown.Both);
                udpSocket.Close();
                udpSocket.Dispose();
                udpSocket = null;
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        private void ReceiveFromCallback(IAsyncResult ar)
        {
            if (udpSocket == null) return;
            try
            {
                int len = udpSocket.EndReceiveFrom(ar, ref remoteEP);
                //do something by controller in here
                Logging.Debug(remoteEP, udpSocket.LocalEndPoint, len, "UDPReceive");
                _controller.HandleUDPMsg(recBuf, len, remoteEP as IPEndPoint);
                lock (recStopLock)
                {
                    if (isRecStop) return;
                }
                udpSocket.BeginReceiveFrom(recBuf, 0, recBuf.Length, SocketFlags.None,
                    ref remoteEP, new AsyncCallback(ReceiveFromCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        private void SendToCallback(IAsyncResult ar)
        {
            if (udpSocket == null) return;
            try
            {
                int len = udpSocket.EndSendTo(ar);
                Logging.Debug(udpSocket.LocalEndPoint, ar.AsyncState as EndPoint, len, "UDP");
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }
    }
}
