using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static TorqueController;

public class MobBrain : InputDecoupler_TorqueController
{
    // Target to move towards.
    public Transform target;

    public Transform localTransform;

    public float YawThreshold = 5.0f;  // A small angular threshold for yaw direction.
    public float PitchThreshold = 0.5f; // A small distance threshold for pitch direction.

    void Update()
    {
        if (target == null) return;

        // Update the thought process each frame.
        GetInputStates();
    }

    public override InputStates GetInputStates()
    {
        if(target == null)
            return new InputStates();

        InputStates states;

        Vector3 directionToTarget = target.position - localTransform.position;
        float angleToTarget = Vector3.SignedAngle(-localTransform.right, directionToTarget, localTransform.up);

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
