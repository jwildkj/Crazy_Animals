using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public enum Pages
{
    TITLE,
    CUSTOMER,
    CUSTOMERINFO,
    RECIPES,

    END
}
public class Dictionarymanager : MonoBehaviour
{
    private static Dictionarymanager instance;
    [SerializeField] private Pages CurPage;
    [SerializeField] private int RecipePageIdx;
    [SerializeField] private GameObject[] PagesObj;
    [Header("Customer")]
    [SerializeField] private Transform CustomerLeftPar;
    [SerializeField] private Transform CustomerRightPar;
    [SerializeField] private GameObject CustomerPrefab;
    [SerializeField] private GameObject CustomerInfo;
    [Header("Recipe")]
    [SerializeField] private Transform Recipe;
    [SerializeField] private GameObject RecipePrefab;
    [SerializeField] private GameObject PaddingPrefab;

    [SerializeField] private GameObject ArrowLeft;
    [SerializeField] private GameObject ArrowRight;

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
    }

    public void TurntoPage(int page)
    {
        CurPage = (Pages)page;
        for (int i = 0; i < PagesObj.Length; i++)
            PagesObj[i].SetActive(false);

        PagesObj[(int)CurPage].SetActive(true);
        switch (CurPage)
        {
            case Pages.CUSTOMER:
                CustomerReload();
                break;
            case Pages.CUSTOMERINFO:
                break;
            case Pages.RECIPES:
                RecipePageIdx = 0;
                RecipesReload();
                UpdateRecipePage();
                break;
        }
    }

    private void CustomerReload()
    {
        if (CustomerLeftPar.childCount > 0) return;

        CustomerData[] customers = Resourcemanager.instance.allCustomers;

        for (int i = 0; i < customers.Length; i++)
        {
            GameObject customercell = null;
            if (i < 10){
                customercell = Instantiate(CustomerPrefab, CustomerLeftPar);
            }
            else{
                customercell = Instantiate(CustomerPrefab, CustomerRightPar);
            }

            TextMeshProUGUI name = customercell.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI desc = customercell.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Image subnail = customercell.transform.GetChild(2).GetComponent<Image>();

            name.text = customers[i].CustomerName;
            desc.text = customers[i].CustomerSummary;
            subnail.sprite = customers[i].Subnail;
            customercell.name = i.ToString();

            Button btn = customercell.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                GetCustomerInfo(Resourcemanager.instance.allCustomers[int.Parse(customercell.name) ]);
            });
        }
    }

    private void GetCustomerInfo(CustomerData data)
    {
        TurntoPage(2);
        Image subnail = CustomerInfo.transform.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI name =CustomerInfo.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI summary =CustomerInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI desc =CustomerInfo.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI history =CustomerInfo.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI favoritetea = CustomerInfo.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();

        subnail.sprite = data.Subnail;
        name.text = data.CustomerName;
        summary.text = data.CustomerSummary;
        desc.text = data.CustomerDesc;
        history.text = data.CustomerHistory;
        favoritetea.text = data.FavoriteTea;

    }

    Transform[] Recipepages = new Transform[5];
    int Maxpage;
    private void RecipesReload()
    {
        for (int i = 0; i < 5; i++){ Recipepages[i] = Recipe.GetChild(i); }

        if (Recipe.GetChild(0).GetChild(0).childCount > 0) return;

        RecipeData[] recipes = Resourcemanager.instance.allRecipes;
        int pagesidx = Maxpage = recipes.Length / 6;
        int leftover = recipes.Length % 6;

        for (int i = 0; i < pagesidx; i++)
        {
            GameObject recipecell = null;
            for (int j = 6 * i; j < 6 * (i+1);  j++)
            {
                if (j < (6*i) + 3)
                {
                    recipecell = Instantiate(RecipePrefab, Recipepages[i].GetChild(0));
                }
                else
                {
                    recipecell = Instantiate(RecipePrefab, Recipepages[i].GetChild(1));
                }

                TextMeshProUGUI name = recipecell.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI desc = recipecell.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                Image subnail = recipecell.transform.GetChild(2).GetComponent<Image>();

                name.text = recipes[j].drinkName;
                desc.text = Recipetostring(recipes[j].requiredBases, recipes[j].requiredToppings);
                subnail.sprite = recipes[j].resultSprite;
                recipecell.name = j.ToString();
            }
        }
        int leftovercount = 0;
        for (int i = pagesidx * 6; i < pagesidx * 6 +leftover; i++)
        {
            GameObject recipecell = null;
            if (i < 3 + pagesidx * 6)
            {
                recipecell = Instantiate(RecipePrefab, Recipepages[pagesidx].GetChild(0));
            }
            else
            {
                recipecell = Instantiate(RecipePrefab, Recipepages[pagesidx].GetChild(1));
            }

            TextMeshProUGUI name = recipecell.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI desc = recipecell.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Image subnail = recipecell.transform.GetChild(2).GetComponent<Image>();

            name.text = recipes[i].drinkName;
            desc.text = Recipetostring(recipes[i].requiredBases, recipes[i].requiredToppings);
            subnail.sprite = recipes[i].resultSprite;
            recipecell.name = i.ToString();
            ++leftovercount;
        }

        if ( 2 == leftovercount) {
            Instantiate(PaddingPrefab, Recipepages[pagesidx].GetChild(0));
        }else if(5 == leftovercount) {
            Instantiate(PaddingPrefab, Recipepages[pagesidx].GetChild(1));
        }

        ArrowLeft = Recipe.GetChild(Recipe.childCount - 2).gameObject;
        ArrowRight = Recipe.GetChild(Recipe.childCount -1).gameObject;
    }

    string Recipetostring(List<BASE> bases, List<TOPPING> toppings)
    {
        if (bases.Contains(BASE.END) || toppings.Contains(TOPPING.END)) return "";
        string baseText = bases.Count > 0 ? string.Join(", ", bases) : "없음";
        string toppingText = toppings.Count > 0 ? string.Join(", ", toppings) : "없음";

        return $"베이스 : {baseText}\n토핑 : {toppingText}";
    }
    public void Next() { RecipePageIdx++; UpdateRecipePage(); }
    public void Prev() { RecipePageIdx--; UpdateRecipePage(); }

    void UpdateRecipePage()
    {
        for (int i = 0; i < Recipepages.Length; i++)
            Recipepages[i].gameObject.SetActive(false);

        Recipepages[RecipePageIdx].gameObject.SetActive(true);  

        if(0 == RecipePageIdx) ArrowLeft.gameObject.SetActive(false);
        else ArrowLeft.gameObject.SetActive(true);

        if (Maxpage <= RecipePageIdx) ArrowRight.gameObject.SetActive(false);
        else ArrowRight.gameObject.SetActive(true);
    }



}
