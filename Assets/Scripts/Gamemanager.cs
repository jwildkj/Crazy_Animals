using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    [SerializeField] private UImanager UIman;
    [SerializeField] private Image Customer;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Dialogue;
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
        UIman = GameObject.FindWithTag("UI").GetComponent<UImanager>();
        Customer = UIman.UIs[0].GetComponent<Image>();
        Name = UIman.UIs[1].GetComponent<TextMeshProUGUI>();
        Dialogue = UIman.UIs[2].GetComponent<TextMeshProUGUI>();
        StartDialogue();
    }
    private void StartDialogue()
    {
        Curdialogue = 0;
        CurdrinkCount = Days[0].Events[Curevent].OrderedDrinks.Count;
        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        Customer.sprite = Days[0].Events[Curevent].GetCustomer(POS.MIDDLE).GetSprite(Curstate);
        Name.text = Days[0].Events[Curevent].GetName(Curstate, Curdialogue);
        string text = Days[0].Events[Curevent].GetDialogue(Curstate, Curdialogue);
        if ("TeaGame" == text)Scenemanager.instance.Changescene("TeaGame");
        else Dialogue.text = text;
        if (Curstate == State.NORMAL && 0 == Curdialogue)
        {
            Animationmanager.instance.PlayAnim(1, "CustomerUp");
            Animationmanager.instance.PlayAnim(0, "DialogueUp");
        }
        else
        {
            Animationmanager.instance.PlayAnim(1, "CustomerInstUp", true);
            Animationmanager.instance.PlayAnim(0, "DialogueInstUp", true);
        }
    }
    
    public void Next()
    {
        ++Curdialogue;
        if (Curdialogue>= Days[0].Events[Curevent].GetDialogueLength(Curstate))
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
        Invoke("StartDialogue", 1);
    }

    public void Judge(List< RecipeData> recipeData)
    {

        if (ScrambledEquals<RecipeData>(Days[0].Events[Curevent].OrderedDrinks, recipeData))
        {
            Curstate = State.HAPPY;
        }
        else
        {
            Curstate = State.ANGRY;
        }
    }
}
