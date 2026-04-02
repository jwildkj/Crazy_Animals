using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using static Func;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    [Header("Property")]
    [SerializeField] private float PointerSpeed;
    [SerializeField] private int Life;
    private int RealLife;
    [Space(100)]
    [Header("Internal")]
    [SerializeField] private GameObject TeagameUI;
    [SerializeField] private GameObject Bar;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Teas;
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

    public void TeagameStart()
    {
        Pointer.transform.localPosition = Target.transform.localPosition;
        RealLife = Life;
        EnDisableChildComponent<Button>(Teas.transform, false);
        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        Animationmanager.instance.PlayAnim(0);
        StopAllCoroutines();
        StartCoroutine(Fade(FADE.IN, 0.2f, barimg, pointerimg, targetimg));
        StartCoroutine(PointerMove());
    }



    IEnumerator PointerMove()
    {
        float length = Bar.GetComponent<RectTransform>().rect.width;
        WaitForSeconds ws = new WaitForSeconds(1 / PointerSpeed );
        float start = -(length / 2);
        float end = length / 2;
        UnityEngine.Vector3 movevec = new UnityEngine.Vector3(0.1f, 0, 0);
        while (true)
        {
            if(Input.GetKeyDown(KeyCode.A)){
                if (Judge()) break;
                else
                {
                    StartCoroutine(CamShake(0.3f, 0.2f));
                    Animationmanager.instance.PlayAnim(2, "Teashake"); 
                    --RealLife;
                    if (RealLife <= 0) Failed();
                }
            }
            if (Pointer.transform.localPosition.x < start)
            movevec.x = 0.1f;
            else if(Pointer.transform.localPosition.x > end)
                movevec.x = -0.1f;

            Pointer.transform.Translate(movevec);
            yield return ws;

        }

        Success();
    }

    string GenerateKey(List<BASE> bases, List<TOPPING> toppings)
    {
        bases.Sort();
        toppings.Sort();
        print($"B:{string.Join(",", bases)}|T:{string.Join(",", toppings)}");
        return $"B:{string.Join(",", bases)}|T:{string.Join(",", toppings)}";
    }
    private bool Judge()
    {
        float PointerPos = Pointer.transform.localPosition.x;
        float TargetPos = Target.transform.localPosition.x;
        float TargetLength = Target.GetComponent<RectTransform>().rect.width;

        UnityEngine.Vector2 TargetRange = new UnityEngine.Vector2(TargetPos - TargetLength/2, TargetPos + TargetLength/2);
        return PointerPos > TargetRange.x && PointerPos < TargetRange.y;
    }

    private void Success()
    {
        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();
        
        StartCoroutine(Fade(FADE.OUT, 0.2f, barimg, pointerimg, targetimg));

        Animationmanager.instance.PlayAnim(1);
        EnDisableChildComponent<Button>(Teas.transform, true);
    }
    private void Failed()
    {
        Image barimg = Bar.GetComponent<Image>();
        Image pointerimg = Pointer.GetComponent<Image>();
        Image targetimg = Target.GetComponent<Image>();

        StartCoroutine(Fade(FADE.OUT, 0.2f, barimg, pointerimg, targetimg));
        
        Animationmanager.instance.PlayAnim(2, "Teafail");
        EnDisableChildComponent<Button>(Teas.transform, true);
    }
}
