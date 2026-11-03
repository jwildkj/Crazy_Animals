using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    private static bool HasInstance = false;
    [SerializeField] public bool DicEnabled;
    [SerializeField] private Pages CurPage;
    [SerializeField] private Pages PrevPage;
    [SerializeField] private GameObject[] PagesObj;
    [SerializeField] private int PageIdx;
    [Header("Customer")]
    [SerializeField] private Transform[] Customerpages = new Transform[4];
    [SerializeField] private int CustomerMaxpage;
    [SerializeField] private Transform Customer;
    [SerializeField] private GameObject CustomerPrefab;
    [SerializeField] private GameObject CustomerInfo;
    [Header("Recipe")]
    [SerializeField] private Transform[] Recipepages = new Transform[4];
    [SerializeField] private int RecipeMaxpage;
    [SerializeField] private Transform Recipe;
    [SerializeField] private GameObject RecipePrefab;

    [SerializeField] private GameObject CustomerArrowLeft;
    [SerializeField] private GameObject CustomerArrowRight;
    [SerializeField] private GameObject RecipeArrowLeft;
    [SerializeField] private GameObject RecipeArrowRight;
    [SerializeField] private Animation[] BookMarkAnim = new Animation[2];

    private void Awake()
    {
        if (HasInstance)
        {
            Destroy(gameObject);
        }
        else
        {
            HasInstance = true;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void TurntoPage(int page)
    {
        for (int i = 0; i < PagesObj.Length; i++)
            PagesObj[i].SetActive(false);
        PrevPage = CurPage;
        CurPage = (Pages)page;

        PagesObj[(int)CurPage].SetActive(true);
        switch (CurPage)
        {
            case Pages.TITLE:
                BookMarkAnim[0].Play( "BookMarkInit1");
                BookMarkAnim[1].Play("BookMarkInit2");
                break;
            case Pages.CUSTOMER:
                PageIdx = 0;
                CustomerReload();
                UpdatePage();
                break;
            case Pages.CUSTOMERINFO:
                break;
            case Pages.RECIPES:
                PageIdx = 0;
                RecipesReload();
                UpdatePage();
                break;
        }
    }

    private void CustomerReload()
    {
        if (PrevPage == Pages.RECIPES) BookMarkAnim[1].Play("BookMarkDeHighlighted2");
        if (PrevPage == Pages.TITLE) BookMarkAnim[0].Play("BookMarkHighlighted1");

        if (Customer.GetChild(0).GetChild(0).childCount > 0) return;

        for (int i = 0; i < Customerpages.Length; i++) { Customerpages[i] = Customer.GetChild(i); }
        CustomerData[] customers = Resourcemanager.instance.allCustomers;
        CustomerMaxpage = (customers.Length / 12) + 1;


        for (int i = 0; i < CustomerMaxpage; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if ((j + 12 * i) >= customers.Length) break;
                GameObject customercell = null;
                if (j < 6){
                    customercell = Instantiate(CustomerPrefab, Customerpages[i].GetChild(0));
                }
                else{
                    customercell = Instantiate(CustomerPrefab, Customerpages[i].GetChild(1));
                }

                TextMeshProUGUI name = customercell.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI desc = customercell.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                Image subnail = customercell.transform.GetChild(2).GetComponent<Image>();

                name.text = customers[j + 12 * i].CustomerName;
                desc.text = customers[j + 12 * i].CustomerSummary;
                subnail.sprite = customers[j + 12 * i].Subnail;
                customercell.name = (j + 12 * i).ToString();

                Button btn = customercell.GetComponent<Button>();
                btn.onClick.AddListener(() => {
                    GetCustomerInfo(Resourcemanager.instance.allCustomers[int.Parse(customercell.name) ]);
                });
                CustomerArrowLeft = Customer.GetChild(Customer.childCount - 2).gameObject;
                CustomerArrowRight = Customer.GetChild(Customer.childCount - 1).gameObject;
            }
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


    private void RecipesReload()
    {
        if (PrevPage == Pages.CUSTOMER) BookMarkAnim[0].Play("BookMarkDeHighlighted1");
        if (PrevPage == Pages.TITLE) BookMarkAnim[1].Play("BookMarkHighlighted2");

        if (Recipe.GetChild(0).GetChild(0).childCount > 0) return;

        for (int i = 0; i < Recipepages.Length; i++){ Recipepages[i] = Recipe.GetChild(i); }


        RecipeData[] recipes = Resourcemanager.instance.allRecipes;
        RecipeMaxpage = (recipes.Length / 6) + 1;

        for (int i = 0; i < RecipeMaxpage; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if ((j + 6 * i) >= recipes.Length) break;
                GameObject recipecell = null;

                if (j < 3)
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

                name.text = recipes[j + 6 * i].drinkName;
                desc.text = Recipetostring(recipes[j + 6 * i].requiredBases, recipes[j + 6 * i].requiredToppings);
                subnail.sprite = recipes[j + 6 * i].resultSprite;
                recipecell.name = (j + 6 * i).ToString();
            }
        }
        RecipeArrowLeft = Recipe.GetChild(Recipe.childCount - 2).gameObject;
        RecipeArrowRight = Recipe.GetChild(Recipe.childCount -1).gameObject;


    }

    string Recipetostring(List<BASE> bases, List<TOPPING> toppings)
    {
        if (bases.Contains(BASE.END) || toppings.Contains(TOPPING.END)) return "";
        string baseText = bases.Count > 0 ? string.Join(", ", bases) : "없음";
        string toppingText = toppings.Count > 0 ? string.Join(", ", toppings) : "없음";

        return $"베이스 : {baseText}\n토핑 : {toppingText}";
    }
    public void Next() { PageIdx++; UpdatePage(); }
    public void Prev() { PageIdx--; UpdatePage(); }

    void UpdatePage()
    {
        if(CurPage == Pages.CUSTOMER)
        {
            for (int i = 0; i < Customerpages.Length; i++)
                Customerpages[i].gameObject.SetActive(false);

            Customerpages[PageIdx].gameObject.SetActive(true);

            if (0 == PageIdx)   CustomerArrowLeft.gameObject.SetActive(false);
            else CustomerArrowLeft.gameObject.SetActive(true);

            if (CustomerMaxpage - 1 <= PageIdx) CustomerArrowRight.gameObject.SetActive(false);
            else CustomerArrowRight.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < Recipepages.Length; i++)
                Recipepages[i].gameObject.SetActive(false);

            Recipepages[PageIdx].gameObject.SetActive(true);  

            if(0 == PageIdx) RecipeArrowLeft.gameObject.SetActive(false);
            else RecipeArrowLeft.gameObject.SetActive(true);

            if (RecipeMaxpage - 1 <= PageIdx) RecipeArrowRight.gameObject.SetActive(false);
            else RecipeArrowRight.gameObject.SetActive(true);
        }
    }

    public void SetEnabled(bool _enabled)
    {
        DicEnabled = _enabled;
    }

    //BookMarkAnim
    public void HighlightBookMark(int _idx)
    {
        if (_idx == 0)
        {
            if(CurPage != Pages.CUSTOMER) BookMarkAnim[0].Play("BookMarkHighlighted1");

        }
        else
        {
            if (CurPage != Pages.RECIPES)BookMarkAnim[1].Play("BookMarkHighlighted2");

        }
    }

    public void DeHighlightBookMark(int _idx)
    {
        if (_idx == 0)
        {
            if (CurPage != Pages.CUSTOMER) BookMarkAnim[0].Play("BookMarkDeHighlighted1");

        }
        else
        {
            if (CurPage != Pages.RECIPES) BookMarkAnim[1].Play("BookMarkDeHighlighted2");

        }
    }

}
