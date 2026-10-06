using UnityEngine;


public class DataManager : MonoBehaviour
{
    const int INITALBASE = 3;
    const int INITALTOPPING = 3;
    
    public static DataManager instance;
    [Header("General")]
    public int Day = 0;
    public int Time = 0;
    public int Money = 0;
    [Space(20)]
    [Header("Farm")]
    public int[] CropGrowthIndex = new int[0];
    [Space(20)]
    [Header("Ingredients")]
    public int[] Base = new int[(int)BASE.END];
    public int[] Topping = new int[(int)TOPPING.END];

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

    private void OnValidate()
    {
        for (int i = 0; i < Base.Length; i++)
        {
            Base[i] = INITALBASE;
        }
        for (int i = 0; i < Topping.Length; i++)
        {
            Topping[i] = INITALTOPPING;
        }
    }
}
