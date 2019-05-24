using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller.Service
{
    class TCPHandler
    {
        private readonly TransactionController _controller;

        private Socket tcpSocket;
        private byte[] recBuf = new byte[4096];

        public bool IsInvalid { get => tcpSocket == null; }

        public TCPHandler(TransactionController controller)
        {
            this._controller = controller;
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
        }

        public void BeginHandle(IPEndPoint remoteEP)
        {
            try
            {
                Logging.Debug(tcpSocket.LocalEndPoint, remoteEP, 0, "begin TCPConnect");
                tcpSocket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public void Send(byte[] dataBuf)
        {
            try
            {
                tcpSocket?.BeginSend(dataBuf, 0, dataBuf.Length, SocketFlags.None,
                    new AsyncCallback(SendCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        public void Close()
        {
            if (tcpSocket == null) return;
            try
            {
                if (tcpSocket.Connected)
                {
                    tcpSocket.Shutdown(SocketShutdown.Both);
                    Logging.Debug("end TCPConnect");
                }
                tcpSocket.Close();
                tcpSocket.Dispose();
                tcpSocket = null;
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (tcpSocket == null) return;
            try
            {
                tcpSocket.EndConnect(ar);
                Logging.Info("Connection Established Successfully");
                //do something by controller in here
                _controller.TCPConnectionSuccessful();
                tcpSocket.BeginReceive(recBuf, 0, recBuf.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveCallback), null);
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (tcpSocket == null) return;
            try
            {
                int len = tcpSocket.EndReceive(ar);
                if (len > 0)
                {
                    //do something by controller in here (TODO: handle packet splicing)
                    Logging.Debug(tcpSocket.RemoteEndPoint,
                        tcpSocket.LocalEndPoint, len, "TCPReceive");
                    _controller.HandleTCPMsg(recBuf, len);
                    tcpSocket.BeginReceive(recBuf, 0, recBuf.Length, SocketFlags.None,
                        new AsyncCallback(ReceiveCallback), null);
                }
                else
                {
                    //remote socket shutdown, so close local socket too
                    Close();
                    //do something by controller in here
                    _controller.TCPConnectionDisconnect();
                }
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            if (tcpSocket == null) return;
            try
            {
                int len = tcpSocket.EndSend(ar);
                Logging.Debug(tcpSocket, len, "TCP");
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }
    }
}
