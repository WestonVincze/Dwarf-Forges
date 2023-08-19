using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSensor : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        SendMessageUpwards(gameObject.name, collider);
    }
}
