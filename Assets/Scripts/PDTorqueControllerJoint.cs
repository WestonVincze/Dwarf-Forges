using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PDTorqueControllerJoint : MonoBehaviour
{
    //public Quaternion targetRotation = Quaternion.identity;
    public Vector3 targetEulerRotation = Vector3.zero;

    public float frequency = 1.0f;
    public float damping = 1.0f;

    private Rigidbody rb;
    private PDTorqueController _PDTorqueController = new PDTorqueController();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //_PDTorqueController.targetRotation = targetRotation;
        _PDTorqueController.targetRotation = Quaternion.Euler(targetEulerRotation);
        _PDTorqueController.frequency = frequency;
        _PDTorqueController.damping = damping;
        rb.AddTorque(_PDTorqueController.Update(transform.rotation, rb.angularVelocity, rb.inertiaTensorRotation, rb.inertiaTensor), ForceMode.Acceleration);
    }
}