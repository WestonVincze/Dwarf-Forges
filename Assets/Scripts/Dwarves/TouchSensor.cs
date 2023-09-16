using UnityEngine;

public class TouchSensor : MonoBehaviour
{
    public Transform parentOverride;
    private void OnTriggerEnter(Collider collider)
    {
        if (parentOverride)
        {
            parentOverride.SendMessage(gameObject.name, collider);
        }
        else
        {
            SendMessageUpwards(gameObject.name, collider);
        }
    }
}
