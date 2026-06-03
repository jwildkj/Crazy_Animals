using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "ScriptableObj/Customer")]
public class CustomerData : ScriptableObject
{
    [SerializeField] private int CustomerID;
    [SerializeField] private string CustomerName;
    [SerializeField] private Sprite[] CustomerSprite;

    //public Sprite GetSprite(State state)
    //{
    //    return CustomerSprite[(int)state];
    //}
    
    public Sprite GetSprite(int _index)
    {
        return CustomerSprite[_index];
    }
}
