using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public enum Pages
{
    TITLE,
    CUSTOMER,
    CUSTOMERINFO,
    RECIPES,

    END
}
public class Dictionarymanager : MonoBehaviour
{
    private static Dictionarymanager instance;
    [SerializeField] private Pages CurPage;
    [SerializeField] private GameObject[] PagesObj;
    [Header("Prefabs")]
    [SerializeField] private Transform CustomerLeftPar;
    [SerializeField] private Transform CustomerRightPar;
    [SerializeField] private GameObject CustomerPrefab;
    [SerializeField] private GameObject CustomerInfo;
    [SerializeField] private Transform RecipePar;
    [SerializeField] private GameObject RecipePrefab;

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

    public void TurntoPage(int page)
    {
        CurPage = (Pages)page;
        for (int i = 0; i < PagesObj.Length; i++)
            PagesObj[i].SetActive(false);

        PagesObj[(int)CurPage].SetActive(true);
        switch (CurPage)
        {
            case Pages.CUSTOMER:
                CustomerReload();
                break;
            case Pages.CUSTOMERINFO:
                break;
            case Pages.RECIPES:
                break;
        }
    }

    private void CustomerReload()
    {
        if (CustomerLeftPar.childCount > 0) return;

        CustomerData[] customers = Resourcemanager.instance.allCustomers;

        for (int i = 0; i < customers.Length; i++)
        {
            GameObject customercell = null;
            if (i < 10){
                customercell = Instantiate(CustomerPrefab, CustomerLeftPar);
            }
            else{
                customercell = Instantiate(CustomerPrefab, CustomerRightPar);
            }

            TextMeshProUGUI name = customercell.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI desc = customercell.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Image subnail = customercell.transform.GetChild(2).GetComponent<Image>();

            name.text = customers[i].CustomerName;
            desc.text = customers[i].CustomerSummary;
            subnail.sprite = customers[i].Subnail;
            customercell.name = i.ToString();

            Button btn = customercell.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                GetCustomerInfo(Resourcemanager.instance.allCustomers[int.Parse(customercell.name) ]);
            });
        }
    }

    private void GetCustomerInfo(CustomerData data)
    {
        TurntoPage(2);
        Image subnail = CustomerInfo.transform.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI name =CustomerInfo.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI summary =CustomerInfo.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI desc =CustomerInfo.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI history =CustomerInfo.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI favoritetea = CustomerInfo.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();

        subnail.sprite = data.Subnail;
        name.text = data.CustomerName;
        summary.text = data.CustomerSummary;
        desc.text = data.CustomerDesc;
        history.text = data.CustomerHistory;
        favoritetea.text = data.FavoriteTea;

    }
    
}
