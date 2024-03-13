using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MainPlayerMovement : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 10.0f;

    public float strikerspeed;
    Rigidbody rb;
    Rigidbody strikerRigidbody;

    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        if(IsOwner)
        {
            float horizontalMovement = Input.GetAxis("Horizontal") * speed;
            float verticalMovement = Input.GetAxis("Vertical") * speed;

            rb.velocity = new Vector3 (horizontalMovement, 0, verticalMovement);
        }
        strikerRigidbody = GameObject.FindGameObjectWithTag("striker").GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "striker")
        {
            Vector3 direction = col.contacts[0].point - transform.position;
            direction = direction.normalized; // Reverse the direction
            strikerRigidbody.velocity = direction * strikerspeed;        
        }
    }

}
