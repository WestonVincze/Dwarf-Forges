using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaEffector : MonoBehaviour
{
    public Vector3 force;
    private List<Rigidbody> effectedObjects = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        RigidbodyPointer rbPointer = other.GetComponent<RigidbodyPointer>();
        Rigidbody rb;
        if (rbPointer)
        {
            rb = rbPointer.rb;
        }
        else
        {
            rb = other.GetComponentInParent<Rigidbody>();
        }

        if (rb) 
        {
            if (effectedObjects.Contains(rb))
                return;

            effectedObjects.Add(rb);
        }

        DestroyableMonoBehavior destroyable = other.GetComponentInParent<DestroyableMonoBehavior>();
        if (destroyable != null)
            destroyable.OnDestroyEvent += () => effectedObjects.Remove(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        RigidbodyPointer rbPointer = other.GetComponent<RigidbodyPointer>();
        Rigidbody rb;
        if (rbPointer)
        {
            rb = rbPointer.rb;
        }
        else
        {
            rb = other.GetComponentInParent<Rigidbody>();
        }

        if (rb != null)
            effectedObjects.Remove(rb);

        DestroyableMonoBehavior destroyable = other.GetComponentInParent<DestroyableMonoBehavior>();
        if (destroyable != null)
            destroyable.OnDestroyEvent += () => effectedObjects.Remove(rb);
    }

    public void FixedUpdate()
    {
        foreach (var rb in effectedObjects)
        {
            rb.AddForce(force);
        }
    }
}
