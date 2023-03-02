using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Basics;

public class LightBulb : InteractiveObject
{
    public Light bulbLight;

    ParticleSystem ps;
    
    // Start is called before the first frame update
    void Start()
    {
        bulbLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAddElement)
        {
            bulbLight.enabled = true;

            ps = GetComponentInChildren<ParticleSystem>(); 
        }

        if(!canAddElement && ps != null)
        {
            Basics.ObjectPool.ObjectPoolManager.instance.RecallObject(ps.gameObject);
        }
    }
}
