using Microsoft.Win32;
using System;

namespace ShakeSocketController.Utils
{
    /// <summary>
    /// 为当前应用程序设置开启自启动
    /// </summary>
    public static class AutoStartup
    {
        /// <summary>
        /// 控制开机自启动的注册表目录
        /// </summary>
        private const string AUTOSTARTUP_REG_PATH = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// 注册表值（应用程序可执行文件的绝对路径）
        /// </summary>
        private static readonly string ExecutablePath = SysUtil.GetExecutablePath();

        /// <summary>
        /// 注册表键（应用程序名_启动程序的目录路径字符串哈希值）
        /// </summary>
        private static readonly string Key = SysUtil.GetAssemblyTitle()
            + "_" + SysUtil.GetTempPath().GetHashCode();


        /// <summary>
        /// 切换注册表中的开机自启动设置
        /// </summary>
        /// <param name="enabled">要切换的状态</param>
        /// <returns>切换结果</returns>
        public static bool Toggle(bool enabled)
        {
            RegistryKey runKey = null;
            try
            {
                runKey = SysUtil.OpenRegKey(AUTOSTARTUP_REG_PATH, true);
                if (runKey == null)
                {
                    return false;
                }

                if (enabled)
                {
                    runKey.SetValue(Key, ExecutablePath);
                }
                else
                {
                    runKey.DeleteValue(Key);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    runKey.Close();
                    runKey.Dispose();
                }
            }
        }

        /// <summary>
        /// 检查注册表中开机自启动设置的状态
        /// </summary>
        /// <returns>开机自启动状态</returns>
        public static bool CheckStatus()
        {
            RegistryKey runKey = null;
            try
            {
                runKey = SysUtil.OpenRegKey(AUTOSTARTUP_REG_PATH, true);
                if (runKey == null)
                {
                    return false;
                }

                foreach (string item in runKey.GetValueNames())
                {
                    if (item.Equals(Key, StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    runKey.Close();
                    runKey.Dispose();
                }
            }
        }

    }
}
