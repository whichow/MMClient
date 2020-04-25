using System.Collections;
using UnityEngine;

namespace Game.Match3
{
    public class M3GridView : MonoBehaviour
    {
        #region Field

        SpriteRenderer _spriteRenderer;

        GameObject _ropeL;
        GameObject _ropeR;
        GameObject _ropeT;
        GameObject _ropeB;
        GameObject tg;

        GameObject conveyor;
        GameObject fishExit;

        //SpriteRenderer _iceSR;

        public Transform gridTransform;

        string portalPrefabName = "transfergate";
        #endregion

        public void Init(M3Grid grid, Transform go)
        {
            gridTransform = go;
            _ropeT = transform.Find("Rope_T").gameObject;
            _ropeB = transform.Find("Rope_B").gameObject;
            _ropeL = transform.Find("Rope_L").gameObject;
            _ropeR = transform.Find("Rope_R").gameObject;
            fishExit = transform.Find("fishExit").gameObject;
            conveyor = transform.Find("conveyor").gameObject;

            tg = transform.Find("transferGate").gameObject;
            tg.SetActive(true);
        }

        public void UpdateView(M3GridInfo gridInfo)
        {
            if (gridInfo == null)
            {
                return;
            }
            GenRope(gridInfo, RopeTypeEnum.Top, _ropeT);
            GenRope(gridInfo, RopeTypeEnum.Bottom, _ropeB);
            GenRope(gridInfo, RopeTypeEnum.Left, _ropeL);
            GenRope(gridInfo, RopeTypeEnum.Rigth, _ropeR);
            transform.SetParent(M3GridManager.Instance.gridParent.transform);
            transform.localPosition = new Vector3(gridInfo.posY * M3Config.DistancePerUnit, -gridInfo.posX * M3Config.DistancePerUnit);
        }
        public void CreatePortal(bool isIn)
        {
            GameObject obj;
            if (KAssetManager.Instance.TryGetMatchPrefab(portalPrefabName, out obj))
            {
                GameObject tgObj = Instantiate(obj);
                tgObj.transform.SetParent(tg.transform, false);
                if (isIn)
                    tgObj.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
        }
        private void GenRope(M3GridInfo gridInfo, RopeTypeEnum type, GameObject parent)
        {
            var ropeDirection = gridInfo.ropeTypeEnum;
            if ((ropeDirection & type) == type)
            {
                parent.SetActive(true);
                M3FxManager.Instance.LoadM3Prefab(gridInfo.ropeElement.ModelName, parent);
            }
        }
        public void GenFishBoard()
        {
            fishExit.SetActive(true);
        }
        public void PlayerFloorElementAnima(M3GridInfo gridInfo)
        {
            //M3FxManager.Instance.PlayM3CommonEffect(gridInfo.floorElement.clearAnim, floorFxObj, M3EffectConfig.MagicScrollSpeed, M3EffectConfig.MagicScrollDestroyTime);
        }

        private IEnumerator PlayIceFx(int iceLevel)
        {
            var fxObj = transform.Find("ice_" + iceLevel).gameObject;
            fxObj.SetActive(true);
            yield return new WaitForSeconds(1f);
            fxObj.SetActive(false);
        }

        #region Unity
        public void SetConveyor(bool v)
        {
            conveyor.SetActive(v);
        }

        #endregion
    }
}