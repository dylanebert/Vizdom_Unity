using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Net;
using System.Net.Sockets;
using System.IO;

/*Communicates with server (IDEA)*/
public class Client : MonoBehaviour {

    public CollapseMenu menu;

    static string Host = "127.0.0.1";
    static int Port = 8000;

    TcpClient tcpSocket;
    NetworkStream netStream;
    
    bool socketReady;

    private void Start()
    {
        try
        {
            tcpSocket = new TcpClient(Host, Port);
            netStream = tcpSocket.GetStream();
            socketReady = true;
            GetAttributes();
        } catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void GetAttributes()
    {
        if (!socketReady) return;

        try
        {
            Debug.Log("Fetching attributes...");

            byte[] data = System.Text.Encoding.ASCII.GetBytes("ga");
            netStream.Write(data, 0, data.Length);

            data = new byte[1024];
            string responseData = "";
            int bytes = netStream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);

            foreach(string attribute in ParseJSONStringArray(responseData))
            {
                menu.AddDataMenuItem(attribute);
            }

            Debug.Log("Finished");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnApplicationQuit()
    {
        tcpSocket.Close();
    }

    List<string> ParseJSONStringArray(string json)
    {
        List<string> attributes = new List<string>();
        JSONArray attributeArray = JSON.Parse(json).AsArray;
        foreach (JSONNode attribute in attributeArray)
        {
            attributes.Add(attribute.Value);
        }
        return attributes;
    }
}