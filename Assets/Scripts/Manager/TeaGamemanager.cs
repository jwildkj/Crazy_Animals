using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Func;
public enum Difficulty
{
    EASY,
    HARD
}
public class TeaGamemanager : MonoBehaviour
{
    const int TOPPINGPRICE = 10;
    [Header("Property")]
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private float PointerSpeed;

    private bool isbrewing;
    private bool brewfin;
    public void BrewbuttonDown() { isbrewing = true; }
    public void BrewbuttonUp() { isbrewing = false; brewfin = true; }

    //---------------------Cacheing
    private TextMeshProUGUI Noti;
    private Transform LeafUIs;
    private Transform Leafs;
    private Transform TeaRect;
    private Image Toppings;
    private Transform Theometer;
    private GameObject Bar;
    private GameObject Gaege; 
    private GameObject Pointer; 
    private RectTransform[] Target = new RectTransform[3];
    private RectTransform GaegeRect;
    private float barheight;
    private float targetheight;
    private float[] targetpos = new float[3]; //182 ~ 575
    private Image[] LeafUImages = new Image[3];
    private Image[] LeafImages = new Image[3];
    private Button[] BaseButtons = new Button[(int)BASE.END];
    private TextMeshProUGUI[] BaseTxt = new TextMeshProUGUI[(int)BASE.END];
    private Button[] ToppingButtons = new Button[(int)TOPPING.END];
    private Button Servebutton;
    private Tooltip Tooltip;
    //-------------------------------

    [Space(100)]
    [Header("Internal")]
    [SerializeField] private RecipeData CurDrink;
    [SerializeField] private List<BASE> CurBases;
    [SerializeField] private List<TOPPING> CurToppings;
    [SerializeField] private List<RecipeData> CurDrinks;
    private bool Brewed = false;

    private void Start()
    {
        Noti = UImanager.instance.UIs[0].GetComponent<TextMeshProUGUI>();
        LeafUIs = UImanager.instance.UIs[1].transform;
        Theometer = UImanager.instance.UIs[2].transform;
        Bar = UImanager.instance.UIs[3];
        Gaege = UImanager.instance.UIs[4];
        Pointer = UImanager.instance.UIs[5];
        Leafs = UImanager.instance.UIs[6].transform;
        Toppings = UImanager.instance.UIs[7].GetComponent<Image>();
        Servebutton = UImanager.instance.UIs[8].GetComponent<Button>();
        TeaRect = UImanager.instance.UIs[9].transform;
        Tooltip = UImanager.instance.UIs[10].GetComponent<Tooltip>();
        GaegeRect = Gaege.GetComponent<RectTransform>();

        barheight     = Bar.GetComponent<RectTransform>().rect.height;
        Target[0] = UImanager.instance.UIs[3].transform.GetChild(0).GetComponent<RectTransform>();
        Target[1] = UImanager.instance.UIs[3].transform.GetChild(1).GetComponent<RectTransform>();
        Target[2] = UImanager.instance.UIs[3].transform.GetChild(2).GetComponent<RectTransform>();

        LeafUImages[0] = LeafUIs.GetChild(0).GetComponent<Image>();
        LeafUImages[1] = LeafUIs.GetChild(1).GetComponent<Image>();
        LeafUImages[2] = LeafUIs.GetChild(2).GetComponent<Image>();

        LeafImages[0] = Leafs.GetChild(0).GetComponent<Image>();
        LeafImages[1] = Leafs.GetChild(1).GetComponent<Image>();
        LeafImages[2] = Leafs.GetChild(2).GetComponent<Image>();
        // Day 0 234
        // Day 1 34
        // Day 2 4
        // Day 3
        if (DataManager.instance.Day < 3)
        {
            for (int i = DataManager.instance.Day + 1; i < 4; i++)
            {
                TeaRect.GetChild(i).gameObject.SetActive(false);
            }

        }
        for (int i = 0; i < TeaRect.childCount; i++)
        {
            Transform ch = TeaRect.GetChild(i);
            for (int j = 0; j < ch.childCount; j++)
            {
                Transform chch = ch.GetChild(j);
                if (chch.gameObject.name[0] == 'T')
                {
                    ToppingButtons[chch.gameObject.name[1] - '0'] = chch.GetComponent<Button>();
                }
                if (chch.gameObject.name[0] < '0' || chch.gameObject.name[0] > '9') continue;
                BaseButtons[Convert.ToInt32(chch.gameObject.name)] = chch.GetComponent<Button>();
                BaseTxt[Convert.ToInt32(chch.gameObject.name)] = chch.GetChild(0).GetComponent<TextMeshProUGUI>();
            }
        }


        RefreshTeaRectUI();

    }
    public void AddBase(int _base)
    {
        if (CurBases.Count > 2){
            Notificate("베이스는 3개 이하여야 합니다");
            return;
        }
        CurBases.Add((BASE)_base);
        --DataManager.instance.Base[_base];
        
        RefreshTeaRectUI();
        if (CurBases.Count - 1 == 0) LeafImages[0].sprite = Resourcemanager.instance.GetSprite("TeaSpritesBottom", _base);
        else if (CurBases.Count - 1 == 1) LeafImages[1].sprite = Resourcemanager.instance.GetSprite("TeaSpritesMiddle", _base);
        else if (CurBases.Count - 1 == 2) LeafImages[2].sprite = Resourcemanager.instance.GetSprite("TeaSpritesTop", _base);

        Animationmanager.instance.PlayAnim(CurBases.Count-1, "TeaLeafAppear", true);
        RefreshleafUI();
    }

    public void AddTopping(int _topping)
    {
        if (CurToppings.Count > 0){
            Notificate("토핑은 1개 이하여야 합니다");
            return;
        }

        if(CurBases.Count == 0)
        {
            Notificate("베이스를 먼저 넣어주세요");
            return;
        }

        CurToppings.Add((TOPPING)_topping);
        DataManager.instance.Money -= TOPPINGPRICE;
        RefreshTeaRectUI();

        Toppings.sprite = Resourcemanager.instance.GetSprite("ToppingSprite", _topping);
        if (CurBases.Count == 1) Animationmanager.instance.PlayAnim(6, "MiddleToppingAppear", true);
        else if (CurBases.Count == 2) Animationmanager.instance.PlayAnim(6, "MiddleToppingAppear", true);
        else if (CurBases.Count == 3) Animationmanager.instance.PlayAnim(6, "TopToppingAppear", true);
    }
    private void Notificate(string _txt)
    {
        Noti.color = Color.black;
        Noti.text = _txt;
        Animationmanager.instance.PlayAnim(4, "", true);
    }

    public void TeagameStart()
    {
        if (CurBases.Count < 1){
            Notificate("재료를 더 넣어주세요"); 
            return;
        }
        if (null != CurDrink) return;
        //init

        if(difficulty == Difficulty.EASY)
        {
            targetpos[0] = UnityEngine.Random.Range(182 , 575); //182 ~ 575
            targetheight = 175;
            Target[0].gameObject.SetActive(true);
            Target[0].sizeDelta = new Vector2(60.64f, targetheight);
            Target[0].anchoredPosition = new Vector2(-0.63f, targetpos[0]);
            Pointer.SetActive(false);
        }
        else
        {
            Pointer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 348);
            targetheight = 80;
            for (global::System.Int32 i = 0; i < CurBases.Count; i++)
            {
                Target[i].gameObject.SetActive(true);
                targetpos[i] = UnityEngine.Random.Range(142, 620);
                Target[i].anchoredPosition = new Vector2(-0.63f, targetpos[i]);
                Target[i].sizeDelta = new Vector2(60.64f, targetheight);
            }


        }

        Animationmanager.instance.PlayAnim(3, "ToBrew");
        LeafUIs.gameObject.SetActive(false);
        StopAllCoroutines();
        Theometer.gameObject.SetActive(true);
        Animationmanager.instance.PlayAnim(5, "TheometerAppear");
        StartCoroutine(Brew());
    }



    IEnumerator Brew()
    {
        GaegeRect.localScale = new Vector2(1, 0);
        Vector2 pointervec = new Vector2(0, -1);
        RectTransform pointertrans = Pointer.GetComponent<RectTransform>();

        yield return new WaitForSeconds(1f);


        while (true)
        {

            if (Input.GetMouseButton(0) && isbrewing)
            {
                if(difficulty == Difficulty.EASY)
                {
                    GaegeRect.localScale += new Vector3(0, PointerSpeed, 1) * Time.deltaTime;
                    if (GaegeRect.localScale.y > 0.99f)
                        GaegeRect.localScale = new Vector2(1, 0.99f);

                }
                else
                {
                    pointertrans.anchoredPosition += new Vector2(0, PointerSpeed * 100 * pointervec.y) * Time.deltaTime;
                    if (100 > pointertrans.anchoredPosition.y || pointertrans.anchoredPosition.y > 640)
                        pointervec *= -1;
                }
                yield return null;
            }
            if (Input.GetMouseButtonUp(0) && brewfin)
            {
                CurDrink = Judge();
                CurDrinks.Add(CurDrink);
                Brewed = true;
                CurDrink = null;            
                Servebutton.interactable = true;
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
    private RecipeData Judge()
    {
        RecipeData outrecipe;
        bool Success = false;
        if(!Resourcemanager.instance.recipeLookUp.TryGetValue(GenerateKey(CurBases, CurToppings), out outrecipe)){
            return Resourcemanager.instance.FailedDrinks[0];
        }

        if(difficulty == Difficulty.EASY)
        {
            Vector2 TargetRange = new Vector2(targetpos[0] - targetheight / 2, targetpos[0] + targetheight / 2);
            TargetRange /= barheight;

            float pointer = GaegeRect.localScale.y;
            if (pointer >= TargetRange.x && pointer <= TargetRange.y) Success = true;
        }
        else
        {
            float pointer = Pointer.GetComponent<RectTransform>().anchoredPosition.y;
            for (global::System.Int32 i = 0; i < CurBases.Count; i++)
            {
                Vector2 TargetRange = new Vector2(targetpos[i] - targetheight / 2, targetpos[i] + targetheight / 2);
                if (pointer >= TargetRange.x && pointer <= TargetRange.y){
                    Success = true;
                    break;
                }
            }

        }

        if (Success) return outrecipe;
        else return Resourcemanager.instance.FailedDrinks[1];

    }



    public void ToMainGame()
    {
        if(CurDrinks.Count < GameManager.instance.CurdrinkCount)
        {
            StartCoroutine(NextTea());
            return;
        }
        if (null != GameManager.instance)
        {
            GameManager.instance.ReturningFromTeaGame = true;
            Animationmanager.instance.PlayAnim(7, "TeaSubmit");

            DelayAction(GameManager.instance, 0.23f, () => {
                Scenemanager.instance.Changescene("MainGame");
                GameManager.instance.StartCoroutine(GameManager.instance.Judge(CurDrinks));
            });

        }
    }
    private void RefreshleafUI()
    {
        for(int i = 0; i < 3; i++)
        {
            if (i < CurBases.Count) LeafUImages[i].sprite = Resourcemanager.instance.GetSprite("LeafUI", 1);
            else LeafUImages[i].sprite = Resourcemanager.instance.GetSprite("LeafUI", 0);
        }
    }
    private void RefreshTeaRectUI()
    {
        for (int i = 0; i < (int)BASE.END; i++)
        {
            if (BaseButtons[i] == null) continue;

            if (DataManager.instance.Base[i] == 0)
            {
                BaseButtons[i].interactable = false;
                BaseButtons[i].gameObject.GetComponent<EventTrigger>().enabled = false;
                Tooltip.SetVisablity(false);
            }
            BaseTxt[i].text = DataManager.instance.Base[i].ToString();
        }
        for (int i = 0; i < (int)TOPPING.END; i++)
        {
            if (ToppingButtons[i] == null) continue;

            if (DataManager.instance.Money < TOPPINGPRICE)
            {
                ToppingButtons[i].interactable = false;
                ToppingButtons[i].gameObject.GetComponent<EventTrigger>().enabled = false;
                Tooltip.SetVisablity(false);
            }
        }

    }
    public void Trash()
    {
        for (int i = 0; i < CurBases.Count; i++) Animationmanager.instance.PlayAnim(i, "TeaLeafDisappear", true);
        if(CurToppings.Count > 0)
        {
            if (CurBases.Count == 1) Animationmanager.instance.PlayAnim(6, "MiddleToppingDisappear", true);
            else if (CurBases.Count == 2) Animationmanager.instance.PlayAnim(6, "MiddleToppingDisappear", true);
            else if (CurBases.Count == 3) Animationmanager.instance.PlayAnim(6, "TopToppingDisappear", true);
        }
        CurBases.Clear();
        CurToppings.Clear();
        CurDrink = null;
        if (Brewed)
        {
            CurDrinks[CurDrinks.Count - 1] = null;
            Brewed = false;
        }

        StopAllCoroutines();
        Animationmanager.instance.PlayAnim(3, "Toingred");
        LeafUIs.gameObject.SetActive(true);
        Theometer.gameObject.SetActive(false);
        Animationmanager.instance.PlayAnim(5, "TheometerDisappear");
        RefreshleafUI();
    }
    IEnumerator NextTea()
    {
        int prevbasecount = CurBases.Count;
        CurBases.Clear();
        CurToppings.Clear();
        CurDrink = null;

        Animationmanager.instance.PlayAnim(5, "TheometerDisappear");
        Animationmanager.instance.PlayAnim(7, "NextTea");
        yield return new WaitForSeconds(0.6f);
        for (int i = 0; i < prevbasecount; i++) Animationmanager.instance.PlayAnim(i, "TeaLeafDisappear", true);
        Animationmanager.instance.PlayAnim(3, "Toingred");
        yield return new WaitForSeconds(0.5f);
        LeafUIs.gameObject.SetActive(true);
        Theometer.gameObject.SetActive(false);
        RefreshleafUI();
    }



}
