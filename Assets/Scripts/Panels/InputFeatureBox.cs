using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*Box in which deep learning input features are dropped*/
[RequireComponent(typeof(Droppable))]
public class InputFeatureBox : MonoBehaviour {

    public Image box;
    public Text text;

    Droppable droppable;

    private void Awake()
    {
        droppable = GetComponent<Droppable>();
        droppable.dropDelegate += OnDrop;
    }

    public void Initialize(string attr)
    {
        text.text = attr;
        text.color = Palette.PrimaryTextColor;
        box.color = Palette.DefaultBGColor;
        box.sprite = null;
        droppable.enabled = false;
        StartCoroutine(this.transform.parent.GetComponent<DeepLearningInputSubpanel>().AddInputFeature());
    }

    public void OnDrop(DataMenuItem itemOver)
    {
        Initialize(itemOver.GetAttribute());
    }
}
