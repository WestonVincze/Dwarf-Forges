using UnityEngine;
using System.Collections.Generic;

public class ChildTransformManager : MonoBehaviour
{
    [System.Serializable]
    public class ChildLockSettings
    {
        public Transform child;
        public bool matchPositionX = true;
        public bool matchPositionY = true;
        public bool matchPositionZ = true;
        public Vector3 positionOffset; // Offset relative to the matched parent's position

        public bool matchRotationX = true;
        public bool matchRotationY = true;
        public bool matchRotationZ = true;
        public Vector3 rotationOffset; // Rotation offset in euler angles relative to the matched parent's rotation
    }

    public List<ChildLockSettings> childSettings = new List<ChildLockSettings>();

    private void Start()
    {
        // Unparent all specified children
        foreach (ChildLockSettings settings in childSettings)
        {
            if (settings.child != null)
            {
                settings.child.SetParent(null, true);
            }
        }
    }

    private void LateUpdate()
    {
        foreach (ChildLockSettings settings in childSettings)
        {
            if (settings.child == null)
                continue;

            Vector3 parentPosition = transform.position;
            Vector3 childPosition = settings.child.position;

            if (settings.matchPositionX) childPosition.x = parentPosition.x + settings.positionOffset.x;
            if (settings.matchPositionY) childPosition.y = parentPosition.y + settings.positionOffset.y;
            if (settings.matchPositionZ) childPosition.z = parentPosition.z + settings.positionOffset.z;

            settings.child.position = childPosition;

            Vector3 parentEulerAngles = transform.rotation.eulerAngles;
            Vector3 childEulerAngles = settings.child.rotation.eulerAngles;

            if (settings.matchRotationX) childEulerAngles.x = parentEulerAngles.x + settings.rotationOffset.x;
            if (settings.matchRotationY) childEulerAngles.y = parentEulerAngles.y + settings.rotationOffset.y;
            if (settings.matchRotationZ) childEulerAngles.z = parentEulerAngles.z + settings.rotationOffset.z;

            settings.child.rotation = Quaternion.Euler(childEulerAngles);
        }
    }

    private void OnDestroy()
    {
        // Destroy all specified children when parent is destroyed
        foreach (ChildLockSettings settings in childSettings)
        {
            if (settings.child != null)
            {
                Destroy(settings.child.gameObject);
            }
        }
    }
}
