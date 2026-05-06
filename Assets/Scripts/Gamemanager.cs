using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Func;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [Header("Property")]
    [SerializeField] private float PointerSpeed;
    [Space(100)]
    [Header("Internal")]
    [SerializeField] private TextMeshProUGUI Noti;
    [SerializeField] private GameObject Bar;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Teas;
    [SerializeField] private List<BASE> CurBases;
    [SerializeField] private List<TOPPING> CurToppings;
    [SerializeField] public RecipeData[] allRecipes;
    [SerializeField] private Dictionary<string, RecipeData> recipeLookUp = new Dictionary<string, RecipeData>();


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        allRecipes = Resources.LoadAll<RecipeData>("Recipes");

        foreach (var recipe in allRecipes)
        {
            recipeLookUp.Add(GenerateKey(recipe.requiredBases, recipe.requiredToppings), recipe);
        }
    }
    public void AddBase(int _base)
    {
        if (CurBases.Count > 2){
            Notificate("Can't add base over 3");
            return;
        }
        CurBases.Add((BASE)_base);
        Animationmanager.instance.PlayAnim(0);
    }

    public void AddTopping(int _topping)
    {
        if (CurToppings.Count > 1){
            Notificate("Can't add topping over 2");
            return;
        } 

        CurToppings.Add((TOPPING)_topping);
        Animationmanager.instance.PlayAnim(0);
    }
    private void Notificate(string _txt)
    {
        Noti.color = Color.white;
        Noti.text = _txt;
        Animationmanager.instance.PlayAnim(2, "", true);
    }

    public void TeagameStart()
    {
        if (CurBases.Count < 1){
            Notificate("At least 1 base is required"); 
            return;
        }
        EnDisableChildComponent<Button>(Teas.transform, false);
        Pointer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 45.9934f);
        Animationmanager.instance.PlayAnim(1);
        StopAllCoroutines();
        StartCoroutine(Brew());
    }



    IEnumerator Brew()
    {
        yield return new WaitForSeconds(2f);

        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        StartCoroutine(Fade(FADE.IN, 0.2f, barimg, pointerimg, targetimg));

        RectTransform pointerrect = Pointer.GetComponent<RectTransform>();

        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                pointerrect.sizeDelta += new Vector2(PointerSpeed, 0) * Time.deltaTime;
                if (pointerrect.sizeDelta.x > 600)
                    pointerrect.sizeDelta = new Vector2(600, 45.9934f);

                yield return null;
            }
            if (Input.GetMouseButtonUp(0))
            {
                AfterTea();
                yield break; 
            } 
            yield return null;
        }
    }

    string GenerateKey(List<BASE> bases, List<TOPPING> toppings)
    {
        bases.Sort();
        toppings.Sort();
        return $"B:{string.Join(",", bases)}|T:{string.Join(",", toppings)}";
    }
    private bool Judge()
    {
        if( !recipeLookUp.ContainsKey( GenerateKey(CurBases, CurToppings)))
            return false;

        float PointerPos = Pointer.GetComponent<RectTransform>().sizeDelta.x;
        float TargetPos = Target.GetComponent<RectTransform>().anchoredPosition.x;
        float TargetLength = Target.GetComponent<RectTransform>().sizeDelta.x;

        Vector2 TargetRange = new Vector2(TargetPos - TargetLength/2, TargetPos + TargetLength/2);

        return PointerPos >= TargetRange.x && PointerPos <= TargetRange.y;
    }

    private void AfterTea()
    {
        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        
        StartCoroutine(Fade(FADE.OUT, 0.2f, barimg, pointerimg, targetimg));

        EnDisableChildComponent<Button>(Teas.transform, true);
        if (Judge()) print("Success");
        else print("Failed");

        CurBases.Clear();
        CurToppings.Clear();
    }


}
