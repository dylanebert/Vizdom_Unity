using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Linq;

/*Communicates with server (IDEA)*/
public class Client : MonoBehaviour {

    public CollapseMenu menu;

    List<string> attributes;

    IEnumerator Start()
    {
        yield return StartCoroutine(GetAttributes());
        PopulateMenu();

    }

    //Populate side menu with attributes from server
    void PopulateMenu()
    {
        foreach (string attribute in attributes)
        {
            menu.AddDataMenuItem(attribute);
        }
    }

    IEnumerator DeepLearning()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/");
    }

    //Request list of attributes from server
    IEnumerator GetAttributes()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/attributes");
        yield return www.Send();

        if (www.error != null)
            Debug.Log(www.error);
        else
        {
            attributes = new List<string>();
            string json = www.downloadHandler.text;
            JSONArray attributeArray = JSON.Parse(json).AsArray;
            foreach(JSONNode attribute in attributeArray)
            {
                Debug.Log(attribute.Value);
                attributes.Add(attribute.Value);
            }
        }
    }
}