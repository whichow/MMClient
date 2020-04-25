/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/23 14:23:03
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using UnityEngine;

namespace Game
{
    public partial class SdkCallbackMessage
    {
        /// <summary>
        /// 保存头像文件成功
        /// </summary>
        /// <param name="str"></param>
        public void PickHeadImgSucc(string str)
        {
            Debuger.Log("保存图片成功: " + str);
            //string url = "file://" + Application.persistentDataPath + "/image.jpg";
            ImageUtils.LoadLocalTexture("image.jpg", (texture) =>
            {
                EventManager.Instance.GlobalDispatcher.DispatchEvent(GlobalEvent.CUSTOM_HEADIMG_PICK_SUCC, new EventData() { Data = texture });
                ImageUtils.UploadPicture(texture, PlayerDataModel.Instance.mPlayerData.mPlayerID + ".jpg", (s) =>
                {
                    Debuger.Log("上传图片完成: " + s);
                    EventManager.Instance.GlobalDispatcher.DispatchEvent(GlobalEvent.CUSTOM_HEADIMG_UPLOAD_SUCC);
                });
            });
        }

    }
}
