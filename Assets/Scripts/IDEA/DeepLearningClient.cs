using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

public class DeepLearningClient : MonoBehaviour {

    public delegate void AccuracyDelegate(float accuracy);
    public AccuracyDelegate accuracyDelegate;

    NetworkStream netStream;
    bool breakFlag;

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
        int i = 0;
        while(true)
        {
            if (breakFlag)
            {
                breakFlag = false;
                break;
            }

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

            i += 1;
            yield return null;
        }
        Debug.Log("Finished");

        ReportDeepLearningAccuracy();
    }

    public void CancelDeepLearning()
    {
        breakFlag = true;
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
    public string train_input_filename;
    public string train_answer_filename;
    public string test_input_filename;
    public string test_answer_filename;

    public NeuralNetworkProperties(int batchSize, string trainingInput, string trainingAnswer, string testingInput, string testingAnswer)
    {
        this.batch_size = batchSize;
        this.train_input_filename = trainingInput;
        this.train_answer_filename = trainingAnswer;
        this.test_input_filename = testingInput;
        this.test_answer_filename = testingAnswer;
    }
}