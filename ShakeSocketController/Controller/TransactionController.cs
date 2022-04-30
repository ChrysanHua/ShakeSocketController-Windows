using ShakeSocketController.Controller.Service;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace ShakeSocketController.Controller
{
    /// <summary>
    /// 全局控制器类
    /// </summary>
    public class TransactionController
    {
        private const string TAG = "ShakeSocketController";

        private readonly ConcurrentDictionary<IPAddress, DeviceInfo> deviceDictionary;  //设备列表字典
        private AppConfig config;                           //程序配置
        private UDPBroadcaster broadcaster;                 //UDP广播器
        private UDPHandler udpHandler;                      //UDP消息Handler
        private TCPHandler tcpHandler;

        public event EventHandler DeviceListChanged;
        public event EventHandler BroadcastStatusChanged;
        public event EventHandler TCPConnecting;
        public event EventHandler TCPConnected;
        public event EventHandler TCPDisConnect;


        public bool GetIsBCStop() => broadcaster.IsBCStop;

        public AppConfig GetCurrentConfig() => config;

        public ICollection<DeviceInfo> GetCurrentDeviceList() => deviceDictionary.Values;


        public TransactionController()
        {
            //加载程序配置
            config = LoadLatestConfig();
            // TODO: 在APP配置类中加入设备连接字典，然后在这里初始化加入全局列表
            //初始化设备列表字典
            deviceDictionary = new ConcurrentDictionary<IPAddress, DeviceInfo>();
            //初始化广播器、UDP消息Handler
            broadcaster = new UDPBroadcaster(config.BcPort, config.BcInterval);
            // TODO: 合理订阅处理UDPBroadcaster里的2个事件
            udpHandler = new UDPHandler(config.MsgMaxReceiveBufSize);
            udpHandler.HandleUDPReceived += UdpHandler_HandleUDPReceived;
            // TODO: 订阅处理UDPHandler另外4个事件
            Logging.SplitLine();
            Logging.Info($"{TAG}({SysUtil.GetVersionStr(4)}) init OK.");
        }

        /// <summary>
        /// 启动全局控制器
        /// </summary>
        public void Run()
        {
            // TODO: 首次启动时需要额外做些什么？
            //根据程序配置状态启动SSC控制
            if (config.IsSSCEnabled)
            {
                StartSSC();
            }
            Logging.Info($"{TAG} started.");
        }

        /// <summary>
        /// 关闭全局控制器
        /// </summary>
        public void Exit()
        {
            broadcaster.Close();
            udpHandler.Close();
            StopTCPHandler();

        }

        /// <summary>
        /// 启动SSC控制状态（监听|广播）
        /// </summary>
        public void StartSSC()
        {
            // TODO: 全局状态在任务栏的菜单选项：【禁用、仅监听、监听&广播】、允许控制
            //无论如何，重新启动UDP消息Handler
            udpHandler.BeginHandle(new IPEndPoint(SysUtil.GetLocalIP(), config.MsgPort));
            //根据程序配置切换广播状态
            ToggleBCState(config.IsBCEnabled);
        }

        /// <summary>
        /// 禁用SSC控制状态
        /// </summary>
        public void StopSSC()
        {
            udpHandler.EndHandle();
            ToggleBCState(false);
        }

        /// <summary>
        /// 从本地加载最新的程序配置，如果加载失败将返回默认值配置
        /// </summary>
        public AppConfig LoadLatestConfig()
        {
            try
            {
                return AppConfig.Load();
            }
            catch (Exception e)
            {
                //无法从本地加载配置，这是十分致命的异常，必须告知用户
                Logging.Error(e);
                // TODO: 告知用户APP配置加载异常
                //返回默认值配置，这只是保持程序正常运行的备用方案
                return new AppConfig();
            }
        }

        public void ToggleBCState(bool enabled)
        {
            if (enabled)
            {
                // TODO: 定下头部的通信协议，修改这里获取广播字节的逻辑；并处理BroadcastStatusChanged事件
                //无论如何，重新开始广播
                broadcaster.BeginBroadcast(StrUtil.StrToByte(config.GetLocalInfo().BCJson));
            }
            else
            {
                //停止广播
                broadcaster.EndBroadcast();
            }
        }

        public void StartTCPHandler(IPAddress remoteIP)
        {
            if (tcpHandler == null || tcpHandler.IsInvalid)
                tcpHandler = new TCPHandler(this);
            //tcpHandler.BeginHandle(new IPEndPoint(remoteIP, TCP_PORT));
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
            //PauseBroadcast();
            TCPConnected?.Invoke(this, new EventArgs());
        }

        public void TCPConnectionDisconnect()
        {
            //TCP connection break off
            //StartBroadcast();
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
