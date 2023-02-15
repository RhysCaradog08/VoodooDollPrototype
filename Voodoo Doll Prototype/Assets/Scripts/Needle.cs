using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    Rigidbody rb;
    LineRenderer line;

    public Transform throwPoint, tetherPoint, needleHolder;
    Quaternion needleStartRotation;
    [SerializeField] float throwDistance, throwForce, recallSpeed, recallDistance, startTime;
    public LayerMask ignoreLayer;

    [SerializeField] bool needleThrown, recallingNeedle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
        Physics.IgnoreLayerCollision(6, 7);

        line.enabled = false;

        transform.position = needleHolder.position;
        needleStartRotation = transform.rotation;

        startTime = 0;

        needleThrown = false;
        recallingNeedle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!needleThrown)
            {
                ThrowNeedle();
            }  
            else if(needleThrown && rb.isKinematic)
            {
                recallingNeedle = true;
            }
        }

        if (recallingNeedle)
        {
            recallDistance = Vector3.Distance(transform.position, throwPoint.position);
            //Debug.Log("Recall Distance: " + recallDistance);

            RecallNeedle();

            if(recallDistance < 1)
            {
                ResetNeedle();
            }
        }

        if (needleThrown)
        {
            line.enabled = true;
            line.SetPosition(0,throwPoint.position);
            line.SetPosition(1, tetherPoint.position);
        }
        else line.enabled = false;
    }

    void ThrowNeedle()
    {
        transform.position = throwPoint.position;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        rb.isKinematic = false;

        RaycastHit hit;
        if (Physics.Raycast(throwPoint.position, throwPoint.TransformDirection(Vector3.forward), out hit, throwDistance, ~ignoreLayer))
        {
            Debug.Log("Raycast Hit");
            Debug.DrawRay(throwPoint.position, throwPoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Vector3 throwDirection = hit.point - transform.position;

            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.up * throwForce, ForceMode.Impulse);
        }

        transform.parent = null;
        needleThrown = true;
    }

    void RecallNeedle()
    {
        rb.isKinematic = false;

        float distanceCovered = (Time.time - startTime) * recallSpeed;
        float smoothing = distanceCovered / recallDistance;
        transform.position = Vector3.Lerp(transform.position, throwPoint.position, smoothing);
    }

    void ResetNeedle()
    {
        recallingNeedle = false;

        transform.parent = needleHolder;
        transform.position = needleHolder.position;
        transform.rotation = needleStartRotation;
        rb.isKinematic = true;

        needleThrown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (needleThrown)
        {
            rb.isKinematic = true;
        }
    }
}
