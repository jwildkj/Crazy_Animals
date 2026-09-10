using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Data")]
    public Dialogue[] dialogues;
    public Select[] selects;

    [Header("References")]
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueParser dialogueParser;
    [SerializeField] private SelectParser selectParser;

    [Header("Dialogue State")]
    private string currentSelectCSV;

    public bool isDialogue = false;
    public bool isSelect = false;
    public bool isNext = false;

    public event Action OnDialogueFinished;

    void Update()
    {
        if (isDialogue && !isSelect)
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
            //Debug.Log($"NEXT INPUT → line={line}, content={content}, dialogues.Length={dialogues.Length}");

            // 안전성 검사
            if (line < 0 || line >= dialogues.Length)
            {
                Debug.LogError($"Dialogue Line Out Of Range : {line}");
                EndDialogue();
                return;
            }

            Dialogue currentDialogue = dialogues[line];

            //1. select
            int selectNum = currentDialogue.select[content];
            if (selectNum > 0)
            {
                ShowSelect(selectNum);
                return;
            }

            //2. skip
            int skipLine = currentDialogue.skip[content];
            if (skipLine > 0)
            {
                dialogueUI.SetLine(skipLine - 1);
                dialogueUI.SetContent(0);

                dialogueUI.DialogueWriter();
                return;
            }

            //3. Next Content
            if (++content < currentDialogue.content.Length)
            {
                dialogueUI.SetContent(content);
                dialogueUI.DialogueWriter(); //같은 line 밑의 context만 변경
                return;
            }

            //4. Next Line
            content = 0;

            if (++line < dialogues.Length)
            {
                dialogueUI.SetLine(line);
                dialogueUI.SetContent(content);

                //Debug.Log($"AFTER SET → line={dialogueUI.LineCount}, content={dialogueUI.ContentCount}");

                dialogueUI.DialogueWriter();
                return;
            }

            //5. End
            EndDialogue();
        }
    }

    #region Dialogue
    public void StartDialogue(string dialogueCSV, string selectCSV = null, int startLine = 0)
    {
        Debug.Log($"START DIALOGUE : {dialogueCSV}, startLine={startLine}");

        if (dialogueParser == null)
        {
            Debug.LogError("DialogueParser is NULL");
            return;
        }

        dialogues = dialogueParser.Parse(dialogueCSV);

        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogError("Dialogue Parse Failed");
            return;
        }

        if (!string.IsNullOrEmpty(selectCSV))
        {
            currentSelectCSV = selectCSV;
        }

        // 선택지 상태 초기화
        isSelect = false;
        selects = null;

        dialogueUI.SetLine(startLine);
        dialogueUI.SetContent(0);

        isDialogue = true;

        dialogueUI.PrintDialogue(dialogues, startLine);
    }

    public void EndDialogue()
    {
        Debug.Log("EndDialogue Called");

        dialogues = null;

        isDialogue = false;
        isNext = false;

        dialogueUI.EndDialogue();

        Debug.Log("Invoke Start");
        OnDialogueFinished?.Invoke();
        Debug.Log("Invoke End");
    }
    #endregion

    #region Select
    private void ShowSelect(int selectNum)
    {
        // SelectParser 확인
        if (selectParser == null)
        {
            Debug.LogError("SelectParser is NULL");
            return;
        }

        // Select CSV가 지정되지 않았는지 확인
        if (string.IsNullOrEmpty(currentSelectCSV))
        {
            Debug.LogError("Select CSV is not specified.");
            return;
        }

        // 선택지 CSV 로드
        // 현재는 Dialogue의 Select 번호를
        // Select CSV의 1-based ID로 사용
        //
        // 예:
        // Select = 1
        // → SelectParser.Parse() 결과의 0번 Select
        selects = selectParser.Parse(currentSelectCSV);

        // Guard
        if (selects == null || selects.Length == 0)
        {
            Debug.LogError("Select Parse Failed");
            return;
        }

        int selectIndex = selectNum - 1;

        if (selectIndex < 0 || selectIndex >= selects.Length)
        {
            Debug.LogError(
                $"Select index out of range. " +
                $"selectNum = {selectNum}, index = {selectIndex}, " +
                $"selects.Length = {selects.Length}"
            );

            return;
        }

        Select select = selects[selectIndex];

        if (select == null)
        {
            Debug.LogError("Select is NULL");
            return;
        }

        // 선택지 모드 진입
        isSelect = true;
        isNext = false;

        dialogueUI.ShowSelect(select);
    }

    public void SelectDialogue(int selectedDialogue)
    {
        // 선택지 종료
        isSelect = false;
        selects = null;

        // Dialogue 번호는 CSV 기준 1부터 시작
        int targetLine = selectedDialogue - 1;

        if (targetLine >= 0 && targetLine < dialogues.Length)
        {
            dialogueUI.SetLine(targetLine);
            dialogueUI.SetContent(0);

            dialogueUI.DialogueWriter();
        }
        else
        {
            Debug.LogError(
                $"Selected dialogue index is out of bounds. " +
                $"selectedDialogue = {selectedDialogue}, " +
                $"targetLine = {targetLine}, " +
                $"dialogues.Length = {dialogues.Length}"
            );

            EndDialogue();
        }
    }

    public void EndSelect()
    {
        isSelect = false;
        selects = null;

        dialogueUI.EndSelect();
    }
    #endregion
}
