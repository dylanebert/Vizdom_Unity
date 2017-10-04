using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour {

    CountryData[] data;

    IEnumerator Start()
    {
        yield return StartCoroutine(GetData());
        Debug.Log(data[0]);
        Debug.Log(data[46]);
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/data_raw");
        yield return www.Send();

        if (www.error != null)
            Debug.Log(www.error);
        else
        {
            List<CountryData> countryData = new List<CountryData>();
            string json = www.downloadHandler.text;
            string[] json_split = json.Split('\n');
            foreach(string s in json_split)
            {
                CountryData cd = new CountryData().createFromJSON(s);
                if(cd != null)
                    countryData.Add(cd);
            }
            data = countryData.ToArray();
        }
    }
}

[System.Serializable]
public class CountryData
{
    public string _id;
    public float agr;
    public float gdp;
    public float gpp;
    public int pop;
    public float co2pp;

    public CountryData createFromJSON(string json)
    {
        return JsonUtility.FromJson<CountryData>(json);
    }

    public override string ToString()
    {
        return "_id: " + _id + ", agr: " + agr + ", gdp: " + gdp + ", gpp: " + gpp + ", pop: " + pop + ", co2pp: " + co2pp;
    }
}