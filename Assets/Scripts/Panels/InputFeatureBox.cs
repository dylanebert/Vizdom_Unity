using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/*Box in which deep learning input features are dropped*/
[RequireComponent(typeof(Droppable))]
public class InputFeatureBox : MonoBehaviour {

    public delegate void AttributeDelegate(InputFeatureBox obj, string attr);
    public AttributeDelegate attributeDelegate;

    public Image box;
    public Text title;
    public string flag;

    Droppable droppable;
    Grayout grayout;
    RectUtil rectUtil;
    bool focused;
    bool initialized;

    private void Awake()
    {
        grayout = GameObject.FindGameObjectWithTag("Grayout").GetComponent<Grayout>();
        droppable = GetComponent<Droppable>();
        droppable.dropDelegate += OnDrop;
        rectUtil = GetComponent<RectUtil>();
    }

    public void Initialize(string attr)
    {
        title.text = attr;
        title.color = Palette.PrimaryTextColor;
        box.color = Palette.DefaultBGColor;
        box.sprite = null;
        droppable.enabled = false;
        initialized = true;
        if (attributeDelegate != null)
            attributeDelegate(this, attr);
    }

    public void OnDrop(DataMenuItem itemOver)
    {
        Initialize(itemOver.GetAttribute());
    }
}
