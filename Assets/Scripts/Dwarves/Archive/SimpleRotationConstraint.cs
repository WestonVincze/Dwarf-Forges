using UnityEngine;

public class SimpleRotationConstraint : MonoBehaviour
{
    public enum Perspective
    {
        Local,
        World
    }

    public Vector3 rotation;
    public Perspective perspective = Perspective.World;

    void LateUpdate()
    {
        if(perspective == Perspective.World)
        {
            transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(rotation);
        }
    }
}
