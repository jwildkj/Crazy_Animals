using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shopmanager : MonoBehaviour
{
    [SerializeField] private GameObject ProductPrefab;
    [Space(80)]
    [Header("Internal")]
    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private Transform Categorypar;
    // Start is called before the first frame update
    void Start()
    {
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


        foreach (var item in Resourcemanager.instance.CategoryOrderedProducts[_categroy].products)
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


        foreach (var item in Resourcemanager.instance.CategoryOrderedProducts[(int)_categroy].products)
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
            if (Gamemanager.instance.Money < Resourcemanager.instance.PriceDic[_name]) return;
            Resourcemanager.instance.OwnedProduct[_name] = true;
            Gamemanager.instance.Money -= Resourcemanager.instance.PriceDic[_name];
            MoneyRefresh();
        }

        if (null == Categorypar) Categorypar = UImanager.instance.UIs[2].transform;
        for (int i = 0; i < Categorypar.childCount; i++)
        {
            GameObject product = Categorypar.GetChild(i).gameObject;
            Button Buybutton = product.transform.GetChild(3).GetComponent<Button>();
            TextMeshProUGUI price = product.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();

            if (Resourcemanager.instance.OwnedProduct[product.name] == true)
            {
                Buybutton.interactable = false;
                price.text = "Ç°Àý";
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
