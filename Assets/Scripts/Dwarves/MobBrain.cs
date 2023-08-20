using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(TorqueController))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof (RopeHandler))]
public class MobBrain : InputHandler
{
    enum BrainState
    {
        Tracking,
        Pulling,
        Dead
    }
    
    // Target to move towards.
    public Transform target;

    public Transform localTransform;

    public float YawThreshold = 5.0f;  // A small angular threshold for yaw direction.
    public float PitchThreshold = 0.5f; // A small distance threshold for pitch direction.

    private BrainState brainState = BrainState.Tracking;
    private RopeHandler ropeHandler;

    public void Start()
    {
        ropeHandler = GetComponent<RopeHandler>();
    }

    public void TouchSensor_Front(Collider collider)
    {
        if (brainState != BrainState.Tracking)
            return;

        if(collider.transform == target)
        {
            ropeHandler.StartPullObject(collider, Random.Range(3,10));
            brainState = BrainState.Pulling;
        }
    }

    public void Die()
    {
        brainState = BrainState.Dead;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 5), 20, Random.Range(0, 5)), ForceMode.Impulse);
        ropeHandler.StopPullObject();
    }

    public override InputStates GetInputStates()
    {
        if(target == null)
            return new InputStates();

        InputStates states;

        Vector3 targetPos;

        if(brainState == BrainState.Tracking)
        {
            targetPos = target.position;
        }
        else// if(brainState == BrainState.Pulling)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, 1000);
        }

        Vector3 directionToTarget = targetPos - localTransform.position;
        float angleToTarget = Vector3.SignedAngle(localTransform.forward, directionToTarget, localTransform.up);

        // "Think" about yaw direction.
        states.E = angleToTarget > YawThreshold;  // yawLeft
        states.Q = angleToTarget < -YawThreshold; // yawRight

        // "Think" about pitch direction.
        states.W = directionToTarget.magnitude > PitchThreshold;   // pitchForward
        states.S = directionToTarget.magnitude < -PitchThreshold;  // pitchBackwards

        // A and D are not used for roll, so they are always false.
        states.A = false;
        states.D = false;

        // Since QDown and EDown are not defined in the original thought process, they remain false by default.
        states.QDown = false;
        states.EDown = false;

        return states;
    }
}
