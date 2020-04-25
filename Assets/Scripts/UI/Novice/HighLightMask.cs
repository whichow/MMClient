using UnityEngine;
using UnityEngine.UI;

public class HighLightMask : Graphic, ICanvasRaycastFilter
{
	public RectTransform targetRectTransform1;

	public RectTransform targetRectTransform2;

	public RectTransform targetRectTransform3;

	private Vector3 _targetPosition1 = Vector3.zero;

	private Vector2 _targetCenter1 = Vector2.zero;

	private Vector2 _targetSize1 = Vector2.zero;

	private Vector3 _targetPosition2 = Vector3.zero;

	private Vector2 _targetCenter2 = Vector2.zero;

	private Vector2 _targetSize2 = Vector2.zero;

    private Vector3 _targetPosition3 = Vector3.zero;

    private Vector2 _targetCenter3 = Vector2.zero;

    private Vector2 _targetSize3 = Vector2.zero;

    private bool _isValid;

    private void Update()
	{
        if (!Application.isPlaying)
        {
            return;
        }
        Camera currentCamera = KUIRoot.Instance.uiCamera;
        if (currentCamera == null || currentCamera.gameObject == null || this.targetRectTransform1 == null || this.targetRectTransform1.gameObject == null || !this.targetRectTransform1.gameObject.activeInHierarchy)
		{
			if (this._isValid)
			{
				this._isValid = false;
				this.SetAllDirty();
			}
			return;
		}
		if (!this._isValid)
		{
			this._isValid = true;
			this.SetAllDirty();
		}
        if (this._targetPosition1 != this.targetRectTransform1.position || this._targetSize1 != this.targetRectTransform1.rect.size)
        {
			this._targetPosition1 = this.targetRectTransform1.position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, RectTransformUtility.WorldToScreenPoint(currentCamera, this._targetPosition1), currentCamera, out this._targetCenter1);
			this._targetSize1 = this.targetRectTransform1.rect.size;
			this.SetAllDirty();
			return;
		}
		if (this.targetRectTransform2 != null && (this._targetPosition2 != this.targetRectTransform2.position || this._targetSize2 != this.targetRectTransform2.rect.size))
		{
			this._targetPosition2 = this.targetRectTransform2.position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, RectTransformUtility.WorldToScreenPoint(currentCamera, this._targetPosition2), currentCamera, out this._targetCenter2);
			this._targetSize2 = this.targetRectTransform2.rect.size;
			this.SetAllDirty();
			return;
		}
        if (this.targetRectTransform3 != null && (this._targetPosition3 != this.targetRectTransform3.position || this._targetSize3 != this.targetRectTransform3.rect.size))
        {
            this._targetPosition3 = this.targetRectTransform3.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, RectTransformUtility.WorldToScreenPoint(currentCamera, this._targetPosition3), currentCamera, out this._targetCenter3);
            this._targetSize3 = this.targetRectTransform3.rect.size;
            this.SetAllDirty();
            return;
        }
    }

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (this.targetRectTransform3 == null)
		{
			if (this.targetRectTransform1 == null || this.targetRectTransform1.gameObject == null || !this.targetRectTransform1.gameObject.activeInHierarchy)
			{
				return false;
			}
			bool flag = RectTransformUtility.RectangleContainsScreenPoint(this.targetRectTransform1, sp, eventCamera);
			if (!(this.targetRectTransform2 != null))
			{
				return !flag;
			}
			if (this.targetRectTransform2 == null || this.targetRectTransform2.gameObject == null || !this.targetRectTransform2.gameObject.activeInHierarchy)
			{
				return false;
			}
			bool flag2 = RectTransformUtility.RectangleContainsScreenPoint(this.targetRectTransform2, sp, eventCamera);
			return !flag && !flag2;
		}
		else
		{
			if (this.targetRectTransform3 == null || this.targetRectTransform3.gameObject == null || !this.targetRectTransform3.gameObject.activeInHierarchy)
			{
				return false;
			}
			bool flag3 = RectTransformUtility.RectangleContainsScreenPoint(this.targetRectTransform3, sp, eventCamera);
			return !flag3;
		}
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
        if (!Application.isPlaying)
        {
            return;
        }
		vh.Clear();
		if (!this._isValid)
		{
			return;
		}
		Vector4 outer = new Vector4(-base.rectTransform.pivot.x * base.rectTransform.rect.width, -base.rectTransform.pivot.y * base.rectTransform.rect.height, (1f - base.rectTransform.pivot.x) * base.rectTransform.rect.width, (1f - base.rectTransform.pivot.y) * base.rectTransform.rect.height);
		Vector4 vector = new Vector4(this._targetCenter1.x - this._targetSize1.x / 2f, this._targetCenter1.y - this._targetSize1.y / 2f, this._targetCenter1.x + this._targetSize1.x * 0.5f, this._targetCenter1.y + this._targetSize1.y * 0.5f);
		Vector4 zero = Vector4.zero;
		if (this.targetRectTransform2 != null)
		{
			zero = new Vector4(this._targetCenter2.x - this._targetSize2.x / 2f, this._targetCenter2.y - this._targetSize2.y / 2f, this._targetCenter2.x + this._targetSize2.x * 0.5f, this._targetCenter2.y + this._targetSize2.y * 0.5f);
        }
        //else if (this.targetRectTransform3!=null)
        //{
        //    zero = new Vector4(this._targetCenter3.x - this._targetSize3.x / 2f, this._targetCenter3.y - this._targetSize3.y / 2f, this._targetCenter3.x + this._targetSize3.x * 0.5f, this._targetCenter3.y + this._targetSize3.y * 0.5f);
        //}
		if (this.targetRectTransform2 == null)
		{
			this.Rect(vh, outer, vector);
		}
		else
		{
			this.XRect(vh, outer, vector, zero);
		}
	}

	private void Rect(VertexHelper vh, Vector4 outer, Vector4 inner)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.position = new Vector2(outer.x, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.x, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.x, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.x, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.x, inner.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.x, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.z, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.z, inner.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.z, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.z, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.x, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.x, inner.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.z, inner.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner.z, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		vh.AddTriangle(0, 1, 2);
		vh.AddTriangle(2, 3, 0);
		vh.AddTriangle(4, 5, 6);
		vh.AddTriangle(6, 7, 4);
		vh.AddTriangle(8, 9, 10);
		vh.AddTriangle(10, 11, 8);
		vh.AddTriangle(12, 13, 14);
		vh.AddTriangle(14, 15, 12);
	}

	private void XRect(VertexHelper vh, Vector4 outer, Vector4 inner1, Vector4 inner2)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.position = new Vector2(outer.x, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.x, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(inner2.x, inner1.y);
        simpleVert.color = base.color;
        vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(inner2.x, outer.y);
        simpleVert.color = base.color;
        vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(outer.x, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.x, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner1.x, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner1.x, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.x, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.x, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(inner2.x, outer.w);
        simpleVert.color = base.color;
        vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(inner2.x, inner1.w);
        simpleVert.color = base.color;
        vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(inner2.x, outer.w);
        simpleVert.color = base.color;
        vh.AddVert(simpleVert);
        simpleVert.position = new Vector2(inner2.x, inner2.w);
        simpleVert.color = base.color;
        vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, inner2.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, outer.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner1.z, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner1.z, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, inner1.w);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, inner1.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(outer.z, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.x, inner2.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.x, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, inner2.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		simpleVert.position = new Vector2(inner2.z, outer.y);
		simpleVert.color = base.color;
		vh.AddVert(simpleVert);
		vh.AddTriangle(0, 1, 2);
		vh.AddTriangle(1, 2, 3);
		vh.AddTriangle(4, 5, 6);
		vh.AddTriangle(5, 6, 7);
		vh.AddTriangle(8, 9, 10);
		vh.AddTriangle(9, 10, 11);
		vh.AddTriangle(12, 13, 14);
		vh.AddTriangle(13, 14, 15);
		vh.AddTriangle(16, 17, 18);
		vh.AddTriangle(17, 18, 19);
		vh.AddTriangle(20, 21, 22);
		vh.AddTriangle(21, 22, 23);
		vh.AddTriangle(24, 25, 26);
		vh.AddTriangle(25, 26, 27);
		vh.AddTriangle(28, 29, 30);
		vh.AddTriangle(29, 30, 31);
	}
}
