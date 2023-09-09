using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public bool showDebugVision; //Enables debug draw rays

    public float yawThreshold = 5.0f;  // A small angular threshold for yaw direction.
    public float distanceThreshold = 0.5f; // Distance threshold to start moving to target

    private BrainState brainState = BrainState.Tracking;
    private RopeHandler ropeHandler;

    public float avoidanceAngle = 90;
    public float avoidanceAngleIncrement = 15;
    public float avoidanceDist = 5;

    public float hopAwayForce = 5;
    public float hopAwayPitch = 45;

    public float avoidanceBiasSwitchCooldown = 1.0f;
    private float nextAvoidanceBiasSwitchTime = 0.0f;
    private bool avoidanceBias = false; //True = right, false = left

    //private float lookAheadAvoidanceLinger = 0;

    public void Start()
    {
        ropeHandler = GetComponentInChildren<RopeHandler>();
    }

    public void TouchSensor_Front(Collider collider)
    {
        if (brainState != BrainState.Tracking)
            return;


        RigidbodyPointer rbPointer = collider.GetComponent<RigidbodyPointer>();
        if (collider.transform == target)
        {
            ropeHandler.StartPullObject(collider, collider.GetComponent<Rigidbody>(), Random.Range(3,10));
            brainState = BrainState.Pulling;
            HopTowardsDestination();
        }
        else if(rbPointer && rbPointer.rb.transform == target)
        {
            ropeHandler.StartPullObject(collider, rbPointer.rb, Random.Range(3, 10));
            brainState = BrainState.Pulling;
            HopTowardsDestination();
        }
    }

    public void Die()
    {
        brainState = BrainState.Dead;
        GetComponentInChildren<Rigidbody>().AddForce(new Vector3(Random.Range(0, 5), 20, Random.Range(0, 5)), ForceMode.Impulse);
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
            //lookAheadAvoidanceLinger = 0;
        }

        Vector3 directionToTarget = targetPos - stableTransform.position;
        float angleToTarget = Vector3.SignedAngle(stableTransform.forward, directionToTarget, stableTransform.up);
        float newLookAheadAvoidance = LookAheadAvoidance(targetPos);
        /*
        if (Mathf.Abs(newLookAheadAvoidance) > Mathf.Abs(lookAheadAvoidanceLinger))
            lookAheadAvoidanceLinger = newLookAheadAvoidance;

        if(Mathf.Abs(lookAheadAvoidanceLinger) > 0.1)
        {
            angleToTarget += lookAheadAvoidanceLinger;

            if (lookAheadAvoidanceLinger > 0)
            {
                lookAheadAvoidanceLinger -= Time.deltaTime * avoidanceAngle;
            }
            else
            {
                lookAheadAvoidanceLinger += Time.deltaTime * avoidanceAngle;
            }
        }
        */
        angleToTarget += newLookAheadAvoidance;

        // "Think" about yaw direction.
        states.E = angleToTarget > yawThreshold;  // yawLeft
        states.Q = angleToTarget < -yawThreshold; // yawRight

        // "Think" about pitch direction.
        states.W = directionToTarget.magnitude > distanceThreshold;   // pitchForward
        states.S = directionToTarget.magnitude < -distanceThreshold;  // pitchBackwards

        // A and D are not used for roll, so they are always false.
        states.A = false;
        states.D = false;

        // Since QDown and EDown are not defined in the original thought process, they remain false by default.
        states.QDown = false;
        states.EDown = false;

        return states;
    }

    //Found code for this here: https://forum.unity.com/threads/left-right-test-function.31420/
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }


    /// <summary>
    /// Looks ahead of the dwarf with a simple raycast against a layer to find obstacles
    /// If it finds anything other than its target, it attempts to avoid it by turning towards
    /// the shorest distance to go around it
    /// </summary>
    /// <returns>A float indicating how much to turn left or right to avoid the obstacle</returns>
    float LookAheadAvoidance(Vector3 targetPos)
    {
        if (target == null)
            return 0;

        float targetAngleDir = AngleDir(stableTransform.forward, targetPos - transform.position, stableTransform.up);

        float retVal = 0;

        // Create a list to store the rays and hit distances
        List<Ray> rays = new List<Ray>();
        List<float> distances = new List<float>();

        // Add the forward ray to the list
        rays.Add(new Ray(stableTransform.position, stableTransform.forward));

        //Get left/right bias
        if(Time.time >= nextAvoidanceBiasSwitchTime)
        {
            if(avoidanceBias != (targetAngleDir > 0))
            {
                avoidanceBias = (targetAngleDir > 0);
                nextAvoidanceBiasSwitchTime = Time.time + avoidanceBiasSwitchCooldown;
            }
        }

        // Generate additional rays based on the desired number of angles
        for (int i = 1; i <= (avoidanceAngle / avoidanceAngleIncrement); i++)
        {
            /*
            //Based on the mob's position relative to the z-axis, determine the look bias
            if ((brainState == BrainState.Tracking && stableTransform.position.z > 0) || (brainState == BrainState.Pulling && stableTransform.position.z < 0))
            {
                // Add left and right rays for each angle increment
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(-i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
            }
            else
            {
                // Add left and right rays for each angle increment
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(-i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
            }
            */
            //If the target angle direction is to the right, favor the right, otherwise favor the left
            if(avoidanceBias)
            {
                // Add left and right rays for each angle increment
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(-i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
            }
            else
            {
                // Add left and right rays for each angle increment
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(-i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
                rays.Add(new Ray(stableTransform.position, Quaternion.AngleAxis(i * avoidanceAngleIncrement, stableTransform.up) * stableTransform.forward));
            }
        }

        // Raycast for each ray and store the results
        foreach (Ray ray in rays)
        {
            RaycastHit hit;
            float distance;

            if (Physics.Raycast(ray, out hit, avoidanceDist))
            {
                if (hit.collider.transform == target && brainState == BrainState.Tracking)
                {
                    distance = avoidanceDist + 2;
                }
                else
                {
                    distance = hit.distance;
                }
            }
            else
            {
                distance = avoidanceDist + 1;
            }

            distances.Add(distance);
        }

        // Identify the ray with the maximum distance
        int maxDistIndex = distances.IndexOf(distances.Max());
        Ray chosenRay = rays[maxDistIndex];

        // Use the chosen ray for decision making
        if (maxDistIndex == 0)
        {
            retVal = 0;
        }
        else
        {
            if (targetAngleDir > 0)
            {
                //Favor right
                if (maxDistIndex % 2 == 0)
                {
                    retVal = maxDistIndex * avoidanceAngleIncrement;
                }
                else
                {
                    retVal = -maxDistIndex * avoidanceAngleIncrement;
                }
            }
            else
            {
                //Favor left
                if (maxDistIndex % 2 == 1)
                {
                    retVal = -maxDistIndex * avoidanceAngleIncrement;
                }
                else
                {
                    retVal = maxDistIndex * avoidanceAngleIncrement;
                }
            }
        }

        if (showDebugVision)
        {
            // Visualize with Debug.DrawRay
            for (int i = 0; i < rays.Count; i++)
            {
                Color rayColor = (distances[i] > avoidanceDist) ? Color.green : Color.red;
                if (i == maxDistIndex)
                    rayColor = Color.blue;

                Debug.DrawRay(rays[i].origin, rays[i].direction * avoidanceDist, rayColor);
            }
        }

        return retVal;
    }

    void HopTowardsDestination()
    {
        Vector3 hopForce = new Vector3(stableTransform.position.x, stableTransform.position.y, 1000);
        hopForce = Quaternion.AngleAxis(hopAwayPitch, Vector3.right) * hopForce;
        
        hopForce = hopForce.normalized * hopAwayForce;
        GetComponentInChildren<Rigidbody>().AddForce(hopForce, ForceMode.Impulse);
    }


    float LookAheadAvoidance_old2()
    {
        if (target == null)
            return 0;

        #region Declare/Initialize
        float retVal = 0;
        string dirChoice = "";

        float avoidanceAngle = 15;
        float avoidanceDist = 5;

        //Get a set of 3 rays
        //One that points directly forwards, and two that are angled
        //avoidanceAngle degrees to the right and left of that around the y axis
        Ray forwardRay = new Ray(stableTransform.position, stableTransform.forward);
        Ray leftRay = new Ray(forwardRay.origin, Quaternion.AngleAxis(-avoidanceAngle, stableTransform.up) * forwardRay.direction);
        Ray rightRay = new Ray(forwardRay.origin, Quaternion.AngleAxis(avoidanceAngle, stableTransform.up) * forwardRay.direction);

        //Raycast out to a max distance and choose the longest ray to deflect towards
        //If forward is the longest ray, don't turn
        RaycastHit forwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;
        float forwardDist;
        float leftDist;
        float rightDist;
        #endregion

        #region RayCasts
        if (Physics.Raycast(forwardRay, out forwardHit, avoidanceDist))
        {
            if (forwardHit.collider == target)
            {
                forwardDist = avoidanceDist + 2;
            }
            forwardDist = forwardHit.distance;
        }
        else
        {
            forwardDist = avoidanceDist + 1;
        }

        if (Physics.Raycast(leftRay, out leftHit, avoidanceDist))
        {
            if (leftHit.collider == target)
            {
                leftDist = avoidanceDist + 2;
            }
            leftDist = leftHit.distance;
        }
        else
        {
            leftDist = avoidanceDist + 1;
        }

        if (Physics.Raycast(rightRay, out rightHit, avoidanceDist))
        {
            if (rightHit.collider == target)
            {
                rightDist = avoidanceDist + 2;
            }
            rightDist = rightHit.distance;
        }
        else
        {
            rightDist = avoidanceDist + 1;
        }
        #endregion

        #region ChooseDirection

        //Decision logic thought process:
        //Check forward first: if it's clear, go forward
        //Check left second: if forward isn't clear, and left is, go left
        //Check right last: if forward and right aren't clear, and right is, go right

        //Draw Ray Color Scheme:
        //The drawRay coloring is meant to always indicate if a ray
        //detects something, and if the ray is being used to make a decision.
        //Green = No detections
        //Blue = Go this way
        //Red = obstacle detected

        #region CheckForward
        if (forwardDist >= leftDist && forwardDist >= rightDist)
        {
            dirChoice = "forward";
            retVal = 0;
            Debug.DrawRay(forwardRay.origin, forwardRay.direction * avoidanceDist, Color.blue);
        }    
        else if(forwardDist <= avoidanceDist)
        {
            Debug.DrawRay(forwardRay.origin, forwardRay.direction * avoidanceDist, Color.red);
        }
        else
        {
            Debug.DrawRay(forwardRay.origin, forwardRay.direction * avoidanceDist, Color.green);
        }
        #endregion

        #region CheckLeft
        if (leftDist >= forwardDist && leftDist >= rightDist && dirChoice == "")
        {
            dirChoice = "left";
            retVal = -avoidanceAngle;

            Debug.DrawRay(leftRay.origin, leftRay.direction * avoidanceDist, Color.blue);
        }
        else if (leftDist < avoidanceDist)
        {
            Debug.DrawRay(leftRay.origin, leftRay.direction * avoidanceDist, Color.red);
        }
        else
        {
            Debug.DrawRay(leftRay.origin, leftRay.direction * avoidanceDist, Color.green);
        }
        #endregion

        #region CheckRight
        if (rightDist >= forwardDist && rightDist >= leftDist && dirChoice == "")
        {
            dirChoice = "right";
            retVal = -avoidanceAngle;

            Debug.DrawRay(rightRay.origin, rightRay.direction * avoidanceDist, Color.blue);
        }
        else if (rightDist < avoidanceDist)
        {
            Debug.DrawRay(rightRay.origin, rightRay.direction * avoidanceDist, Color.red);
        }
        else
        {
            Debug.DrawRay(rightRay.origin, rightRay.direction * avoidanceDist, Color.green);
        }
        #endregion
        #endregion

        return retVal;
    }
    float LookAheadAvoidance_old()
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
