using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mousechecker : MonoBehaviour
{
    [SerializeField]
    GameObject text;

    [SerializeField]
    List<GameObject> gameObjects;

    float timer = 0;
    GameObject selected;
    void Start()
    {
        selected = gameObjects[0];
    }
    void Update()
    {
        if (transform.localPosition == Vector3.zero)
        {
            float dist = 99999;
            foreach (var item in gameObjects)
            {
                if (dist > Vector3.Distance(item.GetComponent<RectTransform>().position, Input.mousePosition))
                {
                    dist = Vector3.Distance(item.GetComponent<RectTransform>().position, Input.mousePosition);
                    selected = item;
                }
                item.GetComponent<RectTransform>().localScale = new Vector3(0.6771638f, 0.6771638f, 0.6771638f);
            }
            selected.GetComponent<RectTransform>().localScale = new Vector3(0.9771638f, 0.9771638f, 0.9771638f);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            transform.localPosition = Vector3.zero;
            timer = 0;
        }
        else
        {
            if (transform.localPosition == Vector3.zero)
            {
                timer = 3;
            }
            transform.localPosition = new Vector3(0, 2000, 0);
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            text.GetComponent<RectTransform>().position = Input.mousePosition;
            text.GetComponent<TextMeshProUGUI>().text = $"selected emote\n\n[{selected.name}]";
        }
        else
        {
            text.GetComponent<RectTransform>().position = new Vector3(0, 2000, 0);
        }
    }
}
