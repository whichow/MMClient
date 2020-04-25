using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public UIList uilist;
    public RawImage rawImage;
    public Button btn;
    public Button btn2;
    public Button btn3;

    // Use this for initialization
    void Start ()
    {
        TestUIList();

        //TestRenderTexture();

        btn2.onClick.AddListener(btnHandler);
        btn3.onClick.AddListener(btnHandler3);
    }

    private void btnHandler()
    {
        Debug.Log("btn");
    }
    private void btnHandler3()
    {
        Debug.Log("btn3");

        //EventSystem.current.SetSelectedGameObject(btn2);
    }
    private void TestUIList()
    {
        uilist.SetRenderHandler(RenderHandler);
        uilist.SetSelectHandler(SelectHandler);
        uilist.SetPointerHandler(PointerHandler);
        List<int> array = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            array.Add(i);
        }
        uilist.DataArray = array;
        uilist.SelectedIndex = 50;
        uilist.Page = 2;
    }

    private void PointerHandler(UIListItem item, int index)
    {
        Debug.Log("PointerHandler " + index);
    }

    private void SelectHandler(UIListItem item, int index)
    {
        Debug.Log(index);
    }

    private void RenderHandler(UIListItem item, int index)
    {
        item.GetComp<Text>("NameTxt").text = index.ToString();
    }


    public GameObject bg;
    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            bg.SetActive(false);
            Debug.Log("GetMouseButtonDown");
        }
        if (Input.GetMouseButtonUp(0))
        {
            bg.SetActive(true);
            Debug.Log("GetMouseButtonUp");
        }

    }

    UnitRenderTexture unitRenderTexture;
    private void TestRenderTexture()
    {
        unitRenderTexture = new UnitRenderTexture();
        //unitRenderTexture.SetRole("abc");
        rawImage.texture = unitRenderTexture.RenderTexture;
        unitRenderTexture.SetPet(1);

        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        unitRenderTexture.PlayAnimation(RoleAvatar.ANIM_SKILL);
    }
}
