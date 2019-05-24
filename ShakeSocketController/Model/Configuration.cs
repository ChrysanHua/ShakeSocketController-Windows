using ShakeSocketController.Controller;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Model
{
    public class Configuration
    {
        public string DeviceName;
        public string NickName;


        public DeviceInfo GetLocalInfo()
        {
            try
            {
                return new DeviceInfo()
                {
                    IP = SysUtil.GetLocalIP(),
                    DeviceName = this.DeviceName,
                    NickName = this.NickName
                };
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            return null;
        }

        public static Configuration Load()
        {
            //default config
            Configuration config = new Configuration
            {
                DeviceName = SysUtil.GetDeviceName(),
                NickName = SysUtil.GetWinUserName(),
            };

            return config;
        }


    }
}
