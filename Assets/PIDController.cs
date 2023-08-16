using UnityEngine;

public class PIDController : MonoBehaviour
{
    public float kp = 1.0f;  // Proportional gain
    public float ki = 0.0f;  // Integral gain
    public float kd = 0.0f;  // Derivative gain

    // For demonstration purposes, you can initialize Pdes and Vdes to some desired values
    // but in a real application, you might set these values based on user input or other game logic.
    public Vector3 Pdes = Vector3.zero;
    public Vector3 Vdes = Vector3.zero;

    private Vector3 integralError = Vector3.zero;  // Integral error accumulator

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Calculate the g coefficient for stability
        float g = 1 / (1 + kd * dt + kp * dt * dt);

        // Calculate the modified gains
        float ksg = kp * g;
        float kig = ki * g * dt;  // Added integral component
        float kdg = (kd + kp * dt) * g;

        // Current position and velocity
        Vector3 Pt0 = transform.position;
        Vector3 Vt0 = GetComponent<Rigidbody>().velocity;  // Fixed the rigidbody call

        // Calculate position and velocity errors
        Vector3 positionError = Pdes - Pt0;
        Vector3 velocityError = Vdes - Vt0;

        // Update integral error
        integralError += positionError * dt;

        // Calculate force using PID equation
        Vector3 F = positionError * ksg + integralError * kig + velocityError * kdg;

        // Apply the force
        GetComponent<Rigidbody>().AddForce(F);
    }
}
