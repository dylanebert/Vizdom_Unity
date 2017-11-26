using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeepLearningTopSubpanel : Subpanel {

    public Text batchSizeText;

    DeepLearningPanel panel;

    public void Initialize(DeepLearningPanel panel)
    {
        this.panel = panel;
    }

    private void OnGUI()
    {
        batchSizeText.text = panel.batchSize.ToString();
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
