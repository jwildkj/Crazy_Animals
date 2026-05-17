using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class Func
{
    private static readonly WaitForSeconds ws = new WaitForSeconds(0.1f);
    /// <summary>
    /// 화면을 페이드인 페이드 아웃합니다. 가려주는 패널 넣어야함
    /// 사용법: BlackInOut(FADE.IN, 1, fadeoutpannel, this, () => Scenemanager.instance.Changescene("Shop"));
    /// </summary>
    /// <param name="_fade">페이드 인아웃 여부</param>
    /// <param name="_time">시간</param>
    /// <param name="_fadepannel">가려주는 패널</param>
    /// <param name="_requestedobj">요청하는 클래스</param>
    /// <param name="_afteraction">페이드 이후 행동</param>
    public static void BlackInOut(FADE _fade, float _time, Image _fadepannel, MonoBehaviour _requestedobj, Action _afteraction = null)
    {
        if(FADE.IN == _fade)
        {
            _fadepannel.enabled = true;
            Color pannelcol = _fadepannel.color;
            if (_fadepannel.color.a == 1) { _fadepannel.color = new Color(pannelcol.r, pannelcol.g, pannelcol.b, 0); }
            _requestedobj.StartCoroutine(Fade(_fade, _time, _fadepannel));
            Endfading += _afteraction;
            Endfading += () => {
                Endfading = null;
            };
        }
        else
        {
            _fadepannel.enabled = true;
            Color pannelcol = _fadepannel.color;
            if (_fadepannel.color.a == 0) { _fadepannel.color = new Color(pannelcol.r, pannelcol.g, pannelcol.b, 1); }
            _requestedobj.StartCoroutine(Fade(_fade, _time, _fadepannel));
            Endfading += _afteraction;
            Endfading += () => {
                _fadepannel.enabled = false;
                Endfading = null;
            };
        }

    }
    /// <summary>
    /// 특정 UI를 페이드인, 페이드 아웃합니다. 꼭 코루틴 안에 넣어줘야함
    /// 사용법: StartCoroutine(Fade(FADE.OUT, 0.5f, barimg, pointerimg, targetimg));
    /// </summary>
    /// <param name="_fade">페이드 인아웃 여부</param>
    /// <param name="_time">시간</param>
    /// <param name="_imgs">대상 이미지들</param>
    public static Action Startfading;
    public static Action Endfading;
    public static IEnumerator Fade(FADE _fade, float _time, params Image[] _imgs)
    {
        if (_imgs.Length < 0) yield break;
        yield return new WaitUntil(() => Animationmanager.instance.isanimplaying() == false);
        bool isDone = false;
        Startfading?.Invoke();

        while (!isDone)
        {
            isDone = true; // 일단 완료되었다고 가정
            for (int i = 0; i < _imgs.Length; i++)
            {
                if (_imgs[i] == null) continue;

                float currentA = _imgs[i].color.a;
                float nextA = Mathf.MoveTowards(currentA, (int)_fade, Time.deltaTime / _time);

                _imgs[i].color = new Color(_imgs[i].color.r, _imgs[i].color.g, _imgs[i].color.b, nextA);

                if (Mathf.Abs(nextA - (int)_fade) > 0.001f) isDone = false;
            }
            yield return null;
        }

        Endfading?.Invoke();
    }

    /// <summary>
    /// 카메라를 흔듭니다.
    /// 사용법: StartCoroutine(CamShake(0.5f, 0.2f));
    /// </summary>
    /// <param name="_duration">흔들리는 시간</param>
    /// <param name="_magnitude">흔들리는 강도</param>
    public static IEnumerator CamShake(float _duration, float _magnitude)
    {
        Transform target = Camera.main.transform;
        Vector3 originalPos = target.localPosition;
        float elapsed = 0.0f;

        while (elapsed < _duration)
        {
            // -1.0 ~ 1.0 사이의 랜덤값을 강도와 곱함
            float x = UnityEngine.Random.Range(-1f, 1f) * _magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * _magnitude;

            target.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 흔들림이 끝나면 원래 위치로 복구
        target.localPosition = originalPos;
    }

    /// <summary>
    /// 부모 아래 자식들의 특정 컴포넌트를 활성화하거나 비활성화합니다.
    /// 사용법: EnDisableChildComponent<Button>(Teas.transform, true);
    /// </summary>
    /// <param name="_parent">부모</param>
    /// <param name="_enabled">활성화 여부</param>
    public static void EnDisableChildComponent<T>(Transform _parent, bool _enabled) where T : Behaviour
    {
        T[] components = _parent.GetComponentsInChildren<T>(true);

        foreach (T comp in components)
        {
            if (comp.transform == _parent) continue;

            comp.enabled = _enabled;
        }
    }
    /// <summary>
    /// 두 리스트를 순서상관없이 비교합니다.
    /// 사용법: ScrambledEquals<RecipeData>(list1, list2);
    /// </summary>
    /// <param name="list1">리스트 1</param>
    /// <param name="list2">리스트 2</param>
    public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
    {
        var cnt = new Dictionary<T, int>();
        foreach (T x in list1)
        {
            if (cnt.ContainsKey(x)) cnt[x]++; else cnt[x] = 1;
        }
        foreach (T x in list2)
        {
            if (cnt.ContainsKey(x)) cnt[x]--; else return false;
        }
        return cnt.Values.All(c => c == 0);
    }
}
