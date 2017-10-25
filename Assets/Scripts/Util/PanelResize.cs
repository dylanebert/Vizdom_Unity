using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*Allows attachee to be dragged to resize the target RectTransform*/
public class PanelResize : MonoBehaviour, IDragHandler
{
    static float MinSize = 128f;

    public RectTransform panel;

    RectTransform canvasRect;

    void Awake()
    {
        canvasRect = GameObject.FindGameObjectWithTag("Canvas").transform as RectTransform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        panel.sizeDelta = new Vector2(Mathf.Max(MinSize, panel.sizeDelta.x + eventData.delta.x / canvasRect.localScale.x), Mathf.Max(MinSize, panel.sizeDelta.y - eventData.delta.y / canvasRect.localScale.y));
    }
}
