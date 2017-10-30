using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerClickHandler {

    public delegate void OnClick();
    public OnClick onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick();
    }
}
