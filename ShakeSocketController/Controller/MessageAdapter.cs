using ShakeSocketController.Handler;
using ShakeSocketController.Model;
using ShakeSocketController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShakeSocketController.Controller
{
    /// <summary>
    /// 消息适配器，处理常规数据报文
    /// </summary>
    public static class MessageAdapter
    {
        // TODO: 在HandleUDPPacket方法里合理处理这两个字段，以及考虑其可访问性修饰符
        public static MsgPacket lastRecPacket;
        public static DateTime lastRecDateTime;


        public static void HandleUDPPacket(TransactionController controller,
            MsgPacket packet, IPEndPoint ipe)
        {
            // TODO: 处理数据报文的入口方法，根据这里定下接口
            //根据数据报文创建对应的Handler
            //CreateMsgHandler(packet);
            if (VerifyVerbose(packet))
            {
                //冗余报文，执行冗余处理
            }
            else if (packet.IsTestOP)
            {
                //测试报文，执行测试处理
            }
            else
            {
                if (packet.NeedRespond)
                {
                    //需要回复
                    if (packet.RespondFirst)
                    {
                        //先回复，后执行
                        MsgPacket respondPacket = null;//伪代码
                        RespondUDPPacket(controller, respondPacket, ipe);
                        //...
                    }
                    else
                    {
                        //先执行，后回复
                    }
                }
                else
                {
                    //不需要回复，直接执行处理即可
                }
            }
        }

        /// <summary>
        /// UDP回复指定的数据报文对象
        /// </summary>
        /// <param name="controller">全局控制器的引用</param>
        /// <param name="packet">要发送的数据报文对象</param>
        /// <param name="ipe">目标地址</param>
        public static void RespondUDPPacket(TransactionController controller,
            MsgPacket packet, IPEndPoint ipe)
        {
            try
            {
                //发送数据报文
                controller.SendUDPMsgPacket(packet, ipe);
                // TODO: 通知发送完成？
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
        }

        /// <summary>
        /// 验证指定的数据报文是否确实为冗余重复报文
        /// </summary>
        /// <param name="packet">要验证的新接收的数据报文</param>
        public static bool VerifyVerbose(MsgPacket packet)
        {
            DateTime nowTime = DateTime.Now;
            //先验证是否确实为冗余报文
            if (packet != null && packet.IsVerbose && packet.VerboseEquals(lastRecPacket))
            {
                //再验证是否在冗余超时期间内
                if ((nowTime - lastRecDateTime).TotalMilliseconds <=
                    Program.MainController.CurConfig.MsgReceiveTimeout)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 将数据报文打包为对象
        /// </summary>
        /// <param name="msgData">完整数据报文Buf</param>
        /// <returns>返回数据报文对象</returns>
        public static MsgPacket PackMsgData(byte[] msgData)
        {
            try
            {
                return new MsgPacket(msgData);
            }
            catch (Exception e)
            {
                Logging.Error(e);
                return null;
            }
        }

        public static IMsgHandler CreateMsgHandler(MsgPacket packet)
        {
            // TODO: 利用反射预存各Handler类，根据主功能码使用反射实例化对象
            IMsgHandler msgHandler = null;
            switch (packet.HeaderHexStr)
            {
                case MsgPacket.TYPE_IP:
                    msgHandler = new IPMsgHandler(packet.DataHexStr);
                    break;
                case MsgPacket.TYPE_CMD:
                    msgHandler = new CmdMsgHandler(packet.DataHexStr);
                    break;
                case MsgPacket.TYPE_ANS:
                    msgHandler = new AnsMsgHandler(packet.DataHexStr);
                    break;
                case MsgPacket.TYPE_GUN:
                    msgHandler = new GunMsgHandler(packet.DataHexStr);
                    break;
            }
            return msgHandler;
        }

        public static IMsgHandler CreateAnsHandler(AnsData ansData)
        {
            try
            {
                return new AnsMsgHandler(StrUtil.ObjectToJson(ansData));
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            return null;
        }

    }
}
