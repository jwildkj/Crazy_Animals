using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    public static PortraitManager instance;

    [SerializeField] private Image portraitImage;
    [SerializeField] private CustomerData currentCustomer;

    private void Awake()
    {
        instance = this;
    }

    public void SetCustomer(CustomerData customer)
    {
        currentCustomer = customer;
    }

    public void SetExpression(int expressionIndex)
    {
        if (currentCustomer == null)
        {
            return;
        }

        portraitImage.sprite = currentCustomer.GetSprite(expressionIndex);
    }
}
