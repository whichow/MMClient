/*******************************************************************************
 * 公共语言运行库 (CLR) 的当前版本: 4.0.30319.42000
 * 
 * Author:          Coamy
 * Created:	        2019/4/26 12:19:17
 * Description:     
 * 
 * Update History:  
 * 
 *******************************************************************************/
using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssembleCharacter
{
    #region Plugins Menu

    [MenuItem("Tools/角色/Create Character Prefab (Form Directory)", false, 51)]
    private static void ManipulateCharacter()
    {
        if (Selection.objects.Length >= 1)
        {
            foreach (Object o in Selection.objects)
            {
                string dir = AssetDatabase.GetAssetPath(o);
                ManipulateCharacter(dir);
            }
        }
    }

    [MenuItem("Tools/角色/Create All Character Prefab（Form Root Directory)", false, 52)]
    private static void ManipulateAllCharacter()
    {
        string allDir = AssetDatabase.GetAssetPath(Selection.activeObject);
        string dataPath = Application.dataPath;
        string fullpath = dataPath + allDir.TrimStart("Assets".ToArray());

        string[] dirs = Directory.GetDirectories(fullpath);
        foreach (var dirName in dirs)
        {
            string dir = dirName.Substring(dirName.IndexOf("Assets"), dirName.Length - dirName.IndexOf("Assets"));
            dir = dir.Replace("\\", "/");
            ManipulateCharacter(dir);
        }
        Debug.Log("Create All Character 执行完毕！" + fullpath);
    }


    [MenuItem("Tools/角色服装/Create CharacterFashion Prefab (Form Directory)", false, 53)]
    private static void ManipulateCharacterFashion()
    {
        if (Selection.objects.Length >= 1)
        {
            foreach (Object o in Selection.objects)
            {
                string dir = AssetDatabase.GetAssetPath(o);
                ManipulateCharacterFashion(dir);
            }
        }
    }

    [MenuItem("Tools/角色服装/Create All CharacterFashion Prefab（Form Root Directory)", false, 54)]
    private static void ManipulateAllCharacterFashion()
    {
        string allDir = AssetDatabase.GetAssetPath(Selection.activeObject);
        string dataPath = Application.dataPath;
        string fullpath = dataPath + allDir.TrimStart("Assets".ToArray());

        string[] dirs = Directory.GetDirectories(fullpath);
        foreach (var dirName in dirs)
        {
            string dir = dirName.Substring(dirName.IndexOf("Assets"), dirName.Length - dirName.IndexOf("Assets"));
            dir = dir.Replace("\\", "/");
            ManipulateCharacterFashion(dir);
        }
        Debug.Log("Create All CharacterFashion 执行完毕！" + fullpath);
    }


    [MenuItem("Tools/宠物/Create Pet Prefab (Form Directory)", false, 55)]
    private static void ManipulatePet()
    {
        if (Selection.objects.Length >= 1)
        {
            foreach (Object o in Selection.objects)
            {
                string dir = AssetDatabase.GetAssetPath(o);
                ManipulatePet(dir);
            }
        }
    }

    [MenuItem("Tools/宠物/Create All Pet Prefab（Form Root Directory)", false, 56)]
    private static void ManipulateAllPet()
    {
        string allDir = AssetDatabase.GetAssetPath(Selection.activeObject);
        string dataPath = Application.dataPath;
        string fullpath = dataPath + allDir.TrimStart("Assets".ToArray());

        string[] dirs = Directory.GetDirectories(fullpath);
        foreach (var dirName in dirs)
        {
            string dir = dirName.Substring(dirName.IndexOf("Assets"), dirName.Length - dirName.IndexOf("Assets"));
            dir = dir.Replace("\\", "/");
            ManipulatePet(dir);
        }
        Debug.Log("Create All Pet 执行完毕！" + fullpath);
    }


    #endregion

    #region Character

    private static void ManipulateCharacter(string dir)
    {
        Debug.Log("开始 assemble Character: " + dir);

        if (EditorUtility.DisplayCancelableProgressBar("Assemble Character", "PrepareFbx", 0.1f))
        {
            EditorUtility.ClearProgressBar();
            return;
        }

        string name = dir.Substring(dir.LastIndexOf("/") + 1, dir.Length - dir.LastIndexOf("/") - 1);

        __PrepareFbx(dir + "/" + name + ".fbx", true);  // tpos

        __PrepareFbx(dir + "/" + name + "_idle.fbx", false, true);
        __PrepareFbx(dir + "/" + name + "_applause.fbx", false, false); //鼓掌
        __PrepareFbx(dir + "/" + name + "_costume.fbx", false, false);
        __PrepareFbx(dir + "/" + name + "_ranking.fbx", false, false);

        if (EditorUtility.DisplayCancelableProgressBar("Assemble Character", "Start Assemble Character", 0.9f))
        {
            EditorUtility.ClearProgressBar();
            return;
        }

        StartAssembleCharacter(dir);

        EditorUtility.ClearProgressBar();
    }

    private static void StartAssembleCharacter(string dir)
    {
        string name = dir.Substring(dir.LastIndexOf("/") + 1, dir.Length - dir.LastIndexOf("/") - 1);
        //string relPath = dir.TrimStart("Assets/".ToCharArray()); // 相对于Asset/的目录路径

        AnimatorController animatorController = CreateAnimatorControllerCharacter(dir, name);

        string goPath = dir + "/" + name + ".fbx";
        string prefabPath = dir + "/" + name + ".prefab";
        GameObject prefab = CreatePrefab(goPath, prefabPath, name);
        if (prefab == null)
        {
            return;
        }
        prefab.transform.Rotate(new Vector3(0, 150, 0));

        if (!ModifyMat(prefab, name, dir))
        {
            //return;
        }

        SetCollider(prefab);

        // 设置prefab的anim controller
        Animator amt = prefab.GetComponent<Animator>();
        amt.runtimeAnimatorController = animatorController;

        AssetDatabase.SaveAssets();

        PrefabMoveToResDir(prefabPath, name);
    }

    private static AnimatorController CreateAnimatorControllerCharacter(string dir, string name)
    {
        string fullAnimDirName = dir + "/Animator";
        string animControllerPath = dir + "/Animator/" + name + "Anim.controller";
        if (!Directory.Exists(fullAnimDirName))
            Directory.CreateDirectory(fullAnimDirName);

        // 创建animationController
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animControllerPath);
        // 得到它的Layer， 默认layer为base 可以去拓展
        AnimatorControllerLayer layer = animatorController.layers[0];

        // 创建parameters
        animatorController.AddParameter("idle", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("costume", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("applause", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("ranking", AnimatorControllerParameterType.Trigger);

        // 把动画文件保存在我们创建的AnimationController中
        AnimatorState idle = AddState("idle", dir + "/" + name + "_idle.fbx", layer, true);
        AnimatorState costume = AddState("costume", dir + "/" + name + "_costume.fbx", layer, false);
        AnimatorState applause = AddState("applause", dir + "/" + name + "_applause.fbx", layer, false);
        AnimatorState ranking = AddState("ranking", dir + "/" + name + "_ranking.fbx", layer, false);

        // 创建transition
        AnimatorStateMachine sm = layer.stateMachine;
        AddTransition(sm, idle, false, "idle");
        AddTransition(sm, costume, false, "costume");
        AddTransition(sm, applause, false, "applause");
        AddTransition(sm, ranking, false, "ranking");

        AddTransition(sm, costume, idle, true, "");
        AddTransition(sm, applause, idle, true, "");
        AddTransition(sm, ranking, idle, true, "");

        return animatorController;
    }

    #endregion

    #region CharacterFashion

    private static void ManipulateCharacterFashion(string dir)
    {
        Debug.Log("开始 assemble CharacterFashion: " + dir);

        if (EditorUtility.DisplayCancelableProgressBar("Assemble Character Fashion", "Start", 0f))
        {
            EditorUtility.ClearProgressBar();
            return;
        }

        string name = dir.Substring(dir.LastIndexOf("/") + 1, dir.Length - dir.LastIndexOf("/") - 1);

        string partNameH = name.Insert(1, "h");
        string partNameJ = name.Insert(1, "j");
        string partNameP = name.Insert(1, "p");
        string partNameS = name.Insert(1, "s");


        PrepareFbxCharacterFashion(dir + "/" + partNameH + ".fbx", 1 / 8f);
        PrepareFbxCharacterFashion(dir + "/" + partNameJ + ".fbx", 2 / 8f);
        PrepareFbxCharacterFashion(dir + "/" + partNameP + ".fbx", 3 / 8f);
        PrepareFbxCharacterFashion(dir + "/" + partNameS + ".fbx", 4 / 8f);

        StartAssembleCharacterFashion(dir, partNameH, 5 / 8f);
        StartAssembleCharacterFashion(dir, partNameJ, 6 / 8f);
        StartAssembleCharacterFashion(dir, partNameP, 7 / 8f);
        StartAssembleCharacterFashion(dir, partNameS, 8 / 8f);

        EditorUtility.ClearProgressBar();
    }

    private static void PrepareFbxCharacterFashion(string path, float progress = 0f)
    {
        if (EditorUtility.DisplayCancelableProgressBar("Assemble Character Fashion", "Prepare Fbx Character Fashion", progress))
        {
            EditorUtility.ClearProgressBar();
            return;
        }

        ModelImporter ip = null;
        try
        {
            ip = AssetImporter.GetAtPath(path) as ModelImporter;
            if (ip == null) return;
        }
        catch (Exception)
        {
            return;
        }

        ip.globalScale = 1;
        ip.importAnimation = false;
        ip.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
        ip.importNormals = ModelImporterNormals.Import;
        ip.importTangents = ModelImporterTangents.None;
        ip.meshCompression = ModelImporterMeshCompression.Medium;
        ip.isReadable = false;
        ip.optimizeMesh = true;
        ip.weldVertices = false;
        ip.importVisibility = false;
        ip.importCameras = false;
        ip.importLights = false;
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(path);
    }

    private static void StartAssembleCharacterFashion(string dir, string partName, float progress = 0f)
    {
        if (EditorUtility.DisplayCancelableProgressBar("Assemble Character Fashion", "Start Assemble Character Fashion", progress))
        {
            EditorUtility.ClearProgressBar();
            return;
        }

        string name = dir.Substring(dir.LastIndexOf("/") + 1, dir.Length - dir.LastIndexOf("/") - 1);
        string goPath = dir + "/" + partName + ".fbx";
        string prefabPath = dir + "/" + partName + ".prefab";
        GameObject prefab = CreatePrefab(goPath, prefabPath, partName);
        if (prefab == null)
        {
            return;
        }
        prefab.transform.Rotate(new Vector3(0, 150, 0));

        var anim = prefab.GetComponent<Animator>();
        GameObject.DestroyImmediate(anim, true);

        if (!ModifyMat(prefab, partName, dir))
        {
            //return;
        }

        AssetDatabase.SaveAssets();

        PrefabMoveToResDir(prefabPath, name);
    }

    #endregion

    #region Pet

    private static void ManipulatePet(string dir)
    {
        Debug.Log("开始 assemble Pet: " + dir);

        string name = dir.Substring(dir.LastIndexOf("/") + 1, dir.Length - dir.LastIndexOf("/") - 1);

        __PrepareFbx(dir + "/" + name + ".fbx", true);  // tpos

        __PrepareFbx(dir + "/" + name + "_stand.fbx", false, true);
        __PrepareFbx(dir + "/" + name + "_walk.fbx", false, true);
        __PrepareFbx(dir + "/" + name + "_win.fbx", false, true);
        __PrepareFbx(dir + "/" + name + "_unique4.fbx", false, false);

        StartAssemblePet(dir);
    }

    private static void StartAssemblePet(string dir)
    {
        string name = dir.Substring(dir.LastIndexOf("/") + 1, dir.Length - dir.LastIndexOf("/") - 1);
        //string relPath = dir.TrimStart("Assets/".ToCharArray()); // 相对于Asset/的目录路径

        AnimatorController animatorController = CreateAnimatorControllerPet(dir, name);

        string goPath = dir + "/" + name + ".fbx";
        string prefabPath = dir + "/" + name + ".prefab";
        GameObject prefab = CreatePrefab(goPath, prefabPath, name);
        if (prefab == null)
        {
            return;
        }
        prefab.transform.Rotate(new Vector3(0, 150, 0));

        if (!ModifyMat(prefab, name, dir))
        {
            //return;
        }

        SetCollider(prefab);

        // 设置prefab的anim controller
        Animator amt = prefab.GetComponent<Animator>();
        amt.runtimeAnimatorController = animatorController;

        AssetDatabase.SaveAssets();

        PrefabMoveToResDir(prefabPath, name);
    }

    private static AnimatorController CreateAnimatorControllerPet(string dir, string name)
    {
        string fullAnimDirName = dir + "/Animator";
        string animControllerPath = dir + "/Animator/" + name + "Anim.controller";
        if (!Directory.Exists(fullAnimDirName))
            Directory.CreateDirectory(fullAnimDirName);

        // 创建animationController
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animControllerPath);
        // 得到它的Layer， 默认layer为base 可以去拓展
        AnimatorControllerLayer layer = animatorController.layers[0];

        // 创建parameters
        animatorController.AddParameter("idle", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("walk", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("win", AnimatorControllerParameterType.Trigger);
        animatorController.AddParameter("skill", AnimatorControllerParameterType.Trigger);

        // 把动画文件保存在我们创建的AnimationController中
        AnimatorState idle = AddState("idle", dir + "/" + name + "_stand.fbx", layer, true);
        AnimatorState walk = AddState("walk", dir + "/" + name + "_walk.fbx", layer, true);
        AnimatorState win = AddState("win", dir + "/" + name + "_win.fbx", layer, true);
        AnimatorState skill = AddState("skill", dir + "/" + name + "_unique4.fbx", layer, false);

        // 创建transition
        AnimatorStateMachine sm = layer.stateMachine;
        AddTransition(sm, idle, false, "idle");
        AddTransition(sm, walk, false, "walk");
        AddTransition(sm, win, false, "win");
        AddTransition(sm, skill, false, "skill");
        AddTransition(sm, skill, idle, true, "");

        return animatorController;
    }

    #endregion

    private static void PrefabMoveToResDir(string prefabPath, string name)
    {
        // 全部完成后，将prefab移动res文件夹中
        //string parentFolder = dir.Replace("ResRaw", "Res");
        //string absParentFolder = Application.dataPath + parentFolder.TrimStart("Assets".ToCharArray());
        //if (!Directory.Exists(absParentFolder))
        //{
        //    Directory.CreateDirectory(absParentFolder);
        //}
        string absFullPrefabPath = Application.dataPath + prefabPath.TrimStart("Assets".ToCharArray());
        string newFullPrefabPath = prefabPath.Replace("ResRaw", "Res");
        newFullPrefabPath = newFullPrefabPath.Replace(name + "/", "");
        string absnewFullPrefabPath = Application.dataPath + newFullPrefabPath.TrimStart("Assets".ToCharArray());
        if (File.Exists(absnewFullPrefabPath))
            File.Delete(absnewFullPrefabPath);

        File.Move(absFullPrefabPath, absnewFullPrefabPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("生成Prefab完成：" + newFullPrefabPath);
    }

    private static void __PrepareFbx(string path, bool tpos = false, bool loop = false)
    {
        ModelImporter ip = null;
        try
        {
            ip = AssetImporter.GetAtPath(path) as ModelImporter;
            if (ip == null) return;
        }
        catch (Exception)
        {
            return;
        }

        ip.globalScale = 1;
        ip.importAnimation = true;
        ip.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
        //ip.animationCompression = ModelImporterAnimationCompression.Off;   // 必须开压缩，不然太大了/压缩完后大概到1/5
        ip.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
        //ip.importNormals = ModelImporterNormals.None;           // 不使用实时光照，关闭法线信息
        ip.importNormals = ModelImporterNormals.Import;
        ip.importTangents = ModelImporterTangents.None;
        ip.isReadable = false;
        AssetDatabase.ImportAsset(path);   // scale 改变后，先重导入一下

        ip = AssetImporter.GetAtPath(path) as ModelImporter;
        SerializedObject so = new SerializedObject(ip);
        if (tpos)
        {

        }
        else
        {
            List<ModelImporterClipAnimation> newlcs = new List<ModelImporterClipAnimation>();
            TakeInfo[] tis = ip.importedTakeInfos;
            if (tis.Length == 0)
            {
                Debug.LogError(path + " 动作文件里么有动作信息，请补上！");
                return;
            }
            TakeInfo ti = tis[0];
            {
                ModelImporterClipAnimation newc = new ModelImporterClipAnimation();
                newc.name = ti.defaultClipName;
                //newc.takeName = c.takeName;
                newc.firstFrame = 0;
                newc.lastFrame = (int)(ti.stopTime * ti.sampleRate);   // 必须取int，不然不会成功

                //// bake into pose
                //newc.keepOriginalPositionY = true;
                //newc.keepOriginalPositionXZ = true;
                //newc.keepOriginalOrientation = true;
                //newc.lockRootPositionXZ = true;
                //newc.lockRootHeightY = true;
                //newc.lockRootRotation = true;

                if (loop)
                {
                    newc.loop = true;
                    newc.loopTime = true;
                    newc.loopPose = true;
                }
                newlcs.Add(newc);
            }
            ip.clipAnimations = newlcs.ToArray();

            so.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path);   // 重导入一下
        }
    }

    private static AnimatorState AddState(string name, string path, AnimatorControllerLayer layer, bool loop = false)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        AnimationClip clip = null;
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                //根据动画文件读取它的AnimationClip对象
                clip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
            }
            catch (Exception)
            {
                return null;
            }

            if (clip == null)
                return null;
        }

        AnimatorState state = sm.AddState(name);
        // 创建NewClip
        AnimationClip newClip = Object.Instantiate(clip) as AnimationClip;
        newClip.name = name;

        //// 设置loop
        //if (loop)
        //{
        //    //AnimationClipSettings st = AnimationUtility.GetAnimationClipSettings(newClip);
        //    //st.loopTime = true;
        //    newClip.wrapMode = WrapMode.Loop;
        //}

        //CompressAnimationClip(newClip);

        //取出动画名子 添加到state里面
        state.motion = newClip;

        // 创建animationclip
        string clipPath = Path.GetDirectoryName(path) + "/Animator/" + newClip.name + ".anim";
        AssetDatabase.CreateAsset(newClip, clipPath);

        return state;
    }

    private static void AddTransition(AnimatorStateMachine sm, AnimatorState src, AnimatorState dst,bool exitTime, string par = "")
    {
        if (src == null || dst == null) return;

        //连接每个状态，并添加切换条件
        src.AddExitTransition(exitTime);
        AnimatorStateTransition _animatorStateTransition = src.AddTransition(dst);
        foreach (var item in _animatorStateTransition.conditions)
        {
            _animatorStateTransition.RemoveCondition(item);
        }
        if (!string.IsNullOrEmpty(par))
        {
            _animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, par);
        }
        _animatorStateTransition.hasExitTime = false;
    }

    private static void AddTransition(AnimatorStateMachine sm, AnimatorState st, bool exitTime, string par = "")
    {
        if (st == null) return;

        AnimatorStateTransition trans = sm.AddAnyStateTransition(st);
        foreach (var item in trans.conditions)
        {
            trans.RemoveCondition(item);
        }
        trans.canTransitionToSelf = true;
        trans.AddCondition(AnimatorConditionMode.If, 0, par);
        trans.hasExitTime = exitTime;
    }

    private static GameObject CreatePrefab(string goPath, string prefabPath, string name)
    {
        GameObject prefab = null;
        int count = 0;
        while (count < 100)
        {
            if (!SureMakingPrefab(goPath, prefabPath, out prefab))
            {
                count++;
            }
            else
            {
                count = 100;
            }
        } // 如果创建不成功，则不断尝试
        if (prefab == null)
        {
            Debug.LogError("生成失败：" + goPath);
            return null;
        }
        return prefab;
    }

    private static bool SureMakingPrefab(string goPath, string prefabPath, out GameObject prefab)  // 确保创建Prefab
    {
        prefab = null;

        // 创建的prefab
        GameObject go = AssetDatabase.LoadAssetAtPath(goPath, typeof(GameObject)) as GameObject;
        if (go == null)
        {
            Debug.LogError("无法加载：" + goPath);
            return false;
        }

        //// unity 自身bug，有概率会创建失败，无法找到原因 bug:https://community.unity.com/t5/Scripting/Unity-4-7-quot-m-InstanceID-0-quot-when-calling-PrefabUtility/m-p/2525388
        try
        {
            prefab = PrefabUtility.CreatePrefab(prefabPath, go);

            if (prefab == null)
            {
                Debug.LogError("创建Prefab失败:" + prefab + " ");    // 无法捕获
                return false;
            }
            Transform tf = prefab.transform;   // 比对null无法捕获，只能通过这种方式触发异常
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("创建Prefab失败:" + prefab + " " + e);   // 无法捕获
            return false;
        }
    }

    private static bool ModifyMat(GameObject prefab, string name, string dir)
    {
        //// 修改下mat的
        //Bounds skinBounds = new Bounds(new Vector3(0, 0f, 0), new Vector3(0, 0, 0));
        //Transform skillTf = prefab.transform.Find(name);
        //if (skillTf == null)
        //{
        //    string error = null;
        //    for (int i = 0; i != prefab.transform.childCount; ++i)
        //    {
        //        error += "  child:" + prefab.transform.GetChild(i).name;
        //    }
        //    Debug.LogError("模型制作有问题，skin名称和模型名不同:" + name + " " + error);
        //    return false;
        //}
        //GameObject skinGo = skillTf.gameObject;
        //string texName = null;
        //SkinnedMeshRenderer smr = skinGo.GetComponent<SkinnedMeshRenderer>();
        //if (smr != null)
        //{
        //    // smr.sharedMaterial.color = new Color(0.9f, 0.9f, 0.9f, 1f); // 修改颜色
        //    // smr.sharedMaterial.shader = Shader.Find("Diffuse");    // 暂时默认将npc的shader都设成Diffuse
        //    //smr.useLightProbes = true;          // 使用lightprobes
        //    skinBounds = smr.localBounds;
        //    texName = smr.sharedMaterial.mainTexture.name;
        //}

        // 设置texture aniso level
        //{
        //    string texPath = dir + "/texture/" + texName + ".tga";
        //    // 处理下贴图，默认anisolevel设2
        //    TextureImporter ti = AssetImporter.GetAtPath(texPath) as TextureImporter;
        //    ti.anisoLevel = 2;
        //    AssetDatabase.ImportAsset(texPath);
        //}

        // 设置 texture mipmapEnabled
        SkinnedMeshRenderer[] smrs = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var smr in smrs)
        {
            foreach (var material in smr.sharedMaterials)
            {
                if (material.mainTexture == null)
                {
                    Debug.LogError(name+ " not found mainTexture! materialName:" + material.name);
                    continue;
                }
                string texPath = dir + "/Materials/" + material.mainTexture.name + ".tga";
                TextureImporter ti = AssetImporter.GetAtPath(texPath) as TextureImporter;
                if (ti == null)
                {
                    Debug.LogError(name + " not found Texture! Path:" + texPath);
                    continue;
                }
                ti.isReadable = false;
                ti.mipmapEnabled = false;
                AssetDatabase.ImportAsset(texPath);
            }
        }

        return true;
    }

    private static bool SetCollider(GameObject prefab)
    {
        //// 设置rigidBody用于碰撞
        //prefab.AddComponent<Rigidbody>().isKinematic = true;
        //prefab.GetComponent<Rigidbody>().useGravity = false;

        // 设置collider用于碰撞
        // NPC
        //// skinBounds.extents.x; 高度
        //// skinBounds.extents.y; 前后
        //// skinBounds.extents.z; 左右
        //float cHeight, cRadius;
        //cHeight = skinBounds.extents.x;
        //cRadius = skinBounds.extents.y < skinBounds.extents.z ? skinBounds.extents.y : skinBounds.extents.z;

        //prefab.AddComponent<CapsuleCollider>().isTrigger = true;
        //prefab.GetComponent<CapsuleCollider>().center = skinBounds.center;
        //prefab.GetComponent<CapsuleCollider>().radius = cRadius;
        //prefab.GetComponent<CapsuleCollider>().height = cHeight;

        //prefab.AddComponent<CapsuleCollider>().isTrigger = true;
        //prefab.GetComponent<CapsuleCollider>().center = new Vector3(0, skinBounds.extents.x, 0);
        //prefab.GetComponent<CapsuleCollider>().radius = skinBounds.extents.y * 3 / 4;   // 取3/4左右
        //prefab.GetComponent<CapsuleCollider>().height = skinBounds.extents.x * 2;

        return true;
    }

    //AnimationClip压缩-动画文件压缩
    public static bool CompressAnimationClip(AnimationClip clip)
    {
        try
        {
            //去掉AnimationClip中的无效曲线：例如ScaleCurve
            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(clip))
            {
                string propertyName = theCurveBinding.propertyName.ToLower();
                if (propertyName.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(clip, theCurveBinding, null);
                }
            }

            //压缩AnimationClip文件float的精度
            AnimationClipCurveData[] curves = null;
            curves = AnimationUtility.GetAllCurves(clip);
            Keyframe key;
            Keyframe[] keyFrames;
            for (int ii = 0; ii < curves.Length; ++ii)
            {
                AnimationClipCurveData curveDate = curves[ii];
                if (curveDate.curve == null || curveDate.curve.keys == null)
                {
                    //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                    continue;
                }
                keyFrames = curveDate.curve.keys;
                for (int i = 0; i < keyFrames.Length; i++)
                {
                    key = keyFrames[i];
                    key.value = float.Parse(key.value.ToString("f3"));
                    key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                    key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                    keyFrames[i] = key;
                }
                curveDate.curve.keys = keyFrames;
                clip.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
            }
            //Debug.Log(string.Format("  CompressAnimationClip {0} Success !!!", clip.name));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", clip.name, e));
            return false;
        }
    }

}
