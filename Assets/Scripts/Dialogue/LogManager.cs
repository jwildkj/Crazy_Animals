using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject logPanel;         // 전체 로그 창
    public Transform contentTransform;  // Scroll View의 Content
    public GameObject logItemPrefab;    // 대사 한 줄 프리팹
    public ScrollRect scrollRect;       // 자동 스크롤용

    // 대사 기록을 저장할 리스트 (최대 개수 제한 가능)
    private List<Log> logHistory = new List<Log>();

    private void Awake()
    {
        logPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            OpenLog();
        }
    }

    public void AddLog(string _name, string _content)
    {
        // 데이터 저장
        Log newDialogue = new Log(_name, _content);
        logHistory.Add(newDialogue);

        // UI 아이템 생성 및 텍스트 적용
        GameObject item = Instantiate(logItemPrefab, contentTransform);
        Text[] texts = item.GetComponentsInChildren<Text>();

        // 프리팹 자식 순서에 맞게 인덱스 조정 (0: 화자, 1: 대사)
        texts[0].text = _name;
        texts[1].text = _content;
    }

    public void OpenLog()
    {
        bool isActive = !logPanel.activeSelf;
        logPanel.SetActive(isActive);

        if (isActive)
        {
            // 창을 열 때 스크롤을 맨 아래(최신 대사)로 이동
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    public void ClearLog()
    {
        // 로그 데이터 초기화
        logHistory.Clear();

        // UI 갱신
        RefreshLog();
    }

    public void RefreshLog()
    {
        // 기존 UI 삭제
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // 저장된 로그를 다시 UI에 생성
        foreach (Log log in logHistory)
        {
            GameObject item = Instantiate(logItemPrefab, contentTransform);
            Text[] texts = item.GetComponentsInChildren<Text>();

            texts[0].text = log.name;
            texts[1].text = log.content;
        }

        // UI 레이아웃 갱신
        Canvas.ForceUpdateCanvases();
    }
}
