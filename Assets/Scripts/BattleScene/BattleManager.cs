﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour {

    public static BattleManager instance = null;
    public GameObject BattleCharacterPrefab; //, SkillSlotPrefab;

    public GameObject BattleCanvas;
    GameObject SkillStackPrefab;

    public GameObject PlayerPlatesHolder;
    public GameObject[] PlayerPlates = new GameObject[9];
    
    public GameObject EnemyPlatesHolder;
    public GameObject[] EnemyPlates = new GameObject[9];

    /// ////////////////////////////////////////
    //한번만 생성하고 전투 시작시 캔버스 통째로 끌어내리고, 전투 끝나면 끌어올리자.

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        ///////////Prefab Set////////
        //SquarePrefab = Resources.Load("Prefabs/" + "BattleSquare") as GameObject;
        //BattleCharacterPrefab = Resources.Load("Prefabs/" + "BattleCharacter") as GameObject;
        //SkillStackPrefab = Resources.Load("Prefabs/" + "SkillStack") as GameObject;

        ///////////Plate Set////////

        //생성해서 변수만 가지고 나머지는 캔버스매니저에 맡겨도 될지도
        for(int i = 0; i < _GameManager.instance.PlayerParty.PartyMemberCount; i ++)
        {
            GameObject Inst = Instantiate(BattleCharacterPrefab, PlayerPlates[_GameManager.instance.PlayerParty.Characters[i].CurrentPlate].transform);
            Inst.GetComponent<BattleCharacter>().OnPlate = PlayerPlates[_GameManager.instance.PlayerParty.Characters[i].CurrentPlate];
            Inst.GetComponent<BattleCharacter>().Data = _GameManager.instance.PlayerParty.Characters[i];

            Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Char/" + Inst.GetComponent<BattleCharacter>().Data.BaseData.CharImage);
            Inst.transform.GetChild(0).GetComponent<Image>().sprite = newSprite[Inst.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber];
            Inst.transform.GetChild(0).GetComponent<Image>().enabled = true;
            Inst.transform.GetChild(1).GetComponent<Slider>().value = Inst.GetComponent<BattleCharacter>().Data.CurrentHP / (float)Inst.GetComponent<BattleCharacter>().Data.MaxHp;
            Inst.GetComponent<BattleCharacter>().isPlayers = true;

            Debug.Log("" + Inst.GetComponent<BattleCharacter>().Data.BaseData.CharImage + Inst.GetComponent<BattleCharacter>().Data.BaseData.CharImageNumber);
            //적은 어떻게하지...

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) =>
            { BattleCanvasManager.instance.CharTouched(Inst); });
            Inst.GetComponent<EventTrigger>().triggers.Add(entry);

        }
    }


    //       //SkillSlot Set//
    //       for (int i=0;i<5;i++)
    //       {
    //           GameObject Inst = Instantiate(SkillSlotPrefab);
    //           Inst.transform.parent = BattleCanvas.transform.Find("Bottom/Status/Skills");
    //           Inst.transform.localScale = SquarePrefab.transform.localScale;
    //           Inst.transform.localPosition = new Vector3(-490 + 100 * i, 0, 0);

    //           if(i == 4)
    //           {
    //               Inst.name = "SkillSlot_" + "Wait";
    //               Image Img = Inst.transform.Find("Image").GetComponent<Image>();

    //               Sprite newSprite = Resources.Load<Sprite>("Images/UI/wait");
    //               Img.sprite = newSprite;

    //               GameObject SkillSlot = Inst;
    //               GameObject SkillCDtext = SkillSlot.transform.Find("CDtext").gameObject;
    //               GameObject SkillImg = SkillSlot.transform.Find("CDimage").gameObject;

    //               SkillCDtext.GetComponent<Text>().text = "";
    //               SkillImg.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

    //           }
    //           else
    //           {
    //               Inst.name = "SkillSlot_" + (i + 1);

    //               EventTrigger.Entry entry = new EventTrigger.Entry();
    //               entry.eventID = EventTriggerType.PointerEnter;
    //               entry.callback.AddListener((eventData) =>
    //               {
    //                   SkillTextWindow.SetActive(true);
    //                   SkillTextWindow.transform.localPosition = Inst.transform.localPosition;
    //                   SkillTextWindow.transform.localPosition += new Vector3(200, -50, 0);
    //                   SkillWindowPopup(Inst);
    //               });
    //               Inst.transform.Find("OnClick").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

    //               entry = new EventTrigger.Entry();
    //               entry.eventID = EventTriggerType.PointerExit;
    //               entry.callback.AddListener((eventData) =>
    //               {
    //                   SkillTextWindow.SetActive(false);
    //               });
    //               Inst.transform.Find("OnClick").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);

    //               entry = new EventTrigger.Entry();
    //               entry.eventID = EventTriggerType.PointerClick;
    //               entry.callback.AddListener((eventData) =>
    //               {
    //                   SkillSlotOnClick(Inst.name);
    //               });
    //               Inst.transform.Find("OnClick").gameObject.GetComponent<EventTrigger>().triggers.Add(entry);
    //           }
    //       }


    //       ///////////Test Set////////


    //       Init();
    //   }

    //   // Update is called once per frame
    //   void Update () {

    //}

    //   void Init()
    //   {
    //       for(int i=0;i< _GameManager.instance.PlayerParty.PartyMemberCount;i++)
    //       {
    //           Character Char = _GameManager.instance.PlayerParty.Characters[i];
    //           Char.Point = i; // temp
    //           PlayerCharaterInit(Char, Char.Point, i);
    //       }

    //       StatusRefresh();
    //   }

    //   void PlayerCharaterInit(Character characterData, int plate, int number)  //임시
    //   {
    //       GameObject Inst = Instantiate(BattleCharacterPrefab);
    //       Image Img = Inst.GetComponent<Image>().GetComponent<Image>();
    //       Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Char/" + characterData.CharImage);
    //       Img.sprite = newSprite[characterData.CharImageNumber];

    //       //
    //       BattleCharacter character = new BattleCharacter(characterData);

    //       character.BattleCharacterGO = Inst;
    //       character.OnPlate = Plates[0][plate];
    //       character.isPlayers = 0;

    //       PlayerCharacters[number] = character;

    //       CharacterOnPlate(character.OnPlate, character);
    //       HPRefreshOnSquare(character);
    //       //Debug.Log(number + " Max : "+PlayerCharacters[number].MaxHP);
    //       //Debug.Log("Current : " + PlayerCharacters[number].CurrentHP);
    //   }

    //   void HPRefreshOnSquare(BattleCharacter character)
    //   {
    //       GameObject redbar;
    //       float HPremainPer;

    //       HPremainPer = 100 - ((float)character.CurrentHP / (float)character.MaxHP)*100;

    //       redbar = character.BattleCharacterGO.transform.Find("HPbar/HPred").gameObject;
    //       redbar.transform.localPosition = new Vector3(-HPremainPer, -9,0);
    //   }

    //   void StatusRefresh()
    //   {
    //       GameObject FaceImage, NameText, Skills, redbar, hpText;
    //       BattleCharacter character = PlayerCharacters[CurrentTurnChar];
    //       float HPremainPer;

    //       /////////////////////////////////////////

    //       //HP 갱신
    //       HPremainPer = 225 - ((float)character.CurrentHP / (float)character.MaxHP) * 225;
    //       redbar = GameObject.Find("Bots/HP/HPbar/HPred");
    //       redbar.transform.localPosition = new Vector3(-HPremainPer, 0, 0);
    //       hpText = GameObject.Find("Bots/HP/HPbar/Text");
    //       hpText.GetComponent<Text>().text = (int)character.CurrentHP + " / " + (int)character.MaxHP;

    //       //이미지, 이름 갱신
    //       FaceImage = GameObject.Find("Status/Face/Image");
    //       NameText = GameObject.Find("Status/Name/Text");

    //       Image Img = FaceImage.GetComponent<Image>();
    //       Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Face/" + character.FaceImage);
    //       Img.sprite = newSprite[character.FaceImageNumber];

    //       NameText.GetComponent<Text>().text = character.Name;

    //       //스킬창 갱신
    //       Skills = GameObject.Find("Status/Skills");

    //       GameObject SkillSlot,SkillImg,SkillCDtext,SkillCDimage;
    //       for (int i=0;i<4;i++)
    //       {
    //           SkillSlot = Skills.transform.Find("SkillSlot_" + (i+1)).gameObject;
    //           SkillImg = SkillSlot.transform.Find("Image").gameObject;

    //           Img = SkillImg.GetComponent<Image>();
    //           Debug.Log(character.Skills[i].Image);
    //           newSprite = Resources.LoadAll<Sprite>("Images/UI/Icons/" + character.Skills[i].Image);
    //           Img.sprite = newSprite[character.Skills[i].ImageNumber];

    //           SkillCDtext = SkillSlot.transform.Find("CDtext").gameObject;
    //           SkillCDimage = SkillSlot.transform.Find("CDimage").gameObject;

    //           if(character.Skills[i].CurrentSkillCD == 0)
    //           {
    //               SkillCDtext.GetComponent<Text>().text = "";
    //               SkillCDimage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
    //           }
    //           else
    //           {
    //               SkillCDtext.GetComponent<Text>().text = "" + character.Skills[i].CurrentSkillCD;
    //               SkillCDimage.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f, 0.5f);
    //           }
    //       }

    //   }

    //   void Effect(int plateNumber,int amount,int TargetAreaNumber) // temp
    //   {
    //       BattleCharacter Char = Plates[TargetAreaNumber][plateNumber-1].OnPlaterChar;

    //       if (Char == null)
    //           return;
    //       else
    //       {
    //           Char.CurrentHP += amount;
    //           if (Char.CurrentHP > Char.MaxHP)
    //               Char.CurrentHP = Char.MaxHP;
    //           else if (Char.CurrentHP < 0)
    //               Char.CurrentHP = 0;

    //           HPRefreshOnSquare(Char);
    //           if (Char.Equals(PlayerCharacters[CurrentTurnChar]))
    //           {
    //               StatusRefresh();
    //           }
    //       }

    //   }
    //   void CharacterOnPlate(Plate plate, BattleCharacter Char)
    //   {
    //       plate.OnPlaterChar = Char;

    //       Char.BattleCharacterGO.transform.parent = Char.OnPlate.PlateGO.transform;
    //       Char.BattleCharacterGO.transform.localPosition = new Vector2(0, -12);
    //       Char.BattleCharacterGO.transform.localScale = BattleCharacterPrefab.transform.localScale;
    //   }

    //   void SkillSlotOnClick(string Name)
    //   {
    //       string[] spt = Name.Split('_');
    //       int numb = int.Parse(spt[1]);

    //       BattleStack BattleStackInfo = new BattleStack();
    //       BattleStackInfo.User = PlayerCharacters[CurrentTurnChar];
    //       BattleStackInfo.SkillOnStack = BattleStackInfo.User.Skills[numb];
    //       BattleStackInfo.CastPlate = 1;
    //       BattleStackInfo.CastPoint = 4;

    //       GameObject Inst = Instantiate(SkillStackPrefab);
    //       //Inst.transform.parent = GameObject.Find()
    //   }

    //   class BattleStackManager
    //   {
    //       LinkedList<BattleStack> Stack_PREP;
    //       LinkedList<BattleStack> Stack_DASH;
    //       LinkedList<BattleStack> Stack_BLAST;

    //       LinkedList<Skill> Stack_PlayerAct;
    //   }


    //   class BattleStack
    //   {
    //       public Character User;
    //       public Skill SkillOnStack;
    //       public int CastPlate;      //0 -> 플레이어 , 1 -> 상대
    //       public int CastPoint;  //사용한 위치
    //   }
}
