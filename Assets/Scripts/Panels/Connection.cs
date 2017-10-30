using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Connection : MonoBehaviour, IPointerClickHandler {

    DeepLearningLayersSubpanel panel;
    int index;
    bool initialized;

    public void Initialize(DeepLearningLayersSubpanel panel, int index)
    {
        this.panel = panel;
        this.index = index;
        initialized = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!initialized) return;

        panel.InvokeAddHiddenLayer(index);
    }
}
