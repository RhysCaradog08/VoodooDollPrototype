using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    Rigidbody rb;
    LineRenderer line;

    public Transform throwPoint, tetherPoint, needleHolder;
    Quaternion needleStartRotation;
    [SerializeField] float throwForce, recallSpeed, recallDistance;

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

        needleThrown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !needleThrown)
        {
            if (!needleThrown)
            {
                ThrowNeedle();
            }           
        }
        else if(Input.GetKeyDown(KeyCode.Mouse0) && needleThrown)
        {
            recallingNeedle = true;
        }

        if(recallingNeedle)
        {
            recallDistance = Vector3.Distance(transform.position, throwPoint.position);
            if (recallDistance > 0)
            {
                RecallNeedle();
            }

            if(recallDistance < 35)
            {
                recallingNeedle = false;
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
        rb.AddForce(transform.up * throwForce, ForceMode.Impulse);

        transform.parent = null;
        needleThrown = true;
    }

    void RecallNeedle()
    {
        rb.isKinematic = false;

        transform.position = Vector3.Lerp(transform.position, throwPoint.position, recallSpeed);
    }

    void ResetNeedle()
    {
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
