using System;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    [SerializeField]private SwipeUI SwipeUI;
    private Transform CropsPar;
    private Image[] CropsImg;
    private Button[] CropsBtn;
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
        CropsPar = UImanager.instance.UIs[0].GetComponent<Transform>();
        LArrAnim = UImanager.instance.UIs[1].GetComponent<Animator>();
        RArrAnim = UImanager.instance.UIs[2].GetComponent<Animator>();
        Array.Resize(ref CropsBtn, CropsPar.childCount);
        Array.Resize(ref CropsImg, CropsPar.childCount);
        for (int i = 0; i < CropsPar.childCount; i++)
        {
            CropsImg[i] = CropsPar.GetChild(i).GetComponent<Image>();
            CropsBtn[i] = CropsPar.GetChild(i).GetComponent<Button>();
        }
        for (int i = 0; i < CropsBtn.Length; i++)
        {
            int idx = i;
            CropsBtn[i].onClick.AddListener(()=>{
                CropsImg[idx].sprite = PlantedCropData[idx].GrowthSprites[0];
                CropsBtn[idx].interactable = false;
                CropGrowthIndex[idx] = 0;
                ++DataManager.instance.Base[Convert.ToInt32( CropsBtn[idx].gameObject.name)];
            });
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
        SwipeUI.ToLeft();
    }
    public void ScrollRight()
    {
        SwipeUI.ToRight();

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
