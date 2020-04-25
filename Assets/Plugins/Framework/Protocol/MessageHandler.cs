// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 2016-10-12
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Reflection;
using K.Events;

namespace K.Protocol
{
    /// <summary>
    /// 消息处理器
    /// </summary>
    public class MessageHandler
    {
        #region STATIC

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<MessageID, SimpleEvent<Message>> _MessageHandleFunction = new Dictionary<MessageID, SimpleEvent<Message>>();

        /// <summary>
        /// Attribute获取
        /// </summary> 
        //static MessageHandler()
        public static void Init()
        {

            var allTypes = new List<Type>();

            var execAssembly = Assembly.GetExecutingAssembly();
            if (execAssembly != null)
            {
                allTypes.AddRange(execAssembly.GetTypes());
            }

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null && entryAssembly != execAssembly)
            {
                allTypes.AddRange(entryAssembly.GetTypes());
            }

            var callAssembly = Assembly.GetCallingAssembly();
            if (callAssembly != null && callAssembly != execAssembly && callAssembly != entryAssembly)
            {
                allTypes.AddRange(callAssembly.GetTypes());
            }

            var typeAssembly = Assembly.GetAssembly(typeof(Message));
            if (typeAssembly != null && typeAssembly != execAssembly && typeAssembly != entryAssembly && typeAssembly != callAssembly)
            {
                allTypes.AddRange(typeAssembly.GetTypes());
            }

            var msgHandlerType = typeof(MessageHandler);
            foreach (var type in allTypes)
            {
                if (!type.IsSubclassOf(msgHandlerType))
                {
                    continue;
                }
                var staticMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                foreach (var staticMethod in staticMethods)
                {
                    var methodCustomAttributes = staticMethod.GetCustomAttributes(typeof(MessageHandlerAttribute), false);
                    if (methodCustomAttributes == null || methodCustomAttributes.Length == 0)
                    {
                        continue;
                    }
                    foreach (var methodCustomAttribute in methodCustomAttributes)
                    {
                        var msgID = ((MessageHandlerAttribute)methodCustomAttribute).messageID;
                        var msgHandler = Delegate.CreateDelegate(typeof(Action<Message>), staticMethod) as Action<Message>;
                        RegisterMessageHandler(msgID, msgHandler);
                    }
                }
            }
        }

        /// <summary>
        /// 手动注册消息处理机             
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="msgHandler"></param>
        public static void RegisterMessageHandler(MessageID msgID, Action<Message> msgHandler)
        {
            SimpleEvent<Message> handles;
            if (!_MessageHandleFunction.TryGetValue(msgID, out handles))
            {
                _MessageHandleFunction.Add(msgID, handles = new SimpleEvent<Message>());
            }
            handles.Add(msgHandler);
        }
        /// <summary>
        /// 发送消息到处理机
        /// </summary>
        /// <param name="msg"></param>
        public static void Send(Message msg)
        {
            if (msg != null)
            {
                SimpleEvent<Message> handles;
                if (_MessageHandleFunction.TryGetValue(msg.messageID, out handles))
                {
                    handles.Invoke(msg);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 只能绑定公开静态函数方法
    /// 签名(public static void Func(Message))
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MessageHandlerAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageID messageID
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        public MessageHandlerAttribute(short msgID)
        {
            this.messageID = msgID;
        }
    }
}