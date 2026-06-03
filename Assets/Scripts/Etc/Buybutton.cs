using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buybutton : MonoBehaviour
{
    public void Buy()
    {
        Delegate.OnItemBuy?.Invoke(gameObject.name);
    }
}
