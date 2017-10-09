﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataMenuItem : MenuItem {

    string attribute;

    public void Initialize(string attribute)
    {
        this.attribute = attribute;
        this.gameObject.name = attribute;
        GetComponentInChildren<Text>().text = attribute;
    }

    public string GetAttribute()
    {
        return attribute;
    }
}