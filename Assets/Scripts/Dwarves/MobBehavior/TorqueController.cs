using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TorqueController_Old3 : MonoBehaviour
{
  public Vector3 torqueStrength = new Vector3(5.0f, 0.0f, 5.0f);
  public float motorVelocity = 100;

  public float maxAngularVelocity = 10;

  public Vector3 stableTransformRotation = Vector3.zero;

  public Rigidbody rollerBody;
  private Rigidbody rb;
  private InputHandler inputHandler;

  private HingeJoint joint;

  void Start()
  {
    if(rollerBody == null)
      rollerBody = GetComponent<Rigidbody>();
    rollerBody.maxAngularVelocity = maxAngularVelocity;

    rb = GetComponent<Rigidbody>();
    inputHandler = GetComponent<InputHandler>();

    joint = GetComponent<HingeJoint>();
  }
  private void FixedUpdate()
  {
    ApplyTorque(inputHandler.GetInputStates());
  }
  private void ApplyTorque(InputHandler.InputStates inputStates)
  {
    //UpdateStableTransformRotation();

    Vector3 yawAxis = transform.up;
    Vector3 pitchAxis = transform.right;
    Vector3 rollAxis = transform.forward;

    Vector3 torque = Vector3.zero;

    if (inputStates.W)
      torque += torqueStrength.z * pitchAxis;
    if (inputStates.S)
      torque -= torqueStrength.z * pitchAxis;
    if (inputStates.A)
      torque += torqueStrength.x * rollAxis;
    if (inputStates.D)
      torque -= torqueStrength.x * rollAxis;

    if(joint && joint.useMotor)
    {
      JointMotor motor = joint.motor;
      motor.targetVelocity = motorVelocity * torque.z;
      motor.force = torque.z;
      joint.motor = motor;
    }
    else
    {
      rollerBody.AddTorque(torque);
    }

    torque = Vector3.zero;

    if (inputStates.Q)
      torque -= torqueStrength.y * yawAxis;
    if (inputStates.E)
      torque += torqueStrength.y * yawAxis;

    rb.AddTorque(torque);
  }
}