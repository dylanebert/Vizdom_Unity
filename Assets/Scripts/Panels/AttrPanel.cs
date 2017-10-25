using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrPanel : Panel {

	public void InitializeWithAttribute(string attr)
    {
        centerText.text = attr;
    }
}
