using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelDrag : MonoBehaviour, IDragHandler
{
    RectTransform parent;

    void Awake()
    {
        parent = transform.parent as RectTransform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        parent.Translate(eventData.delta);
    }
}
