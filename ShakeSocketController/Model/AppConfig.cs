using Newtonsoft.Json;
using ShakeSocketController.Utils;
using System;

namespace ShakeSocketController.Model
{
    public class AppConfig
    {
        [JsonProperty]
        public readonly string UUID;                    //本设备唯一标识符
        public string NickName;                         //昵称（默认与用户名相同，可自定义修改）


        [JsonIgnore]
        public readonly string DeviceName;              //本机设备名
        [JsonIgnore]
        public readonly string UserName;                //本机当前登录的用户名
        [JsonIgnore]
        private LocalInfo localInfo;                    //本机设备信息对象


        [JsonConstructor]
        public AppConfig()
        {
            //set default config here
            UUID = Guid.NewGuid().ToString();
            DeviceName = SysUtil.GetDeviceName();
            UserName = SysUtil.GetWinUserName();
        }

        public static AppConfig Load()
        {
            //default config
            //Configuration config = new Configuration
            //{
            //    DeviceName = SysUtil.GetDeviceName(),
            //    UserName = SysUtil.GetWinUserName(),
            //};

            //return config;
            return null;
        }

        public LocalInfo GetLocalInfo()
        {
            if (localInfo == null)
            {
                localInfo = new LocalInfo(UUID, DeviceName, UserName);
            }

            return localInfo;
        }

    }
}
