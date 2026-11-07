
using UnityEngine;

public class CursorEff : MonoBehaviour
{
    GameObject ImgGameobj;
    RectTransform ImgTrans ;
    Animator ImgAnim ;
    RectTransform CanvasRect ;
    private void Awake()
    {
       ImgGameobj = transform.GetChild(0).gameObject;
       ImgTrans = ImgGameobj.GetComponent<RectTransform>();
        ImgAnim = ImgGameobj.GetComponent<Animator>();
        CanvasRect = GetComponent<RectTransform>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                CanvasRect,
                Input.mousePosition,
                null,
                out localPoint))
            {
                ImgTrans.anchoredPosition = localPoint;
                ImgTrans.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
                ImgAnim.Play("CursorAnim");
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                CanvasRect,
                Input.mousePosition,
                null,
                out localPoint))
            {
                ImgTrans.anchoredPosition = localPoint;
            }
        }
    }
}
