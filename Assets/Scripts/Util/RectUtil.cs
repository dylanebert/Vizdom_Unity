using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class RectUtil : MonoBehaviour {

    Grayout grayout;
    Transform originalParent;
    Transform canvas;
    RectTransform rect;
    Vector2 baseSize;
    CanvasGroup canvasGroup;

    void Awake()
    {
        grayout = GameObject.FindGameObjectWithTag("Grayout").GetComponent<Grayout>();
        originalParent = transform.parent;
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        rect = transform as RectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Move rect to foreground of canvas
    /// </summary>
    public void MoveToForeground()
    {
        transform.SetParent(canvas);
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// Move rect back to its original parent
    /// </summary>
    public void RestoreParent()
    {
        transform.SetParent(originalParent);
    }

    /// <summary>
    /// Sinusoidal animated resize of panel to target size over time
    /// </summary>
    /// <param name="targetSize">The new size to set the panel to</param>
    /// <param name="time">Time taken to resize</param>
    /// <returns></returns>
    public IEnumerator AnimatedResize(Vector2 targetSize, float time = 1f)
    {
        canvasGroup.blocksRaycasts = false;
        baseSize = rect.sizeDelta;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            rect.sizeDelta = Vector2.Lerp(baseSize, targetSize, v);
            yield return null;
        }
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Animated return to size when RectUtil was awoken
    /// </summary>
    /// <param name="time">Time taken to resize</param>
    /// <returns></returns>
    public IEnumerator AnimateToStartSize(float time = 1f)
    {
        canvasGroup.blocksRaycasts = false;
        Vector2 startSize = rect.sizeDelta;
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime / time;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            rect.sizeDelta = Vector2.Lerp(startSize, baseSize, v);
            yield return null;
        }
        canvasGroup.blocksRaycasts = true;
    }
}
