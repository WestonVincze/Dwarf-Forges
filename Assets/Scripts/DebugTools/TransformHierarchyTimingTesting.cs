using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHierarchyTimingTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 velocity = Vector3.forward * Time.deltaTime * 500;

        Debug.Log("1) " + transform.parent.position + ", " + transform.position + ", " + velocity);
        //transform.position = Vector3.zero;
        Debug.Log("2) " + transform.parent.position + ", " + transform.position + ", " + velocity);
        transform.parent.position += velocity;
        Debug.Log("3) " + transform.parent.position + ", " + transform.position + ", " + velocity);

    }
}
