using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleController : MonoBehaviour
{
    public GameObject needle1, needle2;
    Needle needleScript1, needleScript2;

    // Start is called before the first frame update
    void Start()
    {
        needleScript1 = needle1.GetComponent<Needle>();
        needleScript2 = needle2.GetComponent<Needle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) //Operates Needle1
        {
            if(!needleScript1.needleThrown)
            {
                needleScript1.FindTargets();
                needleScript1.FindClosestTarget();
                needleScript1.ThrowNeedle();
            }
            else if (needleScript1.needleThrown && needleScript1.rb.isKinematic)
            {
                needleScript1.recallingNeedle = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) //Operates Needle2
        {
            if (!needleScript2.needleThrown)
            {
                needleScript2.FindTargets();
                needleScript2.FindClosestTarget();
                needleScript2.ThrowNeedle();
            }
            else if (needleScript2.needleThrown && needleScript2.rb.isKinematic)
            {
                needleScript2.recallingNeedle = true;
            }
        }
    }
}
