using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public float fadeInDuration;
    public float fadeOutDuration;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 渐入渐出
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOutIn()
    {
        yield return FadeOut();
        yield return FadeIn();
    }

    /// <summary>
    /// 渐出
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeOutDuration;     //增加alpha值
            yield return null;
        }
    }

    /// <summary>
    /// 渐入
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeInDuration;     //减小alpha值
            yield return null;
        }

        Destroy(gameObject);
    }
}
