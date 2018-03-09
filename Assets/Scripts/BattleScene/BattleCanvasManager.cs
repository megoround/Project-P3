using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvasManager : MonoBehaviour
{
    public static BattleCanvasManager instance = null;

    public Slider HPSlider;
    public Text HPText;

    public Image FaceImage;
    public Text NameText;

    private void Start()
    {
        instance = this;
    }

    public void RefreshSelectedChar(GameObject Target)
    {
        Sprite[] newSprite = Resources.LoadAll<Sprite>("Images/Face/" + Target.GetComponent<BattleCharacter>().Data.BaseData.FaceImage);
        FaceImage.sprite = newSprite[Target.GetComponent<BattleCharacter>().Data.BaseData.FaceImageNumber];
        NameText.text = Target.GetComponent<BattleCharacter>().Data.Name;
        HPSlider.value = Target.GetComponent<BattleCharacter>().Data.CurrentHP / (float)Target.GetComponent<BattleCharacter>().Data.MaxHp;
        HPText.text = "" + Target.GetComponent<BattleCharacter>().Data.CurrentHP + " / " + (float)Target.GetComponent<BattleCharacter>().Data.MaxHp;
    }

}
