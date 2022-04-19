using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravity = -9.81f;      
    public Transform groundchecker;
    public float groundcheckdistance = 0.4f;
    public LayerMask groundmask;

    public Vector3 velocity;
    private bool isgrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        fall();
    }

    private void fall()
    {
        // for the gravity
        isgrounded = Physics.CheckSphere(groundchecker.position, groundcheckdistance, groundmask);
        if (isgrounded)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        /*if (Input.GetButtonDown("Jump") && isgrounded)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * Jumpheight);
        }*/

        transform.position += (velocity * Time.deltaTime);
    }
}
