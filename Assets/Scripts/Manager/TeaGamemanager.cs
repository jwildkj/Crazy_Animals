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
    [SerializeField] private GameObject Bar;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Teas;
    [SerializeField] private List<BASE> CurBases;
    [SerializeField] private List<TOPPING> CurToppings;
    [SerializeField] private List<RecipeData> CurDrinks;

    private void Start()
    {


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
        Debug.Log("TeaGameStart()");

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
        Debug.Log("Brew()");

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

    private void AfterTea()
    {
        Debug.Log("AfterTea Start");

        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        
        StartCoroutine(Fade(FADE.OUT, 0.2f, barimg, pointerimg, targetimg));

        EnDisableChildComponent<Button>(Teas.transform, true);
        CurDrinks.Add(Judge());
        CurBases.Clear();
        CurToppings.Clear();
        if(Gamemanager.instance.CurdrinkCount == CurDrinks.Count)
        {
            Debug.Log("AfterTea If문 안쪽입니다.");
            Gamemanager.instance.ReturningFromTeaGame = true;
            Gamemanager.instance.StartCoroutine(Gamemanager.instance.Judge(CurDrinks));
            Scenemanager.instance.Changescene("MainGame");
        }

    }


}
