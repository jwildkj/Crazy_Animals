using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
class Cuts
{
    public List<Transform> CutTrans = new List<Transform>();
}
public class PrologueManager : MonoBehaviour
{
    private Transform ComicPar;
    
    [SerializeField]List<Cuts> pages;
    [SerializeField]int Pageidx;
    [SerializeField]int Cutidx;
    
    private void Start()
    {
        ComicPar = UImanager.instance.UIs[0].transform;
        for (int i = 0; i < ComicPar.childCount; i++)
        {
            Transform page = ComicPar.GetChild(i);
            Cuts cut = new Cuts();
            for (int j = 0; j < page.childCount; j++)
            {
                cut.CutTrans.Add(page.GetChild(j));
            }
            pages.Add(cut);
        }

    }


    public void Next()
    {
        if(Pageidx == pages.Count - 1 && Cutidx == pages[Pageidx].CutTrans.Count)
        {
            Audiomanager.instance.PlayBGM(0);
            Scenemanager.instance.FadeOutAndChangeScene("Farm");
            return;
        }
        if (Pageidx < pages.Count && Cutidx < pages[Pageidx].CutTrans.Count) ++Cutidx;
        else if (Pageidx < pages.Count) {
            for (int i = 0; i < pages[Pageidx].CutTrans.Count; i++)
            {
                pages[Pageidx].CutTrans[i].gameObject.SetActive(false);
            }
            ++Pageidx; Cutidx = 0; return; 
        }
        
        pages[Pageidx].CutTrans[Cutidx - 1].gameObject.SetActive(true);
    }
}
