using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    RectTransform rect;
    Image img;
    [SerializeField] Canvas parentCanvas;
    [SerializeField] bool follow;
    public void SetFollow(bool _set) { follow = _set; }
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            Vector2 mousePosition = Input.mousePosition;

            // Overlay 모드 Canvas일 때
            if (parentCanvas != null && parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                 rect.position = mousePosition;
            }
            // Camera 모드 Canvas일 때
            else
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    mousePosition,
                    parentCanvas.worldCamera,
                    out Vector2 localPoint
                );
                rect.localPosition = localPoint;
            }

        }
    }

    public void setText(string _text)
    {
        text.text = _text;
    }

    public void SetVisablity(bool _visablity)
    {
        if(_visablity == true)
        {
            img.enabled = true;
            text.enabled = true;
        }else
        {
            img.enabled =false;
            text.enabled = false;
        }
    }
}
