using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static TorqueController;

public class MobBrain : InputHandler
{
    enum BrainState
    {
        Tracking,
        Pulling
    }
    
    // Target to move towards.
    public Transform target;

    public Transform localTransform;

    public float YawThreshold = 5.0f;  // A small angular threshold for yaw direction.
    public float PitchThreshold = 0.5f; // A small distance threshold for pitch direction.

    private BrainState brainState = BrainState.Tracking;

    private LineRenderer lineRenderer;
    private SpringJoint rope;

    public void Update()
    {
        if(lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, target.position);
        }
    }

    public void TouchSensor_Front(Collider collider)
    {
        if (brainState != BrainState.Tracking)
            return;

        if(collider.transform == target)
        {
            rope = gameObject.AddComponent<SpringJoint>();
            rope.connectedBody = collider.gameObject.GetComponent<Rigidbody>();
            rope.maxDistance = 3;
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;

            brainState = BrainState.Pulling;
        }
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
