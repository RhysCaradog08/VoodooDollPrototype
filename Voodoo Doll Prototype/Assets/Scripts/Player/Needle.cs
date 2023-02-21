using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Basics.ObjectPool;

public class Needle : MonoBehaviour
{
    ElementalController elementControl;
    [SerializeField] InteractiveObject interactObj;
    [SerializeField] Needle otherNeedle;

    public Rigidbody rb;
    LineRenderer line;

    [Header("Throw_RecallNeedle")]
    public Transform throwPoint, tetherPoint, needleHolder, player;
    Quaternion needleStartRotation;
    [SerializeField] float throwForce, moveSpeed, moveDistance, startTime, smoothing;
    public bool needleThrown, recallingNeedle, isTethered;

    [Header("FindTargets")]
    [SerializeField] Transform target;
    [SerializeField] List<Transform> targetLocations = new List<Transform>();
    Collider[] hitColliders;
    [SerializeField] float targetRadius;

    [Header("Elemental Effects")]
    [SerializeField] ParticleSystem elementFX;
    GameObject electricity, fire, ice;
    Transform particlePosition;

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
            moveDistance = Vector3.Distance(transform.position, throwPoint.position);
            //Debug.Log("Recall Distance: " + recallDistance);

            RecallNeedle();

            if(moveDistance < 1)
            {
                ResetNeedle();
            }
        }

        if (needleThrown)
        {
            if (target != null) //Throws Needle towards designated target
            {
                //Debug.Log("Has Target");
                moveDistance = Vector3.Distance(transform.position, target.position);

                if (moveDistance > 0.01)
                {
                    CalculateMoveDistance();
                    transform.position = Vector3.Lerp(transform.position, target.position, smoothing);
                }
            }

            line.enabled = true;
            line.SetPosition(0, player.position);
            line.SetPosition(1, tetherPoint.position);
        }
        else line.enabled = false;
        
        if(isTethered)
        {
            if(!otherNeedle.isTethered)
            {
                if (interactObj != null)
                {
                    RemoveElementFromObject();
                }
            }
            
            if(otherNeedle.isTethered) 
            {
                if (interactObj.canAddElement)
                {
                    AddElementToObject();
                }
            }
        }

        foreach (Transform t in targetLocations)
        {
            Debug.DrawLine(throwPoint.position, t.position, Color.red);
        }
    }

    public void ThrowNeedle()
    {
        transform.position = throwPoint.position;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        rb.isKinematic = false;

        if (target == null) // If no target is available throw Needle in Players forward vector 
        {
            //Debug.Log("No Target");
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

        CalculateMoveDistance();
        transform.position = Vector3.Lerp(transform.position, throwPoint.position, smoothing);
    }

    void ResetNeedle() //Resets Needle back to needleHolder and rotates it accordingly
    {
        recallingNeedle = false;

        transform.parent = needleHolder;
        transform.position = needleHolder.position;
        transform.rotation = needleStartRotation;
        rb.isKinematic = true;

        moveDistance = 0;
        smoothing = 0;

        needleThrown = false;
    }

    void CalculateMoveDistance()
    {
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        smoothing = distanceCovered / moveDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (needleThrown) //Needle sticks in whatever object it collides with
        {
            rb.isKinematic = true;
            isTethered = true;
        }

        if (other.gameObject.tag == "Target") //Needle becomes a child of whatever Target it collides with
        {
            transform.parent = other.transform;

            if (other.gameObject.GetComponentInChildren<ParticleSystem>())
            {
                //Debug.Log("Elemental Effect in children");
                elementFX = other.gameObject.GetComponentInChildren<ParticleSystem>();

                GetElementalEffect();
            }

            if (other.gameObject.GetComponent<InteractiveObject>())
            {
                interactObj = other.gameObject.GetComponent<InteractiveObject>();
                particlePosition = other.transform;

                /*if (interactObj.canAddElement && otherNeedle.isTethered)
                    {
                        particlePosition = other.transform;
                        AddElementToObject();
                    }*/
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isTethered)
        {
            isTethered = false;
        }

        if (other.gameObject.tag == "Target")
        {
            if (interactObj != null)
            {
                RemoveElementFromObject();
                particlePosition = null;
                interactObj = null;
            }

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
            //Debug.Log("Has Electricity " + elementControl.hasElectricity);
            elementControl.hasElectricity = true;
        }

        if (elementFX.transform.parent.tag == "Fire")
        {
            //Debug.Log("Has Fire" + elementControl.hasFire);
            elementControl.hasFire = true;
        }

        if (elementFX.transform.parent.tag == "Ice")
        {
           //Debug.Log("Has Ice" + elementControl.hasIce);
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

    void AddElementToObject()
    {
        if(elementControl.hasElectricity)
        {
            if (electricity == null)
            {
                electricity = ObjectPoolManager.instance.CallObject("Electricty Effect", particlePosition, particlePosition.position, Quaternion.identity);
            }
        }

        if (elementControl.hasFire)
        {
            if (fire == null)
            {
                fire = ObjectPoolManager.instance.CallObject("Fire Effect", particlePosition, particlePosition.position, Quaternion.identity);
            }
        }

        if(elementControl.hasIce)
        {
            if (ice == null)
            {
                ice = ObjectPoolManager.instance.CallObject("Ice Effect", particlePosition, particlePosition.position, Quaternion.identity);
            }
        }
    }

    void RemoveElementFromObject()
    {
        if (electricity != null)
        {
            ObjectPoolManager.instance.RecallObject(electricity);
            electricity = null;
        }

        if (fire != null)
        {
            ObjectPoolManager.instance.RecallObject(fire);
            fire = null;
        }

        if (ice != null)
        {
            ObjectPoolManager.instance.RecallObject(ice);
            ice = null;
        }
    }

    public void FindTargets() //Generates an OverlapSphere to try and find "targets" within a radius
    {
        //Debug.Log("Find Targets");
        hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + targetRadius), targetRadius);

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
        //Debug.Log("Find Closest Target");
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
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + targetRadius), targetRadius);
    }
}
