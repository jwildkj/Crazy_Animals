using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public static UImanager instance;   
    public List<GameObject> UIs;
    private void Awake()
    {
        instance = this;
    }

    public void NextDialogue()
    {
        Delegate.OnNextDialogueRequeated?.Invoke();
    }
}
