using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*Parent class for all panels
 * Includes high level manipulations like highlight and selection*/
 [RequireComponent(typeof(Selectable))]
public class Panel : MonoBehaviour, ISelectHandler
{
    public Text centerText;
    public RectTransform main;

    protected RectTransform rect;

    public virtual void Awake()
    {
        rect = transform as RectTransform;
    }

    public virtual void Start()
    {
        rect.anchoredPosition += new Vector2(-rect.rect.width / 2f, rect.rect.height / 2f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// Sinusoidal animated resize of panel to target size over time
    /// </summary>
    /// <param name="targetSize">The new size to set the panel to</param>
    /// <param name="time">Time taken to resize</param>
    /// <returns></returns>
    public IEnumerator AnimatedResize(Vector2 targetSize, float time)
    {
        Vector2 startSize = rect.sizeDelta;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            rect.sizeDelta = Vector2.Lerp(startSize, targetSize, v);
            yield return null;
        }
    }
}
