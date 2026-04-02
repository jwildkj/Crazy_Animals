using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "DrinkSystem/Recipe")]
public class RecipeData : ScriptableObject
{
    public string drinkName;
    public List<BASE> requiredBases;    // 베이스 ID 리스트 (최대 3개)
    public List<TOPPING> requiredToppings; // 토핑 ID 리스트 (최대 2개)
    public GameObject resultPrefab;    // 결과물 프리팹
}

