using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlots : MonoBehaviour {
    public Image SkillIcon;

    public Image CooldownGray;
    public Text  CooldownText;

    public void UpdateSkillSlot(string ImageAddr, int ImageNumb, int cd)
    {
        Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/UI/Icons/" + ImageAddr);
        SkillIcon.sprite = newSprite[ImageNumb];

        if(cd > 0)
        {
            CooldownGray.enabled = true;
            CooldownText.text = "" + cd;
        }
        else
        {
            CooldownGray.enabled = false;
            CooldownText.text = "";
        }

        CDCheck(cd);
    }

    public void ReduceCooldown(int cd)
    {
        int CurrentCooldown = int.Parse(CooldownText.text);
        CurrentCooldown -= cd;

        CDCheck(CurrentCooldown);
    }

    void CDCheck(int cd)
    {
        if (cd > 0)
        {
            CooldownGray.enabled = true;
            CooldownText.text = "" + cd;
        }
        else
        {
            CooldownGray.enabled = false;
            CooldownText.text = "";
        }
    }

    //EventTriggers

    public void OnPointerEnter()
    {
        Debug.Log("OnPointerEnter");

        BattleCanvasManager.instance.RefreshSkillInfoWindow(int.Parse(gameObject.name.Split('_')[1]),gameObject);

        //Wait은 어떻게 할까? 따로 만들까?
        //모바일은 터치시 보여주고 한번 더 터치하면 클릭
    }

    public void OnPointerExit()
    {
        Debug.Log("OnPointerEnter");

        BattleCanvasManager.instance.HideSkillInfoWindow();
    }

    public void OnPointerClick()
    {
        Debug.Log("OnPointerClick");

        //이거는 BattleManager에서
    }
}
