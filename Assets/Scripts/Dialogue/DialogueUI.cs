using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject namePanel;

    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogueText;

    //private GameObject continueIcon;
    //private GameObject portrait;

    private int lineCount = 0; //대화 row 카운트
    private int contentCount = 0; //대사 카운트

    public int LineCount => lineCount;
    public int ContentCount => contentCount;

    public bool ignoreInputFrame = false;
    public bool isContentTyping = false;
    public bool skipContentTyping = false;

    private void Update()
    {
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

    public void SetLine(int value)
    {
        lineCount = value;
    }

    public void SetContent(int value)
    {
        contentCount = value;
    }

    public void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    public void PrintDialogue(Dialogue[] _dialogues)
    {
        dialogueManager.dialogues = _dialogues;

        dialoguePanel.SetActive(true);

        nameText.text = "";
        dialogueText.text = "";

        lineCount = 0;
        contentCount = 0;

        DialogueWriter();
    }

    public void DialogueWriter()
    {
        bool isNamed = !string.IsNullOrEmpty(dialogueManager.dialogues[lineCount].name);

        //NamePanel
        if (isNamed)
        {
            namePanel.SetActive(true);
            nameText.text = dialogueManager.dialogues[lineCount].name;
        }
        else
        {
            namePanel.SetActive(false);
            nameText.text = dialogueManager.dialogues[lineCount].name;
        }

        dialoguePanel.SetActive(true);

        //replaceText
        string replaceText = dialogueManager.dialogues[lineCount].content[contentCount];

        replaceText = replaceText.Replace("#", ","); //# → ,
        replaceText = replaceText.Replace("@", "\n"); //@ → \n

        StartCoroutine(ContentTyping(replaceText));
    }

    private IEnumerator ContentTyping(string content)
    {
        ignoreInputFrame = true;

        yield return null; //1프레임 대기

        ignoreInputFrame = false;

        isContentTyping = true;
        dialogueManager.isNext = false;

        dialogueText.text = "";

        for (int i = 0; i < content.Length; i++)
        {
            if (skipContentTyping)
            {
                dialogueText.text = content;
                break;
            }

            dialogueText.text += content[i];

            yield return new WaitForSeconds(0.03f);
        }

        isContentTyping = false;
        skipContentTyping = false;

        dialogueManager.isNext = true;
    }

    public void EndDialogue()
    {
        dialogueManager.dialogues = null;

        dialogueManager.isDialogue = false;
        dialogueManager.isNext = false;
        dialogueManager.isFinish = true;

        lineCount = 0;
        contentCount = 0;

        dialoguePanel.SetActive(false);
        namePanel.SetActive(false);
    }
}
