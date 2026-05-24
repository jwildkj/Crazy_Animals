using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Category
{
    [SerializeField] public List<ProductData> products = new List<ProductData>();
}

public class Resourcemanager : MonoBehaviour
{
    public static Resourcemanager instance;
    [Header("Customers")]
    [Space(20)]
    [SerializeField] public CustomerData[] allCustomers;

    [Header("Recipes")]
    [Space(20)]
    [SerializeField] public RecipeData[] allRecipes;
    [SerializeField] public Dictionary<string, RecipeData> recipeLookUp = new Dictionary<string, RecipeData>();
    [SerializeField] public RecipeData[] FailedDrinks = new RecipeData[2];

    [Header("Products")]
    [Space(20)]
    [SerializeField] public ProductData[] AllProducts; //모든 상품
    [SerializeField] public Category[] CategoryOrderedProducts = new Category[(int)CATEGORY.END]; //카테고리별 정렬된 상품
    public Dictionary<string, bool> OwnedProduct = new Dictionary<string, bool>(); //현재 가지고 있는 제품
    public Dictionary<CATEGORY, ProductData> ApplyedProduct = new Dictionary<CATEGORY, ProductData>(); //현재 적용한 제품
    [SerializeField] public ProductData[] Starters = new ProductData[(int)CATEGORY.END]; //기본템
    [SerializeField] public Dictionary<string, int> PriceDic = new Dictionary<string, int>(); //상품 가격

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        allCustomers = Resources.LoadAll<CustomerData>("Customers");

        allRecipes = Resources.LoadAll<RecipeData>("Recipes");

        foreach (var recipe in allRecipes)
        {
            recipeLookUp.Add(GenerateKey(recipe.requiredBases, recipe.requiredToppings), recipe);
        }

        AllProducts = Resources.LoadAll<ProductData>("Products");

        foreach (var product in AllProducts)
        {
            OwnedProduct.Add(product.name, false);
        }

        foreach (var product in AllProducts)
        {
            CategoryOrderedProducts[(int)product.Category].products.Add(product);
            PriceDic.Add(product.name, product.Price);
        }
    }

    string GenerateKey(List<BASE> bases, List<TOPPING> toppings)
    {
        bases.Sort();
        toppings.Sort();
        return $"B:{string.Join(",", bases)}|T:{string.Join(",", toppings)}";
    }


}
