#if UNITY_EDITOR
using System;
/** 
*FileName:     M3EditorCell.cs 
*Author:       HeJunJie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-07-07 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using M3Editor;
using UnityEngine.UI;
using Game.Match3;
using Game.DataModel;

public class M3EditorCell : MonoBehaviour
{
    public int gridX;
    public int gridY;
    public M3CellData cellData;
    public M3ItemDropContrl drop;

    private bool isEmpty;
    private bool isPort;

    public ElementXDM[] configs;//单元格上的元素数组
    public GameObject[] configsObj;
    public Button btn;


    public int validElementCount = 0;
    private Image hiddenImg;
    private GameObject portObj;
    private GameObject emptyObj;
    private GameObject conveyorObj;
    private GameObject fishExitObj;
    private GameObject fishPortObj;

    private GameObject portInObj;
    private GameObject portOutObj;

    public Sprite cellSprite;

    private GameObject root;

    private GameObject fishFlag;

    public string portRule = string.Empty;

    private Text posText;

    private bool isConveyor = false;

    private bool isFishExit;
    public bool IsPort
    {
        get
        {
            return isPort;
        }

        set
        {
            isPort = value;
            portObj.SetActive(value);
        }
    }

    public bool IsEmpty
    {
        get
        {
            return isEmpty;
        }

        set
        {
            isEmpty = value;
            emptyObj.SetActive(value);
        }
    }

    public bool IsConveyor
    {
        get
        {
            return isConveyor;
        }

        set
        {
            SetConveyor(value);
            isConveyor = value;
        }
    }

    public void SetHiddenPiece(Color color)
    {
        hiddenImg.color = color;
    }

    public bool IsFishExit
    {
        get
        {
            return isFishExit;
        }

        set
        {
            SetCollectExit(value);
            isFishExit = value;
        }
    }

    public bool IsFishPort
    {
        get
        {
            return isFishPort;
        }

        set
        {
            SetCollectPort(value);
            isFishPort = value;
        }
    }

    public bool IsHidden
    {
        get
        {
            return isHidden;
        }

        set
        {
            SetHidden(value);
            isHidden = value;
        }
    }

    private bool isFishPort;
    private string portalType;
    private bool isHidden;

    private void Awake()
    {

    }
    private void Start()
    {
    }
    public void Init(int x, int y, int index, M3CellData data)
    {
        fishFlag = transform.Find("fishFlag").gameObject;
        root = transform.Find("root").gameObject;
        portObj = transform.Find("root/port").gameObject;
        emptyObj = transform.Find("root/empty").gameObject;
        conveyorObj = transform.Find("root/conveyor").gameObject;
        fishExitObj = transform.Find("root/fishExit").gameObject;
        fishPortObj = transform.Find("fishPort").gameObject;
        posText = transform.Find("Text").GetComponent<Text>();
        portInObj = transform.Find("portin").gameObject;
        portOutObj = transform.Find("portout").gameObject;
        hiddenImg = transform.Find("hidden").GetComponent<Image>();
        hiddenImg.gameObject.SetActive(false);
        portInObj.SetActive(false);
        portOutObj.SetActive(false);
        portObj.SetActive(false);
        emptyObj.SetActive(false);
        //cellSprite = transform.Find("board").GetComponent<Image>().sprite;
        btn = GetComponent<Button>();
        drop = GetComponent<M3ItemDropContrl>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnCellClick);
        gridX = x; gridY = y;
        posText.text = "(" + x.ToString() + "," + y.ToString();
        cellData = data;
        configs = new ElementXDM[6];
        configsObj = new GameObject[6];
        drop.dropHandler = AddElement;
        //drop.toolHandler = AddTool;
        validElementCount = 0;
        foreach (var item in cellData.elementsList)
        {
            AddElement(item);
        }
        fishFlag.SetActive(false);

        portalType = string.Empty;
        IsHidden = false;
    }

    public string GetPortal()
    {
        return this.portalType;
    }
    public void SetPortal(string v, Color c)
    {
        portalType = v;
        if (v == "portal_in")
        {
            portInObj.GetComponent<Image>().color = c;
            portInObj.SetActive(true);
        }
        else if (v == "portal_out")
        {
            portOutObj.GetComponent<Image>().color = c;
            portOutObj.SetActive(true);
        }
    }
    public void RemovePortal()
    {
        portalType = string.Empty;
        portInObj.SetActive(false);
        portOutObj.SetActive(false);
    }
    private void SetConveyor(bool v)
    {
        conveyorObj.SetActive(v);
    }

    private void SetCollectExit(bool v)
    {
        fishExitObj.SetActive(v);
    }
    private void SetCollectPort(bool v)
    {
        fishPortObj.SetActive(v);
    }
    private void SetHidden(bool v)
    {
        hiddenImg.gameObject.SetActive(v);
    }


    private void OnCellClick()
    {
        if (M3EditorController.instance.EditorMode == mEditorMode.mDelete)
        {
            OnDeleteElement();
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mPortal)
        {
            M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorPortal>().SetPortal(this, false);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mPortalRemove)
        {
            M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorPortal>().removePortal(this);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mConveyor)
        {
            Debug.Log("传送带");
            M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorConveyor>().SetConveyor(this);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mConveyorRemove)
        {
            Debug.Log("传送带移除");
            M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorConveyor>().RemoveConveyor(this);
            M3EditorController.instance.EditorMode = mEditorMode.mEditor;
        }

        else if (M3EditorController.instance.EditorMode == mEditorMode.SelectFish)
        {
            var v = (M3EditorController.instance.fishCtrl.current.ReciveCell(this));
            if (v)
                fishFlag.SetActive(false);
            else
                fishFlag.SetActive(true);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.ShowFish)
        {

        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mBrush)
        {
            AddElement(M3EditorController.instance.gridCtrl.currentBrush);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mHiddenStart)
        {
            M3EditorHidden module = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorHidden>();
            module.count = 0;
            module.countmax = 1;
            module.SetHidden(this);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mHiddenRemove)
        {
            M3EditorHidden module = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorHidden>();
            module.RemoveHidden(this);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mRandomHiddenStart)
        {
            M3EditorHidden module = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorHidden>();
            module.StartRandom(this);
        }
        else if (M3EditorController.instance.EditorMode == mEditorMode.mRandomHiddenRemove)
        {
            M3EditorHidden module = M3EditorController.instance.editorMain.moduleManager.GetModule<M3EditorHidden>();
            module.RemoveRandom(this);
        }
        else
        {
            M3EditorController.instance.OpenToolBar(this);
        }
    }

    public void OnShowFishFlag(bool v)
    {
        fishFlag.SetActive(v);
    }
    public void SetRoot(bool v)
    {
        root.SetActive(v);
    }
    public void SetPort(string rule)
    {
        if (!isPort)
            portRule = rule;
        else
        {
            portRule = string.Empty;
        }
        IsPort = !IsPort;

    }
    public void SetEmpty()
    {
        IsEmpty = !IsEmpty;
    }
    private void AddElement(int Id)
    {
        if (validElementCount == -1)
        {
            return;
        }
        if (Id == -1)
        {
            validElementCount = -1;
            RemoveCell();
            return;
        }
        if (Id == 0)
            return;
        if (validElementCount > 6)
        {
            Debug.Log("当前格子摆放元素已达上限");
        }
        ElementXDM config = XTable.ElementXTable.GetByID(Id);
        if (config == null)
        {
            //Debug.LogError("Element ID: " + Id + "  Is Null");
            return;
        }
        if (config.TypeLevel > configs.Length)
            return;
        if (configs[config.TypeLevel - 1] != null)
        {
            Debug.Log("该单元格元素层级重复");
            return;
        }
        configs[config.TypeLevel - 1] = config;
        validElementCount++;
        AddElementGO(config);
    }

    private void RemoveCell()
    {
        //this.transform.GetComponent<Image>().sprite = null;
        this.transform.GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);
    }
    private void AddCell()
    {
        //this.transform.GetComponent<Image>().sprite = cellSprite;
        this.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }


    private void AddElementGO(ElementXDM config)
    {

        var eleObj = this.transform.Find("root/" + (config.TypeLevel).ToString()).gameObject;
        Image img = eleObj.AddComponent<Image>();
        img.sprite = Game.KIconManager.Instance.GetMatch3ElementIcon(config.Icon);
        configsObj[config.TypeLevel - 1] = eleObj;
    }

    public void OnDeleteElement()
    {
        if (validElementCount == 0)
        {
            RemoveCell();
            validElementCount = -1;
            return;
        }
        if (validElementCount == -1)
        {
            AddCell();
            validElementCount = 0;
            return;
        }

        for (int i = 0; i < configs.Length; i++)
        {
            configs[i] = null;
            validElementCount = 0;
        }
        for (int i = 0; i < configsObj.Length; i++)
        {
            if (configsObj[i])
            {
                var img = configsObj[i].GetComponent<Image>();
                if (img != null)
                {
                    Destroy(configsObj[i].GetComponent<Image>());
                }
                configsObj[i] = null;
            }
        }
    }
    public void ClearCell()
    {

    }
    public override string ToString()
    {
        if (validElementCount == -1)
            return "-1";
        if (validElementCount == 0)
            return "0";
        string value = null;
        for (int i = 0; i < configs.Length; i++)
        {
            if (configs[i] == null)
                continue;
            if (value == null)
                value += configs[i].ID;
            else
                value += ("|" + configs[i].ID);
        }
        return value;
    }
    public int needRandomElement()
    {
        if (configs[2] == null)
            return 1;
        return 0;
    }

    public void ShowDisk(bool value)
    {
       SetRoot(value);
    }
    public void ShowHidden(bool value)
    {

    }
}
#endif
