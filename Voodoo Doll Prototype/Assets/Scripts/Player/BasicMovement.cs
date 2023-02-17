using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField]CharacterController cc;
    [SerializeField]Animator playerAnim;

    public float moveSpeed, rotationSpeed;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        playerAnim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movePlayer = new Vector3(moveX, 0, moveZ).normalized;

        if (movePlayer.magnitude > 0)
        {
            playerAnim.SetBool("IsMoving", true);
        }
        else playerAnim.SetBool("IsMoving", false);

        cc.Move(movePlayer * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(movePlayer);
    }
}
