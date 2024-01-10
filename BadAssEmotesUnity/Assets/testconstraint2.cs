using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testconstraint2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform me;
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        me.position = target.position;
    }
}
