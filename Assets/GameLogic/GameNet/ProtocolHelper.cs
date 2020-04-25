/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.5
 * 
 * Author:          Coamy
 * Created:	        2019/3/13 10:10:10
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;

namespace Game
{
    class ProtocolHelper
    {
        /// <summary>
        /// 获取消息ID
        /// </summary>
        /// <param name="tt"></param>
        /// <returns></returns>
        public static int GetMsgID(Type tt)
        {
            int msgID = 0;
            Type types = tt.GetNestedType("Types");
            Type eProtocol = types.GetNestedType("EProtocol");
            msgID = Convert.ToInt16(Enum.GetValues(eProtocol).GetValue(1));
            return msgID;
        }

        /// <summary>
        /// 获取消息ID
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static int GetMsgID(MessageDescriptor descriptor)
        {
            int msgID = 0;
            var enumTypes = descriptor.EnumTypes;
            for (int i = 0; i < enumTypes.Count; i++)
            {
                if (enumTypes[i].Name == "EProtocol")
                {
                    var values = enumTypes[i].Values;
                    for (int j = values.Count - 1; j >= 0; j--)
                    {
                        if (values[j].Name == "ProtoID")
                        {
                            msgID = values[j].Number;
                            break;
                        }
                    }
                    break;
                }
            }
            if (msgID == 0)
            {
                Debuger.Log(">>>[GetMsgID] 找不到ProtoID! " + descriptor.Name);
            }
            return msgID;
        }

    }
}