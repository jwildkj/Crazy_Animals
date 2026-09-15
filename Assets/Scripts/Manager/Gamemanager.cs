
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Func;

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
    [SerializeField] private int Curevent = 0;
    public int CurdrinkCount = 0;
    public List<RecipeData> CurDrinks;

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

        //Animationmanager.instance.animations[0] = DialogueAnimation;
        //Customer = UImanager.instance.UIs[1].GetComponent<Image>();
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
    private void StartDialogue(string dialogueCSV, string selectCSV = null, int startLine = 0)
    {
        SetCSV();
        dialogueManager.StartDialogue(dialogueCSV, selectCSV, startLine);
    }

    private void SetCSV() //csv 배열 중 어느 대화를 진행할 지 랜덤으로 지정
    {
        EventData eventdata = Days[Day].Events[Curevent];
        //추후 일차에 따라 대화 내용(주문하는 차) 조건이 들어가야 하면 수정

        if (!(eventdata.introList.Length == eventdata.selectList.Length && eventdata.introList.Length == eventdata.resultList.Length))
        {
            Debug.LogWarning("introList, selectList, resultList의 길이가 다릅니다.");
            return;
        }

        int i = UnityEngine.Random.Range(0, eventdata.introList.Length);

        eventdata.introCSV = eventdata.introList[i];
        eventdata.selectCSV = eventdata.selectList[i];
        eventdata.resultCSV = eventdata.resultList[i];
        Debug.Log($"introCSV = {eventdata.introCSV}\nselectCSV = {eventdata.selectCSV}\nresultCSV = {eventdata.resultCSV}");
    }
    private void StartDay()
    {
        Debug.Log("START DAY");

        Invoke("StartIntro", 2);
    }
    private void StartIntro()
    {
        Debug.Log("INTRO START");
        //Curdialogue = 0;
        CurdrinkCount = Days[Day].Events[Curevent].OrderedDrinks.Count;
        CurDrinks = Days[Day].Events[Curevent].OrderedDrinks;
        //UpdateDialogue();

        //PortraitManager.instance.SetCustomer(Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE));

        //isResultDialogue = false;
        currentDialogueType = DialogueType.INTRO;

        StartDialogue($"Dialogues/{Days[Day].Events[Curevent].introCSV}", $"Dialogues/{Days[Day].Events[Curevent].selectCSV}");
    }




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

        //PortraitManager.instance.SetCustomer(Days[Day].Events[Curevent].GetCustomer(POS.MIDDLE));

        if (ScrambledEquals<RecipeData>(Days[Day].Events[Curevent].OrderedDrinks, recipeData))
        {
            Debug.Log("GameManager Judge 성공");

            StartDialogue($"Dialogues/{Days[Day].Events[Curevent].resultCSV}", $"Dialogues/{Days[Day].Events[Curevent].selectCSV}", 0);

            foreach (var item in recipeData)
            {
                Money += item.price;
            }
        }
        else
        {
            Debug.Log("GameManager Judge 실패");

            StartDialogue($"Dialogues/{Days[Day].Events[Curevent].resultCSV}", $"Dialogues/{Days[Day].Events[Curevent].selectCSV}", 1);
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
        Debug.Log("OnIntroFinished 실행");
        
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
            Invoke("StartIntro", 1);
        }
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
