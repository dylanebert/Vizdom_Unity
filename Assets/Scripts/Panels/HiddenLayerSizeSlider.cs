using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenLayerSizeSlider : MonoBehaviour {

    static int sizeMin = 1;
    static int sizeMax = 1000;

    public Text sizeText;

    Slider slider;
    int hiddenLayerSize;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        hiddenLayerSize = 10;
        InverseAdjust();
    }

    public void Adjust()
    {
        hiddenLayerSize = Mathf.RoundToInt(Mathf.Lerp(sizeMin, sizeMax, slider.value * slider.value));
        sizeText.text = "Size: " + hiddenLayerSize;
    }

    public void InverseAdjust()
    {
        slider.value = Mathf.Sqrt(Mathf.InverseLerp(sizeMin, sizeMax, hiddenLayerSize));
    }

    public void IncrementSize()
    {
        hiddenLayerSize++;
        InverseAdjust();
    }

    public void DecrementSize()
    {
        hiddenLayerSize--;
        InverseAdjust();
    }

    public int GetHiddenLayerSize()
    {
        return hiddenLayerSize;
    }
}
