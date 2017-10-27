using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*Allows touch panning in the view
 * Works by moving the "container" which contains all panels*/
public class Panning : MonoBehaviour, IDragHandler {

    public RectTransform container;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData data)
    {        
        if(Input.touchCount == 1)
            container.Translate(data.delta);
    }

    public IEnumerator PanToPanel(RectTransform panel, float time)
    {
        Vector2 startPos = container.anchoredPosition;
        Vector2 targetPos = -panel.anchoredPosition + new Vector2(-panel.rect.width / 2f, panel.rect.height / 2f);
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime / time;
            float v = Mathf.Sin(Mathf.PI * t / 2f);
            container.anchoredPosition = Vector2.Lerp(startPos, targetPos, v);
            yield return null;
        }
    }
}
