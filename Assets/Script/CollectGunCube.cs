using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CollectGunCube : NetworkBehaviour
{
    public int Bullet = 0;
    void Update()
    {
        if(Bullet == 1)
        {
            
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "GunCube")
        {
            if(Bullet < 2)
            {
                Bullet++;
            }
        }
    }
}
