using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public List<GameObject> UIs;

    public void NextDialogue()
    {
        Delegate.OnNextDialogueRequeated?.Invoke();
    }
}
