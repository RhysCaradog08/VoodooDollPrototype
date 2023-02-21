using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinAttackController : MonoBehaviour
{
    Pin pin;

    private void Awake()
    {
        pin = FindObjectOfType<Pin>();
    }

    public void SetAttackBool()
    {
        pin.isAttacking = !pin.isAttacking;
    }
}
