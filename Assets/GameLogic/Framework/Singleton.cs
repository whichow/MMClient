//namespace Framework
//{
//    public class Singleton<T> where T : class, new()
//    {
//        // 单件子类实例
//        private static T _instance;

//        protected Singleton()
//        {
//        }

//        /// <summary>
//        ///     获得类型的单件实例
//        /// </summary>
//        /// <returns>类型实例</returns>
//        public static T Instance()
//        {
//            if (null == _instance)
//            {
//                _instance = new T();
//            }

//            return _instance;
//        }

//        /// <summary>
//        /// 删除单件实例
//        /// </summary>
//        public static void DestroyInstance()
//        {
//            _instance = null;
//        }
//    }
//}