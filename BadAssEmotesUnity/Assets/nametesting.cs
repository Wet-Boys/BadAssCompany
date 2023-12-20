using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nametesting : MonoBehaviour
{
    // Start is called before the first frame update\
    string cachedName;
    bool which = false;
    void Start()
    {
        cachedName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (which)
        {
            if (gameObject.name == "bignametestingtime")
            {
                Debug.Log(".name");
            }
        }
        else
        {
            if (cachedName == "bignametestingtime")
            {
                Debug.Log("cachedName");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            which = !which;
        }
    }
}
