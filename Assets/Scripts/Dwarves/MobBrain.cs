using Unity.VisualScripting;
using UnityEngine;

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
    public Transform stableTransform; //This is a transform disconnected from the rigidbody movement

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

        Vector3 targetPos = Vector3.zero;

        if(brainState == BrainState.Tracking)
        {
            targetPos = target.position;
        }
        else if(brainState == BrainState.Pulling)
        {
            targetPos = new Vector3(stableTransform.position.x, stableTransform.position.y, 1000);
        }

        Vector3 directionToTarget = targetPos - stableTransform.position;
        float angleToTarget = Vector3.SignedAngle(stableTransform.forward, directionToTarget, stableTransform.up);
        angleToTarget += LookAheadAvoidance();

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

    /// <summary>
    /// Looks ahead of the dwarf with a simple raycast against a layer to find obstacles
    /// If it finds anything other than its target, it attempts to avoid it by turning towards
    /// the shorest distance to go around it
    /// </summary>
    /// <returns>A float indicating how much to turn left or right to avoid the obstacle</returns>
    float LookAheadAvoidance()
    {
        if (target == null)
            return 0;

        float avoidanceAngle = 15;
        float avoidanceDist = 5;

        //Get a set of 3 rays
        //One that points directly forwards, and two that are angled
        //25 degrees to the right and left of that around the y axis
        Ray forward = new Ray(stableTransform.position, stableTransform.forward);// target.position - transform.position);

        Ray left = new Ray(forward.origin, Quaternion.AngleAxis(-avoidanceAngle, stableTransform.up) * forward.direction);
        Ray right = new Ray(forward.origin, Quaternion.AngleAxis(avoidanceAngle, stableTransform.up) * forward.direction);

        Debug.DrawRay(forward.origin, forward.direction * avoidanceDist, Color.green);
        Debug.DrawRay(left.origin, left.direction * avoidanceDist, Color.green);
        Debug.DrawRay(right.origin, right.direction * avoidanceDist, Color.green);

        //Raycast out to a max distance and choose the longest ray to deflect towards
        //If forward is the longest ray, don't turn
        RaycastHit hit;
        float forwardDist;
        float leftDist;
        float rightDist;
        if (Physics.Raycast(forward, out hit, avoidanceDist))
        {
            if(hit.collider == target)
            {
                return 0;
            }
            forwardDist = hit.distance;
            Debug.DrawRay(forward.origin, forward.direction * avoidanceDist, Color.red);
        }
        else
        {
            return 0;
        }

        if (Physics.Raycast(left, out hit, avoidanceDist))
        {
            if (hit.collider == target)
            {
                return -avoidanceAngle;
            }
            leftDist = hit.distance;

            Debug.DrawRay(left.origin, left.direction * avoidanceDist, Color.red);
        }
        else
        {
            return -avoidanceAngle;
        }

        if (Physics.Raycast(right, out hit, avoidanceDist))
        {
            if (hit.collider == target)
            {
                return avoidanceAngle;
            }
            rightDist = hit.distance;
            Debug.DrawRay(right.origin, right.direction * avoidanceDist, Color.red);
        }
        else
        {
            return avoidanceAngle;
        }

        if(forwardDist >= leftDist && forwardDist >= rightDist)
        {
            return 0;
        }
        else if(leftDist >= rightDist && leftDist >= forwardDist)
        {
            return -avoidanceAngle;
        }    
        else if(rightDist >= leftDist && rightDist >= forwardDist)
        {
            return avoidanceAngle;
        }

        return 0;
    }
}
