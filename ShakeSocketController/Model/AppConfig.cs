using Newtonsoft.Json;
using ShakeSocketController.Controller;
using ShakeSocketController.Utils;
using System;
using System.IO;

namespace ShakeSocketController.Model
{
    /// <summary>
    /// 程序配置数据类
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 配置文件名（必须跟程序同目录）
        /// </summary>
        public const string CONFIG_FILE_NAME = "config.json";

        [JsonProperty]
        public readonly string UUID;                    //本设备唯一标识符
        public string NickName;                         //昵称（默认与用户名相同，可自定义修改）

        public int BcPort;                              //广播端口号
        public int BcInterval;                          //每次广播的间隔时间（毫秒）
        public int MsgPort;                             //消息端口号
        public int MsgMaxReceiveBufSize;                //接收消息数据包的Buf大小

        public bool AllowCtrlOperation;                 //允许Ctrl控制
        public bool IsSSCEnabled;                       //SSC启用状态
        public bool IsBCEnabled;                        //广播启用状态

        [JsonIgnore]
        public bool IsDefault;                          //默认配置标志
        [JsonIgnore]
        public bool HadLoadFailed;                      //本地配置加载失败标志
        [JsonIgnore]
        public readonly string DeviceName;              //本机设备名
        [JsonIgnore]
        public readonly string UserName;                //本机当前登录的用户名
        [JsonIgnore]
        private LocalInfo localInfo;                    //本机设备信息对象


        [JsonConstructor]
        private AppConfig()
        {
            //在此设定默认值
            UUID = Guid.NewGuid().ToString();
            DeviceName = SysUtil.GetDeviceName();
            UserName = SysUtil.GetWinUserName();
            NickName = UserName;

            BcPort = 19019;
            BcInterval = 3000;
            MsgPort = 10019;
            MsgMaxReceiveBufSize = 4096;

            AllowCtrlOperation = true;
            IsSSCEnabled = true;
            IsBCEnabled = true;

            IsDefault = false;
            HadLoadFailed = false;
        }

        /// <summary>
        /// 首次获取初始配置时不要直接使用此方法，使用load()方法加载配置！
        /// </summary>
        /// <param name="loadFailed">表示是否因为加载本地配置失败才调用该方法</param>
        /// <returns>返回默认配置对象</returns>
        public static AppConfig GetDefaultConfig(bool loadFailed = false)
        {
            return new AppConfig()
            {
                IsDefault = true,
                HadLoadFailed = loadFailed,
            };
        }

        /// <summary>
        /// 从本地加载配置
        /// </summary>
        /// <returns>返回从本地加载的程序配置，本地没有则返回默认配置（并自动保存它到本地）</returns>
        public static AppConfig Load()
        {
            AppConfig config = null;

            try
            {
                //从文件中读取
                string configContent = File.ReadAllText(CONFIG_FILE_NAME);
                config = StrUtil.JsonToObject<AppConfig>(configContent);

                //检查各配置项
                if (!CheckConfig(config))
                {
                    //存在非法配置项，抛异常
                    throw new Exception("There is an exception in the value of the AppConfig item!");
                }
            }
            catch (FileNotFoundException)
            {
                //本地没有配置文件，直接忽略该异常
            }

            if (config == null)
            {
                config = GetDefaultConfig();
                Save(config);
                Logging.Info("Init AppConfig.");
            }

            return config;
        }

        /// <summary>
        /// 保存配置到本地
        /// </summary>
        /// <param name="config">要保存的程序配置，传入null等效于清空原文件</param>
        public static void Save(AppConfig config)
        {
            string configContent = config == null ? string.Empty
                : StrUtil.ObjectToJson(config, true);
            //保存到文件中
            File.WriteAllText(CONFIG_FILE_NAME, configContent);
        }

        /// <summary>
        /// 检查配置是否有效
        /// </summary>
        public static bool CheckConfig(AppConfig config)
        {
            // TODO: 完善这里的配置检查代码
            if (string.IsNullOrWhiteSpace(config.UUID) || config.UUID.Length != 36
                || string.IsNullOrWhiteSpace(config.NickName))
                return false;
            if (config.BcPort <= 0 || config.BcPort > 65535)
                return false;
            if (config.MsgPort <= 0 || config.MsgPort > 65535)
                return false;
            if (config.BcInterval < 0)
                return false;
            if (config.MsgMaxReceiveBufSize < 1024 || config.MsgMaxReceiveBufSize > 65536)
                return false;
            if (config.IsBCEnabled && !config.IsSSCEnabled)
                return false;

            return true;
        }

        /// <summary>
        /// 获取本地信息数据对象
        /// </summary>
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
