using ShakeSocketController.Utils;

namespace ShakeSocketController.Model
{
    /// <summary>
    /// 本地信息数据类
    /// </summary>
    public class LocalInfo
    {
        private readonly DeviceInfo deviceInfo;         //本地设备信息（重用DeviceInfo里的字段）
        /// <summary>
        /// 用于发送广播的Json字符串
        /// </summary>
        public readonly string BCJson;

        public LocalInfo(string deviceID, string deviceName, string userName)
        {
            //根据参数创建本地设备信息对象
            deviceInfo = new DeviceInfo(deviceID, deviceName, userName)
            {
                NickName = null,
                ClientVersion = SysUtil.GetVersionStr(4)
            };
            //初始化广播Json
            BCJson = StrUtil.ObjectToJson(
                new DeviceInfo(deviceID, deviceName, userName) { NickName = null },
                false, false);
        }

        /// <summary>
        /// 获取完整的本地信息Json字符串（用于连接等）
        /// </summary>
        /// <param name="shakeMark">目标手机设备连接信息的ShakeMark</param>
        public string GetIntactJson(string shakeMark)
        {
            deviceInfo.ShakeMark = shakeMark;
            deviceInfo.IP = SysUtil.GetLocalIP();
            return StrUtil.ObjectToJson(deviceInfo, false, false);
        }
    }
}
