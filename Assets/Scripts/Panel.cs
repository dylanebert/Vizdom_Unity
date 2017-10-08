using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour, ISelectHandler
{
    RectTransform rect;

    void Awake()
    {
        rect = transform as RectTransform;
    }

    void Start()
    {
        rect.anchoredPosition += new Vector2(-rect.rect.width / 2f, rect.rect.height / 2f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
