using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ShakeSocketController.Utils
{
    /// <summary>
    /// 自定义字符串工具类
    /// </summary>
    public static class StrUtil
    {
        /// <summary>
        /// 使用驼峰命名法的默认Json序列化设置
        /// </summary>
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

        /// <summary>
        /// 利用反射获取指定对象的成员字符串（就像重构ToString()方法时所做的行为）
        /// </summary>
        public static string OverrideToString(object obj)
        {
            //只获取了公共字段
            FieldInfo[] fieldInfos = obj.GetType().GetFields();
            StringBuilder result = new StringBuilder("{");
            foreach (FieldInfo fieldInfo in fieldInfos)
                result.AppendFormat("{0}={1}, ", fieldInfo.Name, fieldInfo.GetValue(obj));

            return result.Remove(result.Length - 2, 2).Append("}").ToString();
        }

        /// <summary>
        /// 将对象序列化为Json字符串
        /// </summary>
        /// <returns>返回尽可能短的结果，但保留默认值</returns>
        public static string ObjectToJson(object obj)
        {
            return ObjectToJson(obj, false, true);
        }

        /// <summary>
        /// 将对象序列化为Json字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="easyToRead">是否生成易于阅读的结果（格式化缩进、保留null值）</param>
        /// <param name="includeDefVal">是否生成包含默认值的结果</param>
        /// <returns>返回Json字符串结果</returns>
        public static string ObjectToJson(object obj, bool easyToRead, bool includeDefVal = true)
        {
            //set according to parameters
            JsonSettings.NullValueHandling = easyToRead ?
                NullValueHandling.Include : NullValueHandling.Ignore;
            JsonSettings.Formatting = easyToRead ? Formatting.Indented : Formatting.None;
            JsonSettings.DefaultValueHandling = includeDefVal ?
                DefaultValueHandling.Include : DefaultValueHandling.Ignore;

            return JsonConvert.SerializeObject(obj, JsonSettings);
        }

        /// <summary>
        /// 将Json字符串反序列化为指定类型的对象
        /// </summary>
        public static T JsonToObject<T>(string jsonStr)
        {
            //reset to default
            JsonSettings.NullValueHandling = NullValueHandling.Include;
            JsonSettings.DefaultValueHandling = DefaultValueHandling.Include;

            return JsonConvert.DeserializeObject<T>(jsonStr, JsonSettings);
        }

    }

    /// <summary>
    /// IPAddress类的自定义Json转换器
    /// </summary>
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
