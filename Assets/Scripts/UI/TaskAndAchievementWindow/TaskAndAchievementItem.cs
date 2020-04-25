// ***********************************************************************
// 作用：任务和成就面板的子物体的控制类
//作者：wsy
// ***********************************************************************
using Game.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class TaskAndAchievementItem : KUIItem, IPointerClickHandler
    {
        #region Filed        
        private Text _txt_title;
        private Text _txt_content;
        private GameObject _go_value;
        private Text _txt_value;
        private Image _img_value;
        private Button _btn_get;
        private Transform[] _tras_awardparent;
        private GameObject _go_temAward;
        private const int AwardLength = 3;
        private KMission _TAData;
        #endregion

        #region Method
        private void Awake()
        {
            _txt_title = Find<Text>("Item/txt_title");
            _txt_content = Find<Text>("Item/txt_description");
            _go_value = Find<Transform>("Item/Right/Image").gameObject;
            _txt_value = Find<Text>("Item/Right/Image/Text");
            _img_value = Find<Image>("Item/Right/Image/Image");
            _btn_get = Find<Button>("Item/Right/btn_get");
            _go_temAward = Find<Transform>("Item/Icon/Image").gameObject;
            _go_temAward.SetActive(false);
            _tras_awardparent = new Transform[3];
            for (int i = 0; i < AwardLength; i++)
            {
                _tras_awardparent[i] = Find<Transform>("Item/Icon/tras_" + i.ToString());
            }
        }
        #endregion

        #region Method
        public void ShowItem(KMission[] TAs, int index)
        {
            if (TAs == null || TAs.Length == 0)
            {
                return;
            }
            var TA = TAs[index];
            _TAData = TA;
            _txt_title.text = KLocalization.GetLocalString(_TAData.displayName);
            _txt_content.text = KLocalization.GetLocalString(_TAData.description);
            ShowAwardList();
            _btn_get.onClick.RemoveAllListeners();
            if (_TAData.status == 1)
            {
                _go_value.SetActive(false);
                _btn_get.gameObject.SetActive(true);
                _btn_get.onClick.AddListener(GetAward);
            }
            else if (_TAData.status == 0)
            {
                _go_value.SetActive(true);
                _btn_get.gameObject.SetActive(false);
            }
            else if (_TAData.status == 2)
            {
                _go_value.SetActive(true);
                _btn_get.gameObject.SetActive(false);
                RefreshScheduleValue(_TAData.curValue, _TAData.maxValue, _TAData.status);
            }
            else
            {
                Debug.LogError("Task or Achievement Status is wrong.");
            }
        }
        private void GetAward()
        {
            //Debug.Log("Ccurrent type == " + _TAData.type);
            if (_TAData.type == 1)
            {
                KMissionManager.Instance.RewardDialy(_TAData.id, GetAwardAction);
            }
            else if (_TAData.type == 2)
            {
                KMissionManager.Instance.RewardAchievement(_TAData.id, GetAwardAction);
            }
            else
            {
                Debug.LogError("Task or Achievement Type is wrong，current type == " + _TAData.type);
            }
        }
        private void GetAwardAction(int code, string message, object data)
        {
            KUIWindow.OpenWindow<ShowAwardList>(_TAData);
            //Int3[] list = new Int3[_TAData.bonusItems.Length];
            //for (int i = 0; i < _TAData.bonusItems.Length; i++)
            //{
            //    list[i].x = _TAData.bonusItems[i].itemID;
            //    list[i].y = _TAData.bonusItems[i].itemCount;
            //    list[i].z = (int)KItem.GiftType.kNone;
            //}
            //IconFlyMgr.Instance.IconPathGroupStart(_btn_get.gameObject.transform.position, 0.2f, list, true);


            KUIWindow.GetWindow<TaskAndAchievementWindow>().RefreshListAfterGetAward(code, message, data);
        }
        private void ShowAwardList()
        {
            if (_TAData.bonusItems == null || _TAData.bonusItems.Length == 0)
            {
                for (int i = 0; i < AwardLength; i++)
                {
                    _tras_awardparent[i].gameObject.SetActive(false);
                }
                return;
            }
            for (int i = 0; i < AwardLength; i++)
            {
                if (i > _TAData.bonusItems.Length - 1)
                {
                    _tras_awardparent[i].gameObject.SetActive(false);
                }
                else
                {
                    RefreshSingalAward(_TAData.bonusItems[i], i);
                }
            }
        }

        private void RefreshSingalAward(KItem.ItemInfo data, int index)
        {
            GameObject oneAward;
            if (_tras_awardparent[index].childCount == 0)
            {
                oneAward = GameObject.Instantiate(_go_temAward);
                oneAward.transform.SetParent(_tras_awardparent[index], false);
                oneAward.transform.localPosition = Vector3.zero;
                oneAward.transform.localScale = Vector3.one;
            }
            else
            {
                oneAward = _tras_awardparent[index].GetChild(0).gameObject;
                for (int j = 0; j < _tras_awardparent[index].childCount; j++)
                {
                    if (j != 0)
                    {
                        GameObject.Destroy(_tras_awardparent[index].GetChild(j).gameObject);
                    }
                }
            }
            oneAward.SetActive(true);
            Text nm = oneAward.transform.Find("Text").gameObject.GetComponent<Text>();
            Image awIc = oneAward.transform.Find("Icon").gameObject.GetComponent<Image>();
            RefreshScheduleValue(_TAData.curValue, _TAData.maxValue, _TAData.status);
            nm.text = data.itemCount.ToString();
            awIc.overrideSprite = KIconManager.Instance.GetItemIcon(data.itemID);
            //Debug.Log("Refresh Icon：" + data.itemID);
            KItem onedataforshow = KItemManager.Instance.GetItem(data.itemID);
            oneAward.GetComponent<Button>().onClick.AddListener(() =>
            {
                KUIWindow.OpenWindow<ItemInformationBoard>(
                    new ItemInformationBoard.ItemInformationData
                    {
                        _title = KLocalization.GetLocalString(onedataforshow.itemName),
                        _int_id = onedataforshow.itemID,
                        _txt_first = KLocalization.GetLocalString(onedataforshow.description),
                    }
                    );
            }
            );
        }

        public void RefreshScheduleValue(int cV, int mV, int st)
        {
            if (st == 2)
            {
                _img_value.gameObject.SetActive(false);
                _img_value.fillAmount = (float)cV / mV;
                _txt_value.text = "已领取";
            }
            else
            {
                _img_value.gameObject.SetActive(true);
                _img_value.fillAmount = (float)cV / mV;
                _txt_value.text = cV.ToString() + "/" + mV.ToString();
            }
        }
        #endregion
        #region Interface
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {

        }
        #endregion
    }
}