using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    ElementalController element;

    [SerializeField] Transform mechanics, arm;
    Vector3 restPosition, attackPosition;
    Quaternion restRotation;
    [SerializeField]float attackDelay;
    [SerializeField] TrailRenderer neutral, electricty, fire, ice;

    public bool isAttacking;

    private void Awake()
    {
        element = FindObjectOfType<ElementalController>(); 
        neutral = GetComponentInChildren<TrailRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        restPosition = transform.localPosition;
        restRotation = transform.rotation;
        neutral.enabled = false;
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
            SetTrailEffect();
        }
        else
        {
            DisableTrailEffect();
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
        transform.localPosition = restPosition;
        transform.localRotation = restRotation;
    }

    void SetTrailEffect()
    {
        if (!element.hasElectricity && !element.hasFire && !element.hasIce)
        {
            neutral.enabled = true;
        }
        else neutral.enabled = false;

        if (element.hasElectricity)
        {
            electricty.enabled = true;
        }
        else electricty.enabled = false;

        if (element.hasFire)
        {
            fire.enabled = true;
        }
        else fire.enabled = false;

        if (element.hasIce)
        {
            ice.enabled = true;
        }
        else ice.enabled = false;
    }

    void DisableTrailEffect()
    {
        neutral.enabled = false;
        electricty.enabled = false;
        fire.enabled = false;
        ice.enabled = false;
    }
}
