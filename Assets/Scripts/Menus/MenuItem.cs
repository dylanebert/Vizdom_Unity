using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    protected static float MinDragDist = 48f;

    public GameObject target;

    [HideInInspector]
    public bool preventDrop;

    protected Client client;
    protected MenuItem ghost;
    protected Vector2 dragStartPos;
    protected bool isGhost;

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
            EventSystem.current.SetSelectedGameObject(ghost.gameObject);
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
