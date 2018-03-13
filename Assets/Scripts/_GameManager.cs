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

    public PlayerPartyClass PlayerParty;
    public List<Skill> XML_Skills;

    //XML데이터도 다른데에 박아두는게 나을 것 같다

    // Use this for initialization
    void Start () {

        instance = this;
        DontDestroyOnLoad(gameObject);//다른 씬을 로드해도 파괴되지 않음.

        //XMLData init
        LoadXML();

        //
        Init();

        OpenMainStage("BT1");
    }

    public void LoadXML()
    {
        XML_LoadSkills();
    }

    private void XML_LoadSkills()
    {
        XML_Skills = new List<Skill>();

        //XML 만들기 전 임시
        for (int i = 0; i < 10; i++)
        {
            Skill newSkill = new Skill();
            newSkill.Name = "Alpha" + i;
            newSkill.Phase = i % 3;
            newSkill.isFree = i % 2;
            newSkill.isHealing = i % 2;
            newSkill.CastArea = new int[3] { 1, 1, 0 };
            newSkill.TargetArea = 
                new int[9] { 0, 1, 1,
                             0, 1, 0,
                             1, 1, 0 };
            newSkill.EffectArea =
                new int[9] { 0, 1, 0,
                             1, 1, 1,
                             0, 1, 0 };

            newSkill.ExtraText = "TEMPSkill_ExtraText";
            newSkill.Image = "Sample";
            newSkill.ImageNumber = i;
            newSkill.SkillCD = i % 4;
            newSkill.Numb = 0;
            newSkill.Text = "TempSkillText";

            XML_Skills.Add(newSkill);
        }
    }
	
    //

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
        //tempSet
        //실제로는 BaseData를 담은 XML을 읽어올 것

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

        //테스트용, 임시로 쿨다운과 스킬번호를 지정해줌

        for (int k = 0; k < 3; k ++)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerParty.Characters[k].CurrentSkillCD[i] = i;
                PlayerParty.Characters[k].BaseData.SkillNumber = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    PlayerParty.Characters[k].BaseData.SkillNumber[j] = i + j;
                }
            }
        }
    }

}


public class PlayerPartyClass
{
    public int PartyMemberCount = 3;    //현재 파티원 수. 3이 최대.
    public CharacterOnParty[] Characters;
} // 파티 안에 없는 캐릭터들도 있잖아...

public class CharacterOnParty   //파티 내의 캐릭터. 객체.
{
    public XML_CharacterBaseData BaseData;

    public string Name = "Default_Name";
    public int NumbInParty = 0; //파티내의 번호. 노출되지 않음.
    public int Level = 1;
    public int MaxHp = 100;
    public int CurrentHP = 80;
    public int CurrentPlate = 0;    //배치 위치.

    public int[] CurrentSkillCD = new int[4] { 0, 0, 0, 0 };
}

public class XML_CharacterBaseData  //캐릭터 기반 XML 데이터. 틀.
{
    public int Numb = 0;    //XML상의 순서
    public string JobName = "Default_JOB";
    public string FaceImage = "Face_Actor1";
    public int FaceImageNumber = 0;
    public string CharImage = "Char_Actor1";
    public int CharImageNumber = 25;
    public int[] SkillNumber;

    //BaseStatus
    //public int BaseLevel;
    //public int BaseMaxHP;
}

public class Skill // 리스트로 XML 스킬들 불러와 저장하고 캐릭터에는 스킬 번호만 있어야함.
{
    public int Numb = 0;
    public string Name = "Swing";
    public string Text = ":X";
    public string Image = "Sample";
    public int ImageNumber = 10;

    public int SkillCD = 2;
    //public int CurrentSkillCD = 0;

    //public SkillTrait[] SkillTraits;
    //public int SelectedTraitNumber; 이건 캐릭터에 있어야지

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