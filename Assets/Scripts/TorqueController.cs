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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyTorque();
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

    void ApplyTorque()
    {
        Vector3 yawAxis = ClosestLocalAxis(referenceTransform.up);
        Vector3 pitchAxis = ClosestLocalAxis(referenceTransform.forward);
        Vector3 rollAxis = ClosestLocalAxis(referenceTransform.right);

        Vector3 torque = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            torque += torqueStrength.x * pitchAxis;
        if (Input.GetKey(KeyCode.S))
            torque -= torqueStrength.x * pitchAxis;

        if (Input.GetKey(KeyCode.A))
            torque += torqueStrength.z * rollAxis;
        if (Input.GetKey(KeyCode.D))
            torque -= torqueStrength.z * rollAxis;

        if (Input.GetKey(KeyCode.Q))
            torque -= torqueStrength.y * yawAxis;
        if (Input.GetKey(KeyCode.E))
            torque += torqueStrength.y * yawAxis;

        rb.AddTorque(torque);

        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextImpulseTime)
        {
            rb.AddTorque(-impulseStrength * yawAxis, ForceMode.Impulse);
            nextImpulseTime = Time.time + impulseCooldown;
        }
        else if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextImpulseTime)
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
}
