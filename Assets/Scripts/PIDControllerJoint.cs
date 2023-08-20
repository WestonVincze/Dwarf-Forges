using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDControllerJoint : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 targetVel;

    public float frequency = 1;
    public float damping = 1;

    private Rigidbody rb;
    private PIDController _PIDController = new PIDController();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _PIDController.targetPos = targetPos;
        _PIDController.targetVel = targetVel;
        _PIDController.frequency = frequency;
        _PIDController.damping = damping;
        rb.AddForce(_PIDController.Update(transform.position, rb.velocity), ForceMode.Acceleration);
    }
}
