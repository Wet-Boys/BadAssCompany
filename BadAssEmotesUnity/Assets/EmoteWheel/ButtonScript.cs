using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject emoteInQuestion;
    void Start()
    {
        
    }

    public void PickNewEmote()
    {
        emoteInQuestion.GetComponentInChildren<TextMeshProUGUI>().text = GetComponentInChildren<TextMeshProUGUI>().text;
        gameObject.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.Find("Wheels").gameObject.SetActive(true);
        gameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject.SetActive(false);
    }
    public void Finish()
    {
        gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
