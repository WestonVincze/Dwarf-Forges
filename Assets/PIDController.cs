using UnityEngine;

public class PIDController : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 targetVel;

    public float frequency = 1;
    public float damping = 1;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;

        float dt = Time.fixedDeltaTime;
        float g = 1 / (1 + kd * dt + kp * dt * dt);
        float ksg = kp * g;
        float kdg = (kd + kp * dt) * g;
        Vector3 Pt0 = transform.position;
        Vector3 Vt0 = rb.velocity;
        Vector3 F = (targetPos - Pt0) * ksg + (targetVel - Vt0) * kdg;
        
        rb.AddForce(F);
    }
}
