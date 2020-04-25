using UnityEngine;

namespace Game.Match3
{
    public class M3Background : MonoBehaviour
    {
        #region Static

        public static M3Background Instance;

        #endregion

        public Sprite[] bgSprites;

        #region UNITY

        private void Start()
        {
            GameObject obj;
            if (KAssetManager.Instance.TryGetMatchPrefab("Match3Bg", out obj))
            {
                bgSprites = obj.GetComponent<KUIAtlas>().sprites;
            }
            GetComponent<SpriteRenderer>().sprite = bgSprites[0];
        }

        #endregion
    }
}