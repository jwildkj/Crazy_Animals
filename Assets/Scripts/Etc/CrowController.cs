using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowController : MonoBehaviour
{
    [SerializeField] private int Percent;
    Animator animator;
    RectTransform rectTransform;
    private void Start()
    {
        if (Random.Range(0f, 100f) < Percent)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();

    }
    public void Caw()
    {
        if (!gameObject.activeInHierarchy) return;
        animator.Play("CrowCaw");
    }

    public void Fly()
    {
        if (!gameObject.activeInHierarchy) return;
        animator.Play("CrowFly");
        StartCoroutine(FlyAnim());
    }

    IEnumerator FlyAnim()
    {
        Vector2 dir = new Vector2(Random.Range(-0.15f, 0.15f), 0.1f);
        while (rectTransform.anchoredPosition.y < 700)
        {
            transform.Translate(dir);
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
    }
}
