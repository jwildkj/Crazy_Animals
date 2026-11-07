using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "ScriptableObj/Customer")]
public class CustomerData : ScriptableObject
{
    public int CustomerID;
    public Sprite Subnail;
    public string CustomerName;
    [TextArea]
    public string CustomerSummary;
    public string FavoriteTea;
    [TextArea]
    public string CustomerDesc;
    [TextArea]
    public string CustomerHistory;

    public Sprite[] CustomerSprite;

    [Header("Portrait Setting")]
    public PortraitSetting portraitSetting;

    //public Sprite GetSprite(State state)
    //{
    //    return CustomerSprite[(int)state];
    //}
    
    public Sprite GetSprite(int _index)
    {
        if (_index < 0 || _index >= CustomerSprite.Length)
        {
            return null;
        }

        return CustomerSprite[_index];
    }
}

[System.Serializable]
public class PortraitSetting
{
    [Header("Position")]
    public float positionY;

    [Header("Size")]
    public Vector2 size;
}