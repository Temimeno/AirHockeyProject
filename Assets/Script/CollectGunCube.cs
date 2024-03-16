using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CollectGunCube : NetworkBehaviour
{
    public int Bullet = 0;
    public GameObject Gun;
    public SpawnerBulletCube spawnerBulletCube;

    void Update()
    {
        if(Bullet == 1)
        {
            Gun.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "GunCube")
        {
            if(Bullet < 2)
            {
                Bullet++;
                ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
                spawnerBulletCube.DestroyServerRpc(networkObjId);
            }
        }
    }
}
