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
    /// ���뽥��
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOutIn()
    {
        yield return FadeOut();
        yield return FadeIn();
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / fadeOutDuration;     //����alphaֵ
            yield return null;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeInDuration;     //��Сalphaֵ
            yield return null;
        }

        Destroy(gameObject);
    }
}
