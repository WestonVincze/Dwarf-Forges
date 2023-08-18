using UnityEngine;

public class RigidbodyMover : MonoBehaviour
{
    public float power = 50.0f;
    public float maxSpeed = 10.0f;
    public float jumpForce = 5.0f;
    private bool isGrounded;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 force = new Vector3(moveX, 0, moveZ) * power;

        // Predict the new velocity after applying the force
        Vector3 newVelocity = rb.velocity + force * Time.fixedDeltaTime;

        // Check if the predicted horizontal velocity exceeds the maxSpeed
        if (new Vector3(newVelocity.x, 0, newVelocity.z).magnitude > maxSpeed)
        {
            // Reduce the force to not exceed maxSpeed
            force = force.normalized * (maxSpeed - rb.velocity.magnitude);
        }

        rb.AddForce(force, ForceMode.Force);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
