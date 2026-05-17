using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
[System.Serializable]
public class Category
{
    [SerializeField] public List<ProductData> products = new List<ProductData>();
}

public class Shopmanager : MonoBehaviour
{
    [SerializeField] private GameObject ProductPrefab;
    [Space(80)]
    [Header("Internal")]
    [SerializeField] private ProductData[] AllProducts;
    [SerializeField] private Category[] Products = new Category[(int)CATEGORY.END];
    [SerializeField] private Dictionary<string, int> PriceDic = new Dictionary<string, int>();
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private Transform Categorypar;
    // Start is called before the first frame update
    void Start()
    {
        AllProducts = Resources.LoadAll<ProductData>("Products");

        if(null != Gamemanager.instance && 0 == Gamemanager.instance.CurProduct.Count)
            foreach (var product in AllProducts)
                Gamemanager.instance.CurProduct.Add(product.name, false);
        
        foreach (var product in AllProducts){
            Products[(int)product.Category].products.Add(product);
            PriceDic.Add(product.name, product.Price);
        }

        MoneyRefresh();
        CategoryRefresh(CATEGORY.INGREIDENT);
        if(null == Delegate.OnItemBuy) Delegate.OnItemBuy += ReorderSoldedItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CategoryRefresh(int _categroy)
    {
        for (int i = 0; i < Categorypar.childCount; i++)
            Destroy(Categorypar.GetChild(i).gameObject);


        foreach (var item in Products[_categroy].products)
        {
           GameObject product = Instantiate(ProductPrefab, Categorypar);
           Image Subnail = product.transform.GetChild(0).GetComponent<Image>();
           TextMeshProUGUI name = product.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
           TextMeshProUGUI desc = product.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Button Buybutton = product.transform.GetChild(3).GetComponent<Button>();
            TextMeshProUGUI price = product.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();

            product.name = item.name;
            Subnail.sprite = item.Subnail;
            name.text = item.name;
            desc.text = item.ProductDesc;
            price.text = item.Price.ToString();
            ReorderSoldedItem();
        }
    }
    public void CategoryRefresh(CATEGORY _categroy)
    {
        for (int i = 0; i < Categorypar.childCount; i++)
            Destroy(Categorypar.GetChild(i));


        foreach (var item in Products[(int)_categroy].products)
        {
            GameObject product = Instantiate(ProductPrefab, Categorypar);
            Image Subnail = product.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI name = product.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI desc = product.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Button Buybutton = product.transform.GetChild(3).GetComponent<Button>();
            TextMeshProUGUI price = product.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();

            product.name = item.name;
            Subnail.sprite = item.Subnail;
            name.text = item.name;
            desc.text = item.ProductDesc;
            price.text = item.Price.ToString();
            ReorderSoldedItem();
            
        }
    }

    private void ReorderSoldedItem(string _name = "")
    {
        if (_name != "" && null != Gamemanager.instance) //bought
        {
            if (Gamemanager.instance.Money < PriceDic[_name]) return;
            Gamemanager.instance.CurProduct[_name] = true;
            Gamemanager.instance.Money -= PriceDic[_name];
            MoneyRefresh();
        }

        if (null == Categorypar) Categorypar = UImanager.instance.UIs[2].transform;
        for (int i = 0; i < Categorypar.childCount; i++)
        {
            GameObject product = Categorypar.GetChild(i).gameObject;
            Button Buybutton = product.transform.GetChild(3).GetComponent<Button>();
            TextMeshProUGUI price = product.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();

            if (null != Gamemanager.instance && Gamemanager.instance.CurProduct[product.name] == true)
            {
                Buybutton.interactable = false;
                price.text = "Sold";
                //product.transform.SetAsLastSibling();
            }
        }
    }


    void MoneyRefresh()
    {
        if (null == MoneyText) MoneyText = UImanager.instance.UIs[1].GetComponent<TextMeshProUGUI>();
        if (null != Gamemanager.instance) MoneyText.text = Gamemanager.instance.Money.ToString();
    }
}
