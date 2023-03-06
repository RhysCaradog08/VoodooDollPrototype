using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadController : MonoBehaviour
{
    Needle needle;

    [SerializeField] LineRenderer line;

    [SerializeField] GameObject[] threads;

    public Transform player, tetherPoint;

    private void Awake()
    {
        needle = GetComponent<Needle>();
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
        if (needle.needleThrown)
        {
            line.enabled = true;
            line.SetPosition(0, player.position);
            line.SetPosition(1, tetherPoint.position);

            Debug.DrawLine(player.position, tetherPoint.position, Color.yellow);
        }
        else line.enabled = false;
    }
}
