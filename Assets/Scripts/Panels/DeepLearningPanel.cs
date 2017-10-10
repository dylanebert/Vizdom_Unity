using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeepLearningPanel : Panel, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    DataMenuItem itemOver;

    public void OnDrop(PointerEventData eventData)
    {
        if (itemOver == null)
            return;

        Debug.Log(itemOver.GetAttribute());

        EventSystem.current.SetSelectedGameObject(this.gameObject);
        Unhighlight();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj == null)
            return;

        DataMenuItem dataMenuItem = obj.GetComponent<DataMenuItem>();
        if (dataMenuItem == null)
            return;

        Highlight();
        itemOver = dataMenuItem;
        itemOver.PreventDrop(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        Unhighlight();
        if (itemOver != null)
        {
            itemOver.PreventDrop(false);
            itemOver = null;
        }
    }
}