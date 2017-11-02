using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Parent panel for deep learning
 * Functionality embedded in subpanels*/
[RequireComponent(typeof(Droppable))]
public class DeepLearningPanel : Panel
{
    static Vector2 FullSize = new Vector2(576, 384);

    public GameObject inputSubpanelObj;
    public GameObject layersSubpanelObj;
    public GameObject outputSubpanelObj;
    public GameObject topSubpanelObj;

    Droppable droppable;

    public override void Awake()
    {
        base.Awake();
        droppable = GetComponent<Droppable>();
        droppable.dropDelegate += OnDrop;
    }

    public void OnDrop(DataMenuItem itemOver)
    {
        StartCoroutine(InitializeOnAttribute(itemOver.GetAttribute()));
    }

    IEnumerator InitializeOnAttribute(string attr)
    {
        droppable.enabled = false;
        centerText.enabled = false;
        yield return StartCoroutine(GetComponent<RectUtil>().AnimatedResize(FullSize, .25f));
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("Background").GetComponent<Panning>().PanToPanel(rect, .25f));
        InitializeSubpanels(attr);
    }

    void InitializeSubpanels(string attr)
    {
        Instantiate(inputSubpanelObj, main);
        Instantiate(layersSubpanelObj, main);
        Instantiate(outputSubpanelObj, main).GetComponent<DeepLearningOutputSubpanel>().Initialize(attr);
        Instantiate(topSubpanelObj, main);
    }
}