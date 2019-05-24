using Newtonsoft.Json;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Model
{
    public class DeviceInfo
    {
        [JsonConverter(typeof(IPAddressConverter))]
        public IPAddress IP;
        public string DeviceName;
        public string NickName;

        public bool IsComplete()
        {
            return !((IP == null) ||
                (string.IsNullOrEmpty(DeviceName)) ||
                (string.IsNullOrEmpty(NickName)));
        }

    }
}
