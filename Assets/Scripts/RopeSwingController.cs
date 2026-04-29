using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwingController : MonoBehaviour
{
    public LayerMask ropeLayer;
    public float swingSpeed = 5f;
    public float climbSpeed = 2f;
    public KeyCode swingKey = KeyCode.Space;
    public KeyCode climbUpKey = KeyCode.W;
    public KeyCode climbDownKey = KeyCode.S;
    public KeyCode jumpOffKey = KeyCode.Space;

    private DistanceJoint2D distanceJoint;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private Transform ropePoint;
    private bool isSwinging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distanceJoint = gameObject.AddComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void Update()
    {
        if (Input.GetKeyDown(swingKey))
        {
            if (!isSwinging)
            {
                DetectRope();
            }
            else
            {
                JumpOff();
            }
        }

        if (isSwinging)
        {
            if (Input.GetKey(climbUpKey))
            {
                distanceJoint.distance -= climbSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(climbDownKey))
            {
                distanceJoint.distance += climbSpeed * Time.deltaTime;
            }
        }
    }

    void DetectRope()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, ropeLayer);
        if (hit.collider != null)
        {
            ropePoint = hit.transform;
            distanceJoint.connectedAnchor = ropePoint.position;
            distanceJoint.autoConfigureDistance = false;
            distanceJoint.distance = Vector2.Distance(transform.position, ropePoint.position);
            distanceJoint.enableCollision = true;
            distanceJoint.enabled = true;
            rb.gravityScale = 1;
            isSwinging = true;
        }
    }

    void JumpOff()
    {
        distanceJoint.enabled = false;
        rb.gravityScale = 1;
        isSwinging = false;
    }

    void LateUpdate()
    {
        if (isSwinging)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, ropePoint.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
