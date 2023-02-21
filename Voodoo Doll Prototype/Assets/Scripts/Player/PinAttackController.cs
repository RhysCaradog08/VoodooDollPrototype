using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinAttackController : MonoBehaviour
{
    Pin pin;

    //Script for setting Pin attacking bool in animation event
    private void Awake()
    {
        pin = FindObjectOfType<Pin>();
    }

    public void SetAttackBool()
    {
        pin.isAttacking = !pin.isAttacking;
    }
}
