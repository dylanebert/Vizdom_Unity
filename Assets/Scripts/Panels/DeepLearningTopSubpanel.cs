using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeepLearningTopSubpanel : Subpanel {

    public GameObject playButtonContainer;
    public GameObject accuracyContainer;
    public Image playButtonImage;
    public Text batchSizeText;
    public Text accuracyText;

    DeepLearningPanel panel;

    public void Initialize(DeepLearningPanel panel)
    {
        this.panel = panel;
        panel.client.deepLearningClient.accuracyDelegate += UpdateAccuracy;
    }

    private void OnGUI()
    {
        batchSizeText.text = panel.batchSize.ToString();
    }

    private void OnDestroy()
    {
        panel.client.deepLearningClient.accuracyDelegate -= UpdateAccuracy;
    }

    public void UpdateAccuracy(float accuracy)
    {
        accuracyText.text = accuracy.ToString(".000");
    }

    public void Play()
    {
        StartCoroutine(panel.Train());
    }    

    public void IncreaseBatchSize()
    {
        panel.batchSize++;
    }

    public void DecreaseBatchSize()
    {
        panel.batchSize = Mathf.Max(1, panel.batchSize - 1);
    }

    public void DragBatchSize(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        panel.batchSize = Mathf.Max(1, Mathf.RoundToInt(panel.batchSize + pointerEventData.delta.x));
    }
}
