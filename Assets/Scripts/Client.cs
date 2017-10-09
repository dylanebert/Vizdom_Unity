using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;

public class Client : MonoBehaviour {

    public CollapseMenu menu;

    List<string> attributes;

    IEnumerator Start()
    {
        yield return StartCoroutine(GetAttributes());
        PopulateMenu();
    }

    void PopulateMenu()
    {
        foreach (string attribute in attributes)
        {
            menu.AddDataMenuItem(attribute);
        }
    }

    IEnumerator GetAttributes()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/data_raw");
        yield return www.Send();

        if (www.error != null)
            Debug.Log(www.error);
        else
        {
            attributes = new List<string>();
            string json = www.downloadHandler.text;
            string[] json_split = json.Split('\n');
            foreach(string s in json_split)
            {
                JSONObject jsonObj = JSON.Parse(s).AsObject;
                foreach(string key in jsonObj.Keys)
                {
                    if(!attributes.Contains(key))
                    {
                        attributes.Add(key);
                    }
                }
            }
        }
    }
}