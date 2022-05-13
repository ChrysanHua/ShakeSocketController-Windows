using System;

namespace ShakeSocketController.Model
{
    public class DeviceInfoEventArgs : EventArgs
    {
        public DeviceInfo TargetInfo { get; }
        public int MainIndex { get; }
        public bool IsConnectStateChanged { get; }
        public bool IsDeleteOP { get; }
        public bool IsConnectedInfo => TargetInfo.IsConnected;

        public DeviceInfoEventArgs(DeviceInfo targetInfo, int mainIndex,
            bool isConnectStateChanged = false, bool isDeleteOP = false)
        {
            this.TargetInfo = targetInfo;
            this.MainIndex = mainIndex;
            this.IsConnectStateChanged = isConnectStateChanged;
            this.IsDeleteOP = isDeleteOP;
        }
    }
}
