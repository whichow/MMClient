// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
namespace K.AB
{

    public enum AssetType
    {
        /// <summary>
        /// 普通资源
        /// </summary>
        Asset = 1,
        /// <summary>
        /// 内置
        /// </summary>
        Builtin = 2,
        /// <summary>
        ///(Resources目录)
        /// </summary>
        Resource = 4,
        /// <summary>
        /// 材质球
        /// </summary>
        Material = 8,
    }


    public enum BuildType
    {
        /// <summary>
        /// 根
        /// </summary>
        Root = 1,
        /// <summary>
        /// 普通素材，被根素材依赖的
        /// </summary>
        Asset = 2,
        /// <summary>
        /// 需要单独打包，说明这个素材是被两个或以上的素材依赖的
        /// </summary>
        Standalone = 4,
    }

}