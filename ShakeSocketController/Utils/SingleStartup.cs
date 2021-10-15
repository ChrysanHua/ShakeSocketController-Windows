using ShakeSocketController.Controller;
using System;
using System.Threading;

namespace ShakeSocketController.Utils
{
    /// <summary>
    /// 为当前应用程序设置单例启动（防多开）
    /// </summary>
    public static class SingleStartup
    {
        private static EventWaitHandle repeatAutoSignal;        //全局进程同步事件
        private static Thread bgListenThread;                   //后台监听线程
        private static CancellationTokenSource unregisterCTS;   //结束线程信号标志

        /// <summary>
        /// 当前进程注册失败时发生（注意：要在调用Register()方法之前绑定才有效）
        /// </summary>
        public static event EventHandler EndStartup;
        /// <summary>
        /// 当前应用程序出现多开时发生
        /// </summary>
        public static event EventHandler RepeatStarting;
        /// <summary>
        /// 后台监听线程出现异常时发生
        /// </summary>
        public static event EventHandler BGListenerException;

        /// <summary>
        /// 注册的系统全局唯一限定名称
        /// </summary>
        public static string GlobalName { get; private set; }
        /// <summary>
        /// 返回当前进程是否已注册（是否正在监听多开）
        /// </summary>
        public static bool IsListening =>
            bgListenThread == null ? false : bgListenThread.IsAlive;

        /// <summary>
        /// 注册防止多开，注册成功后可以限制当前应用程序后续的重复运行；
        /// 返回true则意味着当前是首次启动，并将会开始在后台监听后续发生的多开
        /// </summary>
        /// <param name="globalName">用于注册的系统全局唯一限定名称（应以‘Global\’开头）</param>
        /// <returns>注册是否成功</returns>
        public static bool Register(string globalName)
        {
            if (string.IsNullOrWhiteSpace(globalName))
                throw new ArgumentNullException(nameof(globalName));
            if (repeatAutoSignal != null)
                throw new InvalidOperationException("SingleStartup has been registered!");

            try
            {
                GlobalName = globalName;
                //create system global EventWaitHandle
                repeatAutoSignal = new EventWaitHandle(false,
                    EventResetMode.AutoReset, globalName, out bool isSingle);

                if (isSingle)
                {
                    //this is the single one, create the listenThread
                    unregisterCTS = new CancellationTokenSource();
                    bgListenThread = new Thread(BGListenWork)
                    {
                        Name = globalName + "_Thread",
                        IsBackground = true,
                    };

                    //begin background listening
                    bgListenThread.Start(unregisterCTS.Token);
                    return true;
                }
                else
                {
                    //not the single one, give a signal to the running one
                    repeatAutoSignal.Set();
                    Unregister(false);
                    EndStartup?.Invoke(null, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                Unregister(false);
            }

            return false;
        }

        /// <summary>
        /// 注销注册并停止后台监听
        /// </summary>
        public static void Unregister() => Unregister(true);

        private static void Unregister(bool callByOutside)
        {
            if (callByOutside)
            {
                //stop BGListener
                unregisterCTS?.Cancel();
                repeatAutoSignal?.Set();
            }

            unregisterCTS?.Dispose();
            unregisterCTS = null;
            repeatAutoSignal?.Close();
            repeatAutoSignal?.Dispose();
            repeatAutoSignal = null;
        }

        private static void BGListenWork(object paraObj)
        {
            Logging.Debug("SingleStartup's listenThread started");

            try
            {
                var ctsToken = (CancellationToken)paraObj;
                while (!ctsToken.IsCancellationRequested)
                {
                    //get the repeatSignal, which means repeated startup
                    repeatAutoSignal.WaitOne();
                    if (!ctsToken.IsCancellationRequested)
                    {
                        RepeatStarting?.Invoke(null, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                BGListenerException?.Invoke(null, EventArgs.Empty);
            }
            finally
            {
                //in any case, take the initiative to dispose at the end of the listenThread
                Unregister(false);
                Logging.Debug("SingleStartup's listenThread stopped");
            }
        }
    }
}
