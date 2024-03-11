using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAttackStriker : MonoBehaviour
{
    public float strikerspeed = 10f;
    Rigidbody strikerRigidbody;

    void Start()
    {
        strikerRigidbody = GameObject.FindGameObjectWithTag("striker").GetComponent<Rigidbody>();

    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "striker")
        {
            //strikerRigidbody.velocity = new Vector3(strikerspeed, strikerRigidbody.velocity.y, strikerspeed);
        }
    }
}
