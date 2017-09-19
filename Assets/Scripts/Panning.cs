using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Panning : MonoBehaviour, IDragHandler {

    public RectTransform container;

    public void OnDrag(PointerEventData data)
    {        
        container.Translate(data.delta);
    }
}
