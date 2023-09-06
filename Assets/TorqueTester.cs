using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TorqueTester : MonoBehaviour
{
    public Vector3 torque;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = 7;
    }

    // Update is called once per frame
    void Update()
    {
        rb.maxAngularVelocity = 11;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddTorque(torque, ForceMode.Force);
        }
    }
}
