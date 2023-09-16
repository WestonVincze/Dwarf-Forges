using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]
public class TorqueController_Old : MonoBehaviour
{
    public Vector3 torqueStrength = new Vector3(5.0f, 7.0f, 5.0f);
    public float yawFreq = 1.0f;
    public float yawDamp = 1.0f;
    public float impulseStrength = 20.0f;
    public float impulseCooldown = 0.2f;
    private float nextImpulseTime = 0f;
    public Transform visualTransform;
    public Transform stableTransform;
    public float turnSpeed = 1.5f;
    public float tweenSpeed = 0.05f;

    public float centerOfMassOffsetDist = 0.5f;
    public float centerOfMassShiftSpeed = 1.0f;
    public Vector3 inertiaTensor = -Vector3.one;

    private Rigidbody rb;
    private InputHandler inputHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 10;
        inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        HandleTurning(inputHandler.GetInputStates());
    }

    void FixedUpdate()
    {
        ApplyTorque(inputHandler.GetInputStates());
        //HandleInertiaTensor();
        HandleCenterOfMass();
        //TweenReferenceForward();
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

    void ApplyTorque(InputHandler.InputStates inputStates)
    {
        Vector3 yawAxis = ClosestLocalAxis(stableTransform.up);
        Vector3 pitchAxis = ClosestLocalAxis(stableTransform.right);
        Vector3 rollAxis = ClosestLocalAxis(stableTransform.forward);

        Vector3 torque = Vector3.zero;

        if (inputStates.W)
            torque += torqueStrength.z * pitchAxis;
        if (inputStates.S)
            torque -= torqueStrength.z * pitchAxis;
        if (inputStates.A)
            torque += torqueStrength.x * rollAxis;
        if (inputStates.D)
            torque -= torqueStrength.x * rollAxis;

        float torqueY = PDControllerTorque(Quaternion.Euler(rollAxis),
                                                Quaternion.Euler(stableTransform.forward),
                                                yawFreq,
                                                yawDamp,
                                                Time.fixedDeltaTime,
                                                rb.angularVelocity,
                                                rb.inertiaTensorRotation,
                                                rb.inertiaTensor).y;
        /*
        if(torqueY > 0.1f)
        {
            //Debug.Log(rollAxis + ", " + referenceTransform.forward);
            Debug.Log(torqueY);
        }
        */

        Debug.Log(torqueY);

        //float angleToReference = Mathf.Deg2Rad * Vector3.SignedAngle(rollAxis, referenceTransform.forward, referenceTransform.up);
        torque += yawAxis * torqueY;
        /*
        if (inputStates.Q)
            torque -= torqueStrength.y * yawAxis;
        if (inputStates.E)
            torque += torqueStrength.y * yawAxis;
        */

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

    void TweenReferenceForward()
    {
        Vector3 targetDirection = ClosestLocalAxis(visualTransform.forward);
        visualTransform.forward = Vector3.Slerp(visualTransform.forward, targetDirection, tweenSpeed);
    }



    Vector3 PDControllerTorque(Quaternion currentRotation,
                               Quaternion desiredRotation,
                               float frequency,
                               float damping,
                               float deltaTime,
                               Vector3 angularVelocity,
                               Quaternion inertiaTensorRotation,
                               Vector3 inertiaTensor)
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * deltaTime + kp * deltaTime * deltaTime);

        float ksg = kp * g;
        float kdg = (kd + kp * deltaTime) * g;
        Vector3 x;
        float xMag;
        Quaternion q = desiredRotation * Quaternion.Inverse(transform.rotation);
        // Q can be the-long-rotation-around-the-sphere eg. 350 degrees
        // We want the equivalant short rotation eg. -10 degrees
        // Check if rotation is greater than 190 degees == q.w is negative
        if (q.w < 0)
        {
            // Convert the quaterion to eqivalent "short way around" quaterion
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out xMag, out x);
        x.Normalize();
        x *= Mathf.Deg2Rad;
        Vector3 pidv = kp * x * xMag - kd * angularVelocity;
        Quaternion rotInertia2World = inertiaTensorRotation * transform.rotation;
        pidv = Quaternion.Inverse(rotInertia2World) * pidv;
        pidv.Scale(inertiaTensor);
        pidv = rotInertia2World * pidv;

        //rigidbody.AddTorque(pidv);

        return pidv;
    }


    float PDControllerTorqueYAxis
    (
    Quaternion currentRotation,
    Quaternion desiredRotation,
    float frequency,
    float damping,
    float deltaTime,
    Vector3 angularVelocity,
    Quaternion inertiaTensorRotation,
    Vector3 inertiaTensor)
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * deltaTime + kp * deltaTime * deltaTime);

        Quaternion q = desiredRotation * Quaternion.Inverse(currentRotation);

        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }

        float angleY = 2 * Mathf.Atan2(q.y, q.w); // Extract only the Y axis rotation angle

        float pidvY = kp * angleY - kd * angularVelocity.y; // Calculate PID only for y axis

        Quaternion rotInertia2World = inertiaTensorRotation * currentRotation;
        pidvY = pidvY / (rotInertia2World * Vector3.up).y;
        pidvY *= inertiaTensor.y;

        return pidvY;
    }

    private void HandleInertiaTensor()
    {
        rb.inertiaTensor = new Vector3(inertiaTensor.x < 0 ? rb.inertiaTensor.x : inertiaTensor.x, inertiaTensor.y < 0 ? rb.inertiaTensor.y : inertiaTensor.y, inertiaTensor.z < 0 ? rb.inertiaTensor.z : inertiaTensor.z);
        rb.inertiaTensorRotation = Quaternion.Euler(ClosestLocalAxis(-stableTransform.up));
    }

    private void HandleCenterOfMass()
    {
        Vector3 newCenterOfMass = ClosestLocalAxis(-stableTransform.up) * centerOfMassOffsetDist;
        if (rb.centerOfMass != newCenterOfMass)
        {
            rb.WakeUp();
        }
        
        rb.centerOfMass = Vector3.Slerp(rb.centerOfMass, newCenterOfMass, Time.fixedDeltaTime * centerOfMassShiftSpeed);

        Debug.DrawRay(rb.position, newCenterOfMass * 2, Color.red);
        Debug.DrawRay(rb.position, rb.centerOfMass * 2, Color.green, 0, false);
    }

    /*
    private void HandleCenterOfMass()
    {
        if (!centerOfMass)
            return;

        if(rb.centerOfMass != centerOfMass.localPosition)
        {
            rb.WakeUp();
        }
        rb.centerOfMass = centerOfMass.localPosition;
    }
    */
}
