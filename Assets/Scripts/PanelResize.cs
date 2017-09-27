using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelResize : MonoBehaviour, IDragHandler
{
    static float MinSize = 256f;

    public RectTransform panel;
    public RectTransform canvasRect;

    public void OnDrag(PointerEventData eventData)
    {
        panel.sizeDelta = new Vector2(Mathf.Max(MinSize, panel.sizeDelta.x + eventData.delta.x / canvasRect.localScale.x), Mathf.Max(MinSize, panel.sizeDelta.y - eventData.delta.y / canvasRect.localScale.y));
    }
}
