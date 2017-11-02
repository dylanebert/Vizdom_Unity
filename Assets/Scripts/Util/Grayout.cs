using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grayout : MonoBehaviour, IPointerClickHandler {

    public delegate void ClickOutDelegate();
    public ClickOutDelegate ClickOut;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator Show()
    {
        canvasGroup.blocksRaycasts = true;
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * 5f;
            canvasGroup.alpha = t;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    public IEnumerator Hide()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            canvasGroup.alpha = 1 - t;
            yield return null;
        }
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ClickOut != null)
            ClickOut();
    }
}
