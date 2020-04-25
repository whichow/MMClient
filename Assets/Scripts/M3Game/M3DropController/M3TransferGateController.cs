///** 
// *FileName:     M3TransferGateController.cs 
// *Author:       HeJunJie 
// *Version:      1.0 
// *UnityVersion：5.6.2f1
// *Date:         2017-07-27 
// *Description:    
// *History: 
//*/ 
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class M3TransferController : Singleton<M3TransferController> {

//    private Dictionary<int, Int2> transferGateDic;
//    public float exitOffset = 0.7f;
//    public void Init()
//    {
//        transferGateDic = new Dictionary<int, Int2>();
//    }

//    public void AddGate(int elementId, Int2 pos)
//    {
//        if (transferGateDic.ContainsKey(elementId))
//        {
//            Debug.LogError("传送门重复注册----ID :" + elementId + "   位置" + pos.x + "|" + pos.y);
//            return;
//        }
//        else
//        {
//            transferGateDic.Add(elementId, pos);
//        }
//    }

//    public Int2 GetLinkPos(int elementId)
//    {
//        Int2 pos = new Int2();
//        if (transferGateDic.ContainsKey(elementId))
//        {
//            pos= transferGateDic[elementId];
//        }
//        else
//        {
//            Debug.Log("未注册传送门ID   " + elementId);
//        }
//        return pos;
//    }
//}
