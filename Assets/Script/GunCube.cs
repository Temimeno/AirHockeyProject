using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GunCube : NetworkBehaviour
{
    public SpawnerBulletCube spawnerBulletCube;

    private void OnCollisionEnter(Collision col)
    {
        if(!IsOwner) return;
        if(col.gameObject.tag == "Player")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            spawnerBulletCube.DestroyServerRpc(networkObjId);
        }
        if(col.gameObject.tag == "striker")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            spawnerBulletCube.DestroyServerRpc(networkObjId);
        }
    }
}
