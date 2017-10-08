using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class Client : MonoBehaviour {

    public CollapseMenu menu;

    Dictionary<string, Dictionary<string, string>> data;
    List<string> attributes;

    IEnumerator Start()
    {
        yield return StartCoroutine(GetData());
        PopulateMenu();
    }

    void PopulateMenu()
    {
        foreach (string attribute in attributes)
        {
            menu.AddDataMenuItem(attribute);
        }
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/data_raw");
        yield return www.Send();

        if (www.error != null)
            Debug.Log(www.error);
        else
        {
            data = new Dictionary<string, Dictionary<string, string>>();
            attributes = new List<string>();
            string json = www.downloadHandler.text;
            string[] json_split = json.Split('\n');
            foreach(string s in json_split)
            {
                JSONObject jsonObj = JSON.Parse(s).AsObject;
                Dictionary<string, string> entries = new Dictionary<string, string>();
                foreach(string key in jsonObj.Keys)
                {
                    if(!attributes.Contains(key))
                    {
                        attributes.Add(key);
                    }
                    entries.Add(key, jsonObj[key]);
                }
                data.Add(jsonObj[0], entries);
            }
        }
    }
}