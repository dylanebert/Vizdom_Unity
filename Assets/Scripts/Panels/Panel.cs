using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*Parent class for all panels
 * Includes high level manipulations like highlight and selection*/
 [RequireComponent(typeof(Selectable))]
public class Panel : MonoBehaviour, ISelectHandler
{
    public Text centerText;
    public RectTransform main;

    protected RectTransform rect;

    public virtual void Awake()
    {
        rect = transform as RectTransform;
    }

    public virtual void Start()
    {
        rect.anchoredPosition += new Vector2(-rect.rect.width / 2f, rect.rect.height / 2f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
