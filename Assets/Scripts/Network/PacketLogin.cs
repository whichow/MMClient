//// ***********************************************************************
//// Company          : 
//// Author           : KimCh
//// Created          : 
////
//// Last Modified By : KimCh
//// Last Modified On : 
//// ***********************************************************************
//namespace Game
//{
//    using Google.Protobuf;
//    using Msg.ClientMessage;
//    using System.Collections;

//    public class PacketLogin : PacketBinary
//    {
//        //客户端版本
//        //移动终端操作系统版本
//        //移动终端机型
//        //运营商
//        //4G/3G/WIFI/2G
//        //显示屏宽度
//        //显示屏高度
//        //像素密度
//        //cpu类型|频率|核数
//        //opengl render信息
//        //opengl版本信息
//        //设备ID
//        protected override void GenerateData(IMessage data)
//        {
//            var loginRequest = new C2SLoginRequest()
//            {
//                Acc = KUser.AccountID,
//                //Token = KUser.AccountToken
//            };
//            base.GenerateData(loginRequest);
//        }

//        protected override void HandlingData(ArrayList dataList)
//        {
//            for (int i = 0; i < dataList.Count; i++)
//            {
//                var loginResponse = dataList[i] as S2CLoginResponse;
//                if (loginResponse != null)
//                {
//                    //KUser.CreateSelfPlayer(loginResponse.PlayerId, loginResponse.Name, loginResponse.Acc);
//                    KUser.CreateSelfPlayer(0, loginResponse.PlayerInfo.PlayerName, loginResponse.Acc);
//                }
//            }

//            base.HandlingData(dataList);
//        }
//    }
//}