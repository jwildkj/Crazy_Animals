using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    [SerializeField]TeaGamemanager teagamemanager;
    GameObject prev;
    bool Block;

    private void Start()
    {
        if (DataManager.instance.Teagame > 1) return;   
        if (DataManager.instance.Teagame == 0){
            teagamemanager.DisableTeaButton(new Vector2(0, 0));
            teagamemanager.DisableTeaButton(new Vector2(1, 0));
            prev = gameObject.transform.GetChild(0).gameObject;
            prev.SetActive(true);
        }
        else if (DataManager.instance.Teagame == 1) {
            prev = gameObject.transform.GetChild(2).gameObject;
            prev.SetActive(true);
        }
        Block = true;
        Func.DelayAction(this, 0.3f, () => {
            Block = false;
            prev.GetComponent<Button>().interactable = true;
        });
    }

    public void TheometerTutorial()
    {
        if (DataManager.instance.Teagame == 0)
        {
            prev = gameObject.transform.GetChild(1).gameObject;
            prev.SetActive(true);
            Block = true ;
            Func.DelayAction(this, 0.3f, () => {
                Block = false;
                prev.GetComponent<Button>().interactable = true;
            });
        }
    }
    public void Show(int _page)
    {
        prev = gameObject.transform.GetChild(_page).gameObject;
        prev.SetActive(true);
        Block = true;
        Func.DelayAction(this, 0.3f, () => {
            Block = false;
            prev.GetComponent<Button>().interactable = true;
        });
    }

    public void Close()
    {
        if (prev != null && !Block)
        {
            prev.SetActive(false);
        }
    }

}
