using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLearningTopSubpanel : Subpanel {

   Client client;

    private void Awake()
    {
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
    }

    public void Play()
    {
        StartCoroutine(PlayCoroutine());
    }

    IEnumerator PlayCoroutine()
    {
        yield return null;
        //yield return StartCoroutine(client.DeepLearning());
    }
}
