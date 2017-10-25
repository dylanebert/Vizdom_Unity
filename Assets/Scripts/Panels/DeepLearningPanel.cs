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

    public GameObject[] subpanels;

    DataMenuItem itemOver;
    bool initialized;

    public void OnDrop(PointerEventData eventData)
    {
        if (initialized || itemOver == null)
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

        itemOver = dataMenuItem;
        itemOver.PreventDrop(true);
        if(!initialized)
            Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!initialized)
            Unhighlight();

        if (itemOver != null)
        {
            itemOver.PreventDrop(false);
            itemOver = null;
        }
    }

    IEnumerator InitializeOnAttribute(string attribute)
    {
        initialized = true;
        centerText.enabled = false;
        resizeButton.SetActive(false);
        yield return StartCoroutine(AnimatedResize(FullSize, .25f));
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("Background").GetComponent<Panning>().PanToPanel(rect, .25f));
        InitializeSubpanels();
    }

    void InitializeSubpanels()
    {
        foreach(GameObject subpanel in subpanels)
        {
            Instantiate(subpanel, main);
        }
    }
}