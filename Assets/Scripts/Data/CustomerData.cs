using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "ScriptableObj/Customer")]
public class CustomerData : ScriptableObject
{
    public int CustomerID;
    public Sprite Subnail;
    public string CustomerName;
    public string CustomerSummary;
    public string FavoriteTea;
    [TextArea]
    public string CustomerDesc;
    [TextArea]
    public string CustomerHistory;
    public Sprite[] CustomerSprite;

    public Sprite GetSprite(State state)
    {
        return CustomerSprite[(int)state];
    }
}
