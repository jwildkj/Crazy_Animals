using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Func;

public class TeaGamemanager : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private float PointerSpeed;

    private bool isbrewing;
    private bool brewfin;
    public void BrewbuttonDown() { isbrewing = true; }
    public void BrewbuttonUp() { isbrewing = false; brewfin = true; }

    //---------------------Cacheing
    private TextMeshProUGUI Noti;
    private Transform LeafUIs;
    private Transform Leafs;
    private Image Toppings;
    private Transform Theometer;
    private GameObject Bar;
    private GameObject Pointer; 
    private GameObject Target;
    private RectTransform pointerrect;
    private float barheight;
    private float pointer;
    private float targetheight;
    private float targetpos;
    private Image[] LeafUImages = new Image[3];
    private Image[] LeafImages = new Image[3];
    private Button Servebutton;
    //-------------------------------

    [Space(100)]
    [Header("Internal")]
    [SerializeField] private RecipeData CurDrink;
    [SerializeField] private List<BASE> CurBases;
    [SerializeField] private List<TOPPING> CurToppings;
    [SerializeField] private List<RecipeData> CurDrinks;

    private void Start()
    {
        Noti = UImanager.instance.UIs[0].GetComponent<TextMeshProUGUI>();
        LeafUIs = UImanager.instance.UIs[1].transform;
        Theometer = UImanager.instance.UIs[2].transform;
        Bar = UImanager.instance.UIs[3];
        Pointer = UImanager.instance.UIs[4];
        Target = UImanager.instance.UIs[5];
        Leafs = UImanager.instance.UIs[6].transform;
        Toppings = UImanager.instance.UIs[7].GetComponent<Image>();
        Servebutton = UImanager.instance.UIs[8].GetComponent<Button>();
        pointerrect = Pointer.GetComponent<RectTransform>();

        barheight     = Bar.GetComponent<RectTransform>().rect.height;
        pointer       = Pointer.GetComponent<RectTransform>().localScale.y;
        targetheight  = Target.GetComponent<RectTransform>().rect.height;
        targetpos     = Target.GetComponent<RectTransform>().anchoredPosition.y;

        LeafUImages[0] = LeafUIs.GetChild(0).GetComponent<Image>();
        LeafUImages[1] = LeafUIs.GetChild(1).GetComponent<Image>();
        LeafUImages[2] = LeafUIs.GetChild(2).GetComponent<Image>();

        LeafImages[0] = Leafs.GetChild(0).GetComponent<Image>();
        LeafImages[1] = Leafs.GetChild(1).GetComponent<Image>();
        LeafImages[2] = Leafs.GetChild(2).GetComponent<Image>();

    }
    public void AddBase(int _base)
    {
        if (CurBases.Count > 2){
            Notificate("베이스는 3개 이하여야 합니다");
            return;
        }
        CurBases.Add((BASE)_base);
        if(CurBases.Count - 1 == 0) LeafImages[0].sprite = Resourcemanager.instance.GetSprite("TeaSpritesBottom", _base);
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
        Toppings.sprite = Resourcemanager.instance.GetSprite("ToppingSprite", _topping);
        Animationmanager.instance.PlayAnim(6, "TeaLeafAppear", true);
        CurToppings.Add((TOPPING)_topping);
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

        Animationmanager.instance.PlayAnim(3, "ToBrew");
        LeafUIs.gameObject.SetActive(false);
        StopAllCoroutines();
        Theometer.gameObject.SetActive(true);
        Animationmanager.instance.PlayAnim(5, "TheometerAppear");
        StartCoroutine(Brew());
    }



    IEnumerator Brew()
    {
        pointerrect.localScale = new Vector2(1, 0);

        yield return new WaitForSeconds(1f);


        while (true)
        {
            if (null != Dictionarymanager.instance && true == Dictionarymanager.instance.DicEnabled)
                continue;

            if (Input.GetMouseButton(0) && isbrewing)
            {
                pointerrect.localScale += new Vector3(0, PointerSpeed, 1) * Time.deltaTime;
                if (pointerrect.localScale.y > 1)
                    pointerrect.localScale = new Vector2(1, 1);

                yield return null;
            }
            if (Input.GetMouseButtonUp(0) && brewfin)
            {
                CurDrink = Judge();
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
        if(!Resourcemanager.instance.recipeLookUp.TryGetValue(GenerateKey(CurBases, CurToppings), out outrecipe)){
            return Resourcemanager.instance.FailedDrinks[0];
        }

        Vector2 TargetRange = new Vector2(targetpos - targetheight / 2, targetpos + targetheight / 2);
        TargetRange /= barheight;

        if (pointer >= TargetRange.x && pointer <= TargetRange.y){
            return outrecipe;
        }
        else{
            return Resourcemanager.instance.FailedDrinks[1];
        }
    }



    public void ToMainGame()
    {
        if (null != Gamemanager.instance)
        {
            Gamemanager.instance.ReturningFromTeaGame = true;
            Gamemanager.instance.StartCoroutine(Gamemanager.instance.Judge(CurDrinks));
            Scenemanager.instance.Changescene("MainGame");
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

    public void Trash()
    {
        for (int i = 0; i < CurBases.Count; i++) Animationmanager.instance.PlayAnim(i, "TeaLeafDisappear", true);
        if(CurToppings.Count > 0) Animationmanager.instance.PlayAnim(6, "TeaLeafDisappear", true);
        CurBases.Clear();
        CurToppings.Clear();
        CurDrink = null;

        StopAllCoroutines();
        Animationmanager.instance.PlayAnim(3, "Toingred");
        LeafUIs.gameObject.SetActive(true);
        Theometer.gameObject.SetActive(false);
        Animationmanager.instance.PlayAnim(5, "TheometerDisappear");
        RefreshleafUI();
    }




}
