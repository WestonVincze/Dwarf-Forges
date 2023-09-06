using UnityEngine;

public class PIDController
{
    public Vector3 targetPos;
    public Vector3 targetVel;

    public float frequency = 1;
    public float damping = 1;

    /// <summary>
    /// Runs an update tick for the PID Controller. Should be called in a FixedUpdate
    /// </summary>
    /// <returns>Force that should be applied to rigidbody</returns>
    public Vector3 Update(Vector3 position, Vector3 velocity)
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;

        float dt = Time.fixedDeltaTime;
        float g = 1 / (1 + kd * dt + kp * dt * dt);
        float ksg = kp * g;
        float kdg = (kd + kp * dt) * g;
        Vector3 Pt0 = position;
        Vector3 Vt0 = velocity;
        Vector3 F = (targetPos - Pt0) * ksg + (targetVel - Vt0) * kdg;

        return F;
    }
}
