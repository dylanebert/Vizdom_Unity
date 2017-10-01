using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataLoader : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/data");
        yield return www.Send();

        if (www.error != null)
            Debug.Log(www.error);
        else
        {
            Debug.Log(www.downloadHandler.text);

            byte[] results = www.downloadHandler.data;
        }
    }
}
