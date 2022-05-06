using Newtonsoft.Json;
using ShakeSocketController.Utils;
using System.Collections.Generic;
using System.Net;

namespace ShakeSocketController.Model
{
    /// <summary>
    /// 设备连接信息数据类
    /// </summary>
    public class DeviceInfo
    {
        private const string UNKNOWN_DEVICE_NAME = "未知设备";
        private const string UNKNOWN_USER_NAME = "未知";

        public readonly string DeviceID;                //1.设备的唯一标识符
        public readonly string DeviceName;              //2.设备名
        public readonly string UserName;                //3.用户名
        [JsonConverter(typeof(IPAddressConverter))]
        public IPAddress IP;                            //地址
        public string NickName;                         //昵称（默认与用户名相同，可自定义修改）
        public string ShakeMark;                        //通信互认标识符
        public string ClientVersion;                    //手机客户端的版本号

        public bool IsAutoConnect;                      //是否自动连接
        public bool IsDisabled;                         //是否禁用

        [JsonIgnore]
        public bool IsConnected;                        //当前是否已连接

        [JsonIgnore]
        public bool HadConnected =>
            !string.IsNullOrWhiteSpace(ShakeMark);      //曾经是否有连接过


        public DeviceInfo() : this(string.Empty, UNKNOWN_DEVICE_NAME, UNKNOWN_USER_NAME) { }

        [JsonConstructor]
        public DeviceInfo(string deviceID, string deviceName, string userName)
        {
            DeviceID = string.IsNullOrWhiteSpace(deviceID) ? string.Empty : deviceID;
            DeviceName = string.IsNullOrWhiteSpace(deviceName) ? UNKNOWN_DEVICE_NAME : deviceName;
            UserName = string.IsNullOrWhiteSpace(userName) ? UNKNOWN_USER_NAME : userName;
            NickName = UserName;
        }

        public DeviceInfo(string deviceID, string deviceName, string userName, IPAddress ip)
            : this(deviceID, deviceName, userName)
        {
            IP = ip;
        }

        /// <summary>
        /// 获取当前设备连接信息是否可用
        /// </summary>
        public bool IsUsable()
        {
            return !string.IsNullOrWhiteSpace(DeviceID) &&
                DeviceName != UNKNOWN_DEVICE_NAME &&
                UserName != UNKNOWN_USER_NAME &&
                IP != null &&
                !string.IsNullOrWhiteSpace(NickName) &&
                !string.IsNullOrWhiteSpace(ClientVersion);
        }

        /// <summary>
        /// 判断与指定设备是否本质上相同
        /// </summary>
        public bool IsSameDevice(DeviceInfo info)
        {
            return info != null && DeviceID == info.DeviceID;
        }

        public override string ToString()
        {
            return StrUtil.OverrideToString(this);
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceInfo info &&
                   DeviceID == info.DeviceID &&
                   DeviceName == info.DeviceName &&
                   UserName == info.UserName;
        }

        public override int GetHashCode()
        {
            var hashCode = -1650219153;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DeviceID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DeviceName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
            return hashCode;
        }
    }
}
