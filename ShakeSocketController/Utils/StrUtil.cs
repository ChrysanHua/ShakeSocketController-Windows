using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ShakeSocketController.Utils
{
    public static class StrUtil
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
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

        public static string ByteToHexStr(byte[] strByte, string separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strByte.Length - 1; i++)
            {
                sb.Append($"{strByte[i]:X2}{separator}");
            }
            if (strByte.Length > 0)
            {
                sb.Append(strByte[strByte.Length - 1].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string ByteToHexStr(byte[] strByte, bool format = false)
        {
            //space separator is used by default
            return ByteToHexStr(strByte, format ? " " : null);
        }

        public static byte[] HexStrToByte(string hexStr, string separator = " ")
        {
            //handle the separator first
            hexStr = hexStr.Replace(separator, string.Empty);
            if ((hexStr.Length % 2) != 0)
            {
                //length complement
                hexStr += " ";
            }
            byte[] resultByte = new byte[hexStr.Length / 2];
            for (int i = 0; i < resultByte.Length; i++)
            {
                resultByte[i] = byte.Parse(hexStr.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
            }
            return resultByte;
        }

        public static int StringToInt(string str)
        {
            if (int.TryParse(str, out int val))
                return val;
            else
                return 0;
        }

        public static DateTime StringToDateTime(string str)
        {
            if (DateTime.TryParse(str, out DateTime date))
                return date;
            else
                return DateTime.MinValue;
        }

        public static bool IsEnglish(string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z]+$", RegexOptions.Compiled);
        }

        public static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$", RegexOptions.Compiled);
        }

        public static bool IsNumOrEng(string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z0-9]+$", RegexOptions.Compiled);
        }

        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings);
        }

        public static T JsonToObject<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr, JsonSettings);
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
