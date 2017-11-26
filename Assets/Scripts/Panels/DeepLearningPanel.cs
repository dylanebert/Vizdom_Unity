using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject overlay;
    public Sprite playSprite;
    public Sprite stopSprite;

    [HideInInspector]
    public int batchSize = 100;
    [HideInInspector]
    public Client client;

    List<RectUtil> overTrainingFocus;
    Image trainPlayButtonImage;
    Droppable droppable;
    bool training;

    public override void Awake()
    {
        base.Awake();
        droppable = GetComponent<Droppable>();
        droppable.dropDelegate += OnDrop;

        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        overTrainingFocus = new List<RectUtil>();
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
        DeepLearningTopSubpanel topSubpanel = Instantiate(topSubpanelObj, main).GetComponent<DeepLearningTopSubpanel>();
        topSubpanel.Initialize(this);
        overTrainingFocus.Add(topSubpanel.playButtonContainer.GetComponent<RectUtil>());
        overTrainingFocus.Add(topSubpanel.accuracyContainer.GetComponent<RectUtil>());
        trainPlayButtonImage = topSubpanel.playButtonImage;
    }

    public IEnumerator Train()
    {
        if (training)
            yield break;
        NeuralNetworkProperties properties = new NeuralNetworkProperties(batchSize);
        FocusTraining();
        yield return StartCoroutine(client.deepLearningClient.DeepLearning(properties));
        DefocusTraining();
    }

    void FocusTraining()
    {
        training = true;
        overlay.SetActive(true);
        foreach(RectUtil rectUtil in overTrainingFocus)
        {
            rectUtil.MoveToForeground(this.transform);
        }
        trainPlayButtonImage.sprite = stopSprite;
    }

    void DefocusTraining()
    {
        overlay.SetActive(false);
        foreach (RectUtil rectUtil in overTrainingFocus)
        {
            rectUtil.RestoreParent();
        }
        training = false;
        trainPlayButtonImage.sprite = playSprite;
    }
}