using ShakeSocketController.Controller.Service;
using ShakeSocketController.Handler;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller
{
    public class TransactionController
    {
        private const int BC_PORT = 19019;
        private const int MSG_PORT = 10019;
        private const int TCP_PORT = 11019;

        private readonly ConcurrentDictionary<IPAddress, DeviceInfo> deviceDictionary;
        private UDPBroadcaster broadcaster;
        private UDPHandler udpHandler;
        private TCPHandler tcpHandler;
        private Configuration config;

        public event EventHandler DeviceListChanged;
        public event EventHandler BroadcastStatusChanged;
        public event EventHandler TCPConnecting;
        public event EventHandler TCPConnected;
        public event EventHandler TCPDisConnect;


        public bool GetIsBCStop() => broadcaster.IsBCStop;

        public Configuration GetCurrentConfig() => config;

        public ICollection<DeviceInfo> GetCurrentDeviceList() => deviceDictionary.Values;


        public TransactionController()
        {
            deviceDictionary = new ConcurrentDictionary<IPAddress, DeviceInfo>();
            broadcaster = new UDPBroadcaster(BC_PORT);
            udpHandler = new UDPHandler();
            udpHandler.HandleUDPReceived += UdpHandler_HandleUDPReceived;
        }

        public void Start()
        {
            deviceDictionary.Clear();
            config = Configuration.Load();
            udpHandler.BeginHandle(new IPEndPoint(SysUtil.GetLocalIP(), MSG_PORT));
            StartBroadcast(true);
            
        }

        public void Stop()
        {
            broadcaster.Close();
            udpHandler.Close();
            StopTCPHandler();

        }

        public void StartTCPHandler(IPAddress remoteIP)
        {
            if (tcpHandler == null || tcpHandler.IsInvalid)
                tcpHandler = new TCPHandler(this);
            tcpHandler.BeginHandle(new IPEndPoint(remoteIP, TCP_PORT));
            TCPConnecting?.Invoke(this, new EventArgs());
        }

        public void StopTCPHandler()
        {
            if (tcpHandler == null) return;
            tcpHandler.Close();
            TCPDisConnect?.Invoke(this, new EventArgs());
        }

        public void TCPConnectionSuccessful()
        {
            PauseBroadcast();
            TCPConnected?.Invoke(this, new EventArgs());
        }

        public void TCPConnectionDisconnect()
        {
            //TCP connection break off
            StartBroadcast();
            TCPDisConnect?.Invoke(this, new EventArgs());
        }

        public void SendUDP(byte[] dataBuf, IPEndPoint targetEP)
        {
            udpHandler.SendTo(dataBuf, targetEP);
        }

        public void SendTCP(byte[] dataBuf)
        {
            tcpHandler.Send(dataBuf);
        }

        public void StartBroadcast(bool firstTime = false)
        {
            if (firstTime)
            {
                DeviceInfo localInfo = config.GetLocalInfo();
                if (localInfo != null)
                {
                    try
                    {
                        string jsonStr = StrUtil.ObjectToJson(localInfo);
                        StartBroadcast(MsgPacket.Parse(MsgPacket.TYPE_IP, jsonStr));
                    }
                    catch (Exception e)
                    {
                        Logging.Error(e);
                    }
                }
            }
            else
            {
                broadcaster.Reload();
            }
            BroadcastStatusChanged?.Invoke(this, new EventArgs());
        }

        private void StartBroadcast(MsgPacket bcPacket)
        {
            //first start
            broadcaster.BeginBroadcast(bcPacket.MsgData);
        }

        public void PauseBroadcast()
        {
            broadcaster.EndBroadcast();
            BroadcastStatusChanged?.Invoke(this, new EventArgs());
        }

        public void CheckInDevice(IPAddress targetIP, DeviceInfo targetInfo)
        {
            deviceDictionary[targetIP] = targetInfo;
            Logging.Debug($"IP check in: {targetIP} ({targetInfo.DeviceName})");
            DeviceListChanged?.Invoke(this, new EventArgs());
        }

        public DeviceInfo CheckOutDevice(IPAddress targetIP)
        {
            if (deviceDictionary.ContainsKey(targetIP))
            {
                return deviceDictionary[targetIP];
            }
            return null;
        }

        public bool ShouldHandleMsg(IPAddress targetIP, DeviceInfo targetInfo = null)
        {
            if (targetInfo != null)
            {
                return targetInfo.Equals(CheckOutDevice(targetIP));
            }
            else if (GetIsBCStop())
            {
                return deviceDictionary.ContainsKey(targetIP);
            }
            return true;
        }

        private void UdpHandler_HandleUDPReceived(object sender, HandleUDPEventArgs e)
        {
            if (ShouldHandleMsg(e.IPE.Address))
            {
                MsgPacket packet = MessageAdapter.PackDataBuf(e.DataBuf, e.Length);
                Logging.Debug($"type:{packet.TypeStr}, data:{packet.DataStr}.");
                MessageAdapter.CreateMsgHandler(packet)?.Handle(this, e.IPE.Address);
            }
        }

        public void HandleTCPMsg(byte[] msgDataBuf, int bufLen)
        {
            MsgPacket packet = MessageAdapter.PackDataBuf(msgDataBuf, bufLen);
            Logging.Debug($"type:{packet.TypeStr}, data:{packet.DataStr}.");
            MessageAdapter.CreateMsgHandler(packet)?.Handle(this, null);
        }

    }
}
