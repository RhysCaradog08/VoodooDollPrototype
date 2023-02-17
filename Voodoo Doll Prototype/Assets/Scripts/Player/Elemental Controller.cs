using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Basics.ObjectPool;

public class ElementalController : MonoBehaviour
{
    [SerializeField] Transform particlePosition;
    public GameObject electricity, fire, ice;
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
                electricity = ObjectPoolManager.instance.CallObject("Electricty Effect", particlePosition, particlePosition.position, Quaternion.identity);
        }

        if(hasFire)
        {
                fire = ObjectPoolManager.instance.CallObject("Fire Effect", particlePosition, particlePosition.position, Quaternion.identity);
        }
        
        if (hasIce)
        {
                ice = ObjectPoolManager.instance.CallObject("Ice Effect", particlePosition, particlePosition.position, Quaternion.identity);
        }
    }
}
