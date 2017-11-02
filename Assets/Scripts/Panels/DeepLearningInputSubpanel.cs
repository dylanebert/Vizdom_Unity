using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLearningInputSubpanel : Subpanel {

    static float startY = 48f;
    static float yStep = 64f;

    public GameObject inputFeatureBoxObj;

    List<InputFeatureBox> inputFeatureBoxes;
    DataMenuItem itemOver;

    private void Awake()
    {
        inputFeatureBoxes = new List<InputFeatureBox>();
    }

    private void Start()
    {
        StartCoroutine(AddInputFeature());
    }

    //Create an input feature box and animate downward
    public IEnumerator AddInputFeature(string attr = null)
    {
        InputFeatureBox inputFeatureBox = Instantiate(inputFeatureBoxObj, this.transform).GetComponent<InputFeatureBox>();
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
    }
}
