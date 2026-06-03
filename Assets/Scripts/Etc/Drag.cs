using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool Dragable;
    public bool Smoothreturn;
    Vector3 DefaultPos;
    private Coroutine returnCoroutine; // 실행 중인 코루틴을 제어하기 위한 변수

    // 드래그 시작
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (!Dragable) return;
        // 올바르지 않은 곳에 드래그 했을 때 돌아갈 위치를 저장해준다
        DefaultPos = this.transform.position;
        // 드래그 시작 되었을 때는 드래그 중인 오브젝트의 레이캐스트 타겟을 꺼줘야 오류가 생기지 않는다
        GetComponent<Image>().raycastTarget = false;
    }

    // 드래그 중
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!Dragable) return;
        // 현재 터치되고 있는 좌표를 저장해서 오브젝트가 손가락을 따라갈 수 있도록 오브젝트의 좌표로 넣어준다
        Vector3 currentPos = Camera.main.ScreenToWorldPoint(eventData.position);

        // 이렇게 넣어주지 않으면 오브젝트가 이상한 곳에 위치하길래 조정해주었다
        // 원인을 아시는 분은 댓글로 알려주시면 감사하겠습니다
        currentPos.z = 0;
        this.transform.position = currentPos;
    }

    // 드래그 끝
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!Dragable) return;
        // 손님 오브젝트에서 음식 오브젝트에 대한 처리를 하고 오브젝트를 제거한다
        // 제거되지 않았다면 올바르지 않은 곳에 드롭된 것이기 때문에 원래 위치로 돌려준다
        if(Smoothreturn) ReturnToDefaultPosition();
        else this.transform.position = DefaultPos;
        // 레이캐스트 타겟도 원래대로 돌려준다
        GetComponent<Image>().raycastTarget = true;
    }

    // 원래 위치로 부드럽게 되돌리는 메서드
    public void ReturnToDefaultPosition()
    {
        // 혹시 이미 이동 중인 코루틴이 있다면 중복 실행되지 않도록 멈춰줍니다.
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        // 부드러운 이동 코루틴 시작 (원하는 시간(초)을 인자로 넘깁니다. 예: 0.3초)
        returnCoroutine = StartCoroutine(SmoothMove(DefaultPos, 0.3f));
    }

    private IEnumerator SmoothMove(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = this.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // 경과 시간에 따른 비율 계산 (0에서 1까지)
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t);

            this.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null; // 다음 프레임까지 대기
        }

        // 오차 범위를 없애기 위해 마지막에 정확한 목적지 좌표를 꽂아줍니다.
        this.transform.position = targetPosition;
        returnCoroutine = null;
    }
}
