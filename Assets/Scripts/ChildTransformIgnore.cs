using UnityEngine;
using System.Collections.Generic;

public class ChildTransformIgnore : MonoBehaviour
{
    [System.Serializable]
    public class ChildLockSettings
    {
        public Transform child;
        public bool ignorePositionX;
        public bool ignorePositionY;
        public bool ignorePositionZ;
        public bool ignoreRotationX;
        public bool ignoreRotationY;
        public bool ignoreRotationZ;

        // These fields store the locked world transformation
        [Tooltip("The world position you want to lock to.")]
        public Vector3 lockedPosition;

        [Tooltip("The world rotation you want to lock to.")]
        public Quaternion lockedRotation = Quaternion.identity;
    }

    public List<ChildLockSettings> childSettings = new List<ChildLockSettings>();

    private void LateUpdate()
    {
        foreach (ChildLockSettings settings in childSettings)
        {
            if (settings.child == null)
                continue;

            Vector3 currentWorldPosition = settings.child.position;
            Quaternion currentWorldRotation = settings.child.rotation;

            if (settings.ignorePositionX) currentWorldPosition.x = settings.lockedPosition.x;
            if (settings.ignorePositionY) currentWorldPosition.y = settings.lockedPosition.y;
            if (settings.ignorePositionZ) currentWorldPosition.z = settings.lockedPosition.z;

            settings.child.position = currentWorldPosition;

            if (settings.ignoreRotationX || settings.ignoreRotationY || settings.ignoreRotationZ)
            {
                Vector3 finalEulerAngles = currentWorldRotation.eulerAngles;
                Vector3 lockedEulerAngles = settings.lockedRotation.eulerAngles;

                if (settings.ignoreRotationX) finalEulerAngles.x = lockedEulerAngles.x;
                if (settings.ignoreRotationY) finalEulerAngles.y = lockedEulerAngles.y;
                if (settings.ignoreRotationZ) finalEulerAngles.z = lockedEulerAngles.z;

                settings.child.rotation = Quaternion.Euler(finalEulerAngles);
            }
        }
    }
}
