using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObj/Event")]
public class EventData:ScriptableObject
{
    public string EventName;

    public CustomerData[] Customers;

    [Header("Dialogue CSV")]
    [SerializeField] private string[] introList;
    [SerializeField] private string[] selectList;
    [SerializeField] private string[] resultList;

    public string introCSV;
    public string selectCSV;
    public string resultCSV;

    //이거 한 번에 두 잔 이상 주문 들어가는 것 때문에 배열이었던 걸로 기억하는데
    //그... 주문 요소 자체를 배열화해서 랜덤으로 돌리고 싶거든? 저 위에 intro select result랑 주문 들어가는 음료랑 인덱스 맞춰서
    public List<RecipeData> OrderedDrinks;

    //private void OnEnable()
    //{
        //for (int i = 0; i < (int)State.END; i++)
        //{
        //    if (Dialogues[i] == null)
        //    {
        //        Dialogues[i] = new DialogueLinee();
        //    }
        //    Dialogues[i].state = (State)i;
        //}

        //DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        //dialogueManager.StartDialogue($"Dialogues/{dialogueCSV}");
        //dialogueManager.StartDialogue(dialogueCSV);
    //}

    public string GetEventName()
    {
        return EventName;
    }

    public CustomerData GetCustomer(POS pos)
    {
        return Customers[(int)pos];
    }

    public void SetCSV() //csv 배열 중 어느 대화를 진행할 지 랜덤으로 지정
    {
        //추후 일차에 따라 대화 내용(주문하는 차) 조건이 들어가야 하면 수정

        if (!(introList.Length == selectList.Length && introList.Length == resultList.Length))
        {
            Debug.LogWarning("introList, selectList, resultList의 길이가 다릅니다.");
            return;
        }

        int i = Random.Range(0, introList.Length);

        introCSV = introList[i];
        selectCSV = selectList[i];
        resultCSV = resultList[i];
    }

}
