using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLearningLayersSubpanel : Subpanel {

    static float HiddenLayerStep = 192f;

    public GameObject hiddenLayerObj;
    public GameObject connectionObj;

    List<Connection> connections;
    List<HiddenLayer> hiddenLayers;

    private void Awake()
    {
        connections = new List<Connection>();
        hiddenLayers = new List<HiddenLayer>();
    }

    private IEnumerator Start()
    {
        for(int i = 0; i < 1; i++)
            yield return StartCoroutine(AddHiddenLayer());
    }

    public void InvokeAddHiddenLayer(int index = 0)
    {
        StartCoroutine(AddHiddenLayer(index));
    }

    public IEnumerator AddHiddenLayer(int index = 0)
    {
        ClearConnections();

        //Instantiate new layer
        HiddenLayer hiddenLayer = Instantiate(hiddenLayerObj, this.transform).GetComponent<HiddenLayer>();
        hiddenLayers.Insert(index, hiddenLayer);
        hiddenLayer.enabled = false;
        hiddenLayer.Initialize(this);

        //Enlarge panel if more hidden layers than 1
        if (hiddenLayers.Count > 1)
        {
            RectTransform parentPanel = transform.parent.parent as RectTransform;
            StartCoroutine(parentPanel.GetComponent<RectUtil>().AnimatedResize(parentPanel.sizeDelta + Vector2.right * HiddenLayerStep, .5f));
        }

        //Place new layer at the indicated index
        if (index > 0)
        {
            ((RectTransform)hiddenLayer.transform).anchoredPosition = ((RectTransform)hiddenLayers[index - 1].transform).anchoredPosition + Vector2.right * HiddenLayerStep / 2f;
        } else if(index < hiddenLayers.Count - 1)
        {
            ((RectTransform)hiddenLayer.transform).anchoredPosition = ((RectTransform)hiddenLayers[index + 1].transform).anchoredPosition + Vector2.left * HiddenLayerStep / 2f;
        }

        //Create dictionary of start and end positions for other layers to animate to
        Dictionary<HiddenLayer, Vector2> startPositions = new Dictionary<HiddenLayer, Vector2>();
        Dictionary<HiddenLayer, Vector2> targetPositions = new Dictionary<HiddenLayer, Vector2>();
        for(int i = 0; i < hiddenLayers.Count; i++)
        {
            HiddenLayer layer = hiddenLayers[i];
            startPositions.Add(layer, ((RectTransform)layer.transform).anchoredPosition);
            if(i < index)
            {
                targetPositions.Add(layer, ((RectTransform)layer.transform).anchoredPosition + Vector2.left * HiddenLayerStep / 2f);
            } else if (i > index)
            {
                targetPositions.Add(layer, ((RectTransform)layer.transform).anchoredPosition + Vector2.right * HiddenLayerStep / 2f);
            } else
            {
                targetPositions.Add(layer, startPositions[layer]);
            }
        }

        //Move layers to target positions
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            foreach(HiddenLayer layer in hiddenLayers)
            {
                ((RectTransform)layer.transform).anchoredPosition = Vector2.Lerp(startPositions[layer], targetPositions[layer], v);
            }
            yield return null;
        }

        hiddenLayer.enabled = true;

        yield return StartCoroutine(DrawConnections());
    }

    public IEnumerator RemoveHiddenLayer(HiddenLayer hiddenLayer)
    {
        ClearConnections();

        hiddenLayers.Remove(hiddenLayer);
        Destroy(hiddenLayer.gameObject);

        yield return StartCoroutine(DrawConnections());
    }

    void ClearConnections()
    {
        Stack<Connection> connectionStack = new Stack<Connection>(connections);
        while (connectionStack.Count > 0)
        {
            Destroy(connectionStack.Pop().gameObject);
        }
        connections.Clear();
    }

    IEnumerator DrawConnections()
    {
        for(int i = 0; i <= hiddenLayers.Count; i++)
        {
            Connection connection = Instantiate(connectionObj, this.transform).GetComponent<Connection>();
            ((RectTransform)connection.transform).anchoredPosition = ((RectTransform)hiddenLayers[0].transform).anchoredPosition + Vector2.right * HiddenLayerStep * (i - .5f);
            connections.Add(connection);
            yield return new WaitForSeconds(.05f);
        }

        for (int i = 0; i < connections.Count; i++)
        {
            connections[i].Initialize(this, i);
            connections[i].transform.SetAsFirstSibling();
        }
    }
}
