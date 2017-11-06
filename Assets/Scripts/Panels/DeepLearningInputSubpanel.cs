using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLearningInputSubpanel : Subpanel {

    static float startY = 48f;
    static float yStep = 64f;

    public GameObject inputFeatureBoxObj;

    CanvasGroup canvasGroup;
    List<InputFeatureBox> inputFeatureBoxes;
    DataMenuItem itemOver;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inputFeatureBoxes = new List<InputFeatureBox>();
    }

    private void Start()
    {
        StartCoroutine(AddInputFeature());
    }

    //Create an input feature box and animate downward
    public IEnumerator AddInputFeature(string attr = null)
    {
        canvasGroup.blocksRaycasts = false;
        InputFeatureBox inputFeatureBox = Instantiate(inputFeatureBoxObj, this.transform).GetComponent<InputFeatureBox>();
        inputFeatureBox.SetPanel(this);
        if (attr != null)
            inputFeatureBox.Initialize(attr);
        inputFeatureBoxes.Add(inputFeatureBox);
        RectTransform inputFeatureBoxRect = inputFeatureBox.transform as RectTransform;
        CanvasGroup inputFeatureBoxCanvasGroup = inputFeatureBox.GetComponent<CanvasGroup>();
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * 2f;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            inputFeatureBoxRect.anchoredPosition = Vector2.Lerp(Vector2.down * startY + Vector2.down * (inputFeatureBoxes.Count - 2) * yStep, Vector2.down * startY + Vector2.down * (inputFeatureBoxes.Count - 1) * yStep, v);
            inputFeatureBoxCanvasGroup.alpha = t;
            yield return null;
        }
        inputFeatureBoxCanvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public IEnumerator RemoveInputFeature(InputFeatureBox box)
    {
        int index = inputFeatureBoxes.IndexOf(box);
        canvasGroup.blocksRaycasts = false;

        Dictionary<InputFeatureBox, Vector2> startPositions = new Dictionary<InputFeatureBox, Vector2>();
        Dictionary<InputFeatureBox, Vector2> targetPositions = new Dictionary<InputFeatureBox, Vector2>();
        for(int i = 0; i < inputFeatureBoxes.Count; i++)
        {
            if(i < index)
            {
                startPositions.Add(inputFeatureBoxes[i], ((RectTransform)inputFeatureBoxes[i].transform).anchoredPosition);
                targetPositions.Add(inputFeatureBoxes[i], startPositions[inputFeatureBoxes[i]]);
            } else if (i > index)
            {
                startPositions.Add(inputFeatureBoxes[i], ((RectTransform)inputFeatureBoxes[i].transform).anchoredPosition);
                targetPositions.Add(inputFeatureBoxes[i], startPositions[inputFeatureBoxes[i]] + Vector2.up * yStep);
            }    
        }

        inputFeatureBoxes.Remove(box);

        CanvasGroup boxCanvasGroup = box.GetComponent<CanvasGroup>();
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime * 2f;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            foreach(InputFeatureBox inputFeatureBox in inputFeatureBoxes)
            {
                ((RectTransform)inputFeatureBox.transform).anchoredPosition = Vector2.Lerp(startPositions[inputFeatureBox], targetPositions[inputFeatureBox], v);
            }
            boxCanvasGroup.alpha = 1 - t;
            yield return null;
        }

        canvasGroup.blocksRaycasts = true;
        Destroy(box.gameObject);
    }
}
