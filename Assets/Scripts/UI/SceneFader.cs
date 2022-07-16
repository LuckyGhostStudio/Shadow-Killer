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
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="time">����ʱ��</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;     //����alphaֵ
            yield return null;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="time">����ʱ��</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;     //��Сalphaֵ
            yield return null;
        }

        Destroy(gameObject);
    }
}
