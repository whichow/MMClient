// ***********************************************************************
// Company          : 
// Author           : KimCh
// Created          : 
//
// Last Modified By : KimCh
// Last Modified On : 
// ***********************************************************************
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class BuildAnimation : Editor
{
    //生成出的AnimationController的路径
    private static string FxAnimatorControllerPath = "Assets/Resources/Animations/M3";
    //生成出的Animation的路径
    private static string FxAnimationClipPath = "Assets/Resources/Animations/M3";
    //生成出的Prefab的路径
    private static string FxPrefabPath = "Assets/Res/Match3/Fx";
    //美术给的原始图片路径
    private static string FxTexturePath = Application.dataPath + "/GameRes/Textures/Match3/Fx";

    /// <summary>
    /// 美术给的原始图片路径
    /// </summary>
    private static string ItemTexturePath = Application.dataPath + "/GameRes/Textures/Match3/Items";
    /// <summary>
    /// 生成出的AnimationController的路径
    /// </summary>
    private static string ItemAnimatorControllerPath = "Assets/Resources/Animations/M3";
    /// <summary>
    /// 生成出的Animation的路径
    /// </summary>
    private static string ItemAnimationClipPath = "Assets/Resources/Animations/M3";
    /// <summary>
    /// 生成出的Prefab的路径
    /// </summary>
    private static string ItemPrefabPath = "Assets/Res/Match3/Items";

    [MenuItem("Game/Animation/BuildItem")]
    static void BuildAniamtion1()
    {
        BuildItem(false);
    }

    [MenuItem("Game/Animation/BuildFx")]
    static void BuildFx1()
    {
        BuildFx(false);
    }

    static void BuildFx(bool legacy)
    {
        var rootDir = new DirectoryInfo(FxTexturePath);
        if (legacy)
        {
            foreach (var dir in rootDir.GetDirectories())
            {
                //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
                bool loop = dir.Name.Contains("flag");
                BuildAnimationClip(dir, true, loop, FxAnimationClipPath);
            }
        }
        else
        {
            foreach (var dir in rootDir.GetDirectories())
            {
                var clipList = new List<AnimationClip>();
                //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
                bool loop = dir.Name.Contains("flag");
                clipList.AddRange(BuildAnimationClip(dir, false, loop, FxAnimationClipPath));
                var controller = BuildAnimatorController(clipList.ToArray(), dir.Name, FxAnimatorControllerPath);
                BuildPrefab(dir, controller, FxPrefabPath);
            }
        }
    }

    static void BuildItem(bool legacy)
    {
        var rootDir = new DirectoryInfo(ItemTexturePath);
        foreach (var dir in rootDir.GetDirectories())
        {
            if (legacy)
            {
                foreach (var subDir in dir.GetDirectories())
                {
                    //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
                    var arr = subDir.Name.Split('_');
                    string type = arr[arr.Length - 1];
                    bool loop = type != "die";
                    BuildAnimationClip(subDir, true, loop, ItemAnimationClipPath);
                }
            }
            else
            {
                var clipList = new List<AnimationClip>();
                foreach (var subDir in dir.GetDirectories())
                {
                    //每个文件夹就是一组帧动画，这里把每个文件夹下的所有图片生成出一个动画文件
                    var arr = subDir.Name.Split('_');
                    string type = arr[arr.Length - 1];
                    bool loop = type != "die";
                    clipList.AddRange(BuildAnimationClip(subDir, false, loop, ItemAnimationClipPath));
                }

                //把所有的动画文件生成在一个AnimationController里
                var controller = BuildAnimatorController(clipList.ToArray(), dir.Name, ItemAnimatorControllerPath);

                //最后生成程序用的Prefab文件
                BuildPrefab(dir, controller, ItemPrefabPath);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <returns></returns>
    static AnimationClip[] BuildAnimationClip(DirectoryInfo directoryInfo, bool legacy, bool loop, string path)
    {
        var clipList = new List<AnimationClip>();

        string animationName = directoryInfo.Name;
        //查找所有图片，因为我找的动画是 
        FileInfo[] images = directoryInfo.GetFiles("*.png");

        var clip = new AnimationClip();
        clipList.Add(clip);

        clip.name = animationName;
        clip.legacy = legacy;
        //动画帧率，30比较合适
        clip.frameRate = 30;

        var curveBinding = new EditorCurveBinding();

        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";

        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        var frameTime = 1f / 10f;
        var animKeyFrames = new ObjectReferenceKeyframe[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images[i].FullName));
            animKeyFrames[i] = new ObjectReferenceKeyframe();
            animKeyFrames[i].time = frameTime * i;
            animKeyFrames[i].value = sprite;
        }

        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, animKeyFrames);

        //有些动画我希望天生它就动画循环
        if (loop)
        {
            //设置idle文件为循环动画
            var serializedClip = new SerializedObject(clip);
            var clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
            clipSettings.loopTime = true;
            serializedClip.ApplyModifiedProperties();
        }

        return clipList.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clips"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static AnimatorController BuildAnimatorController(AnimationClip[] clips, string name, string path)
    {
        var controller = AnimatorController.CreateAnimatorControllerAtPath(path + Path.DirectorySeparatorChar + name + ".controller");

        var layer = controller.layers[0];
        var stateMachine = layer.stateMachine;

        foreach (var clip in clips)
        {
            var state = stateMachine.AddState(clip.name);
            state.motion = clip;

            AssetDatabase.AddObjectToAsset(clip, controller);
        }

        foreach (var state in stateMachine.states)
        {
            if (state.state.name == "idle")
            {
                stateMachine.defaultState = state.state;
            }
        }

        AssetDatabase.SaveAssets();
        return controller;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <param name="animatorCountorller"></param>
    private static void BuildPrefab(DirectoryInfo directoryInfo, AnimatorController animatorCountorller, string path)
    {
        var gObj = new GameObject();
        gObj.name = directoryInfo.Name;

        var spriteRender = gObj.AddComponent<SpriteRenderer>();

        //生成Prefab 添加一张预览用的Sprite
        //var images = directory.GetDirectories()[0].GetFiles("*.png")[0];
        //spriteRender.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images.FullName));

        var animator = gObj.AddComponent<Animator>();
        animator.runtimeAnimatorController = animatorCountorller;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        PrefabUtility.CreatePrefab(path + "/" + gObj.name + ".prefab", gObj);
        DestroyImmediate(gObj);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            return path.Substring(path.IndexOf("Assets\\"));
        }
        else
        {
            return path.Substring(path.IndexOf("Assets/"));
        }
    }


    class AnimationClipSettings
    {
        SerializedProperty m_Property;

        private SerializedProperty Get(string property) { return m_Property.FindPropertyRelative(property); }

        public AnimationClipSettings(SerializedProperty prop) { m_Property = prop; }

        public float startTime { get { return Get("m_StartTime").floatValue; } set { Get("m_StartTime").floatValue = value; } }
        public float stopTime { get { return Get("m_StopTime").floatValue; } set { Get("m_StopTime").floatValue = value; } }
        public float orientationOffsetY { get { return Get("m_OrientationOffsetY").floatValue; } set { Get("m_OrientationOffsetY").floatValue = value; } }
        public float level { get { return Get("m_Level").floatValue; } set { Get("m_Level").floatValue = value; } }
        public float cycleOffset { get { return Get("m_CycleOffset").floatValue; } set { Get("m_CycleOffset").floatValue = value; } }

        public bool loopTime { get { return Get("m_LoopTime").boolValue; } set { Get("m_LoopTime").boolValue = value; } }
        public bool loopBlend { get { return Get("m_LoopBlend").boolValue; } set { Get("m_LoopBlend").boolValue = value; } }
        public bool loopBlendOrientation { get { return Get("m_LoopBlendOrientation").boolValue; } set { Get("m_LoopBlendOrientation").boolValue = value; } }
        public bool loopBlendPositionY { get { return Get("m_LoopBlendPositionY").boolValue; } set { Get("m_LoopBlendPositionY").boolValue = value; } }
        public bool loopBlendPositionXZ { get { return Get("m_LoopBlendPositionXZ").boolValue; } set { Get("m_LoopBlendPositionXZ").boolValue = value; } }
        public bool keepOriginalOrientation { get { return Get("m_KeepOriginalOrientation").boolValue; } set { Get("m_KeepOriginalOrientation").boolValue = value; } }
        public bool keepOriginalPositionY { get { return Get("m_KeepOriginalPositionY").boolValue; } set { Get("m_KeepOriginalPositionY").boolValue = value; } }
        public bool keepOriginalPositionXZ { get { return Get("m_KeepOriginalPositionXZ").boolValue; } set { Get("m_KeepOriginalPositionXZ").boolValue = value; } }
        public bool heightFromFeet { get { return Get("m_HeightFromFeet").boolValue; } set { Get("m_HeightFromFeet").boolValue = value; } }
        public bool mirror { get { return Get("m_Mirror").boolValue; } set { Get("m_Mirror").boolValue = value; } }
    }
}
