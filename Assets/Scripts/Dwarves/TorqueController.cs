using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TorqueController : MonoBehaviour
{
    public Vector3 torqueStrength = new Vector3(5.0f, 7.0f, 5.0f);
    public float impulseStrength = 20.0f;
    public float impulseCooldown = 0.2f;
    private float nextImpulseTime = 0f;
    public Transform referenceTransform;
    public float tweenSpeed = 0.05f;  // Speed of the tweening effect

    private Rigidbody rb;
    public InputDecoupler_TorqueController inputDecoupler;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputDecoupler = GetComponent<InputDecoupler_TorqueController>();

        if(inputDecoupler == null)
            inputDecoupler = new InputDecoupler_TorqueController();
    }

    void FixedUpdate()
    {
        ApplyTorque(inputDecoupler.GetInputStates());
        TweenReferenceForward();
    }

    Vector3 ClosestLocalAxis(Vector3 direction)
    {
        Vector3 localDir = transform.InverseTransformDirection(direction);
        Vector3 absDir = new Vector3(Mathf.Abs(localDir.x), Mathf.Abs(localDir.y), Mathf.Abs(localDir.z));

        if (absDir.x > absDir.y && absDir.x > absDir.z)
            return transform.right * Mathf.Sign(localDir.x);
        if (absDir.y > absDir.x && absDir.y > absDir.z)
            return transform.up * Mathf.Sign(localDir.y);
        return transform.forward * Mathf.Sign(localDir.z);
    }

    void ApplyTorque(InputDecoupler_TorqueController.InputStates inputStates)
    {
        Vector3 yawAxis = ClosestLocalAxis(referenceTransform.up);
        Vector3 pitchAxis = ClosestLocalAxis(referenceTransform.forward);
        Vector3 rollAxis = ClosestLocalAxis(referenceTransform.right);

        Vector3 torque = Vector3.zero;

        if (inputStates.W)
            torque += torqueStrength.x * pitchAxis;
        if (inputStates.S)
            torque -= torqueStrength.x * pitchAxis;
        if (inputStates.A)
            torque += torqueStrength.z * rollAxis;
        if (inputStates.D)
            torque -= torqueStrength.z * rollAxis;
        if (inputStates.Q)
            torque -= torqueStrength.y * yawAxis;
        if (inputStates.E)
            torque += torqueStrength.y * yawAxis;

        rb.AddTorque(torque);

        if (inputStates.QDown && Time.time >= nextImpulseTime)
        {
            rb.AddTorque(-impulseStrength * yawAxis, ForceMode.Impulse);
            nextImpulseTime = Time.time + impulseCooldown;
        }
        else if (inputStates.EDown && Time.time >= nextImpulseTime)
        {
            rb.AddTorque(impulseStrength * yawAxis, ForceMode.Impulse);
            nextImpulseTime = Time.time + impulseCooldown;
        }
    }

    void TweenReferenceForward()
    {
        Vector3 targetDirection = ClosestLocalAxis(referenceTransform.forward);
        referenceTransform.forward = Vector3.Slerp(referenceTransform.forward, targetDirection, tweenSpeed);
    }

    // Subclass definition
    public class InputDecoupler_TorqueController : MonoBehaviour
    {
        public struct InputStates
        {
            public bool W, S, A, D, Q, E;
            public bool QDown, EDown;
        }

        virtual public InputStates GetInputStates()
        {
            InputStates states;

            states.W = Input.GetKey(KeyCode.W);
            states.S = Input.GetKey(KeyCode.S);
            states.A = Input.GetKey(KeyCode.A);
            states.D = Input.GetKey(KeyCode.D);
            states.Q = Input.GetKey(KeyCode.Q);
            states.E = Input.GetKey(KeyCode.E);
            states.QDown = Input.GetKeyDown(KeyCode.Q);
            states.EDown = Input.GetKeyDown(KeyCode.E);

            return states;
        }
    }
}
