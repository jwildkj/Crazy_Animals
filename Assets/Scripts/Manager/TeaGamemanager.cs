using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Func;

public class TeaGamemanager : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private float PointerSpeed;
    [Space(100)]
    [Header("Internal")]
    [SerializeField] private TextMeshProUGUI Noti;
    [SerializeField] private GameObject Teacup;
    [SerializeField] private Drag Teacattle;
    [SerializeField] private Transform Tray;
    [SerializeField] private GameObject Bar;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Teas;
    [SerializeField] private RecipeData CurDrink;
    [SerializeField] private List<BASE> CurBases;
    [SerializeField] private List<TOPPING> CurToppings;
    [SerializeField] private List<RecipeData> CurDrinks;

    private void Start()
    {


    }
    public void AddBase(int _base)
    {
        if (CurBases.Count > 2){
            Notificate("º£ÀÌ½º´Â ÃÖ´ë 3°³±îÁö ³ÖÀ» ¼ö ÀÖ½À´Ï´Ù!");
            return;
        }
        CurBases.Add((BASE)_base);
        Animationmanager.instance.PlayAnim(0);
    }

    public void AddTopping(int _topping)
    {
        if (CurToppings.Count > 1){
            Notificate("ÅäÇÎÀº ÃÖ´ë 2°³±îÁö ³ÖÀ» ¼ö ÀÖ½À´Ï´Ù!");
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
        Debug.Log("TeaGameStart()");

        if (CurBases.Count < 1){
            Notificate("ÃÖ¼Ò 1°³ÀÇ º£ÀÌ½º°¡ ÇÊ¿äÇÕ´Ï´Ù!"); 
            return;
        }
        if (null != CurDrink) return;
        //init
        Teacup.GetComponent<Drag>().Dragable = false;
        EnDisableChildComponent<Button>(Teas.transform, false);
        Pointer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 25.236f);
        Animationmanager.instance.PlayAnim(1);
        StopAllCoroutines();

        StartCoroutine(Brew());
    }



    IEnumerator Brew()
    {
        Debug.Log("Brew()");

        yield return new WaitForSeconds(1f);

        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        Coroutine fadein = StartCoroutine(Fade(FADE.IN, 0.2f, barimg, pointerimg, targetimg));

        RectTransform pointerrect = Pointer.GetComponent<RectTransform>();

        while (true)
        {
            if (null != Dictionarymanager.instance && true == Dictionarymanager.instance.DicEnabled)
                continue;
            if (Input.GetMouseButton(0))
            {
                pointerrect.sizeDelta += new Vector2(PointerSpeed, 0) * Time.deltaTime;
                if (pointerrect.sizeDelta.x > 500)
                    pointerrect.sizeDelta = new Vector2(500, 25.236f);

                yield return null;
            }
            if (Input.GetMouseButtonUp(0))
            {
                StopCoroutine(fadein);
                StartCoroutine(Fade(FADE.OUT, 0.2f, barimg, pointerimg, targetimg));
                CurDrink = Judge();
                if (null != CurDrink.resultSprite) Teacup.GetComponent<Image>().sprite = CurDrink.resultSprite;
                Teacup.GetComponent<Drag>().Dragable = true;
                Teacattle.Dragable = true;
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
        Debug.Log("Judge()");

        Debug.Log(GenerateKey(CurBases, CurToppings));
        Debug.Log(Resourcemanager.instance);
        Debug.Log(Resourcemanager.instance.recipeLookUp);

        RecipeData outrecipe;
        if(!Resourcemanager.instance.recipeLookUp.TryGetValue(GenerateKey(CurBases, CurToppings), out outrecipe)){
            return Resourcemanager.instance.FailedDrinks[0];
        }

        float PointerPos = Pointer.GetComponent<RectTransform>().sizeDelta.x;
        float TargetPos = Target.GetComponent<RectTransform>().anchoredPosition.x;
        float TargetLength = Target.GetComponent<RectTransform>().sizeDelta.x;

        Vector2 TargetRange = new Vector2(TargetPos - TargetLength/2, TargetPos + TargetLength/2);

        if (PointerPos >= TargetRange.x && PointerPos <= TargetRange.y){
            return outrecipe;
        }
        else{
            return Resourcemanager.instance.FailedDrinks[1];
        }
    }



    public void PutTeaOnTray(GameObject tea)
    {
        
        Debug.Log("AfterTea Start");

        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        
        StartCoroutine(Fade(FADE.OUT, 0.2f, barimg, pointerimg, targetimg));
        
        if(null == CurDrink) return;

        EnDisableChildComponent<Button>(Teas.transform, true);
        CurDrinks.Add(CurDrink);
        CurBases.Clear();
        CurToppings.Clear();


        //ÄÅ ¾ø¾îÁü
        Animationmanager.instance.PlayAnim(3, "MainTeacupDisappear", true);
        
        //Æ®·¹ÀÌ À§ ÄÅ È°¼ºÈ­
        GameObject teaontray = Tray.GetChild(CurDrinks.Count-1).gameObject;
        teaontray.SetActive(true);
        
        //Æ®·¹ÀÌ À§ ÄÅ Á¤·Ä
        HorizontalLayoutGroup Aliegn = Tray.GetComponent<HorizontalLayoutGroup>();
        Aliegn.enabled = true;
        DelayAction(this, 0.01f, () => Aliegn.enabled = false);
        if (null != CurDrink.resultSprite) teaontray.GetComponent<Image>().sprite = CurDrink.resultSprite;
        
        //Æ®·¹ÀÌ À§ ÄÅ ¾Ö´Ï¸ÅÀÌ¼Ç
        teaontray.GetComponent<Animation>().Play();
        CurDrink = null;

        Invoke("ToMainGame", 0.2f);

        //ÄÅ ¸®½ºÆù ¾Ö´Ï¸ÅÀÌ¼Ç
        DelayAction(this, 0.3f,()=> Animationmanager.instance.PlayAnim(3, "MainTeacupAppear"));
    }

    private void ToMainGame()
    {
        if (null != Gamemanager.instance && Gamemanager.instance.CurdrinkCount == CurDrinks.Count)
        {
            Debug.Log("AfterTea If¹® ¾ÈÂÊÀÔ´Ï´Ù.");
            Gamemanager.instance.ReturningFromTeaGame = true;
            Gamemanager.instance.StartCoroutine(Gamemanager.instance.Judge(CurDrinks));
            Scenemanager.instance.Changescene("MainGame");
        }
    }


}
