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
    public CustomerData[] allCustomers;

    [Header("Recipes")]
    [Space(20)]
    public RecipeData[] allRecipes;
    public Dictionary<string, RecipeData> recipeLookUp = new Dictionary<string, RecipeData>();
    public RecipeData[] FailedDrinks = new RecipeData[2];

    [Header("Products")]
    [Space(20)]
    public ProductData[] ApplyedProduct = new ProductData[(int)CATEGORY.END]; //현재 적용된 템
    public ProductData[] Starters = new ProductData[(int)CATEGORY.END]; //기본템
    public Dictionary<string, bool> NametoOwned = new Dictionary<string, bool>(); //이름 -> 보유 여부
    
    //여기부턴 그냥 정렬 다른 데이터들
    public ProductData[] AllProducts; //모든 상품
    public Dictionary<string, ProductData> NametoProducts = new Dictionary<string, ProductData>(); //이름 -> 상품
    public Dictionary<string, int> NametoPrice = new Dictionary<string, int>(); //이름 -> 가격
    public Category[] CategoryOrderedProducts = new Category[(int)CATEGORY.END]; //카테고리별 정렬된 상품


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
            NametoProducts.Add(product.name, product);
            NametoPrice.Add(product.name, product.Price);
            NametoOwned.Add(product.name, false);

            CategoryOrderedProducts[(int)product.Category].products.Add(product);
        }

        for (int i = 0; i < Starters.Length; i++)
        {
            if (null != Starters[i]) NametoOwned[Starters[i].name] = true;
        }
        Starters.CopyTo(ApplyedProduct, 0);


    }

    string GenerateKey(List<BASE> bases, List<TOPPING> toppings)
    {
        bases.Sort();
        toppings.Sort();
        return $"B:{string.Join(",", bases)}|T:{string.Join(",", toppings)}";
    }


}
