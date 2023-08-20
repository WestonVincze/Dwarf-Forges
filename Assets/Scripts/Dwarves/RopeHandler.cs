using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeHandler : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private SpringJoint rope;
    private Transform target;
    private Collider targetCollider;
    private Vector3 targetOffset;
    private bool pulling = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void StartPullObject(Collider collider, float ropeLength = 3)
    {
        target = collider.transform;
        targetCollider = collider;
        rope = gameObject.AddComponent<SpringJoint>();
        rope.connectedBody = collider.gameObject.GetComponent<Rigidbody>();
        rope.maxDistance = ropeLength;
        rope.enableCollision = true;
        rope.autoConfigureConnectedAnchor = false;

        lineRenderer.enabled = true;

        pulling = true;
    }

    public void StopPullObject()
    {
        lineRenderer.enabled = false;
        if(rope != null)
        {
            Destroy(rope);
        }

        pulling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pulling)
            return;

        //Theoretically I don't need to use any ray casts if I already know what I'm attached
        //to. I can just find the nearest point on the collider.
        /*
        RaycastHit hit;

        if (targetCollider.Raycast(new Ray(transform.position, Vector3.back), out hit, 2000))
        {
            targetOffset = hit.point - target.position;
        }

        Vector3 visualPulledTargetOffset = targetOffset * 0.9f; //Add a bit of a buffer, so the rope looks like it's inside the object
        //visualPulledTargetOffset.y = targetCollider.bounds.center.y - (targetCollider.bounds.size.y / 2); //Attach visual to the bottom edge of the forge
        //visualPulledTargetOffset.y = targetCollider.bounds.min.y;
        Vector3 connectedRopeAnchorPos = target.position + visualPulledTargetOffset;
        connectedRopeAnchorPos.y = targetCollider.bounds.min.y;
        */

        Vector3 connectedRopeAnchorPos = targetCollider.ClosestPointOnBounds(new Vector3(transform.position.x, targetCollider.bounds.min.y - 0.1f, transform.position.z));
        targetOffset = connectedRopeAnchorPos - target.position;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, connectedRopeAnchorPos);

        Vector3 dist = connectedRopeAnchorPos - transform.position;

        for (int i = 1; i < lineRenderer.positionCount - 1; i++)
        {
            lineRenderer.SetPosition(i, transform.position + (dist * lineRenderer.colorGradient.colorKeys[i].time));
        }
    }

    private void FixedUpdate()
    {
        if (!pulling)
            return;
        
        rope.connectedAnchor = targetOffset;
    }
}
