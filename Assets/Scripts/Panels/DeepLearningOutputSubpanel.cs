using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeepLearningOutputSubpanel : Subpanel {

    public Text yText;

    public void Initialize(string attr)
    {
        yText.text = attr;
    }
}
