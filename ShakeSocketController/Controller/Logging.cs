using ShakeSocketController.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ShakeSocketController.Controller
{
    public static class Logging
    {
        private static FileStream _fs;
        private static StreamWriterWithTimestamp _sw;

        public static string LogFilePath { get; private set; }
        public static bool IsLogToFile => (_fs != null && _sw != null);

        /// <summary>
        /// After Init, the log is output to a file
        /// </summary>
        public static bool Init(string logFileName)
        {
            if (string.IsNullOrWhiteSpace(logFileName))
                throw new ArgumentNullException(nameof(logFileName));
            if (IsLogToFile)
                throw new InvalidOperationException("Logging has been initialized!");
            
            try
            {
                LogFilePath = SysUtil.GetTempPath(logFileName);

#if DEBUG       //in DEBUG mode, it is also output to the Console,
                //and the logFile is cleared each run
                _fs = new FileStream(LogFilePath, FileMode.Create);
#else
                _fs = new FileStream(LogFilePath, FileMode.Append);
#endif

                _sw = new StreamWriterWithTimestamp(_fs)
                {
                    AutoFlush = true
                };

                Console.SetOut(_sw);

#if !DEBUG
                Console.SetError(_sw);
#endif

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _sw?.Close();
                _sw?.Dispose();
                _sw = null;
                _fs?.Close();
                _fs?.Dispose();
                _fs = null;
                return false;
            }
        }

        private static void WriteToLogFile(object o)
        {
            //write a line without the timestamp
            try
            {
                Console.Write(o);
                Console.Write(Environment.NewLine);

#if DEBUG
                if (IsLogToFile)
                {
                    Console.Error.Write(o);
                    Console.Error.Write(Environment.NewLine);
                }
#endif
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private static void WriteLineToLogFile(object o)
        {
            try
            {
                Console.WriteLine(o);

#if DEBUG
                if (IsLogToFile)
                    Console.Error.WriteLine(o);
#endif
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public static void Error(object o)
        {
            WriteLineToLogFile("[E] " + o);
        }

        public static void Info(object o)
        {
            WriteLineToLogFile(o);
        }

        public static void SplitLine(int len = 25, char symbol = '-')
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(symbol, len);
            WriteToLogFile(sb.ToString());
        }

        public static void Clear()
        {
            _sw?.Close();
            _sw?.Dispose();
            _sw = null;
            _fs?.Close();
            _fs?.Dispose();
            _fs = null;

            if (!string.IsNullOrEmpty(LogFilePath))
            {
                File.Delete(LogFilePath);
                Init(LogFilePath);
            }
        }

        [Conditional("DEBUG")]
        public static void Debug(object o)
        {
            WriteLineToLogFile("[D] " + o);
        }

        [Conditional("DEBUG")]
        public static void Dump(string tag, byte[] arr, int length)
        {
            var sb = new StringBuilder($"{Environment.NewLine}{tag}: ");
            for (int i = 0; i < length - 1; i++)
            {
                sb.Append($"0x{arr[i]:X2}, ");
            }
            sb.Append($"0x{arr[length - 1]:X2}");
            sb.Append(Environment.NewLine);
            Debug(sb.ToString());
        }

        [Conditional("DEBUG")]
        public static void Debug(EndPoint local, EndPoint remote, int len, string header = null, string tailer = null)
        {
            if (header == null && tailer == null)
                Debug($"{local} => {remote} (size={len})");
            else if (header == null && tailer != null)
                Debug($"{local} => {remote} (size={len}), {tailer}");
            else if (header != null && tailer == null)
                Debug($"{header}: {local} => {remote} (size={len})");
            else
                Debug($"{header}: {local} => {remote} (size={len}), {tailer}");
        }

        [Conditional("DEBUG")]
        public static void Debug(Socket sock, int len, string header = null, string tailer = null)
        {
            Debug(sock.LocalEndPoint, sock.RemoteEndPoint, len, header, tailer);
        }

        public static void LogUsefulException(Exception e)
        {
            // just log useful exceptions, not all of them
            if (e is SocketException se)
            {
                if (se.SocketErrorCode == SocketError.ConnectionAborted)
                {
                    // closed by browser when sending
                    // normally happens when download is canceled or a tab is closed before page is loaded
                }
                else if (se.SocketErrorCode == SocketError.ConnectionReset)
                {
                    // received rst
                }
                else if (se.SocketErrorCode == SocketError.NotConnected)
                {
                    // The application tried to send or receive data, and the System.Net.Sockets.Socket is not connected.
                }
                else if (se.SocketErrorCode == SocketError.HostUnreachable)
                {
                    // There is no network route to the specified host.
                }
                else if (se.SocketErrorCode == SocketError.TimedOut)
                {
                    // The connection attempt timed out, or the connected host has failed to respond.
                }
                else
                {
                    Info(e);
                }
            }
            else if (e is ObjectDisposedException)
            {
            }
            else if (e is Win32Exception win32Ex)
            {
                // Win32Exception (0x80004005): A 32 bit processes cannot access modules of a 64 bit process.
                if ((uint)win32Ex.ErrorCode != 0x80004005)
                {
                    Info(e);
                }
            }
            else
            {
                Info(e);
            }
        }
    }

    // Simply extended System.IO.StreamWriter for adding timestamp workaround
    public class StreamWriterWithTimestamp : StreamWriter
    {
        public StreamWriterWithTimestamp(Stream stream) : base(stream)
        {
        }

        private string GetTimestamp()
        {
            return "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ";
        }

        //extend only the 'WriteLine' method
        public override void WriteLine(string value)
        {
            base.WriteLine(GetTimestamp() + value);
        }
    }
}
