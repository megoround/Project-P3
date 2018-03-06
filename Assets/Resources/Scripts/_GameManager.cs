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

        OpenMainStage("BT1");

        Init();
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
        PlayerParty = new PlayerPartyClass();
        PlayerParty.Characters = new Character[PlayerParty.PartyMemberCount];
        for(int i=0;i< PlayerParty.PartyMemberCount;i++)
        {
            PlayerParty.Characters[i] = new Character();
            PlayerParty.Characters[i].Skills = new Skill[4];
            for(int j=0;j<4;j++)
            {
                PlayerParty.Characters[i].Skills[j] = new Skill();
                PlayerParty.Characters[i].Skills[j].Name = "Alpha" + j;

                PlayerParty.Characters[i].Skills[j].Phase = 2; // 0 : PREP, 1 : DASH, 2 : BLAST
                PlayerParty.Characters[i].Skills[j].isFree = 0; // 1이면 Free Action
                PlayerParty.Characters[i].Skills[j].isHealing = 0; // 1이면 Heal

                PlayerParty.Characters[i].Skills[j].CastArea = new int[3]{ 1 , 1 , 0};
                PlayerParty.Characters[i].Skills[j].TargetArea = 
                    new int[9] { 0, 1, 1,
                                 0, 1, 0,
                                 1, 1, 0 }; //거꾸로

                PlayerParty.Characters[i].Skills[j].EffectArea =
                    new int[9] { 0, 1, 0,
                                 1, 1, 1,
                                 0, 1, 0 };

                PlayerParty.Characters[i].Skills[j].ExtraText = "Free, Heal";
            }
        }

        //

        PlayerParty.Characters[1].Name = "Iye";
        PlayerParty.Characters[1].FaceImageNumber = 1;
        PlayerParty.Characters[1].CharImageNumber = 35;
        PlayerParty.Characters[2].Name = "Wynn";
        PlayerParty.Characters[2].FaceImageNumber = 2;
        PlayerParty.Characters[2].CharImageNumber = 38;
        PlayerParty.Characters[3].Name = "Lou";
        PlayerParty.Characters[3].FaceImageNumber = 3;
        PlayerParty.Characters[3].CharImageNumber = 41;

    }

    public PlayerPartyClass PlayerParty;
}


public class PlayerPartyClass
{
    public int PartyMemberCount = 4;
    public Character[] Characters;
}
public class Character
{
    //
    public int Numb = 0;
    public string Name = "Aterial";
    public string FaceImage = "Face_Actor1";
    public int FaceImageNumber = 0;
    public string CharImage = "Char_Actor1";
    public int CharImageNumber = 25;
    public Skill[] Skills;

    //
    public int Level = 1;
    public int MaxHP = 100;
    public int CurrentHP = 80;
    public int Point = 1;
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

    public SkillTrait[] SkillTraits;
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
public class SkillTrait
{
    public int Numb = 0;
    public string Name = "Soft";
    public string Text = ":D";
    public string Image = "Sample";
    public int ImageNumber = 11;
}