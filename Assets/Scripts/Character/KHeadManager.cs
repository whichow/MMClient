//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Game
//{
//    public class KHeadManager : SingletonUnity<KHeadManager>
//    {

//        private List<KHead> _allHeadTexture = new List<KHead>();

//        public KHead[] allHeadTexture
//        {
//            get { return _allHeadTexture.ToArray(); }
//        }

//        public int allHeadTextureCount
//        {
//            get { return _allHeadTexture.Count; }
//        }

//        IEnumerator Start()
//        {
//            yield return null;
//            yield return null;
//            yield return null;
//            yield return null;
//            yield return null;

//            for (int i = 0; i < 2; i++)
//            {
//                _allHeadTexture.Add(new KHead
//                {
//                    id = i + 01,
//                    icon = "Icon_Touxiang1_0" + (i + 1),
//                    headType = KHead.HeadType.人的头像,
//                    Lvl = 0,
//                    locked = false,
//                }
//                );
//            }
//            for (int i = 1001; i < 1007; i++)
//            {
//                _allHeadTexture.Add(new KHead
//                {
//                    id = i,
//                    icon = "Icon_Touxiang1_" + i,
//                    headType = KHead.HeadType.猫的头像,
//                    Lvl = 1,
//                    locked = false,
//                }
//                );
//            }
//            for (int i = 2001; i < 2007; i++)
//            {
//                _allHeadTexture.Add(new KHead
//                {
//                    id = i,
//                    icon = "Icon_Touxiang1_" + i,
//                    headType = KHead.HeadType.猫的头像,
//                    Lvl = 1,
//                    locked = false,
//                }
//                );
//            }
//            for (int i = 3001; i < 3007; i++)
//            {
//                _allHeadTexture.Add(new KHead
//                {
//                    id = i,
//                    icon = "Icon_Touxiang1_" + i,
//                    headType = KHead.HeadType.猫的头像,
//                    Lvl = 1,
//                    locked = false,
//                }
//                );
//            }
//            for (int i = 4001; i < 4011; i++)
//            {
//                _allHeadTexture.Add(new KHead
//                {
//                    id = i,
//                    icon = "Icon_Touxiang1_" + i,
//                    headType = KHead.HeadType.猫的头像,
//                    Lvl = 1,
//                    locked = false,
//                }
//                );
//            }


//        }
//    }

//    public class KHead
//    {
//        public int id;
//        public string icon;
//        public HeadType headType;
//        public int Lvl;
//        public bool locked;
//        public enum HeadType
//        {
//            人的头像 = 1,
//            猫的头像 = 2,
//            头像框 = 3,
//        }
//    }


//}
