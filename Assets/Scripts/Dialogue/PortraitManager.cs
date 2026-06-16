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
        Debug.Log($"SetExpression : {expressionIndex}");

        if (currentCustomer == null)
        {
            return;
        }

        if (expressionIndex < 0 || expressionIndex >= currentCustomer.CustomerSprite.Length)
        {
            Debug.LogWarning($"Expression Index Out Of Range : {expressionIndex}");

            return;
        }

        portraitImage.sprite = currentCustomer.GetSprite(expressionIndex);

        portraitImage.gameObject.SetActive(true);
    }

    public void HidePortrait()
    {
        portraitImage.gameObject.SetActive(false);
    }
}
