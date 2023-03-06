using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadController : MonoBehaviour
{
    Needle needle;
    ElementalController element;

    [SerializeField] LineRenderer line;

    [SerializeField] GameObject[] threads;

    public Transform player, tetherPoint;

    private void Awake()
    {
        needle = GetComponent<Needle>();
        element = FindObjectOfType<ElementalController>();
        line = threads[0].GetComponentInChildren<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        line.enabled = false;

        player = transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        if(!element.hasElectricity && !element.hasFire && !element.hasIce)
        {
            line.enabled = false; //Disables previous Line Renderer.

            line = threads[0].GetComponentInChildren<LineRenderer>(); //sets Line Renderer to be whatever elemeent the player is/is not thethered to.
            line.enabled = true; //Enables new Line Renderer.
        }

        if(element.hasElectricity)
        {
            line.enabled = false;

            line = threads[1].GetComponentInChildren<LineRenderer>();
            line.enabled = true;
        }

        if(element.hasFire)
        {
            line.enabled = false;

            line = threads[2].GetComponentInChildren<LineRenderer>();
            line.enabled = true;
        }

        if(element.hasIce)
        {
            line.enabled = false;

            line = threads[3].GetComponentInChildren<LineRenderer>();
            line.enabled = true;
        }

        if (needle.needleThrown)
        {
            line.enabled = true;
            line.SetPosition(0, player.position);
            line.SetPosition(1, tetherPoint.position);

            //Debug.DrawLine(player.position, tetherPoint.position, Color.yellow);
        }
        else line.enabled = false;
    }
}
