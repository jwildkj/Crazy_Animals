using System;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    [SerializeField]private SwipeUI SwipeUI;
    [SerializeField] private Image[] CropsImg;
    [SerializeField] private Button[] CropsBtn;
    private Animator LArrAnim;
    private Animator RArrAnim;
    [Header("Data")]
    [SerializeField] private CropData[] PlantedCropData;
    private int[] CropGrowthIndex
    {
        get
        {
            if (DataManager.instance != null) return DataManager.instance.CropGrowthIndex;
            else return null;
        }
        set 
        { 
            if(DataManager.instance != null) DataManager.instance.CropGrowthIndex = value;
        }
            
    }

    private void Start()
    {
        LArrAnim = UImanager.instance.UIs[1].GetComponent<Animator>();
        RArrAnim = UImanager.instance.UIs[2].GetComponent<Animator>();

        Transform[] CropsPars = new Transform[2] {
            UImanager.instance.UIs[0].GetComponent<Transform>(),
            UImanager.instance.UIs[5].GetComponent<Transform>()
        };
        Array.Resize(ref CropsBtn, PlantedCropData.Length);
        Array.Resize(ref CropsImg, PlantedCropData.Length);
        int realidx = 0;
        for (int i = 0; i < CropsPars.Length; i++)
        {
            for (int j = 0; j < CropsPars[i].childCount; j++)
            {
                if (CropsPars[i].GetChild(j).gameObject.name[0] < '0' || '9' < CropsPars[i].GetChild(j).gameObject.name[0]) continue;
                CropsImg[realidx] = CropsPars[i].GetChild(j).GetComponent<Image>();
                CropsBtn[realidx] = CropsPars[i].GetChild(j).GetComponent<Button>();
                int idx = realidx;
                CropsBtn[realidx].onClick.AddListener(()=>{
                    CropsImg[idx].sprite = PlantedCropData[idx].GrowthSprites[0];
                    CropsBtn[idx].interactable = false;
                    CropGrowthIndex[idx] = 0;
                    ++DataManager.instance.Base[Convert.ToInt32( CropsBtn[idx].gameObject.name)];
                });    
                realidx++;  
            }
        }

        if(PlantedCropData.Length != CropGrowthIndex.Length){
            int[] cropgrowthindex = CropGrowthIndex;
            Array.Resize(ref cropgrowthindex, PlantedCropData.Length);
            CropGrowthIndex = cropgrowthindex;
        }

        for (int i = 0; i < CropGrowthIndex.Length; i++)
        {
            CropsImg[i].sprite = PlantedCropData[i].GrowthSprites[CropGrowthIndex[i]];
        }

        if(DataManager.instance.Day > 0)Growth();
    }

    public void Growth()
    {
        for (int i = 0; i < CropGrowthIndex.Length; i++)
        {
            if(CropGrowthIndex[i] < PlantedCropData[i].MaxGrowthIdx - 1)
            {
                ++CropGrowthIndex[i];
                CropsImg[i].sprite = PlantedCropData[i].GrowthSprites[CropGrowthIndex[i]];

                if(CropGrowthIndex[i] == PlantedCropData[i].MaxGrowthIdx - 1) CropsBtn[i].interactable = true;
            }
        }
    }

    public void ScrollLeft()
    {
        Scenemanager.instance.FadeOut(0.2f);
        Func.DelayAction(this, 0.2f, () =>
        {
            SwipeUI.ToLeft();
            ArrowCheck();
        });
        Func.DelayAction(this, 0.3f, () => { Scenemanager.instance.FadeIn(0.2f); });
    }
    public void ScrollRight()
    {
        Scenemanager.instance.FadeOut(0.2f);
        Func.DelayAction(this, 0.2f, () =>
        {
            SwipeUI.ToRight();
            ArrowCheck();
        });
        Func.DelayAction(this, 0.3f, () =>{ Scenemanager.instance.FadeIn(0.2f); });
        

    }
    private void ArrowCheck()
    {
        if (SwipeUI.currentPage == 0) { UImanager.instance.UIs[1].SetActive(false); }
        else if (SwipeUI.currentPage == SwipeUI.maxPage - 1) { UImanager.instance.UIs[2].SetActive(false); }
        else { UImanager.instance.UIs[1].SetActive(true); UImanager.instance.UIs[2].SetActive(true); }
    }
    public void ArrAnimEnter(string _dir)
    {
        if (_dir == "L")
        {
            LArrAnim.SetBool("MOver", true);
        }
        else
        {
            RArrAnim.SetBool("MOver", true);

        }
    }

    public void ArrAnimExit(string _dir)
    {
        if (_dir == "L")
        {
            LArrAnim.SetBool("MOver", false);
        }
        else
        {
            RArrAnim.SetBool("MOver", false);

        }
    }

}
