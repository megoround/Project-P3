using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvasManager : MonoBehaviour
{
    //BattleManager는 로직 처리
    //이쪽은 UI 처리를 할거에요
    //아마

    public static BattleCanvasManager instance = null;

    public GameObject SkillInfoWindow;
    public GameObject CurrentSelectedChar;

    //Status
    public Image FaceImage;
    public Text NameText;

    public Slider HPSlider;
    public Text HPText;

    //Skills
    public SkillSlots[] SkillSlots = new SkillSlots[5]; // 마지막은 대기

    private void Awake()
    {
        instance = this;
    }

    public void RefreshSelectedChar(GameObject Target)
    {
        //Status
        BattleCharacter bc = Target.GetComponent<BattleCharacter>();

        Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Face/" + bc.Data.BaseData.FaceImage);
        FaceImage.sprite = newSprite[bc.Data.BaseData.FaceImageNumber];
        NameText.text = bc.Data.Name;
        HPSlider.value = bc.Data.CurrentHP / (float)bc.Data.MaxHp;
        HPText.text = "" + bc.Data.CurrentHP + " / " + (float)bc.Data.MaxHp;

        //Skill
        for (int i = 0; i < 4; i++)
        {
            //SkillSlots[i].UpdateSkillSlot(bc.Data.BaseData.Skills[i].Image, bc.Data.BaseData.Skills[i].ImageNumber, bc.Data.CurrentSkillCD[i]);

            SkillSlots[i].UpdateSkillSlot(_GameManager.instance.XML_Skills[bc.Data.BaseData.SkillNumber[i]].Image
                , _GameManager.instance.XML_Skills[bc.Data.BaseData.SkillNumber[i]].ImageNumber, bc.Data.CurrentSkillCD[i]);
        }
    }

    IEnumerator SkillWindowMove()
    {
        while(SkillInfoWindow.activeSelf)
        {
            Debug.Log(Input.mousePosition);
            SkillInfoWindow.transform.position = Input.mousePosition;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void RefreshSkillInfoWindow(int SkillNumber,GameObject Button)
    {
        //SkillInfoWindow 스크립트를 만들어서 넘길까...

        Debug.Log(Button.name);
        Debug.Log(SkillInfoWindow.transform.position);
        Debug.Log(Button.transform.position);

        int SN = CurrentSelectedChar.GetComponent<BattleCharacter>().Data.BaseData.SkillNumber[SkillNumber];
        Skill Selected = _GameManager.instance.XML_Skills[SN];

        string PhaseColor = "";
        switch (Selected.Phase)
        {
            case 0: PhaseColor = "<color=#00ff00>"; break; //prep
            case 1: PhaseColor = "<color=#ffff00>"; break; //dash
            case 2: PhaseColor = "<color=#ff0000>"; break; //blast  
        }

        SkillInfoWindow.transform.Find("SkillMainWindow/Name").gameObject.GetComponent<Text>().text = PhaseColor + Selected.Name + "</color>";
        SkillInfoWindow.transform.Find("SkillMainWindow/Text").gameObject.GetComponent<Text>().text = Selected.Text;
        SkillInfoWindow.transform.Find("SkillMainWindow/SkillCD/CDtext").gameObject.GetComponent<Text>().text = "" + Selected.SkillCD;

        if (Selected.isFree == 1)
            SkillInfoWindow.transform.Find("SkillMainWindow/Free").gameObject.SetActive(true);
        else
            SkillInfoWindow.transform.Find("SkillMainWindow/Free").gameObject.SetActive(false);

        ///

        for (int i = 0; i < 3; i++)
        {
            GameObject TargetSquare =
            SkillInfoWindow.transform.Find("SkillSubWindow/Area1/CastAreaSquare/Square" + (i + 1)).gameObject;

            if (Selected.CastArea[i] == 1)
                TargetSquare.GetComponent<Image>().color = Color.blue;
            else
                TargetSquare.GetComponent<Image>().color = Color.gray;
        }
        for (int i = 0; i < 9; i++)
        {
            GameObject TargetSquare =
            SkillInfoWindow.transform.Find("SkillSubWindow/Area2/TargetAreaSquare/Square" + (i + 1)).gameObject;

            if (Selected.TargetArea[i] == 1)
            {
                if (Selected.isHealing == 1)
                    TargetSquare.GetComponent<Image>().color = Color.yellow;
                else
                    TargetSquare.GetComponent<Image>().color = Color.yellow;
            }
            else
                TargetSquare.GetComponent<Image>().color = Color.gray;
        }
        for (int i = 0; i < 9; i++)
        {
            GameObject TargetSquare =
            SkillInfoWindow.transform.Find("SkillSubWindow/Area3/EffectAreaSquare/Square" + (i + 1)).gameObject;

            if (Selected.EffectArea[i] == 1)
            {
                if (Selected.isHealing == 1)
                    TargetSquare.GetComponent<Image>().color = Color.green;
                else
                    TargetSquare.GetComponent<Image>().color = Color.red;
            }
            else
                TargetSquare.GetComponent<Image>().color = Color.gray;
        }
        //
        SkillInfoWindow.transform.Find("SkillSubWindow/Area4/Window/EffectText").gameObject.GetComponent<Text>().text = Selected.ExtraText;

        SkillInfoWindow.SetActive(true);
        SkillInfoWindow.transform.position = Button.transform.position + new Vector3(0, 25);
    }

    public void HideSkillInfoWindow()
    {
        SkillInfoWindow.SetActive(false);
    }

    //

    public void CharTouched(GameObject Target)
    {
        //Battle에서 다하지 말고 적당히 쌓이면 UI클래스를 하나 더 만들자
        //if not targetting time

        if (CurrentSelectedChar != null)
        {
            if (Target == CurrentSelectedChar) return;

            //StopCoroutine(CharTouchedMove(CurrentSelectedChar));
            Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Char/" + Target.GetComponent<BattleCharacter>().Data.BaseData.CharImage);
            Target.transform.GetChild(0).GetComponent<Image>().sprite = newSprite[Target.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber];
        }

        CurrentSelectedChar = Target;
        Debug.Log(Target.GetComponent<BattleCharacter>().Data.Name);
        RefreshSelectedChar(Target);
        StartCoroutine(CharTouchedMove(Target));
    }
    IEnumerator CharTouchedMove(GameObject Target) // temp
    {
        Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Char/" + Target.GetComponent<BattleCharacter>().Data.BaseData.CharImage);

        int[] CharImageNumbers = new int[4];
        CharImageNumbers[0] = Target.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber - 1;
        CharImageNumbers[1] = Target.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber;
        CharImageNumbers[2] = Target.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber + 1;
        CharImageNumbers[3] = Target.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber;

        while (Target == CurrentSelectedChar)
        {
            for (int i = 0; i < 4 && Target == CurrentSelectedChar; i++)
            {
                yield return new WaitForSeconds(0.2f);
                Target.transform.GetChild(0).GetComponent<Image>().sprite = newSprite[CharImageNumbers[i]];
            }
        }
    }

    //void SkillWindowPopup(GameObject Name)
    //{
    //    string[] substring = Name.name.Split('_');
    //    int number = int.Parse(substring[1]);
    //    Debug.Log("" + name);
    //    Skill Selected = _GameManager.instance.PlayerParty.Characters[0].BaseData.Skills[number - 1];

    //    string PhaseColor = "";
    //    switch (Selected.Phase)
    //    {
    //        case 0: PhaseColor = "<color=#00ff00>"; break; //prep
    //        case 1: PhaseColor = "<color=#ffff00>"; break; //dash
    //        case 2: PhaseColor = "<color=#ff0000>"; break; //blast  
    //    }

    //    SkillInfoWindow.transform.Find("SkillMainWindow/Name").gameObject.GetComponent<Text>().text = PhaseColor + Selected.Name + "</color>";
    //    SkillInfoWindow.transform.Find("SkillMainWindow/Text").gameObject.GetComponent<Text>().text = Selected.Text;
    //    SkillInfoWindow.transform.Find("SkillMainWindow/SkillCD/CDtext").gameObject.GetComponent<Text>().text = "" + Selected.SkillCD;

    //    if (Selected.isFree == 1)
    //        SkillInfoWindow.transform.Find("SkillMainWindow/Free").gameObject.SetActive(true);
    //    else
    //        SkillInfoWindow.transform.Find("SkillMainWindow/Free").gameObject.SetActive(false);

    //    ///

    //    for (int i = 0; i < 3; i++)
    //    {
    //        GameObject TargetSquare =
    //        SkillInfoWindow.transform.Find("SkillSubWindow/Area1/CastAreaSquare/Square" + (i + 1)).gameObject;

    //        if (Selected.CastArea[i] == 1)
    //            TargetSquare.GetComponent<Image>().color = Color.blue;
    //        else
    //            TargetSquare.GetComponent<Image>().color = Color.gray;
    //    }
    //    for (int i = 0; i < 9; i++)
    //    {
    //        GameObject TargetSquare =
    //        SkillInfoWindow.transform.Find("SkillSubWindow/Area2/TargetAreaSquare/Square" + (i + 1)).gameObject;

    //        if (Selected.TargetArea[i] == 1)
    //        {
    //            if (Selected.isHealing == 1)
    //                TargetSquare.GetComponent<Image>().color = Color.yellow;
    //            else
    //                TargetSquare.GetComponent<Image>().color = Color.yellow;
    //        }
    //        else
    //            TargetSquare.GetComponent<Image>().color = Color.gray;
    //    }
    //    for (int i = 0; i < 9; i++)
    //    {
    //        GameObject TargetSquare =
    //        SkillInfoWindow.transform.Find("SkillSubWindow/Area3/EffectAreaSquare/Square" + (i + 1)).gameObject;

    //        if (Selected.EffectArea[i] == 1)
    //        {
    //            if (Selected.isHealing == 1)
    //                TargetSquare.GetComponent<Image>().color = Color.green;
    //            else
    //                TargetSquare.GetComponent<Image>().color = Color.red;
    //        }
    //        else
    //            TargetSquare.GetComponent<Image>().color = Color.gray;
    //    }
    //    //
    //    SkillInfoWindow.transform.Find("SkillSubWindow/Area4/Window/EffectText").gameObject.GetComponent<Text>().text = Selected.ExtraText;
    //}

}
