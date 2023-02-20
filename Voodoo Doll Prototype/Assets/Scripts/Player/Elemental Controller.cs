using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Basics.ObjectPool;

public class ElementalController : MonoBehaviour
{
    [SerializeField] Transform particlePosition;
    [SerializeField] GameObject electricity, fire, ice;
    public bool hasElectricity, hasFire, hasIce;

    // Start is called before the first frame update
    void Start()
    {
        hasElectricity = false;
        hasFire = false;
        hasIce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasElectricity)
        {
            if (electricity == null)
            {
                electricity = ObjectPoolManager.instance.CallObject("Electricty Effect", particlePosition, particlePosition.position, Quaternion.identity);
            }
        }
        else if (!hasElectricity)
        {
            if(electricity != null)
            {
                ObjectPoolManager.instance.RecallObject(electricity);
                electricity = null;
            }
        }

        if (hasFire)
        {
            if (fire == null)
            {
                fire = ObjectPoolManager.instance.CallObject("Fire Effect", particlePosition, particlePosition.position, Quaternion.identity);
            }
        }
        else if(!hasFire)
        {
            if(fire != null)
            {
                ObjectPoolManager.instance.RecallObject(fire);
                fire = null;
            }
        }

        if (hasIce)
        {
            if (ice == null)
            {
                ice = ObjectPoolManager.instance.CallObject("Ice Effect", particlePosition, particlePosition.position, Quaternion.identity);
            }
        }
        else if (!hasIce)
        {
            if (ice != null)
            {
                ObjectPoolManager.instance.RecallObject(ice);
                ice = null;
            }
        }
    }
}
