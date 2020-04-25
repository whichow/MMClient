
using Game;
/** 
*FileName:     M3EditorCatSelect.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-30 
*Description:    
*History: 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class M3EditorCatSelect : MonoBehaviour
{

    public InputField catIdInput;
    public InputField catStarInput;
    public InputField catLvInput;
    public InputField catMatch;
    public InputField catSkillLV;
    public Button backBtn;
    public Button saveBtn;

    private KCat cat;


    public void Awake()
    {
        backBtn.onClick.AddListener(BackBtnCLick);
        saveBtn.onClick.AddListener(SaveBtnClick);
    }
    private void OnEnable()
    {
        catIdInput.text = PlayerPrefs.GetInt("M3EditorCatID",10401).ToString();
        catStarInput.text = PlayerPrefs.GetInt("M3Editorstar", 1).ToString();
        catLvInput.text = PlayerPrefs.GetInt("M3Editorlv", 1).ToString();
        catMatch.text= PlayerPrefs.GetInt("M3Editormatch", 1).ToString();
        catSkillLV.text = PlayerPrefs.GetInt("M3EditorSkillLV", 1).ToString();
    }
    private void SaveBtnClick()
    {
        int id = int.Parse(catIdInput.text);
        int star = int.Parse(catStarInput.text);
        int lv = int.Parse(catLvInput.text);
        int match = int.Parse(catMatch.text);
        int skillLV = int.Parse(catSkillLV.text);
        PlayerPrefs.SetInt("M3EditorCatID", id);
        PlayerPrefs.SetInt("M3Editorstar", star);
        PlayerPrefs.SetInt("M3Editorlv", lv);
        PlayerPrefs.SetInt("M3Editormatch", match);
        PlayerPrefs.SetInt("M3EditorSkillLV", skillLV);
        this.gameObject.SetActive(false);
    }

    private void BackBtnCLick()
    {
        this.gameObject.SetActive(false);
    }
}
