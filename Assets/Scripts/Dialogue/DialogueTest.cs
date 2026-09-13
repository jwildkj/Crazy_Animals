using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DialogueExample()
    {
        FindObjectOfType<DialogueManager>().StartDialogue("Dialogues/DialogueExample");
    }

    public void SkipTest()
    {
        FindObjectOfType<DialogueManager>().StartDialogue("Dialogues/SkipTest");
    }

    public void SelectTest()
    {
        FindObjectOfType<DialogueManager>().StartDialogue("Dialogues/dialoguecsvTest", "Dialogues/selectcsvTest");
    }
}
