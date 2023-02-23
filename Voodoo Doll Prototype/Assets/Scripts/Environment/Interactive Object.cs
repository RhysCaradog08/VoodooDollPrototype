using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Transform particlePosition;

    public bool canAddElement, needsElectricty, needsFire, needsIce;

    // Start is called before the first frame update
    void Start()
    {
        if(particlePosition == null)
        {
            particlePosition = this.transform;
        }

        canAddElement = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
