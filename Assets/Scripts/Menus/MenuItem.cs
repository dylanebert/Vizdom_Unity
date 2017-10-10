using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    static float MinDragDist = 48f;

    public GameObject target;

    protected Client client;

    MenuItem ghost;
    Vector2 dragStartPos;
    bool isGhost;
    bool preventDrop;

    void Awake()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (ghost == null && !isGhost)
        {
            ghost = Instantiate(this.gameObject, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("Canvas").transform).GetComponent<MenuItem>();
            ghost.SetGhost(eventData);
            dragStartPos = eventData.position;
            ghost.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (ghost != null && !isGhost)
        {
            ghost.transform.Translate(eventData.delta);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (ghost != null && !isGhost)
        {
            if ((eventData.position - dragStartPos).magnitude > MinDragDist && !ghost.preventDrop)
            {
                GameObject panel = Instantiate(target, eventData.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Container").transform);
                EventSystem.current.SetSelectedGameObject(panel);
            }
            Destroy(ghost.gameObject);
            ghost = null;
        }
    }

    public void SetGhost(PointerEventData eventData)
    {
        isGhost = true;
        GetComponent<CanvasGroup>().alpha = .5f;
    }

    public void PreventDrop(bool prevent)
    {
        if (ghost != null)
            ghost.preventDrop = prevent;
    }
}
