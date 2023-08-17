using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraTracking : MonoBehaviour
{
    public Transform targetTransform;      // The primary target the camera will follow for position
    public Transform lookAtTransform;      // The secondary target the camera will look at. If null, it'll look at the primary target.
    public Vector3 positionOffset;         // The positional offset from the primary target
    public Vector3 rotationOffset;         // The rotational offset from the target in Euler angles
    public bool matchRotation;             // Whether or not the camera matches the primary target's rotation

    [Range(-1, 1)]
    public float positionSoftness = 0.5f;  // The softness factor for camera's position interpolation
    [Range(-1, 1)]
    public float rotationSoftness = 0.5f;  // The softness factor for camera's rotation interpolation

    public enum UpdateMode
    {
        FixedUpdate,
        Update,
        LateUpdate
    }

    public UpdateMode updateType;   // Determines in which update function the camera tracking will happen

    private void FixedUpdate()
    {
        if (updateType == UpdateMode.FixedUpdate)
            TrackTarget();
    }

    private void Update()
    {
        if (updateType == UpdateMode.Update)
            TrackTarget();
    }

    private void LateUpdate()
    {
        if (updateType == UpdateMode.LateUpdate)
            TrackTarget();
    }

    void TrackTarget()
    {
        if (targetTransform == null)
            return;

        Vector3 desiredPosition = targetTransform.position + positionOffset;

        // Use Lerp for position if softness is not -1
        if (positionSoftness != -1)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSoftness);
        else
            transform.position = desiredPosition;

        // Rotation Logic
        Quaternion desiredRotation;
        if (lookAtTransform != null)
        {
            // Calculate the rotation to look at the secondary target
            desiredRotation = Quaternion.LookRotation(lookAtTransform.position - transform.position) * Quaternion.Euler(rotationOffset);
        }
        else if (matchRotation)
        {
            // If not using the secondary target, and matching rotation, use primary target's rotation
            desiredRotation = targetTransform.rotation * Quaternion.Euler(rotationOffset);
        }
        else
        {
            // If neither using the secondary target nor matching the primary target's rotation, maintain the current rotation.
            return;
        }

        // Use Slerp for rotation if softness is not -1
        if (rotationSoftness != -1)
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSoftness);
        else
            transform.rotation = desiredRotation;
    }
}
