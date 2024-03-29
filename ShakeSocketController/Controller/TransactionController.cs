﻿using ShakeSocketController.Controller.Service;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ShakeSocketController.Controller
{
    // TODO: SSC在自动模式下时，连接成功则自动停止广播，断开连接就自动开始广播。

    /// <summary>
    /// 全局控制器类
    /// </summary>
    public class TransactionController
    {
        private const string TAG = "ShakeSocketController";

        private readonly SynchronizedCollection<DeviceInfo> deviceInfoList; //设备连接列表
        private volatile DeviceInfo ctrlDeviceInfo;         //当前已连接的设备
        private AppConfig config;                           //程序配置
        private UDPBroadcaster broadcaster;                 //UDP广播器
        private UDPHandler udpHandler;                      //UDP消息Handler

        /// <summary>
        /// SSC控制状态变更事件
        /// </summary>
        public event EventHandler SSCStateChanged;
        /// <summary>
        /// 设备连接列表元素变更事件
        /// </summary>
        public event EventHandler DeviceListChanged;
        /// <summary>
        /// 单个设备连接信息元素变更事件
        /// </summary>
        public event EventHandler<DeviceInfoEventArgs> DeviceInfoChanged;


        /// <summary>
        /// 当前设备连接列表
        /// </summary>
        public List<DeviceInfo> CurDeviceList => deviceInfoList.ToList();
        /// <summary>
        /// SSC当前连接Ctrl的设备
        /// </summary>
        public DeviceInfo CurCtrlDeviceInfo => ctrlDeviceInfo;
        /// <summary>
        /// 当前程序配置
        /// </summary>
        public AppConfig CurConfig => config;
        /// <summary>
        /// 广播状态
        /// </summary>
        public bool IsBCStop => broadcaster.IsBCStop;
        /// <summary>
        /// SSC监听状态
        /// </summary>
        public bool IsSSCListening => !udpHandler.IsRecStop && udpHandler.IsHandling;
        /// <summary>
        /// Ctrl是否正在连接
        /// </summary>
        public bool IsCtrlConnecting => ctrlDeviceInfo != null && !ctrlDeviceInfo.IsConnected;
        /// <summary>
        /// Ctrl是否已完成连接
        /// </summary>
        public bool IsCtrlConnected => ctrlDeviceInfo != null && ctrlDeviceInfo.IsConnected;


        public TransactionController()
        {
            //加载程序配置
            config = LoadLatestConfig();
            //初始化设备列表
            deviceInfoList = new SynchronizedCollection<DeviceInfo>();
            //config.historyList.ForEach(item => deviceInfoList.Add(item));
            Program.CreateTestInfoList(10).ForEach(item => deviceInfoList.Add(item));// TODO: 临时
            foreach (var item in deviceInfoList)
            {
                if (item.IsConnected)
                {
                    ctrlDeviceInfo = item;
                    break;
                }
            }
            //初始化广播器、UDP消息Handler
            broadcaster = new UDPBroadcaster(config.BcPort, config.BcInterval);
            // TODO: 合理订阅处理UDPBroadcaster里的2个事件
            udpHandler = new UDPHandler(config.MsgMaxReceiveBufSize);
            udpHandler.HandleUDPReceived += UdpHandler_HandleUDPReceived;
            // TODO: 订阅处理UDPHandler另外4个事件

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
            broadcaster?.Close();
            udpHandler?.Close();
        }

        /// <summary>
        /// 启动SSC控制状态（监听|广播）
        /// </summary>
        public void StartSSC()
        {
            //无论如何，重新启动UDP消息Handler
            udpHandler.BeginHandle(new IPEndPoint(SysUtil.GetLocalIP(), config.MsgPort));
            //根据程序配置切换广播状态
            ToggleBCState(config.IsBCEnabled);
            //触发事件
            SSCStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 禁用SSC控制状态
        /// </summary>
        public void StopSSC()
        {
            udpHandler.EndHandle();
            ToggleBCState(false);
            //触发事件
            SSCStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 切换广播状态
        /// </summary>
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
                //返回默认值配置，这只是保持程序正常运行的备用方案
                return AppConfig.GetDefaultConfig(true);
            }
        }

        /// <summary>
        /// 保存程序配置到本地
        /// </summary>
        /// <param name="config">要保存的程序配置，传入null等效于清空原文件</param>
        /// <returns>返回保存是否成功，如果配置无效将直接返回false而不执行任何保存操作</returns>
        public bool SaveConfig(AppConfig config)
        {
            if (config == null || AppConfig.CheckConfig(config))
            {
                try
                {
                    AppConfig.Save(config);
                    //重置标志
                    config.HadSaveFailed = false;
                    return true;
                }
                catch (Exception e)
                {
                    Logging.Error(e);
                }
            }

            if (config != null)
            {
                //设置保存失败标志
                config.HadSaveFailed = true;
            }
            return false;
        }

        /// <summary>
        /// 保存当前设备连接列表到本地
        /// </summary>
        /// <returns>返回保存是否成功，如果配置无效将直接返回false而不执行任何保存操作</returns>
        public bool SaveDeviceList()
        {
            //先清空旧列表
            config.historyList.Clear();
            //获取新列表
            config.historyList = deviceInfoList.ToList();
            //保存
            return SaveConfig(config);
        }

        /// <summary>
        /// 自定义修改设备连接信息
        /// </summary>
        /// <param name="info">已修改的设备连接信息对象</param>
        public void CustomModifyDevice(DeviceInfo info)
        {
            //保存列表
            if (!SaveDeviceList())
            {
                //保存失败，告知用户
                MessageBox.Show(
                    $"配置已变更，但保存失败！{Environment.NewLine}注意：SSC依然会执行您所选的操作。",
                    "配置保存失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //触发事件
            DeviceInfoChanged?.Invoke(this, new DeviceInfoEventArgs(info, deviceInfoList.IndexOf(info)));
        }

        /// <summary>
        /// 自定义删除设备连接信息
        /// </summary>
        /// <param name="info">要删除的设备连接信息对象</param>
        public void CustomDeleteDevice(DeviceInfo info)
        {
            //执行删除
            if (!deviceInfoList.Remove(info))
            {
                MessageBox.Show("删除失败！", "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存新列表
            if (!SaveDeviceList())
            {
                //保存失败，告知用户
                MessageBox.Show(
                    $"配置已变更，但保存失败！{Environment.NewLine}注意：SSC依然会执行您所选的操作。",
                    "配置保存失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //触发事件
            DeviceInfoChanged?.Invoke(this, new DeviceInfoEventArgs(info, deviceInfoList.IndexOf(info),
                false, true));
        }

        /// <summary>
        /// 断开Ctrl连接
        /// </summary>
        public void DisconnectCtrl()
        {
            // TODO: 1.发送断连信号；2.切换对象状态；3.切换UI状态；

            //先清除对象引用，再设置状态
            DeviceInfo tmp = ctrlDeviceInfo;
            ctrlDeviceInfo = null;
            tmp.IsConnected = false;
            //触发设备连接元素变更事件
            DeviceInfoChanged?.Invoke(this, new DeviceInfoEventArgs(tmp, deviceInfoList.IndexOf(tmp),
                true));
            //切换广播状态（如果需要）
            ToggleBCState(config.IsBCEnabled);
            //触发SSC状态变更事件
            SSCStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 发送UDP数据报文
        /// </summary>
        /// <param name="packet">要发送的数据报文对象</param>
        /// <param name="targetEP">目标地址</param>
        public void SendUDPMsgPacket(MsgPacket packet, IPEndPoint targetEP)
        {
            udpHandler.SendTo(packet?.MsgData, targetEP);
        }

        public void CheckInDevice(IPAddress targetIP, DeviceInfo targetInfo)
        {
            //deviceDictionary[targetIP] = targetInfo;
            //Logging.Debug($"IP check in: {targetIP} ({targetInfo.DeviceName})");
            DeviceListChanged?.Invoke(this, EventArgs.Empty);
        }

        public DeviceInfo CheckOutDevice(IPAddress targetIP)
        {
            //if (deviceDictionary.ContainsKey(targetIP))
            //{
            //    return deviceDictionary[targetIP];
            //}
            return null;
        }

        /// <summary>
        /// 判断是否应该处理指定的数据Buf
        /// </summary>
        /// <param name="msgData">接收到的数据报文</param>
        /// <param name="ipe">对方的地址</param>
        /// <param name="packet">数据报文对象（如果打包失败将返回null）</param>
        public bool ShouldHandleMsg(byte[] msgData, IPEndPoint ipe, out MsgPacket packet)
        {
            // TODO: 旧记录：先判断IP有没有记录，
            //  有记录：恰好是已连接的那个设备，则直接处理；否则，按下面“无记录”走↓；
            //  无记录：检查数据内容类型，如果是连接类型，未连接，则执行连接逻辑*，已连接的则直接忽略；
            //      如果是Ctrl类型，已连接，则请求确认身份*，未连接的则直接忽略；

            // TODO: 加个确认连接事件，在任务栏图标那边利用主线程弹出对话框

            if (msgData.Length >= MsgPacket.HEADER_LENGTH && ipe.Port == config.MsgPort)
            {
                //buf长度以及目标端口符合，尝试构建数据报文对象
                packet = MessageAdapter.PackMsgData(msgData);
                // TODO: 将0x01替换为对应的Handler类常量
                if (packet != null && (packet.MainFunCode.Equals(0x01) || IsCtrlConnected))
                {
                    //数据报文构建成功，且，报文是连接功能相关报文或当前已连接Ctrl
                    return true;
                }
            }

            //其余情况下，均不处理
            packet = null;
            return false;
        }

        /// <summary>
        /// 收到UDP数据报文事件
        /// </summary>
        private void UdpHandler_HandleUDPReceived(object sender, HandleUDPEventArgs e)
        {
            if (ShouldHandleMsg(e.DataBuf, e.IPE, out MsgPacket packet))
            {
                //处理数据报文
                Logging.Debug($"header:{packet.HeaderHexStr}, data:{packet.DataHexStr}.");
                //MessageAdapter.HandleUDPPacket(this, packet, e.IPE);
            }
        }

    }
}
