using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    [SerializeField] private string Target;
    [SerializeField]private UnityEvent Events;
    // 오브젝트가 드롭되었을 시 호출되는 함수
    public void OnDrop(PointerEventData eventData)
    {
        if (null != eventData.pointerDrag && Target == eventData.pointerDrag.name) { Events?.Invoke(); }
    }
}
