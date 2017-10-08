using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    static float MinDragDist = 48f;

    public GameObject target;

    MenuItem ghost;
    Vector2 dragStartPos;
    bool isGhost;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ghost == null && !isGhost)
        {
            ghost = Instantiate(this.gameObject, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<MenuItem>();
            ghost.SetAsGhost(eventData);
            dragStartPos = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghost != null && !isGhost)
        {
            ghost.transform.Translate(eventData.delta);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghost != null && !isGhost)
        {
            if ((eventData.position - dragStartPos).magnitude > MinDragDist)
            {
                GameObject panel = Instantiate(target, eventData.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Container").transform);
                EventSystem.current.SetSelectedGameObject(panel);
            }
            Destroy(ghost.gameObject);
            ghost = null;
        }
    }

    public void SetAsGhost(PointerEventData eventData)
    {
        isGhost = true;
        GetComponent<CanvasGroup>().alpha = .5f;
    }
}
