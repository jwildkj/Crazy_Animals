using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Product", menuName = "ScriptableObj/Product")]
public class ProductData : ScriptableObject
{
    public CATEGORY Category;
    public Sprite Subnail;
    public string ProductName;
    [TextArea]
    public string ProductDesc;
    public int Price;
}
