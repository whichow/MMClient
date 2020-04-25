#if UNITY_EDITOR
using Game.DataModel;
using Game.Match3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M3RuleElementItem : MonoBehaviour {


    public Image icon;
    public int eleId;
    public int weight;
    public InputField input;


    public void Init()
    {
        icon = transform.Find("icon").GetComponent<Image>();
        input = transform.Find("weight").GetComponent<InputField>();
    }
    public void UpdateData(int id,int w)
    {
        icon.overrideSprite = Game.KIconManager.Instance.GetMatch3ElementIcon(XTable.ElementXTable.GetByID(id).Icon);
        input.text = w.ToString();
        eleId = id;
        weight = w;
    }

    public Int2 Get()
    {
        return new Int2(eleId, int.Parse(input.text));
    }

}
#endif