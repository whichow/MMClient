// ***********************************************************************
// Assembly         : Unity
// Author           : Kimch
// Created          : 
//
// Last Modified By : Kimch
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KUIImage" company=""></copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[AddComponentMenu("UGUICustom/KUIImage")]
public class KUIImage : Image
{
    #region Field

    public Sprite[] sprites;

    public bool usePolygon;

    #endregion

    #region Property

    private Sprite currentSprite
    {
        get { return overrideSprite ?? sprite; }
    }

    #endregion

    #region Method

    public void ShowSprite(int index)
    {
        if (sprites != null && index < sprites.Length)
        {
            this.overrideSprite = sprites[index];
        }
    }

    public void ShowGray(bool gray)
    {
        material = gray ? Resources.Load<Material>("Materials/UIGray") : null;
    }

    /// <summary>
    /// Update the UI renderer mesh.
    /// </summary>
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);

        if (!usePolygon || type != Type.Simple)
        {
            return;
        }

        var sprite = currentSprite;
        if (sprite == null || sprite.triangles == null || sprite.triangles.Length <= 6)
        {
            return;
        }

        var vertex = new UIVertex();
        toFill.PopulateUIVertex(ref vertex, 0);
        var lb = vertex.position;
        toFill.PopulateUIVertex(ref vertex, 2);
        var rt = vertex.position;

        Vector2 center = sprite.bounds.center;
        Vector2 invSize = sprite.bounds.size;
        invSize = new Vector2(1f / invSize.x, 1f / invSize.y);

        var vertices = sprite.vertices;
        var verticesLength = sprite.vertices.Length;
        var verticeList = K.ListPool<UIVertex>.Get();
        var uvs = sprite.uv;
        for (int i = 0; i < verticesLength; i++)
        {
            vertex = new UIVertex();

            var x = Mathf.Lerp(lb.x, rt.x, (vertices[i].x - center.x) * invSize.x + 0.5f);
            var y = Mathf.Lerp(lb.y, rt.y, (vertices[i].y - center.y) * invSize.y + 0.5f);

            vertex.position = new Vector3(x, y, 0f);
            vertex.uv0 = uvs[i];
            vertex.color = color;
            verticeList.Add(vertex);
        }

        var triangles = sprite.triangles;
        var trianglesLength = triangles.Length;
        var triangleList = K.ListPool<int>.Get();
        for (int i = 0; i < trianglesLength; i++)
        {
            triangleList.Add(triangles[i]);
        }

        toFill.Clear();
        toFill.AddUIVertexStream(verticeList, triangleList);
        K.ListPool<UIVertex>.Release(verticeList);
        K.ListPool<int>.Release(triangleList);
    }

    #endregion

}

