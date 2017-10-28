using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {

    static float pinchZoomSpeed = .002f;
    static float scrollZoomSpeed = 1f;

    public RectTransform container;

    private void Update()
    {
        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            container.localScale -= Vector3.one * deltaMagnitudeDiff * pinchZoomSpeed;
            container.localScale = Vector3.Max(container.localScale, Vector3.one * .1f);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0)
        {
            container.localScale += Vector3.one * scroll * scrollZoomSpeed;
            container.localScale = Vector3.Max(container.localScale, Vector3.one * .1f);
        }
    }
}
