using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Basics.ObjectPool;

public class Needle : MonoBehaviour
{
    ElementalController elementControl;

    public Rigidbody rb;
    LineRenderer line;

    [Header("Throw_RecallNeedle")]
    public Transform throwPoint, tetherPoint, needleHolder;
    Quaternion needleStartRotation;
    [SerializeField] float throwForce, recallSpeed, recallDistance, startTime;
    public LayerMask ignoreLayer;
    public bool needleThrown, recallingNeedle, isTethered;

    [Header("FindTargets")]
    [SerializeField] Transform target;
    [SerializeField] List<Transform> targetLocations = new List<Transform>();
    Collider[] hitColliders;
    [SerializeField] float targetRadius;

    [Header("Elemental Effects")]
    [SerializeField] ParticleSystem elementFX;

    private void Awake()
    {
        elementControl = FindObjectOfType<ElementalController>();

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
        isTethered = false;
    }

    // Update is called once per frame
    void Update()
    {
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

        foreach(Transform t in targetLocations)
        {
            Debug.DrawLine(throwPoint.position, t.position, Color.red);
        }
    }

    public void ThrowNeedle()
    {
        transform.position = throwPoint.position;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        rb.isKinematic = false;


        if (target != null) //Throws Needle towards designated target
        {
            Vector3 throwDirection = target.position - transform.position;

            rb.AddForce(throwDirection * 5, ForceMode.Impulse);
        }
        else // If no target is available throw Needle in Players forwrad vector 
        {
            rb.AddForce(transform.up * throwForce, ForceMode.Impulse);
        }

        transform.parent = null;
        needleThrown = true;
    }

    public void RecallNeedle() //Lerps Needle back to throwPoint over time
    {
        targetLocations.Clear();
        target = null;
        transform.parent = null;
        rb.isKinematic = false;

        float distanceCovered = (Time.time - startTime) * recallSpeed;
        float smoothing = distanceCovered / recallDistance;
        transform.position = Vector3.Lerp(transform.position, throwPoint.position, smoothing);
    }

    void ResetNeedle() //Resets Needle back to needleHolder and rotates it accordingly
    {
        recallingNeedle = false;

        transform.parent = needleHolder;
        transform.position = needleHolder.position;
        transform.rotation = needleStartRotation;
        rb.isKinematic = true;

        needleThrown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (needleThrown) //Needle sticks in whatever object it collides with
        {
            rb.isKinematic = true;
            //isTethered = true;
        }

        if (other.gameObject.tag == "Target") //Needle becomes a child of whatever Target it collides with
        {
            transform.parent = other.transform;

            if (other.gameObject.GetComponentInChildren<ParticleSystem>())
            {
                Debug.Log("Elemental Effect in children");
                elementFX = other.gameObject.GetComponentInChildren<ParticleSystem>();

                GetElementalEffect();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if(isTethered)
        {
            isTethered = false;
        }*/

        if(other.gameObject.tag == "Target")
        {
            if (other.gameObject.GetComponentInChildren<ParticleSystem>())
            {
                RemoveElementalEffect();
                elementFX = null;
            }
        }
    }

    void GetElementalEffect()
    {
        if (elementFX.transform.parent.tag == "Electricity")
        {
            Debug.Log("Has Electricity " + elementControl.hasElectricity);
            elementControl.hasElectricity = true;
        }

        if (elementFX.transform.parent.tag == "Fire")
        {
            Debug.Log("Has Fire" + elementControl.hasFire);
            elementControl.hasFire = true;
        }

        if (elementFX.transform.parent.tag == "Ice")
        {
            Debug.Log("Has Ice" + elementControl.hasIce);
            elementControl.hasIce = true;
        }
    }

    void RemoveElementalEffect()
    {
        if (elementFX.transform.parent.tag == "Electricity")
        {
            elementControl.hasElectricity = false;
        }

        if (elementFX.transform.parent.tag == "Fire")
        {
            elementControl.hasFire = false;
        }

        if (elementFX.transform.parent.tag == "Ice")
        {
            elementControl.hasIce = false;
        }
    }

    public void FindTargets() //Generates an OverlapSphere to try and find "targets" within a radius
    {
        hitColliders = Physics.OverlapSphere(transform.position, targetRadius);

        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Target")
            {
                if (!targetLocations.Contains(col.transform) && !col.transform.GetComponentInChildren<Needle>())
                {
                    targetLocations.Add(col.transform);
                }
            }
        }
    }

    public void FindClosestTarget() //Sorts targets in targetLocations to determine which target is closest to the player
    {
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.root.position;
        foreach (Transform potentialTarget in targetLocations)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = potentialTarget;
            }
        }

        target = closestTarget;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
    }
}
