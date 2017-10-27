using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Allows item to be dragged by pointer
public class Draggable : MonoBehaviour, IDragHandler
{
    public RectTransform target;

    public void OnDrag(PointerEventData eventData)
    {
        if(Input.touchCount == 1)
            target.Translate(eventData.delta);
    }
}
