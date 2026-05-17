using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Func;

[System.Serializable]
public class EventDataa
{
    public EventData[] Events;
}

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public int Day = 0;
    public int Time = 0;
    public int Money = 0;
    [Space(20)]
    public EventDataa[] Days;
    [Space(100)]
    [Header("Internal")]
    [SerializeField]private State Curstate = 0;
    [SerializeField] private int Curevent = 0;
    [SerializeField] private int Curdialogue = 0;
    public int CurdrinkCount = 0;
    [Space(20)]
    [SerializeField] private Image Customer;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Dialogue;
    public Dictionary<string, bool> CurProduct = new Dictionary<string, bool>();
    public Dictionary<CATEGORY, ProductData> ApplyedProduct = new Dictionary<CATEGORY, ProductData>();
    [SerializeField] private ProductData[] Starters = new ProductData[(int)CATEGORY.END];
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Delegate.OnMainGameLoaded += Init;
            Delegate.OnNextDialogueRequeated += Next;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Delegate.OnMainGameLoaded -= Init;
            Delegate.OnNextDialogueRequeated -= Next;
            Destroy(gameObject);
        }

    }
    private void Init()
    {
        if(0 == ApplyedProduct.Count)
        {
            for (int i = 0; i < (int)CATEGORY.END; i++)
            {
                ApplyedProduct.Add((CATEGORY)i, Starters[i]);
            }
        }

        Customer = UImanager.instance.UIs[1].GetComponent<Image>();
        Name = UImanager.instance.UIs[2].GetComponent<TextMeshProUGUI>();
        Dialogue = UImanager.instance.UIs[3].GetComponent<TextMeshProUGUI>();
        if ( State.NORMAL == Curstate) StartDay();
        else StartDialogue();
    }
    private void StartDay()
    {
        Image fadeoutpannel = UImanager.instance.UIs[0].GetComponent<Image>();
        BlackInOut(FADE.OUT, 0.5f, fadeoutpannel, this);
        Invoke("StartDialogue", 2);
    }
    private void StartDialogue()
    {
        Curdialogue = 0;
        CurdrinkCount = Days[Day].Events[Curevent].OrderedDrinks.Count;
        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        if (null == Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE)){
            Customer.enabled = false;
        }
        else{
            Customer.enabled = true;
            Customer.sprite = Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE).GetSprite(Curstate);
        }

        Name.text = Days[Day].Events[Curevent].GetName(Curstate, Curdialogue);
        string text = Days[Day].Events[Curevent].GetDialogue(Curstate, Curdialogue);

        if ("TeaGame" == text)Scenemanager.instance.Changescene("TeaGame");
        else Dialogue.text = text;

        if (Curstate == State.NORMAL && 0 == Curdialogue){
            Animationmanager.instance.PlayAnim(1, "CustomerUp");
            Animationmanager.instance.PlayAnim(0, "DialogueUp");
        }
        else{
            Animationmanager.instance.PlayAnim(1, "CustomerInstUp", true);
            Animationmanager.instance.PlayAnim(0, "DialogueInstUp", true);
        }
    }
    
    public void Next()
    {
        ++Curdialogue;
        if (Curdialogue>= Days[Day].Events[Curevent].GetDialogueLength(Curstate))
        {
            EndDialogue();
        }
        else
        {
            UpdateDialogue();
        }
    }

    private void EndDialogue()
    {
        Curstate = State.NORMAL;
        Curdialogue = 0;
        ++Curevent;
        Animationmanager.instance.PlayAnim(0, "DialogueDown");
        Animationmanager.instance.PlayAnim(1, "CustomerDown");
        if (Curevent >= Days[Day].Events.Length) Invoke( "DayEnd", 1);
        else Invoke("StartDialogue", 1);
    }

    private void DayEnd()
    {
        ++Day;
        Curevent = 0;
        Scenemanager.instance.FadeOutAndChangeScene("Shop");
    }

    public void Judge(List< RecipeData> recipeData)
    {

        if (ScrambledEquals<RecipeData>(Days[Day].Events[Curevent].OrderedDrinks, recipeData))
        {
            Curstate = State.HAPPY;
            foreach (var item in recipeData)
            {
                Money += item.price;
            }
        }
        else
        {
            Curstate = State.ANGRY;
        }
    }
}
