using UnityEngine;

public class PDTorqueController
{
    public Quaternion targetRotation;

    public float frequency = 1;
    public float damping = 1;

    /// <summary>
    /// Runs an update tick for the PD Torque Controller. Should be called in a FixedUpdate.
    /// </summary>
    /// <returns>Torque that should be applied to rigidbody</returns>
    public Vector3 Update(Quaternion currentRotation, Vector3 angularVelocity, Quaternion inertiaTensorRotation, Vector3 inertiaTensor)
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float dt = Time.fixedDeltaTime;
        float g = 1 / (1 + kd * dt + kp * dt * dt);
        float ksg = kp * g;
        float kdg = (kd + kp * dt) * g;
        Vector3 x;
        float xMag;
        Quaternion q = targetRotation * Quaternion.Inverse(currentRotation);

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
        Quaternion rotInertia2World = inertiaTensorRotation * currentRotation;
        pidv = Quaternion.Inverse(rotInertia2World) * pidv;
        pidv.Scale(inertiaTensor);
        pidv = rotInertia2World * pidv;

        return pidv;
    }
}
