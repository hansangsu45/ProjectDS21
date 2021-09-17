using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CardRuleData", menuName = "Scriptable Object/CardRuleData",order =int.MaxValue)]
public class CardRuleData : ScriptableObject
{
    public Sprite backSprite;
    public float[] mixX;
    public float mixY;
    public Vector3 cardScale, trashCardScale;
}
