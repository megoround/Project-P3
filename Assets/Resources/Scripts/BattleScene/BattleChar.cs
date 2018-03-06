using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : Character    //임시
{
    public GameObject BattleCharacterGO;
    public Plate OnPlate;

    public int isPlayers;

    public BattleCharacter() { }
    public BattleCharacter(Character Par)
    {
        this.Numb = Par.Numb;
        this.Name = Par.Name;

        this.FaceImage = Par.FaceImage;
        this.FaceImageNumber = Par.FaceImageNumber;
        this.CharImage = Par.CharImage;
        this.CharImageNumber = Par.CharImageNumber;
        this.Skills = Par.Skills;

        this.Level = Par.Level;
        this.MaxHP = Par.MaxHP;
        this.CurrentHP = Par.CurrentHP;
        this.Point = Par.Point;
    }
}
