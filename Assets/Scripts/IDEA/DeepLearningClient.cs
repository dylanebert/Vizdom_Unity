using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class DeepLearningClient : MonoBehaviour {

    public delegate void AccuracyDelegate(float accuracy);
    public AccuracyDelegate accuracyDelegate;

    NetworkStream netStream;

    public void Initialize(NetworkStream netStream)
    {
        this.netStream = netStream;
    }

    public IEnumerator DeepLearning(NeuralNetworkProperties properties)
    {
        string json = JsonUtility.ToJson(properties);
        Debug.Log(json);
        byte[] data = System.Text.Encoding.UTF8.GetBytes("ni" + json);
        int bytes;
        string responseData = "";
        netStream.Write(data, 0, data.Length);

        ReportDeepLearningAccuracy();

        Debug.Log("Begin training...");
        for (int i = 0; i < 1000; i++)
        {
            data = System.Text.Encoding.UTF8.GetBytes("nt");
            netStream.Write(data, 0, data.Length);

            data = new byte[1024];
            bytes = netStream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
            float loss = float.Parse(responseData);
            //Debug.Log(loss);

            if (i % 10 == 0)
            {
                ReportDeepLearningAccuracy();
            }

            yield return null;
        }
        Debug.Log("Finished");

        ReportDeepLearningAccuracy();
    }

    void ReportDeepLearningAccuracy()
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes("na");
        netStream.Write(data, 0, data.Length);
        data = new byte[1024];
        int bytes = netStream.Read(data, 0, data.Length);
        string responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
        float accuracy = float.Parse(responseData);
        accuracyDelegate(accuracy);
    }
}

[System.Serializable]
public class NeuralNetworkProperties
{
    public int batch_size;

    public NeuralNetworkProperties(int batchSize = 100)
    {
        this.batch_size = batchSize;
    }
}