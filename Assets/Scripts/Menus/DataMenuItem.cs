using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataMenuItem : MenuItem {

    string attribute;

    public void Initialize(string attribute)
    {
        this.attribute = attribute;
        this.gameObject.name = attribute;
        GetComponentInChildren<Text>().text = attribute;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (ghost != null && !isGhost)
        {
            if ((eventData.position - dragStartPos).magnitude > MinDragDist && !ghost.preventDrop)
            {
                GameObject panel = Instantiate(target, eventData.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Container").transform);
                panel.GetComponent<AttrPanel>().InitializeWithAttribute(attribute);
                EventSystem.current.SetSelectedGameObject(panel);
            }
            Destroy(ghost.gameObject);
            ghost = null;
        }
    }

    public string GetAttribute()
    {
        return attribute;
    }
}