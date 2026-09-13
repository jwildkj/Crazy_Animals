using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueManager dialogueManager;

    [Header("Dialogue Bubble")]
    [SerializeField] private GameObject downBubble;
    [SerializeField] private GameObject rightBubble;

    [SerializeField] private Text downNameText;
    [SerializeField] private Text downDialogueText;

    [SerializeField] private Text rightNameText;
    [SerializeField] private Text rightDialogueText;

    //private GameObject continueIcon;
    //private GameObject portrait;

    [Header("Select UI")]
    [SerializeField] private GameObject selectButtons;

    [SerializeField] private GameObject selectButton1;
    [SerializeField] private GameObject selectButton2;

    [SerializeField] private Text selectText1;
    [SerializeField] private Text selectText2;

    [Header("Select Button Sprites")]
    [SerializeField] private Sprite buttonDefault;
    [SerializeField] private Sprite buttonHighlighted;

    [Header("Dialogue State")]

    private int lineCount = 0; //대화 row 카운트
    private int contentCount = 0; //대사 카운트

    public int LineCount => lineCount;
    public int ContentCount => contentCount;

    [HideInInspector] public bool ignoreInputFrame = false;
    [HideInInspector] public bool isContentTyping = false;
    [HideInInspector] public bool skipContentTyping = false;

    [Header("Select State")]
    private int currentSelectButtonIndex = 0;
    private Select currentSelect;

    private void Awake()
    {

    }

    private void Start()
    {
        EndSelect();
    }

    private void Update()
    {
        if (dialogueManager.isSelect)
        {
            SelectButtonInputHandler();
        }

        /*
        if (dialogueManager.isNext)
        {
            dialogueNext.SetActive(true);
        }
        else
        {
            dialogueNext.SetActive(false);
        }
        */
    }

    #region Set
    public void SetLine(int value)
    {
        lineCount = value;
    }

    public void SetContent(int value)
    {
        contentCount = value;
    }
    #endregion

    #region Clear
    public void ClearNameText()
    {
        downNameText.text = "";
        rightNameText.text = "";
    }

    public void ClearDialogueText()
    {
        downDialogueText.text = "";
        rightDialogueText.text = "";
    }
    #endregion

    public void HideBubbles()
    {
        downBubble.SetActive(false);
        rightBubble.SetActive(false);
    }

    #region Dialogue
    public void PrintDialogue(Dialogue[] _dialogues, int startLine = 0)
    {
        dialogueManager.dialogues = _dialogues;

        ClearNameText();
        ClearDialogueText();

        lineCount = startLine;
        contentCount = 0;

        DialogueWriter();
    }

    public void DialogueWriter()
    {
        Dialogue currentDialogue = dialogueManager.dialogues[lineCount];
        //bool isNamed = !string.IsNullOrEmpty(dialogueManager.dialogues[lineCount].name);
        bool isBoss = currentDialogue.name == "사장";

        downBubble.SetActive(isBoss);
        rightBubble.SetActive(!isBoss);

        Text currentNameText;
        Text currentDialogueText;

        if (isBoss)
        {
            currentNameText = downNameText;
            currentDialogueText = downDialogueText;
        }
        else
        {
            currentNameText = rightNameText;
            currentDialogueText = rightDialogueText;
        }

        currentNameText.text = currentDialogue.name;

        //int expressionIndex = currentDialogue.expression[contentCount];
        //// PortraitManager.instance.SetExpression(expressionIndex);

        //replaceText
        string replaceText = currentDialogue.content[contentCount];

        replaceText = replaceText.Replace("#", ","); //# → ,
        replaceText = replaceText.Replace("@", "\n"); //@ → \n      

        StartCoroutine(ContentTyping(currentDialogueText, replaceText));
    }

    public void EndDialogue()
    {
        lineCount = 0;
        contentCount = 0;


        HideBubbles();

        //PortraitManager.instance.HidePortrait();
    }
    #endregion

    #region Select
    public void ShowSelect(Select select)
    {
        if (select == null)
        {
            Debug.LogError("ShowSelect received NULL Select.");
            return;
        }

        if (select.content == null || select.content.Length < 2)
        {
            Debug.LogError("Select must have exactly 2 choices.");

            return;
        }

        HideBubbles();

        currentSelect = select;
        currentSelectButtonIndex = 0;

        // 선택지 UI 초기화
        selectText1.text = "";
        selectText2.text = "";

        // 대화 UI와 선택지 UI가 겹치지 않도록 처리
        ClearDialogueText();
        //dialogueText.text = "";

        // 선택지 버튼 표시
        selectButtons.SetActive(true);

        selectButton1.SetActive(true);
        selectButton2.SetActive(true);

        // 선택지 내용 출력
        string text1 = currentSelect.content[0];
        string text2 = currentSelect.content[1];

        text1 = text1.Replace("#", ",");
        text1 = text1.Replace("@", "\n");

        text2 = text2.Replace("#", ",");
        text2 = text2.Replace("@", "\n");

        selectText1.text = text1;
        selectText2.text = text2;

        HighlightSelectButton();
    }

    private void SelectButtonInputHandler()
    {
        // 좌측 → 1번 선택지
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            currentSelectButtonIndex = 0;

            HighlightSelectButton();
            return;
        }

        // 우측 → 2번 선택지
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
                 Input.GetKeyDown(KeyCode.D))
        {
            currentSelectButtonIndex = 1;

            HighlightSelectButton();
            return;
        }

        // 선택
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectButtonSelected();
        }
    }

    private void SelectButtonSelected()
    {
        if (currentSelect == null)
        {
            Debug.LogError("Current Select is NULL.");
            return;
        }

        if (currentSelect.dialogue == null || currentSelect.dialogue.Length < 2)
        {
            Debug.LogError("Select dialogue data is invalid.");

            return;
        }

        int selectedDialogue = currentSelect.dialogue[currentSelectButtonIndex];

        Debug.Log(
            $"Select : {currentSelectButtonIndex}, " +
            $"Dialogue : {selectedDialogue}"
        );

        EndSelect();

        dialogueManager.SelectDialogue(selectedDialogue);
    }

    private void HighlightSelectButton()
    {
        if (selectButton1 != null)
        {
            Image image1 =
                selectButton1.GetComponent<Image>();

            if (image1 != null)
            {
                image1.sprite = currentSelectButtonIndex == 0 ? buttonHighlighted : buttonDefault;
            }
        }

        if (selectButton2 != null)
        {
            Image image2 =
                selectButton2.GetComponent<Image>();

            if (image2 != null)
            {
                image2.sprite = currentSelectButtonIndex == 1 ? buttonHighlighted : buttonDefault;
            }
        }
    }

    public void EndSelect()
    {
        currentSelect = null;
        currentSelectButtonIndex = 0;

        if (selectButtons != null)
        {
            selectButtons.SetActive(false);
        }

        if (selectButton1 != null)
        {
            selectButton1.SetActive(false);
        }

        if (selectButton2 != null)
        {
            selectButton2.SetActive(false);
        }

        if (selectText1 != null)
        {
            selectText1.text = "";
        }

        if (selectText2 != null)
        {
            selectText2.text = "";
        }
    }
    #endregion

    private IEnumerator ContentTyping(Text currentText, string content)
    {
        isContentTyping = true;

        ignoreInputFrame = true;
        yield return null; //1프레임 대기
        ignoreInputFrame = false;

        dialogueManager.isNext = false;

        currentText.text = "";

        for (int i = 0; i < content.Length; i++)
        {
            if (skipContentTyping)
            {
                currentText.text = content;
                break;
            }

            currentText.text += content[i];

            yield return new WaitForSeconds(0.03f);
        }

        isContentTyping = false;
        skipContentTyping = false;

        dialogueManager.isNext = true;
    }
}
