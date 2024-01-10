using UnityEngine;
using System.Collections.Generic;

public class ChildAxisIgnore : MonoBehaviour
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
    }

    public List<ChildLockSettings> childSettings = new List<ChildLockSettings>();

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        foreach (ChildLockSettings settings in childSettings)
        {
            if (settings.child == null)
                continue;

            Vector3 positionDifference = transform.position - lastPosition;
            Quaternion rotationDifference = Quaternion.Inverse(lastRotation) * transform.rotation;

            // Counteract position
            if (settings.ignorePositionX) settings.child.position -= new Vector3(positionDifference.x, 0, 0);
            if (settings.ignorePositionY) settings.child.position -= new Vector3(0, positionDifference.y, 0);
            if (settings.ignorePositionZ) settings.child.position -= new Vector3(0, 0, positionDifference.z);

            // Counteract rotation
            if (settings.ignoreRotationX || settings.ignoreRotationY || settings.ignoreRotationZ)
            {
                Vector3 eulerRotationDifference = rotationDifference.eulerAngles;
                Vector3 counterRotation = new Vector3(
                    settings.ignoreRotationX ? eulerRotationDifference.x : 0,
                    settings.ignoreRotationY ? eulerRotationDifference.y : 0,
                    settings.ignoreRotationZ ? eulerRotationDifference.z : 0
                );

                settings.child.rotation *= Quaternion.Euler(-counterRotation);
            }
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
}
