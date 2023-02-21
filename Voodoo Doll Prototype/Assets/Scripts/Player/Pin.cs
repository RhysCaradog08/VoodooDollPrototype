using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] Transform mechanics, arm;
    Vector3 restPosition, attackPosition;
    Quaternion restRotation;
    [SerializeField]float attackDelay;
    TrailRenderer trail;

    public bool isAttacking;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        restPosition = transform.position;
        restRotation = transform.rotation;
        trail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
        }
        if (attackDelay <= 0)
        {
            attackDelay = 0;
            //isAttacking = false;
        }

        if(Input.GetKeyDown(KeyCode.Mouse2))
        {
            if(!isAttacking && attackDelay <=0)
            {
                //isAttacking = true;
                attackDelay = 1;
            }
        }

        if(isAttacking)
        {
            SetAttackPosition();
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
            ResetPinPosition();
        }
    }

    void SetAttackPosition()
    {
        transform.parent = arm;
        transform.localPosition = new Vector3(0, - 2.25f, 0);
        transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }

    void ResetPinPosition()
    {
        transform.parent = mechanics;
        transform.position = restPosition;
        transform.rotation = restRotation;
    }
}
