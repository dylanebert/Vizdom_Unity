using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CollapseMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    static float Padding = 5f;
    static float MenuItemSize = 48f;
    static float AnimationTime = .25f;

    public CanvasGroup childrenGroup;
    public GameObject dataMenuItemObj;

    List<RectTransform> children;

    void Awake()
    {
        children = new List<RectTransform>();
        foreach(RectTransform rect in childrenGroup.GetComponentsInChildren<RectTransform>())
        {
            if (rect.GetComponent<MenuItem>() != null)
                children.Add(rect);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Expand());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(Collapse());
    }

    public void AddDataMenuItem(string id)
    {
        DataMenuItem dmi = Instantiate(dataMenuItemObj, childrenGroup.transform).GetComponent<DataMenuItem>();
        children.Add(dmi.transform as RectTransform);
        dmi.Initialize(id);
    }

    IEnumerator Expand() {
        childrenGroup.gameObject.SetActive(true);

        Dictionary<RectTransform, Vector2> targetPositions = new Dictionary<RectTransform, Vector2>();
        for(int i = 0; i < children.Count; i++)
        {
            children[i].anchoredPosition = Vector2.right * MenuItemSize / 2f;
            children[i].gameObject.SetActive(true);
            Vector2 targetPosition = new Vector2(i * (MenuItemSize + Padding) + Padding + MenuItemSize / 2f, 0);
            targetPositions.Add(children[i], targetPosition);
        }

        float timer = 0f;
        while(timer < AnimationTime)
        {
            timer += Time.deltaTime;
            float v = Mathf.Sin(Mathf.PI * timer / AnimationTime / 2f);
            foreach(RectTransform rect in targetPositions.Keys)
            {
                rect.anchoredPosition = Vector2.Lerp(Vector2.zero, targetPositions[rect], v);
            }
            childrenGroup.alpha = v;
            yield return null;
        }
    }

    IEnumerator Collapse() {
        Dictionary<RectTransform, Vector2> startPositions = new Dictionary<RectTransform, Vector2>();
        foreach(RectTransform child in children)
        {
            startPositions.Add(child, child.anchoredPosition);
        }

        float timer = 0f;
        while (timer < AnimationTime)
        {
            timer += Time.deltaTime;
            float v = Mathf.Sin(Mathf.PI * (1 - timer / AnimationTime) / 2f);
            foreach (RectTransform rect in startPositions.Keys)
            {
                rect.anchoredPosition = Vector2.Lerp(Vector2.right * MenuItemSize / 2f, startPositions[rect], v);
            }
            childrenGroup.alpha = v;
            yield return null;
        }

        childrenGroup.gameObject.SetActive(false);
    }
}
