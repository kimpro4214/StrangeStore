using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class EndingCreditController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float startDelay = 0f;

    private CanvasGroup canvasGroup;
    private TMP_Text[] texts;   // 하위 모든 TMP

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        texts = GetComponentsInChildren<TMP_Text>(true);
        SetAlpha(0f);
    }

    public IEnumerator FadeIn()
    {
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }
        SetAlpha(1f);
    }

    void SetAlpha(float a)
    {
        canvasGroup.alpha = a;          // 이미지 등
        foreach (var t in texts)
            t.alpha = a;                // TMP 직접 제어 (color.a)
    }
}
