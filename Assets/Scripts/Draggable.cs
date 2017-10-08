using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler
{
    public RectTransform target;

    public void OnDrag(PointerEventData eventData)
    {
        target.Translate(eventData.delta);
    }
}
