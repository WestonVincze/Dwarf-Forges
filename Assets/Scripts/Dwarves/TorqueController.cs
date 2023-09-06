using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TorqueController : MonoBehaviour
{
    public Vector3 torqueStrength = new Vector3(5.0f, 0.0f, 5.0f);
    public Transform stableTransform;
    public float turnSpeed = 1.5f;

    public float maxAngularVelocity = 10;

    private Rigidbody rb;
    private InputHandler inputHandler;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.maxAngularVelocity = maxAngularVelocity;
        inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        HandleTurning(inputHandler.GetInputStates());
    }

    void FixedUpdate()
    {
        ApplyTorque(inputHandler.GetInputStates());
    }

    void ApplyTorque(InputHandler.InputStates inputStates)
    {
        Vector3 yawAxis = stableTransform.up;
        Vector3 pitchAxis = stableTransform.right;
        Vector3 rollAxis = stableTransform.forward;

        Vector3 torque = Vector3.zero;

        if (inputStates.W)
            torque += torqueStrength.z * pitchAxis;
        if (inputStates.S)
            torque -= torqueStrength.z * pitchAxis;
        if (inputStates.A)
            torque += torqueStrength.x * rollAxis;
        if (inputStates.D)
            torque -= torqueStrength.x * rollAxis;
        if (inputStates.Q)
            torque -= torqueStrength.y * yawAxis;
        if (inputStates.E)
            torque += torqueStrength.y * yawAxis;

        rb.AddTorque(torque);
    }

    void HandleTurning(InputHandler.InputStates inputStates)
    {
        Vector3 targetDirection = Vector3.zero;
        if (inputStates.Q)
            targetDirection = -stableTransform.right;
        if (inputStates.E)
            targetDirection = stableTransform.right;

        stableTransform.forward = Vector3.Slerp(stableTransform.forward, targetDirection, turnSpeed * Time.deltaTime);
    }
}
