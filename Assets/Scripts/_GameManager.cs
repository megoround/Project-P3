using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;


public class _GameManager : MonoBehaviour
{
    public static _GameManager instance = null;
    public GameObject Loading;

    // Use this for initialization
    void Start () {

        instance = this;
        DontDestroyOnLoad(gameObject);//다른 씬을 로드해도 파괴되지 않음.
        Init();

        OpenMainStage("BT1");
    }
	
    public void OpenMainStage(string NextScene)
    {
        StartCoroutine(LoadMainScene(NextScene));
    }

    public IEnumerator LoadMainScene(string NextScene)
    {
        Loading.SetActive(true);
        yield return SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single); //로딩 다되고 나면 수행
        //yield return SceneManager.LoadSceneAsync("Stage0", LoadSceneMode.Single);
    }

    void Init()
    {
        //

        PlayerParty = new PlayerPartyClass();
        PlayerParty.Characters = new CharacterOnParty[PlayerParty.PartyMemberCount];
        for(int i = 0; i < PlayerParty.PartyMemberCount; i ++)
        {
            PlayerParty.Characters[i] = new CharacterOnParty();
        }

        PlayerParty.Characters[0].BaseData = new XML_CharacterBaseData(); // temp.
        PlayerParty.Characters[0].CurrentPlate = 2;
        PlayerParty.Characters[0].MaxHp = 200;
        PlayerParty.Characters[0].CurrentHP = 33;

        PlayerParty.Characters[1].Name = "Iye";
        PlayerParty.Characters[1].BaseData = new XML_CharacterBaseData(); // temp.
        PlayerParty.Characters[1].BaseData.FaceImageNumber = 1;
        PlayerParty.Characters[1].BaseData.CharImageNumber = 34;
        PlayerParty.Characters[1].CurrentPlate = 4;
        PlayerParty.Characters[1].MaxHp = 180;
        PlayerParty.Characters[1].CurrentHP = 95;

        PlayerParty.Characters[2].Name = "Wynn";
        PlayerParty.Characters[2].BaseData = new XML_CharacterBaseData(); // temp.
        PlayerParty.Characters[2].BaseData.FaceImageNumber = 2;
        PlayerParty.Characters[2].BaseData.CharImageNumber = 31;
        PlayerParty.Characters[2].CurrentPlate = 8;
        PlayerParty.Characters[2].MaxHp = 150;
        PlayerParty.Characters[2].CurrentHP = 125;

        //temp
        for (int i = 0; i < PlayerParty.PartyMemberCount; i++)
        {
            //PlayerParty.Characters[i] = new Character();
            PlayerParty.Characters[i].BaseData.Skills = new Skill[4];
            for (int j = 0; j < 4; j++)
            {
                PlayerParty.Characters[i].BaseData.Skills[j] = new Skill();
                PlayerParty.Characters[i].BaseData.Skills[j].Name = "Alpha" + j;

                PlayerParty.Characters[i].BaseData.Skills[j].Phase = 2; // 0 : PREP, 1 : DASH, 2 : BLAST
                PlayerParty.Characters[i].BaseData.Skills[j].isFree = 0; // 1이면 Free Action
                PlayerParty.Characters[i].BaseData.Skills[j].isHealing = 0; // 1이면 Heal

                PlayerParty.Characters[i].BaseData.Skills[j].CastArea = new int[3] { 1, 1, 0 };
                PlayerParty.Characters[i].BaseData.Skills[j].TargetArea =
                    new int[9] { 0, 1, 1,
                                 0, 1, 0,
                                 1, 1, 0 }; //거꾸로

                PlayerParty.Characters[i].BaseData.Skills[j].EffectArea =
                    new int[9] { 0, 1, 0,
                                 1, 1, 1,
                                 0, 1, 0 };

                PlayerParty.Characters[i].BaseData.Skills[j].ExtraText = "Free, Heal";
            }
        }
    }

    public PlayerPartyClass PlayerParty;
}


public class PlayerPartyClass
{
    public int PartyMemberCount = 3;    //현재 파티원 수. 3이 최대.
    public CharacterOnParty[] Characters;
}

public class CharacterOnParty   //파티 내의 캐릭터. 객체.
{
    public XML_CharacterBaseData BaseData;

    public string Name = "Default_Name";
    public int NumbInParty = 0; //파티내의 번호. 노출되지 않음.
    public int Level = 1;
    public int MaxHp = 100;
    public int CurrentHP = 80;
    public int CurrentPlate = 0;    //배치 위치.
}

public class XML_CharacterBaseData  //캐릭터 기반 XML 데이터. 틀.
{
    public int Numb = 0;    //XML상의 순서
    public string JobName = "Default_JOB";
    public string FaceImage = "Face_Actor1";
    public int FaceImageNumber = 0;
    public string CharImage = "Char_Actor1";
    public int CharImageNumber = 25;
    public Skill[] Skills;
}

public class Skill
{
    public int Numb = 0;
    public string Name = "Swing";
    public string Text = ":X";
    public string Image = "Sample";
    public int ImageNumber = 10;

    public int SkillCD = 2;
    public int CurrentSkillCD = 0;

    //public SkillTrait[] SkillTraits;
    public int SelectedTraitNumber;

    //

    public int Phase = 2; // 0 : PREP, 1 : DASH, 2 : BLAST
    public int isFree = 0; // 1이면 Free Action
    public int isHealing = 0; // 1이면 회복기 -> 초록색

    public int[] CastArea; // 0 후열 1 중열 2 전열
    public int[] TargetArea; // 000 / 000 / 000 
    public int[] EffectArea; // 000 / 000 / 000

    public string ExtraText = "Do Nothing";
}
//public class SkillTrait
//{
//    public int Numb = 0;
//    public string Name = "Soft";
//    public string Text = ":D";
//    public string Image = "Sample";
//    public int ImageNumber = 11;
//}