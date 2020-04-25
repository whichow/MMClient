/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/5/24 15:38:26
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HeadIconUtils
    {
        /// <summary>
        /// 自定义头像本地缓存目录
        /// </summary>
        public static string HeadIconCacheDir = "HeadIcon/";
        public static string HeadIconCachePath;

        /// <summary>
        /// 自定义头像
        /// </summary>
        public static void PickHeadIcon()
        {
            CustomHeadImgPlugins.OpenAlbum();
        }

        /// <summary>
        /// 设置玩家头像
        /// </summary>
        /// <param name="headId"></param>
        /// <param name="playerId"></param>
        /// <param name="img"></param>
        public static void SetHeadIcon(int headId, int playerId, Image img)
        {
            if (headId <= 0)
            {
                string fileName = string.Format("{0}.jpg", playerId);
                SetCustomHeadIcon(fileName, img);
            }
            else
            {
                img.overrideSprite = KIconManager.Instance.GetHeadIcon(headId);
            }
        }

        /// <summary>
        /// 设置自定义头像
        /// </summary>
        /// <param name="img"></param>
        /// <param name="iconName"></param>
        public static void SetCustomHeadIcon(string iconName, Image img)
        {
            string path = Path.Combine(Application.persistentDataPath, HeadIconCacheDir, iconName);
            if (File.Exists(path))
            {
                ImageUtils.LoadLocalImage(HeadIconCacheDir + iconName, img);
            }
            else
            {
                ImageUtils.LoadNetImage(iconName, img, (textue) =>
                {
                    //缓存图片
                    //if (textue != null)
                    //{
                    //    if (string.IsNullOrEmpty(HeadIconCachePath))
                    //    {
                    //        HeadIconCachePath = Path.Combine(Application.persistentDataPath, HeadIconCacheDir);
                    //        if (!Directory.Exists(HeadIconCachePath))
                    //        {
                    //            Directory.CreateDirectory(HeadIconCachePath);
                    //        }
                    //    }

                    //    byte[] data = textue.EncodeToJPG();
                    //    FileStream stream = new FileStream(path, FileMode.Create);
                    //    stream.Write(data, 0, data.Length);
                    //    stream.Flush();
                    //    stream.Close();
                    //}
                });
            }
        }

    }
}
