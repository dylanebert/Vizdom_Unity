using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*Parent panel for deep learning
 * Functionality embedded in subpanels*/
public class DeepLearningPanel : Panel, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    static Vector2 FullSize = new Vector2(768, 384);

    DataMenuItem itemOver;

    public void OnDrop(PointerEventData eventData)
    {
        if (itemOver == null)
            return;

        StartCoroutine(InitializeOnAttribute(itemOver.GetAttribute()));

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

    IEnumerator InitializeOnAttribute(string attribute)
    {
        centerText.enabled = false;
        yield return StartCoroutine(AnimatedResize(FullSize, .25f));
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("Background").GetComponent<Panning>().PanToPanel(rect, .25f));
    }
}