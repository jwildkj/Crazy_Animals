using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue[] dialogues;

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueParser dialogueParser;

    public bool isDialogue = false;
    public bool isNext = false;
    public bool isFinish = false;

    void Update()
    {
        if (isDialogue)
        {
            DialogueInputHandler();
        }
    }

    private void DialogueInputHandler()
    {
        //Space 입력 아니면 return
        if (!Input.GetKeyDown(KeyCode.Space) || dialogueUI.ignoreInputFrame)
        {
            return;
        }

        //ContextTyping() 중이면 Skip
        if (dialogueUI.isContentTyping)
        {
            dialogueUI.skipContentTyping = true;
            return;
        }

        //isNext 아니면 return
        if (!isNext)
        {
            return;
        }

        //초기화
        isNext = false;
        dialogueUI.ClearDialogueText();

        if (dialogues != null) // null에러 방지용
        {
            int line = dialogueUI.LineCount;
            int content = dialogueUI.ContentCount;

            Dialogue currentDialogue = dialogues[line];

            //skip
            if (!string.IsNullOrEmpty(currentDialogue.skip[content]))
            {
                if (int.TryParse(currentDialogue.skip[content], out int skipLine))
                {
                    dialogueUI.SetLine(skipLine - 1);
                    dialogueUI.SetContent(0);

                    dialogueUI.DialogueWriter();
                    return;
                }                
            }

            //Next Content
            if (++content < currentDialogue.content.Length)
            {
                dialogueUI.SetContent(content);
                dialogueUI.DialogueWriter(); //같은 line 밑의 context만 변경
            }
            else
            {
                content = 0;

                //Next Line
                if (++line < dialogues.Length)
                {
                    dialogueUI.SetLine(line);
                    dialogueUI.SetContent(content);

                    dialogueUI.DialogueWriter();
                }
                else
                {
                    dialogueUI.EndDialogue();
                }
            }
        }       
    }

    public void StartDialogue(string csvFileName)
    {
        Debug.Log($"START DIALOGUE : {csvFileName}");

        if (dialogueParser == null)
        {
            Debug.LogError("DialogueParser is NULL");
            return;
        }

        dialogues = dialogueParser.Parse(csvFileName);

        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogError("Dialogue Parse Failed");
            return;
        }

        isDialogue = true;
        isFinish = false;

        dialogueUI.PrintDialogue(dialogues);
    }
}
