using UnityEngine;

[RequireComponent(typeof(Transform))]
public class OverrideParentTransform : MonoBehaviour
{
    [Header("Position Override Settings")]
    public bool ignorePositionX;
    public bool ignorePositionY;
    public bool ignorePositionZ;
    public Vector3 positionOffset; // Offset relative to parent's position

    [Header("Rotation Override Settings")]
    public bool ignoreRotationX;
    public bool ignoreRotationY;
    public bool ignoreRotationZ;
    public Vector3 rotationOffset; // Offset in euler angles relative to parent's rotation

    [Header("Locked Transform Values")]
    [Tooltip("The world position you want to lock to.")]
    public Vector3 lockedPosition;

    [Tooltip("The world rotation you want to lock to.")]
    public Quaternion lockedRotation = Quaternion.identity;

    private Transform _parent;

    private void Start()
    {
        _parent = transform.parent; // Assuming there's always a parent
        if (_parent == null)
        {
            Debug.LogWarning("OverrideParentTransform requires a parent Transform to function. Disabling script.", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 currentWorldPosition = transform.position;
        Quaternion currentWorldRotation = transform.rotation;

        if (ignorePositionX) currentWorldPosition.x = lockedPosition.x;
        if (ignorePositionY) currentWorldPosition.y = lockedPosition.y;
        if (ignorePositionZ) currentWorldPosition.z = lockedPosition.z;

        // Apply position offset
        currentWorldPosition += _parent.TransformDirection(positionOffset);

        transform.position = currentWorldPosition;

        if (ignoreRotationX || ignoreRotationY || ignoreRotationZ)
        {
            Vector3 finalEulerAngles = currentWorldRotation.eulerAngles;
            Vector3 lockedEulerAngles = lockedRotation.eulerAngles;

            if (ignoreRotationX) finalEulerAngles.x = lockedEulerAngles.x;
            if (ignoreRotationY) finalEulerAngles.y = lockedEulerAngles.y;
            if (ignoreRotationZ) finalEulerAngles.z = lockedEulerAngles.z;

            // Apply rotation offset
            finalEulerAngles += rotationOffset;

            transform.rotation = Quaternion.Euler(finalEulerAngles);
        }
    }
}
