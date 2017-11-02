using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HiddenLayer : MonoBehaviour, IPointerClickHandler
{
    public Text title;
    public CanvasGroup options;

    DeepLearningLayersSubpanel panel;
    Grayout grayout;
    RectUtil rectUtil;
    bool focused;

    void Awake()
    {
        grayout = GameObject.FindGameObjectWithTag("Grayout").GetComponent<Grayout>();
        rectUtil = GetComponent<RectUtil>();
    }

    public void Initialize(DeepLearningLayersSubpanel panel)
    {
        this.panel = panel;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!focused)
            StartCoroutine(ToggleFocus());
    }

    void ClickOut()
    {
        if (focused)
            StartCoroutine(ToggleFocus());
    }

    public void Delete()
    {
        StartCoroutine(DeleteCoroutine());
    }

    IEnumerator DeleteCoroutine()
    {
        if (!focused) yield break;
        yield return StartCoroutine(ToggleFocus());
        StartCoroutine(panel.RemoveHiddenLayer(this));
    }

    IEnumerator ToggleFocus()
    {
        if (!focused)
        {
            focused = true;
            rectUtil.MoveToForeground();
            yield return StartCoroutine(grayout.Show());
            yield return StartCoroutine(rectUtil.AnimatedResize(new Vector2(256, -256), .5f));
            yield return StartCoroutine(ShowOptions(.5f));
            grayout.ClickOut += ClickOut;
        }
        else
        {
            focused = false;
            yield return StartCoroutine(HideOptions(.25f));
            yield return StartCoroutine(rectUtil.AnimateToStartSize(.25f));
            yield return StartCoroutine(grayout.Hide());
            rectUtil.RestoreParent();
            grayout.ClickOut -= ClickOut;
        }
    }

    IEnumerator ShowOptions(float time = 1f)
    {
        Vector2 startPos = title.rectTransform.anchoredPosition;
        Vector2 tarPos = new Vector2(0f, 128f);
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime / time;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            title.rectTransform.anchoredPosition = Vector2.Lerp(startPos, tarPos, v);
            options.alpha = t;
            yield return null;
        }
        options.alpha = 1;
        options.blocksRaycasts = true;
    }

    IEnumerator HideOptions(float time = 1f)
    {
        Vector2 startPos = title.rectTransform.anchoredPosition;
        Vector2 tarPos = Vector2.zero;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float v = Mathf.Sin(Mathf.PI * (1 - t) / 2f);
            title.rectTransform.anchoredPosition = Vector2.Lerp(tarPos, startPos, v);
            options.alpha = 1 - t;
            yield return null;
        }
        options.alpha = 0;
        options.blocksRaycasts = false;
    }
}
