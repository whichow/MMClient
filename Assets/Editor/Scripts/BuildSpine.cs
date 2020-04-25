using Spine.Unity;
using Spine.Unity.Editor;
using UnityEditor;
using UnityEngine;

public class BuildSpine : MonoBehaviour
{
    #region 建筑

    [MenuItem("Tools/建筑/Create Building Sprite Prefab(整体)")]
    public static void BuildBuildingSpritePrefabs()
    {
        var folder = "Assets/GameRes/Textures/Building/Item/";
        var savePath = "Assets/Res/Building/Item/";

        var guids = AssetDatabase.FindAssets("t:Sprite", new string[] { folder });
        if (guids != null && guids.Length > 0)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                BuildSpritePrefab(assetPath, savePath);
            }
        }
    }

    [MenuItem("Tools/建筑/Create Building Sprite Prefab(单个)")]
    public static void BuildBuildingSpritePrefab()
    {
        var savePath = "Assets/Res/Building/Item/";
        var objects = Selection.objects;
        foreach (Object obj in objects)
        {
            var assetPath = AssetDatabase.GetAssetPath(obj);
            BuildSpritePrefab(assetPath, savePath);
        }
    }

    private static void BuildSpritePrefab(string assetPath, string savePath)
    {
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

        var saveFile = savePath + sprite.name + ".prefab";
        Vector3 localPosition = Vector3.zero;
        Vector3 localScale = Vector3.one;
        //Vector2[] PolyonPos;
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(saveFile);
        if (prefab && prefab.transform.childCount > 0)
        {
            var child = prefab.transform.GetChild(0);
            if (child)
            {
                localPosition = child.localPosition;
                localScale = child.localScale;
            }
        }

        var spriteRenderer = new GameObject("Model").AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        var gameObject = new GameObject(sprite.name);
        spriteRenderer.transform.SetParent(gameObject.transform, false);
        spriteRenderer.transform.localPosition = localPosition;
        spriteRenderer.transform.localScale = localScale;

        PrefabUtility.CreatePrefab(saveFile, gameObject, ReplacePrefabOptions.ConnectToPrefab);
        DestroyImmediate(gameObject);
    }


    [MenuItem("Tools/建筑/Create Building Spine Prefab(整体)")]
    public static void BuildBuildingSpinePrefabs()
    {
        var folder = "Assets/GameRes/Spines/Building/Item/";
        var savePath = "Assets/Res/Building/Item/";

        var paths = System.IO.Directory.GetDirectories(folder);
        var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
        if (guids != null && guids.Length > 0)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                BuildSpinePrefab(assetPath, savePath);
            }
        }
    }

    [MenuItem("Tools/建筑/Create Building Spine Prefab(单个)")]
    public static void BuildBuildingSpinePrefab()
    {
        var savePath = "Assets/Res/Building/Item/";

        var objects = Selection.objects;
        string filePath =null;
        foreach (Object obj in objects)
        {
            var paths = new string[] { AssetDatabase.GetAssetPath(obj) };
            if (paths.Length > 0)
                filePath = paths[0];
            var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
            if (guids != null && guids.Length > 0)
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    BuildSpinePrefab(assetPath, savePath);
                }
            }
        }
    }

    [MenuItem("Tools/建筑/Create Building Spine Prefab ByOldData(单个原始数据)")]
    public static void BuildBuildingSpinePrefabByOldData()
    {
        var savePath = "Assets/Res/Building/Item/";

        var objects = Selection.objects;
        string filePath = null;
        foreach (Object obj in objects)
        {
            var paths = new string[] { AssetDatabase.GetAssetPath(obj) };
            if (paths.Length > 0)
                filePath = paths[0];
            var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
            if (guids != null && guids.Length > 0)
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    BuildSpinePrefab(assetPath, savePath, filePath, true);
                }
            }
        }
    }

    [MenuItem("Tools/建筑/Change Building SkeletonAnim PropertyAll(整体)")]
    public static void BuildingChgSkeletonAnimPropertyAll()
    {
        string[] paths = new string[1] { "Assets/Res/Building/Item" };
        var guids = AssetDatabase.FindAssets("t:Object", paths);
        GameObject gm;
        if (guids != null && guids.Length > 0)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                Transform trans = AssetDatabase.LoadAssetAtPath<Transform>(assetPath);
                var anim = trans.GetComponentInChildren<SkeletonAnimation>();
                if (anim)
                {
                    anim.pmaVertexColors = true;
                }
                //  var skeletonAnimation = AssetDatabase.LoadAssetAtPath<Transform>(assetPath);
                //skeletonAnimation.pmaVertexColors = true;

                //PrefabUtility.CreatePrefab(saveFile, gameObject, ReplacePrefabOptions.ConnectToPrefab);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Tools/建筑/Change Building ShaderAll(整体)")]
    public static void BuildingChangeShaderAll()
    {
        var paths = System.IO.Directory.GetDirectories("Assets/GameRes/Spines/Building/Item/");
        var guids = AssetDatabase.FindAssets("t:Material", paths);
        if (guids != null && guids.Length > 0)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                material.shader = Shader.Find("Spine/Skeleton Tint");
                //BuildCharactorSpinePrefab(assetPath, savePath);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void BuildSpinePrefab(string assetPath, string savePath,string filePath= null, bool isUseOldModleData = false)
    {
        //var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        //var skeletonAnimation = SpineEditorUtilities.InstantiateSkeletonAnimation(skeletonDataAsset);
        //var saveFile = savePath + skeletonAnimation.name + ".prefab";
        //var materialPath = filePath +"/"+skeletonAnimation.name+"_Material.mat";
        //Vector3 localPosition = Vector3.zero;
        //Vector3 localScale = Vector3.one;
        //Vector2[] PolyonPos = null;

        //var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(saveFile);
        //var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        //if (material)
        //{
        //    material.shader = Shader.Find("Custom/Skeleton Tint");
        //}
        //else
        //{
        //    Debug.LogWarning("Material查找失败" + materialPath);
        //}
        //Transform point =null;
        //if (prefab && prefab.transform.childCount > 0)
        //{
        //    var child = prefab.transform.GetChild(0);

        //    if (child)
        //    {
        //        localPosition = child.localPosition;
        //        localScale = child.localScale;
        //        if (isUseOldModleData)
        //        {
        //            PolygonCollider2D polygonCollider2D = child.GetComponent<PolygonCollider2D>();
        //            if (polygonCollider2D)
        //                PolyonPos = polygonCollider2D.points;
        //        }
        //    }
        //    point = prefab.transform.Find("Point");
        //}

        //var gameObject = new GameObject(skeletonAnimation.name);
        //skeletonAnimation.transform.SetParent(gameObject.transform, false);
        //skeletonAnimation.transform.localPosition = localPosition;
        //skeletonAnimation.transform.localScale = localScale;
        //GameObject pointTemp =null;
        //if (isUseOldModleData)
        //{
        //    if(PolyonPos!=null)
        //    {

        //        PolygonCollider2D polygonCollider2D = skeletonAnimation.gameObject.AddComponent<PolygonCollider2D>();
        //        polygonCollider2D.points = PolyonPos;
        //    }
        //    if (point)
        //    {
        //        GameObject gm = new GameObject("Point");
        //        gm.transform.SetParent(gameObject.transform, false);
        //        gm.transform.localPosition = point.localPosition;
        //    }
        //}
        //skeletonAnimation.AnimationName = skeletonAnimation.AnimationState.Data.SkeletonData.Animations.Items[0].Name;

        //var mr = skeletonAnimation.GetComponent<MeshRenderer>();
        //mr.receiveShadows = false;
        //mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        //mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
    
        //PrefabUtility.CreatePrefab(saveFile, gameObject, ReplacePrefabOptions.ConnectToPrefab);
        //DestroyImmediate(gameObject);
        //if(pointTemp)
        //    DestroyImmediate(pointTemp);
    }

    #endregion

    #region 三消

    [MenuItem("Tools/三消/Create M3Item Spine Prefab")]
    static void InstantiateM3SkeletonAnimator()
    {
        var savePath = "Assets/Res/Match3/NewSpineItem/";

        //var objects = Selection.objects;
        //foreach (Object obj in objects)
        //{
        //    //string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));
        //    //string skinName = EditorPrefs.GetString(guid + "_lastSkin", "");

        //    var paths = new string[] { AssetDatabase.GetAssetPath(obj) };
        //    var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
        //    if (guids != null && guids.Length > 0)
        //    {
        //        for (int i = 0; i < guids.Length; i++)
        //        {
        //            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
        //            var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        //            skeletonDataAsset.scale = 1f / 115f;
        //            var skeletonAnimation = SpineEditorUtilities.InstantiateSkeletonAnimator(skeletonDataAsset, "", true);
        //            PrefabUtility.CreatePrefab(savePath + skeletonAnimation.name + ".prefab", skeletonAnimation.gameObject, ReplacePrefabOptions.ConnectToPrefab);
        //            DestroyImmediate(skeletonAnimation.gameObject);
        //        }
        //    }
        //}
        var objects = Selection.objects;
        foreach (Object obj in objects)
        {
            var paths = new string[] { AssetDatabase.GetAssetPath(obj) };
            var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
            if (guids != null && guids.Length > 0)
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    BuildM3ItemSpinePrefab(assetPath, savePath);
                }
            }
        }
        Debug.Log("Create M3Item spine prefab over! SaveTo:" + savePath);
    }

    [MenuItem("Tools/三消/Create M3Item Fx Spine Prefab")]
    static void InstantiateM3FXSkeletonAnimator()
    {
        //var savePath = "Assets/GameRes/Prefabs/Match3/Fx/";
        //Object[] arr = Selection.objects;
        //foreach (Object o in arr)
        //{
        //    //string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));
        //    //string skinName = EditorPrefs.GetString(guid + "_lastSkin", "");

        //    var paths = new string[] { AssetDatabase.GetAssetPath(o) };
        //    var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
        //    if (guids != null && guids.Length > 0)
        //    {
        //        for (int i = 0; i < guids.Length; i++)
        //        {
        //            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
        //            var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        //            skeletonDataAsset.scale = 1f / 116f;
        //            var skeletonAnimation = SpineEditorUtilities.InstantiateSkeletonAnimator(skeletonDataAsset, "");
        //            PrefabUtility.CreatePrefab(savePath + skeletonAnimation.name + ".prefab", skeletonAnimation.gameObject, ReplacePrefabOptions.ConnectToPrefab);
        //            DestroyImmediate(skeletonAnimation.gameObject);
        //        }
        //    }
        //}
    }

    public static void BuildM3ItemSpinePrefab(string assetPath, string savePath)
    {
        var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        var skeletonAnimation = SpineEditorUtilities.InstantiateSkeletonAnimation(skeletonDataAsset, "");

        var mr = skeletonAnimation.gameObject.GetComponent<MeshRenderer>();
        mr.receiveShadows = false;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        skeletonDataAsset.scale = 1f / 116f;
        PrefabUtility.CreatePrefab(savePath + skeletonAnimation.name + ".prefab", skeletonAnimation.gameObject, ReplacePrefabOptions.ConnectToPrefab);
        DestroyImmediate(skeletonAnimation.gameObject);
    }

    #endregion

    #region 宠物猫

   [MenuItem("Tools/宠物Spine/Create Pet Spine Prefab(整体)")]
    public static void BuildCatSpinePrefabs()
    {
        var savePath = "Assets/Res/Pet2D/";
        var paths = System.IO.Directory.GetDirectories("Assets/GameRes/Spines/Character/");
        var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
        if (guids != null && guids.Length > 0)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                BuildCharactorSpinePrefab(assetPath, savePath);
            }
        }
    }

    [MenuItem("Tools/宠物Spine/Create Pet Spine Prefab(单个)")]
    public static void BuildCatSpinePrefab()
    {
        var savePath = "Assets/Res/Pet2D/";
        var objects = Selection.objects;
        foreach (Object obj in objects)
        {
            var paths = new string[] { AssetDatabase.GetAssetPath(obj) };
            var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
            if (guids != null && guids.Length > 0)
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                    BuildCharactorSpinePrefab(assetPath, savePath);
                }
            }
        }
    }

    [MenuItem("Tools/宠物Spine/Create Character Fx Spine Prefab")]
    static void InstantiateCharacterFXSkeletonAnimator()
    {
        //var savePath = "Assets/GameRes/Prefabs/Character/CharacterFX/";
        //Object[] arr = Selection.objects;
        //foreach (Object o in arr)
        //{
        //    //string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));
        //    //string skinName = EditorPrefs.GetString(guid + "_lastSkin", "");

        //    var paths = new string[] { AssetDatabase.GetAssetPath(o) };
        //    var guids = AssetDatabase.FindAssets("t:SkeletonDataAsset", paths);
        //    if (guids != null && guids.Length > 0)
        //    {
        //        for (int i = 0; i < guids.Length; i++)
        //        {
        //            var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
        //            var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        //            var skeletonAnimation = SpineEditorUtilities.InstantiateSkeletonAnimation(skeletonDataAsset, "");
        //            skeletonAnimation.name = skeletonAnimation.name.ToLower();

        //            var mr = skeletonAnimation.gameObject.GetComponent<MeshRenderer>();
        //            mr.receiveShadows = false;
        //            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //            mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        //            mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

        //            skeletonAnimation.AnimationName = skeletonAnimation.AnimationState.Data.SkeletonData.Animations.Items[0].Name;

        //            var obj = new GameObject(mr.name);
        //            mr.transform.SetParent(obj.transform, false);
        //            mr.transform.localPosition = new Vector3(0, 0, 0);
        //            mr.transform.localEulerAngles = new Vector3(0, 180, 0);

        //            PrefabUtility.CreatePrefab(savePath + skeletonAnimation.name + ".prefab", obj.gameObject, ReplacePrefabOptions.ConnectToPrefab);
        //            DestroyImmediate(obj);
        //        }
        //    }
        //}
    }

    public static void BuildCharactorSpinePrefab(string assetPath, string savePath)
    {
        //var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        //var skeletonAnimation = SpineEditorUtilities.InstantiateSkeletonAnimation(skeletonDataAsset, "");
        //skeletonAnimation.name = skeletonAnimation.name.ToLower();
        //new GameObject("SkillPoint").transform.SetParent(skeletonAnimation.transform);

        //var mr = skeletonAnimation.gameObject.GetComponent<MeshRenderer>();
        //mr.receiveShadows = false;
        //mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        //mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

        //PrefabUtility.CreatePrefab(savePath + skeletonAnimation.name + ".prefab", skeletonAnimation.gameObject, ReplacePrefabOptions.ConnectToPrefab);
        //DestroyImmediate(skeletonAnimation.gameObject);
    }
   
    #endregion


    [MenuItem("/Game/Create Sprite Prefab(拼)")]
    public static void BuildSeparateSpritePrefab()
    {
        var orgin = new GameObject();
        var objects = Selection.objects;
        foreach (Object obj in objects)
        {
            var sprite = obj as Sprite;
            if (sprite)
            {
                orgin.name = sprite.texture.name;

                var sr = new GameObject(obj.name).AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
                var center = sprite.rect.center;
                sr.transform.SetParent(orgin.transform, false);
                sr.transform.localPosition = center * 0.01f;
            }
        }
    }

    [MenuItem("Game/自动切九宫格")]
    static void AutoSaveSlicedTexture()
    {
        var folder = "Assets/GameRes/Textures/UI";

        var paths = System.IO.Directory.GetDirectories(folder);
        var guids = AssetDatabase.FindAssets("t:Texture2D", paths);
        if (guids != null && guids.Length > 0)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                SaveSlicedTexture(assetPath);
            }
        }
    }

    [MenuItem("Game/手动切九宫格")]
    static void ManualSaveSlicedTexture()
    {
        var folder = Selection.activeObject;
        if (folder)
        {
            var assetPath = AssetDatabase.GetAssetPath(folder);
            SaveSlicedTexture(assetPath);
        }
    }

    static void SaveSlicedTexture(string assetPath)
    {
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if (sprite)
        {
            var border = sprite.border;
            int l = (int)border.x; int b = (int)border.y;
            int r = (int)border.z; int t = (int)border.w;

            if (l + r + b + t == 0)
            {
                return;
            }

            var ti = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            ti.isReadable = true;
            ti.SaveAndReimport();

            var oldTex = sprite.texture;
            var ow = oldTex.width;
            var oh = oldTex.height;

            var nw = ow;
            var nh = oh;

            if (l + r != 0)
            {
                nw = l + r + 2;
            }
            if (b + t != 0)
            {
                nh = b + t + 2;
            }
            var newTex = new Texture2D(nw, nh, TextureFormat.RGBA32, false);

            nw--; nh--; ow--; oh--;
            if (l + r == 0)
            {
                for (int i = 0; i <= nw; i++)
                {
                    for (int j = 0; j <= b; j++)
                    {
                        newTex.SetPixel(i, j, oldTex.GetPixel(i, j));
                    }
                }

                for (int i = 0; i <= nw; i++)
                {
                    for (int j = 0; j <= t; j++)
                    {
                        newTex.SetPixel(i, nh - j, oldTex.GetPixel(i, oh - j));
                    }
                }
            }
            else if (b + t == 0)
            {
                for (int i = 0; i <= l; i++)
                {
                    for (int j = 0; j <= nh; j++)
                    {
                        newTex.SetPixel(i, j, oldTex.GetPixel(i, j));
                    }
                }

                for (int i = 0; i <= r; i++)
                {
                    for (int j = 0; j <= nh; j++)
                    {
                        newTex.SetPixel(nw - i, j, oldTex.GetPixel(ow - i, j));
                    }
                }
            }
            else
            {
                for (int i = 0; i <= l; i++)
                {
                    for (int j = 0; j <= b; j++)
                    {
                        newTex.SetPixel(i, j, oldTex.GetPixel(i, j));
                    }
                }

                for (int i = 0; i <= r; i++)
                {
                    for (int j = 0; j <= b; j++)
                    {
                        newTex.SetPixel(nw - i, j, oldTex.GetPixel(ow - i, j));
                    }
                }

                for (int i = 0; i <= r; i++)
                {
                    for (int j = 0; j <= t; j++)
                    {
                        newTex.SetPixel(nw - i, nh - j, oldTex.GetPixel(ow - i, oh - j));
                    }
                }

                for (int i = 0; i <= l; i++)
                {
                    for (int j = 0; j <= t; j++)
                    {
                        newTex.SetPixel(i, nh - j, oldTex.GetPixel(i, oh - j));
                    }
                }
            }

            newTex.Apply();
            System.IO.File.WriteAllBytes(assetPath, newTex.EncodeToPNG());

            ti = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            ti.isReadable = false;
            ti.SaveAndReimport();
        }
    }

}
