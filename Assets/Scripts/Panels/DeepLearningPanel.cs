using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Parent panel for deep learning
 * Functionality embedded in subpanels*/
public class DeepLearningPanel : Panel
{
    static Vector2 FullSize = new Vector2(576, 384);

    public GameObject inputSubpanelObj;
    public GameObject layersSubpanelObj;
    public GameObject outputSubpanelObj;
    public GameObject topSubpanelObj;

    public GameObject overlay;
    public InputFeatureBox trainingInputBox;
    public InputFeatureBox trainingAnswerBox;
    public InputFeatureBox testingInputBox;
    public InputFeatureBox testingAnswerBox;
    public Selectable confirmButton;
    public Sprite playSprite;
    public Sprite stopSprite;

    [HideInInspector]
    public int batchSize = 100;
    [HideInInspector]
    public Client client;
    [HideInInspector]
    public bool training;

    List<RectUtil> overTrainingFocus;
    Image trainPlayButtonImage;
    string trainingInput;
    string trainingAnswer;
    string testingInput;
    string testingAnswer;

    public override void Awake()
    {
        base.Awake();

        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        overTrainingFocus = new List<RectUtil>();
        trainingInputBox.attributeDelegate += SetAttribute;
        trainingAnswerBox.attributeDelegate += SetAttribute;
        testingInputBox.attributeDelegate += SetAttribute;
        testingAnswerBox.attributeDelegate += SetAttribute;
        trainingInputBox.resetDelegate += ResetAttribute;
        trainingAnswerBox.resetDelegate += ResetAttribute;
        testingInputBox.resetDelegate += ResetAttribute;
        testingAnswerBox.resetDelegate += ResetAttribute;
    }

    void SetAttribute(InputFeatureBox obj, string attr)
    {
        switch(obj.flag)
        {
            case "train_input":
                trainingInput = attr;
                break;
            case "train_answer":
                trainingAnswer = attr;
                break;
            case "test_input":
                testingInput = attr;
                break;
            case "test_answer":
                testingAnswer = attr;
                break;
            default:
                break;
        }

        if (trainingInput == null || trainingAnswer == null || testingInput == null || testingAnswer == null)
        {
            confirmButton.interactable = false;
        } else
        {
            confirmButton.interactable = true;
        }
    }

    void ResetAttribute(InputFeatureBox obj)
    {
        switch (obj.flag)
        {
            case "train_input":
                trainingInput = null;
                break;
            case "train_answer":
                trainingAnswer = null;
                break;
            case "test_input":
                testingInput = null;
                break;
            case "test_answer":
                testingAnswer = null;
                break;
            default:
                break;
        }

        confirmButton.interactable = false;
    }

    public void Initialize()
    {
        StartCoroutine(InitializeCoroutine());
    }

    IEnumerator InitializeCoroutine()
    {
        centerText.enabled = false;
        yield return StartCoroutine(GetComponent<RectUtil>().AnimatedResize(FullSize, .25f));
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("Background").GetComponent<Panning>().PanToPanel(rect, .25f));
        InitializeSubpanels();
    }

    void InitializeSubpanels()
    {
        DeepLearningInputSubpanel inputSubpanel = Instantiate(inputSubpanelObj, main).GetComponent<DeepLearningInputSubpanel>();
        StartCoroutine(inputSubpanel.Initialize(trainingInput));
        Instantiate(layersSubpanelObj, main);
        Instantiate(outputSubpanelObj, main).GetComponent<DeepLearningOutputSubpanel>().Initialize(trainingAnswer);
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
        NeuralNetworkProperties properties = new NeuralNetworkProperties(batchSize, trainingInput, trainingAnswer, testingInput, testingAnswer);
        FocusTraining();
        yield return StartCoroutine(client.deepLearningClient.DeepLearning(properties));
        DefocusTraining();
    }

    public void FocusTraining()
    {
        training = true;
        overlay.SetActive(true);
        foreach(RectUtil rectUtil in overTrainingFocus)
        {
            rectUtil.MoveToForeground(this.transform);
        }
        trainPlayButtonImage.sprite = stopSprite;
    }

    public void DefocusTraining()
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