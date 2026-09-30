using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    private Transform CropsPar;
    private Image[] CropsImg;
    private Button[] CropsBtn;
    [Header("Data")]
    [SerializeField] private CropData[] PlantedCropData;
    [SerializeField]private int[] CropGrowthIndex;

    private void OnValidate()
    {
        Array.Resize(ref CropGrowthIndex, PlantedCropData.Length);
    }

    private void Start()
    {
        CropsPar = UImanager.instance.UIs[0].GetComponent<Transform>();
        Array.Resize(ref CropsBtn, CropsPar.childCount);
        Array.Resize(ref CropsImg, CropsPar.childCount);
        for (int i = 0; i < CropsPar.childCount; i++)
        {
            CropsImg[i] = CropsPar.GetChild(i).GetComponent<Image>();
            CropsBtn[i] = CropsPar.GetChild(i).GetComponent<Button>();
        }
        for (int i = 0; i < CropsImg.Length; i++)
        {
            CropsImg[i].sprite = PlantedCropData[i].GrowthSprites[0];
        }
        for (int i = 0; i < CropsBtn.Length; i++)
        {
            int idx = i;
            CropsBtn[i].onClick.AddListener(()=>{
                CropsImg[idx].sprite = PlantedCropData[idx].GrowthSprites[0];
                CropsBtn[idx].interactable = false;
                CropGrowthIndex[idx] = 0;
            });
        }
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

}
