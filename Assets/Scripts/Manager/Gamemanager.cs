
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Func;
using static UnityEditor.VersionControl.Asset;

[System.Serializable]
public class EventDataa
{
    public EventData[] Events;
}

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public DialogueManager dialogueManager;

    public int Day = 0;
    public int Time = 0;
    public int Money = 0;

    [Space(20)]
    public EventDataa[] Days;

    [Space(100)]
    [Header("Internal")]
    //[SerializeField] private State Curstate = 0;
    [SerializeField] private int Curevent = 0;
    //[SerializeField] private int Curdialogue = 0;
    public int CurdrinkCount = 0;

    [Space(20)]
    [SerializeField] private Image Customer;
    //[SerializeField] private TextMeshProUGUI Name;
    //[SerializeField] private TextMeshProUGUI Dialogue;
    [SerializeField] private Animation DialogueAnimation;

    //private bool isResultDialogue; //enum으로 확장 가능
    private DialogueType currentDialogueType;
    public enum DialogueType
    {
        INTRO,
        RESULT
    }

    public bool ReturningFromTeaGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Delegate.OnMainGameLoaded += Init;
            //Delegate.OnNextDialogueRequeated += Next;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Delegate.OnMainGameLoaded -= Init;
            //Delegate.OnNextDialogueRequeated -= Next;
            Destroy(gameObject);
        }

    }
    private void Init()
    {

        Debug.Log("INIT");

        Animationmanager.instance.animations[0] = DialogueAnimation;
        Customer = UImanager.instance.UIs[1].GetComponent<Image>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.Log("GameManager DialogueManager is NULL");
        }
        dialogueManager.OnDialogueFinished -= HandleDialogueFinished; //중복되지 않게 한 번 빼고 더하기
        dialogueManager.OnDialogueFinished += HandleDialogueFinished;
        //Name = UImanager.instance.UIs[2].GetComponent<TextMeshProUGUI>();
        //Dialogue = UImanager.instance.UIs[3].GetComponent<TextMeshProUGUI>();
        if (ReturningFromTeaGame)
        {
            ReturningFromTeaGame = false;
            return;
        }
        StartDay();
        //if ( State.NORMAL == Curstate) StartDay();
        //else StartDialogue();
    }

    private void StartDay()
    {
        Debug.Log("START DAY");

        Invoke("StartDialogue", 2);
    }
    private void StartDialogue()
    {
        Debug.Log("INTRO START");
        //Curdialogue = 0;
        CurdrinkCount = Days[Day].Events[Curevent].OrderedDrinks.Count;
        //UpdateDialogue();

        //isResultDialogue = false;
        currentDialogueType = DialogueType.INTRO;

        dialogueManager.StartDialogue($"Dialogues/{Days[Day].Events[Curevent].introCSV}");
    }

    //private void UpdateDialogue()
    //{
    //    if (null == Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE)){
    //        Customer.enabled = false;
    //    }
    //    else{
    //        Customer.enabled = true;
    //        Customer.sprite = Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE).GetSprite(Curstate);
    //    }

    //Name.text = Days[Day].Events[Curevent].GetName(Curstate, Curdialogue);
    //string text = Days[Day].Events[Curevent].GetDialogue(Curstate, Curdialogue);

    //if ("TeaGame" == text)Scenemanager.instance.Changescene("TeaGame");
    //else Dialogue.text = text;

    //if (Curstate == State.NORMAL && 0 == Curdialogue){
    //    Animationmanager.instance.PlayAnim(1, "CustomerUp");
    //    Animationmanager.instance.PlayAnim(0, "DialogueUp");
    //}
    //else{
    //    Animationmanager.instance.PlayAnim(1, "CustomerInstUp", true);
    //    Animationmanager.instance.PlayAnim(0, "DialogueInstUp", true);
    //}
    //}

    //public void Next()
    //{
    //    ++Curdialogue;
    //    if (Curdialogue>= Days[Day].Events[Curevent].GetDialogueLength(Curstate))
    //    {
    //        EndDialogue();
    //    }
    //    else
    //    {
    //        UpdateDialogue();
    //    }
    //}

    public IEnumerator Judge(List<RecipeData> recipeData)
    {
        //if (null == Customer) return;
        //if (null == Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE)){
        //    Customer.enabled = false;
        Debug.Log("GameManager Judge");
        Debug.Log($"{SceneManager.GetActiveScene().name}");

        yield return new WaitForSeconds(0.1f);

        //isResultDialogue = true;
        currentDialogueType = DialogueType.RESULT;
        Debug.Log("DialogueType => RESULT");

        if (ScrambledEquals<RecipeData>(Days[Day].Events[Curevent].OrderedDrinks, recipeData))
        {
            Debug.Log("GameManager Judge 성공");

            dialogueManager.StartDialogue($"Dialogues/{Days[Day].Events[Curevent].resultCSV}", 0);
            //Curstate = State.HAPPY;
            foreach (var item in recipeData)
            {
                Money += item.price;
            }
        }
        else
        {
            Debug.Log("GameManager Judge 실패");

            dialogueManager.StartDialogue($"Dialogues/{Days[Day].Events[Curevent].resultCSV}", 1);
            //Curstate = State.ANGRY;
        }
    }

    private void HandleDialogueFinished()
    {
        Debug.Log($"HandleDialogueFinished / type = {currentDialogueType}");

        //if (Curstate == State.NORMAL && 0 == Curdialogue)
        //{
        //    Animationmanager.instance.PlayAnim(1, "CustomerUp");
        //    Animationmanager.instance.PlayAnim(0, "DialogueUp");
        //}
        //else
        //{
        //    Animationmanager.instance.PlayAnim(1, "CustomerInstUp", true);
        //    Animationmanager.instance.PlayAnim(0, "DialogueInstUp", true);

        switch (currentDialogueType)
        {
            case DialogueType.INTRO:
                OnIntroFinished();
                break;

            case DialogueType.RESULT:
                OnResultFinished();
                break;
        }
    }
    private void OnIntroFinished()
    {
        string resultCSV = Days[Day].Events[Curevent].resultCSV;

        if (string.IsNullOrWhiteSpace(resultCSV))
        {
            AdvanceEvent();
        }
        else
        {
            Scenemanager.instance.FadeOutAndChangeScene("TeaGame");
        }
    }

    private void OnResultFinished()
    {
        AdvanceEvent();
    }

    private void AdvanceEvent()
    {
        //Curstate = State.NORMAL;
        //Curdialogue = 0;
        //dialogueManager.OnDialogueFinished -= EndDialogue;

        ++Curevent;
        ReturningFromTeaGame = false;

        //Animationmanager.instance.PlayAnim(0, "DialogueDown");
        //Animationmanager.instance.PlayAnim(1, "CustomerDown");

        if (IsDayFinished())
        {
            Invoke("DayEnd", 1);
        }
        else
        {
            Invoke("StartDialogue", 1);
        }
    }
    public void CleanUp()
    {
        Animationmanager.instance.PlayAnim(0, "DialogueInstDown", true);
        Animationmanager.instance.PlayAnim(1, "CustomerDown", true);
    }

    private bool IsDayFinished()
    {
        return Curevent >= Days[Day].Events.Length;
    }

    private void DayEnd()
    {
        ++Day;
        Curevent = 0;
        Scenemanager.instance.FadeOutAndChangeScene("Shop");
    }
}
