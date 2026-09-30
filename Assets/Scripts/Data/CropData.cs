using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Crop", menuName = "ScriptableObj/Crop")]
public class CropData : ScriptableObject
{
    public string Name;
    public int MaxGrowthIdx;
    public Sprite[] GrowthSprites;
}
