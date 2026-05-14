using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLinee
{
    public State state;
    public DialogueLine[] Dialogues;
}

[System.Serializable]
public class DialogueLine
{
    public string name;
    [TextArea]
    public string text;
}
[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObj/Event")]
public class EventData:ScriptableObject
{
    public string EventName;

    public CustomerData[] Customers;

    public DialogueLinee[] Dialogues = new DialogueLinee[(int)State.END];

    public List<RecipeData> OrderedDrinks;

    private void OnEnable()
    {
        for (int i = 0; i < (int)State.END; i++)
        {
            if (Dialogues[i] == null)
            {
                Dialogues[i] = new DialogueLinee();
            }
            Dialogues[i].state = (State)i;
        }
    }
    public string GetEventName()
    {
        return EventName;
    }

    public CustomerData GetCustomer(POS pos)
    {
        return Customers[(int)pos];
    }

    public string GetName(State state, int idx)
    {
        return Dialogues[(int)state].Dialogues[idx].name;
    }

    public string GetDialogue(State state, int idx)
    {
        return Dialogues[(int)state].Dialogues[idx].text;
    }

    public int GetDialogueLength(State state)
    {
        return Dialogues[(int)state].Dialogues.Length;
    }

}
