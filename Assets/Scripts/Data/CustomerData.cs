using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "ScriptableObj/Customer")]
public class CustomerData : ScriptableObject
{
    public int CustomerID;
    public string CustomerName;
    public Sprite[] CustomerSprite;

    public Sprite GetSprite(State state)
    {
        return CustomerSprite[(int)state];
    }
}
