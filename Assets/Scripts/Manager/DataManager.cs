using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("General")]
    public int Day = 0;
    public int Time = 0;
    public int Money = 0;
    [Space(20)]
    [Header("Farm")]
    public int[] CropGrowthIndex = new int[0];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
