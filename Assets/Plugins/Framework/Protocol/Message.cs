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
using System.IO;

namespace K.Protocol
{
    using Network;

    /// <summary>
    /// 传递数据最小单位
    /// </summary>
    public abstract class Message
    {
        #region CONST

        /// <summary>
        /// 消息版本号
        /// </summary>
        public const short CURRENT_MESSAGE_VERSION = 6;

        #endregion

        #region STATIC 

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<MessageID, string> _MessageNames = new Dictionary<MessageID, string>();
        /// <summary>
        /// 消息反序列化
        /// </summary>
        private static Dictionary<MessageID, Func<Stream, Message>> _MessageDeserializers = new Dictionary<MessageID, Func<Stream, Message>>();

        /// <summary>
        /// 后面修改为Attribute获取
        /// </summary>
        //static Message()
        public static void Init()
        {
            // 收集Message
            var allTypes = new List<Type>();

            var execAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            if (execAssembly != null)
            {
                allTypes.AddRange(execAssembly.GetTypes());
            }

            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly != null && entryAssembly != execAssembly)
            {
                allTypes.AddRange(entryAssembly.GetTypes());
            }

            var callAssembly = System.Reflection.Assembly.GetCallingAssembly();
            if (callAssembly != null && callAssembly != execAssembly && callAssembly != entryAssembly)
            {
                allTypes.AddRange(callAssembly.GetTypes());
            }

            var typeAssembly = System.Reflection.Assembly.GetAssembly(typeof(Message));
            if (typeAssembly != null && typeAssembly != execAssembly && typeAssembly != entryAssembly && typeAssembly != callAssembly)
            {
                allTypes.AddRange(typeAssembly.GetTypes());
            }

            var msgType = typeof(Message);
            foreach (var type in allTypes)
            {
                if (!type.IsSubclassOf(msgType))
                {
                    continue;
                }

                var staticMethods = type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                foreach (var staticMethod in staticMethods)
                {
                    var methodCustomAttributes = staticMethod.GetCustomAttributes(typeof(MessageDeserializerAttribute), false);
                    if (methodCustomAttributes == null || methodCustomAttributes.Length == 0)
                    {
                        continue;
                    }
                    foreach (var methodCustomAttribute in methodCustomAttributes)
                    {
                        var msgDeserializer = Delegate.CreateDelegate(typeof(Func<Stream, Message>), staticMethod) as Func<Stream, Message>;
                        short msgID = ((MessageDeserializerAttribute)methodCustomAttribute).messageID;
                        string msgName = ((MessageDeserializerAttribute)methodCustomAttribute).messageName;
                        RegisterMessageDeserializer(msgID, msgDeserializer, msgName);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="msg"></param>
        public static void Serialize(Stream stream, Message msg)
        {
            try
            {
                IO.BigEndianBinaryWriter.WriteInt16(stream, CURRENT_MESSAGE_VERSION);

                var msgID = msg.messageID;
                if (msgID == MessageID.Null)
                {
                    //Util.LogError(string.Format("非法消息, {0}!", msg.GetType().ToString()));
                    return;
                }

                IO.BigEndianBinaryWriter.WriteInt16(stream, msgID);

                msg.SerializeThis(stream);
            }
            catch (Exception ex)
            {
                //Util.LogException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Message Deserialize(Stream stream)
        {
            try
            {
                var version = IO.BigEndianBinaryReader.ReadInt16(stream);
                if (version != CURRENT_MESSAGE_VERSION)
                {
                    //Util.LogError(string.Format("错误的消息Version, {0}!", version));
                    return null;
                }

                var msgID = (MessageID)(IO.BigEndianBinaryReader.ReadInt16(stream));

                var deserializer = GetMessageDeserializer(msgID);
                if (deserializer == null)
                {
                    //Util.LogError(string.Format("错误的消息ID, {0}!", msgID));
                    return null;
                }
                return deserializer(stream);
            }
            catch (Exception ex)
            {
                //Util.LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// 手动注册消息
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="msgDeserializer"></param>
        /// <param name="msgName"></param>
        public static void RegisterMessageDeserializer(MessageID msgID, Func<Stream, Message> msgDeserializer, string msgName = null)
        {
            if (_MessageDeserializers.ContainsKey(msgID))
            {
                //Util.Log(string.Format("消息{0},已经存在.", msgID));
                return;
            }
            _MessageDeserializers.Add(msgID, msgDeserializer);

            if (!string.IsNullOrEmpty(msgName))
            {
                _MessageNames.Add(msgID, msgName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <returns></returns>
        public static string GetMessageNameByID(MessageID msgID)
        {
            if (msgID != MessageID.Null)
            {
                string value;
                _MessageNames.TryGetValue(msgID, out value);
                return value;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <returns></returns>
        static Func<Stream, Message> GetMessageDeserializer(MessageID msgID)
        {
            if (msgID != MessageID.Null)
            {
                Func<Stream, Message> deserializer;
                if (_MessageDeserializers.TryGetValue(msgID, out deserializer))
                {
                    return deserializer;
                }
            }
            return null;
        }

        #endregion

        #region MEMBER

        /// <summary>
        /// 
        /// </summary>
        public Connector connector
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime receiveTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract MessageID messageID
        {
            get;
        }

        #endregion

        #region METHOD

        protected abstract void SerializeThis(Stream stream);

        #endregion
    }

    /// <summary>
    /// 只能绑定公开静态方法 
    /// 签名(public static Message Func(Stream))
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MessageDeserializerAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public short messageID
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string messageName
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        public MessageDeserializerAttribute(short msgID)
        {
            this.messageID = msgID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="msgName"></param>
        public MessageDeserializerAttribute(short msgID, string msgName)
        {
            this.messageID = msgID;
            this.messageName = msgName;
        }
    }
}
