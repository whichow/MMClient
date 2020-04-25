using System.Collections;
using System.Collections.Generic;

namespace Game.Match3
{
    public class M3Effect
    {
        public int id;
        public string name;
        public string modelName;
        public string skin;
        public string animName;
        public float duration;
        public float speed;
        public string description;

        public Hashtable animations;
        static public readonly string urlKey = "EffectConfig";
        static public Dictionary<int, M3Effect> effectDic;

        public static void LoadTable(Hashtable table)
        {
            var list = table.GetArrayList(urlKey);
            effectDic = new Dictionary<int, M3Effect>();
            ArrayList proList = (ArrayList)list[0];
            ArrayList tmpList = new ArrayList();
            int idIndex = proList.IndexOf("id");
            int nameIndex = proList.IndexOf("name");
            int modelNameIndex = proList.IndexOf("modelName");
            int skinIndex = proList.IndexOf("skin");
            int animNameIndex = proList.IndexOf("animName");
            int durationIndex = proList.IndexOf("duration");
            int descriptionIndex = proList.IndexOf("description");
            int speedIndex = proList.IndexOf("speed");

            int animationsIndex = proList.IndexOf("animations");
            for (int i = 1; i < list.Count; i++)
            {
                M3Effect effect = new M3Effect();
                tmpList = (ArrayList)list[i];
                effect.id = (int)tmpList[idIndex];
                effect.name = tmpList[nameIndex].ToString();
                effect.modelName = tmpList[modelNameIndex].ToString();
                effect.skin = tmpList[skinIndex].ToString();
                effect.animName = tmpList[animNameIndex].ToString();
                var tmpTime = (int)tmpList[durationIndex];
                effect.duration = tmpTime / 1000f;
                effect.description = tmpList[descriptionIndex].ToString();
                effect.speed = ((int)tmpList[speedIndex]) / 1000.0f;
                effect.animations = tmpList.GetTable(animationsIndex);
                //Debug.Log(effect.name);
                effectDic.Add(effect.id, effect);
            }
        }

        public static M3Effect GetEffectConfig(int id)
        {
            if (effectDic != null && effectDic.ContainsKey(id))
                return effectDic[id];
            return null;
        }

    }
}