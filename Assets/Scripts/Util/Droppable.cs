using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*Box in which data menu items can be dropped*/
public class Droppable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler {

    public delegate void DropDelegate(DataMenuItem itemOver);
    public DropDelegate dropDelegate;

    public GameObject overlay;

    DataMenuItem itemOver;

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (itemOver == null)
            return;

        Unhighlight();
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        dropDelegate(itemOver);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        var obj = eventData.pointerDrag;
        if (obj == null)
            return;

        DataMenuItem dataMenuItem = obj.GetComponent<DataMenuItem>();
        if (dataMenuItem == null)
            return;

        itemOver = dataMenuItem;
        itemOver.PreventDrop(true);
        Highlight();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Unhighlight();

        if (itemOver != null)
        {
            itemOver.PreventDrop(false);
            itemOver = null;
        }
    }

    public void Highlight()
    {
        overlay.SetActive(true);
    }

    public void Unhighlight()
    {
        overlay.SetActive(false);
    }
}
