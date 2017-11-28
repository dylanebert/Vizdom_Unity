using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/*Box in which deep learning input features are dropped*/
[RequireComponent(typeof(Droppable))]
public class InputFeatureBox : MonoBehaviour, IPointerClickHandler {

    public delegate void AttributeDelegate(InputFeatureBox obj, string attr);
    public delegate void ResetDelegate(InputFeatureBox obj);
    public AttributeDelegate attributeDelegate;
    public ResetDelegate resetDelegate;

    public Image box;
    public Text title;
    public CanvasGroup xButton;
    public string flag;

    Droppable droppable;
    Grayout grayout;
    RectUtil rectUtil;
    Sprite boxSprite;
    bool focused;
    bool initialized;
    bool showingXButton;

    private void Awake()
    {
        grayout = GameObject.FindGameObjectWithTag("Grayout").GetComponent<Grayout>();
        droppable = GetComponent<Droppable>();
        droppable.dropDelegate += OnDrop;
        rectUtil = GetComponent<RectUtil>();
        boxSprite = box.sprite;
    }

    public void Initialize(string attr)
    {
        title.text = attr;
        title.color = Palette.PrimaryTextColor;
        box.color = Palette.DefaultBGColor;
        box.sprite = null;
        droppable.enabled = false;
        initialized = true;
        if (attributeDelegate != null)
            attributeDelegate(this, attr);
    }

    public void OnDrop(DataMenuItem itemOver)
    {
        Initialize(itemOver.GetAttribute());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(initialized && !showingXButton)
        {
            StartCoroutine(ShowXButton());
        }
    }

    public void ResetAttribute()
    {
        title.text = "x";
        title.color = Palette.GrayTextColor;
        box.color = Palette.Transparent;
        box.sprite = boxSprite;
        droppable.enabled = true;
        initialized = false;
        StopAllCoroutines();
        StartCoroutine(HideXButton());
        showingXButton = false;
        if (resetDelegate != null)
            resetDelegate(this);
    }

    IEnumerator ShowXButton()
    {
        showingXButton = true;

        xButton.gameObject.SetActive(true);

        Vector2 startSize = Vector2.zero;
        Vector2 targetSize = Vector2.one;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            xButton.transform.localScale = Vector2.Lerp(startSize, targetSize, v);
            yield return null;
        }
        xButton.transform.localScale = targetSize;

        t = 0f;
        while(t < 5f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(HideXButton());
    }

    IEnumerator HideXButton()
    {
        Vector2 startSize = Vector2.one;
        Vector2 targetSize = Vector2.zero;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            float v = 1f - Mathf.Sin(Mathf.PI * (1f - t) / 2f);
            xButton.transform.localScale = Vector2.Lerp(startSize, targetSize, v);
            yield return null;
        }
        xButton.transform.localScale = targetSize;

        xButton.gameObject.SetActive(false);

        showingXButton = false;
    }
}
