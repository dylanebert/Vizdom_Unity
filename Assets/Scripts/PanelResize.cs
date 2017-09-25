using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelResize : MonoBehaviour, IDragHandler
{
    public RectTransform panel;

    public void OnDrag(PointerEventData eventData)
    {
        panel.sizeDelta += new Vector2(eventData.delta.x, -eventData.delta.y);
    }
}
