using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    GameObject Chatbtn;
    GameObject Receiptbtn;
    private void Start()
    {
        Chatbtn = gameObject.transform.GetChild(1).gameObject;
        Receiptbtn = gameObject.transform.GetChild(2).gameObject;

        Delegate.OnFarmLoaded += FarmLoad;
        Delegate.OnMainGameLoaded += MainGameLoad;
        Delegate.OnTeaGameLoaded += TeaGameLoad;
        Delegate.OnEndingLoaded += EndingLoad;
    }

    void FarmLoad() { Chatbtn.SetActive(false); }
    void MainGameLoad() { Chatbtn.SetActive(true); Receiptbtn.SetActive(false); }
    void TeaGameLoad() { Chatbtn.SetActive(false); Receiptbtn.SetActive(true); }

    void EndingLoad() { gameObject.SetActive(false); }

    private void OnDestroy()
    {
        Delegate.OnFarmLoaded -= FarmLoad;
        Delegate.OnMainGameLoaded -= MainGameLoad;
        Delegate.OnTeaGameLoaded -= TeaGameLoad;
        Delegate.OnEndingLoaded -= EndingLoad;
    }

}
