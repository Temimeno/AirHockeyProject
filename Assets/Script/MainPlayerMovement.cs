using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainPlayerMovement : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float strikerspeed = 100f;

    GameObject st;
    Rigidbody rb;
    Rigidbody striker;

    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        st = GameObject.FindGameObjectWithTag("striker");
        striker = GameObject.FindGameObjectWithTag("striker").GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        if(IsOwner)
        {
            float horizontalMovement = Input.GetAxis("Horizontal") * speed;
            float verticalMovement = Input.GetAxis("Vertical") * speed;

            rb.velocity = new Vector3 (horizontalMovement, 0, verticalMovement);
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "striker")
        {
            
        }
    }
}
