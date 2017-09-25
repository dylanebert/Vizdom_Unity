using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFollow : MonoBehaviour {

    public Transform target;
    public float speed = 1f;

    Vector2 offset;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, (Vector2)target.position + offset, Time.deltaTime * speed);
    }
}
