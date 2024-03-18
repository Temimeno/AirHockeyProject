using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Bullet : NetworkBehaviour
{
    public Gun gun;
    public float bulletspeed;
    public float pushstriker;
    public float time;
    public float timeDespawn;

    Rigidbody strikerRigidbody;

    void Update()
    {
        strikerRigidbody = GameObject.FindGameObjectWithTag("striker").GetComponent<Rigidbody>();
        time += Time.deltaTime;
        if(time > timeDespawn)
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            gun.DestroyServerRpc(networkObjId);
            time = 0f;
        }
    }

    void FixedUpdate()
    {
        transform.position += transform.right * bulletspeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        if(!IsOwner) return;
        if(col.gameObject.tag == "striker")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            gun.DestroyServerRpc(networkObjId);

            Vector3 direction = col.contacts[0].point - transform.position;
            direction = direction.normalized;
            strikerRigidbody.velocity = direction * pushstriker;

        }
        if(col.gameObject.tag == "GunCube")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            gun.DestroyServerRpc(networkObjId);
        }
        if(col.gameObject.tag == "Player2")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            gun.DestroyServerRpc(networkObjId);
        }

        
    }
    

    
}
