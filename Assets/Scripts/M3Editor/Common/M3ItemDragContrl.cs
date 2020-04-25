/** 
*FileName:     M3ItemDragContrl.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-06 
*Description:    
*History: 
*/
#if UNITY_EDITOR
using Game;
using Game.DataModel;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace M3Editor
{
    public class M3ItemDragContrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        #region field
        private RectTransform _dragTransform;
        private GameObject _dragObj;
        public ElementXDM _eleCofing;

        private Image iconimg;
        private Text nameText;
        #endregion

        #region method
        private void SetDragIconPosition(PointerEventData data)
        {
            if (data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
                _dragTransform = data.pointerEnter.transform as RectTransform;
            RectTransform rt = _dragObj.GetComponent<RectTransform>();
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_dragTransform, data.position, data.pressEventCamera, out globalMousePos))
                rt.position = globalMousePos;
        }

        public void Init(int elementID)
        {
            _eleCofing = XTable.ElementXTable.GetByID(elementID);
            _dragTransform = GetComponent<RectTransform>();
            _dragObj = _dragTransform.gameObject;
            iconimg = Game.TransformUtils.GetChildByName(this.transform, "icon").GetComponent<Image>();
            nameText = Game.TransformUtils.GetChildByName(this.transform, "name").GetComponent<Text>();
            Show();
        }

        private void Show()
        {
            try
            {
                iconimg.sprite = KIconManager.Instance.GetMatch3ElementIcon(_eleCofing.Icon);
                nameText.text = _eleCofing.Name;
            }
            catch (Exception)
            {
                Debug.Log(_eleCofing);
            }
        }
        #endregion

        #region unity
        private void Start()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (M3EditorController.instance.EditorMode != mEditorMode.mEditor &&
                M3EditorController.instance.EditorMode != mEditorMode.mDelete && M3EditorController.instance.EditorMode != mEditorMode.mBrush)
                return;

            _dragObj = new GameObject();
            _dragObj.transform.SetParent(M3EditorController.instance.transform, false);
            Image image = _dragObj.AddComponent<Image>();
            image.sprite = iconimg.sprite;
            var group = _dragObj.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
            SetDragIconPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (M3EditorController.instance.EditorMode != mEditorMode.mEditor &&
       M3EditorController.instance.EditorMode != mEditorMode.mDelete && M3EditorController.instance.EditorMode != mEditorMode.mBrush)
                return;
            SetDragIconPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (M3EditorController.instance.EditorMode != mEditorMode.mEditor &&
       M3EditorController.instance.EditorMode != mEditorMode.mDelete && M3EditorController.instance.EditorMode != mEditorMode.mBrush)
                return;
            if (_dragObj != null)
                Destroy(_dragObj);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            M3EditorController.instance.gridCtrl.SetBrush(_eleCofing.ID);
            M3EditorController.instance.itemGridCtrl.SetCurrentBrushIcon(_eleCofing != null ? _eleCofing.Icon : "");
            M3EditorController.instance.EditorMode = mEditorMode.mBrush;
        }
        #endregion
    }
}
#endif
