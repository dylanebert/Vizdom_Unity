using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/*Box in which deep learning input features are dropped*/
[RequireComponent(typeof(Droppable))]
public class InputFeatureBox : MonoBehaviour, IPointerClickHandler {

    public CanvasGroup options;
    public Image box;
    public Text title;

    Droppable droppable;
    Grayout grayout;
    RectUtil rectUtil;
    DeepLearningInputSubpanel panel;
    bool focused;
    bool initialized;

    private void Awake()
    {
        grayout = GameObject.FindGameObjectWithTag("Grayout").GetComponent<Grayout>();
        droppable = GetComponent<Droppable>();
        droppable.dropDelegate += OnDrop;
        rectUtil = GetComponent<RectUtil>();
    }

    public void Initialize(string attr)
    {
        title.text = attr;
        title.color = Palette.PrimaryTextColor;
        box.color = Palette.DefaultBGColor;
        box.sprite = null;
        droppable.enabled = false;
        StartCoroutine(this.transform.parent.GetComponent<DeepLearningInputSubpanel>().AddInputFeature());
        initialized = true;
    }

    public void SetPanel(DeepLearningInputSubpanel panel)
    {
        this.panel = panel;
    }

    public void OnDrop(DataMenuItem itemOver)
    {
        Initialize(itemOver.GetAttribute());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!focused && initialized)
            StartCoroutine(Focus());
    }

    void ClickOut()
    {
        if (focused && initialized)
            StartCoroutine(Defocus());
    }

    public void Delete()
    {
        StartCoroutine(DeleteCoroutine());
    }

    IEnumerator DeleteCoroutine()
    {
        if (!focused) yield break;
        yield return StartCoroutine(Defocus());
        StartCoroutine(panel.RemoveInputFeature(this));
    }

    IEnumerator Focus()
    {
        focused = true;
        rectUtil.MoveToForeground();
        StartCoroutine(grayout.Show());
        yield return StartCoroutine(rectUtil.AnimatedResize(new Vector2(256, 256), .25f));
        yield return StartCoroutine(ShowOptions(.25f));
        grayout.ClickOut += ClickOut;
    }

    IEnumerator Defocus()
    {
        grayout.ClickOut -= ClickOut;
        yield return StartCoroutine(HideOptions(.25f));
        yield return StartCoroutine(rectUtil.AnimateToStartSize(.25f));
        yield return StartCoroutine(grayout.Hide());
        rectUtil.RestoreParent();
        focused = false;
    }

    IEnumerator ShowOptions(float time = 1f)
    {
        Vector2 startPos = title.rectTransform.anchoredPosition;
        Vector2 tarPos = new Vector2(0f, 48f);
        float t = 0f;
        while (t < 1f)
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
