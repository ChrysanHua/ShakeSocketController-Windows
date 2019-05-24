using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Utils
{
    public static class StrUtil
    {
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public static byte[] StrToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ByteToStr(byte[] strByte, int count = -1)
        {
            if (count == -1)
                return Encoding.UTF8.GetString(strByte);
            else
                return Encoding.UTF8.GetString(strByte, 0, count);
        }

        public static string ByteToBase64(byte[] strByte)
        {
            return Convert.ToBase64String(strByte);
        }

        public static byte[] Base64ToByte(string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        public static string ByteToHexStr(byte[] strByte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in strByte)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }

        public static byte[] IntToByte(int num)
        {
            return BitConverter.GetBytes(num);
        }

        public static int ByteToInt(byte[] numByte)
        {
            return BitConverter.ToInt32(numByte, 0);
        }

        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, jsonSettings);
        }

        public static T JsonToObject<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr, jsonSettings);
        }

    }

    class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return IPAddress.Parse((string)reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
