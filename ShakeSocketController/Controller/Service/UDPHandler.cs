using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ShakeSocketController.Controller.Service
{
    public class UDPHandler
    {
        private readonly int REC_BUF_SIZE;

        private Socket udpSocket;
        private EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        private readonly byte[] recBuf;

        private readonly ManualResetEvent handleManualSignal;
        private readonly object recStopLock = new object();
        private bool isRecStop = true;

        public event EventHandler<HandleUDPEventArgs> HandleUDPReceived;
        public event EventHandler<HandleUDPEventArgs> MissUDPReceived;
        public event EventHandler<HandleUDPEventArgs> SendException;
        public event EventHandler<AbnormalDataEventArgs> AbnormalDataReceived;
        public event EventHandler HandlerException;

        public bool IsRecStop
        {
            get { lock (recStopLock) { return isRecStop; } }
            private set { lock (recStopLock) { isRecStop = value; } }
        }

        public bool IsHandling
        {
            get
            {
                try
                {
                    return handleManualSignal.WaitOne(0);
                }
                catch
                {
                    return false;
                }
            }
        }


        public UDPHandler(int recBufSize = 4096)
        {
            this.REC_BUF_SIZE = recBufSize;
            recBuf = new byte[this.REC_BUF_SIZE];
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);
            handleManualSignal = new ManualResetEvent(false);
        }

        public void BeginHandle(IPEndPoint localEP)
        {
            if (udpSocket == null)
                throw new InvalidOperationException("UDPHandler has been closed!");
            if (localEP == null)
                throw new ArgumentNullException(nameof(localEP));
            
            try
            {
                if (udpSocket.IsBound &&
                    (!localEP.Equals(udpSocket.LocalEndPoint) || IsRecStop))
                {
                    //this is not the first binding, so close the old socket first
                    Close(false);
                    //recreate the socket
                    udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                        ProtocolType.Udp);
                    //reset the buf (this is a "meaningless" operation)
                    ByteUtil.ClearByte(recBuf);
                }

                Logging.Debug("begin handling UDP!");
                //set as handle signal
                handleManualSignal.Set();

                if (!udpSocket.IsBound)
                {
                    //started for the first time, or recreate the socket
                    udpSocket.Bind(localEP);
                    Logging.Debug("UDPHandler bind localEP: " + localEP);
                    //flag start status
                    IsRecStop = false;
                    Logging.Debug(null, localEP, REC_BUF_SIZE, "begin UDPReceive");
                    //begin UDPReceive
                    udpSocket.BeginReceiveFrom(recBuf, 0, recBuf.Length, SocketFlags.None,
                        ref remoteEP, new AsyncCallback(ReceiveFromCallback), udpSocket);
                }
            }
            catch (Exception e)
            {
                Logging.Error(e);
                IsRecStop = true;
                HandlerException?.Invoke(this, EventArgs.Empty);
            }
        }

        public void EndHandle()
        {
            if (IsRecStop || !IsHandling) return;
            handleManualSignal.Reset();
            Logging.Debug("end handling UDP!");
        }

        private void Close(bool absolutely)
        {
            if (udpSocket == null) return;
            try
            {
                EndHandle();
                if (absolutely)
                {
                    //completely shut down
                    handleManualSignal.Close();
                }

                udpSocket.Shutdown(SocketShutdown.Both);
                udpSocket.Close();
                udpSocket.Dispose();
                udpSocket = null;
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            finally
            {
                IsRecStop = true;
            }
        }

        public void Close() => Close(true);

        private void ReceiveFromCallback(IAsyncResult ar)
        {
            if (udpSocket == null)
            {
                //the socket has been closed
                Logging.Debug("end UDPReceive!");
                return;
            }

            try
            {
                Socket workSocket = ar.AsyncState as Socket;
                int len = workSocket.EndReceiveFrom(ar, ref remoteEP);
                Logging.Debug(remoteEP, workSocket.LocalEndPoint, len, "UDPReceive");

                if (handleManualSignal.WaitOne(0))
                {
                    //get the handle signal
                    HandleUDPReceived?.Invoke(this,
                        new HandleUDPEventArgs(ByteUtil.SubByte(recBuf, 0, len), remoteEP));
                }
                else
                {
                    //without the handle signal
                    MissUDPReceived?.Invoke(this,
                        new HandleUDPEventArgs(ByteUtil.SubByte(recBuf, 0, len), remoteEP));
                }

                //receive again
                workSocket?.BeginReceiveFrom(recBuf, 0, recBuf.Length, SocketFlags.None,
                    ref remoteEP, new AsyncCallback(ReceiveFromCallback), workSocket);

                return;
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.MessageSize)
            {
                //this is caused by the sender sending large datagrams maliciously
                Logging.Error(e);
                Logging.Info($"发送方({remoteEP})未按既定大小发送数据报！");
                AbnormalDataReceived?.Invoke(this,
                    new AbnormalDataEventArgs(remoteEP, e.SocketErrorCode));
            }
            catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
            {
                //this is caused by the sender sending 'rst' maliciously
                Logging.Error(e);
                Logging.Info($"发送方({remoteEP})强迫关闭了我们的现有连接！");
                AbnormalDataReceived?.Invoke(this,
                    new AbnormalDataEventArgs(remoteEP, e.SocketErrorCode));
            }
            catch (Exception e)
            {
                if ((e is ObjectDisposedException) || (e is NullReferenceException))
                {
                    //this is caused by the socket being closed
                    Logging.Debug("end UDPReceive!");
                    return;
                }
                else
                {
                    //unhandled exception
                    Logging.Error(e);
                }
            }

            IsRecStop = true;
            HandlerException?.Invoke(this, EventArgs.Empty);
        }

        public void SendTo(byte[] dataBuf, IPEndPoint targetEP)
        {
            if (dataBuf == null)
                throw new ArgumentNullException(nameof(dataBuf));
            if (targetEP == null)
                throw new ArgumentNullException(nameof(targetEP));

            try
            {
                udpSocket.BeginSendTo(dataBuf, 0, dataBuf.Length, SocketFlags.None,
                    targetEP, new AsyncCallback(SendToCallback),
                    new SocketAsyncState(udpSocket, dataBuf, targetEP));
            }
            catch (Exception e)
            {
                if ((e is NullReferenceException) || (e is ObjectDisposedException))
                {
                    //this is caused by the socket being closed
                    Logging.Info($"UDPSend was canceled! (target={targetEP}, size={dataBuf.Length})");
                }
                else
                {
                    //unhandled exception
                    Logging.Error(e);
                    SendException?.Invoke(this, new HandleUDPEventArgs(dataBuf, targetEP));
                }
            }
        }

        private void SendToCallback(IAsyncResult ar)
        {
            if (udpSocket == null) return;

            var so = ar.AsyncState as SocketAsyncState;
            try
            {
                int len = so.WorkSocket.EndSendTo(ar);
                if (len != so.DataBuffer.Length)
                    throw new Exception($"发送异常，操作系统未能正确地发送UDP数据报！ " +
                        $"(A/P len={len}/{so.DataBuffer.Length})");

                Logging.Debug(so.WorkSocket.LocalEndPoint, so.RemoteEP, len, "UDPSend");
            }
            catch (Exception e)
            {
                if (!(e is NullReferenceException) && !(e is ObjectDisposedException))
                {
                    Logging.Error(e);
                    SendException?.Invoke(this,
                        new HandleUDPEventArgs(so.DataBuffer, so.RemoteEP));
                }
            }
        }
    }
}
