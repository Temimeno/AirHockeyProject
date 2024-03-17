using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class GunCube : NetworkBehaviour
{
    public SpawnerBulletCube spawnerBulletCube;
    public RoomManager_SpawnStriker roomManager_SpawnStriker;

    void Update()
    {
        roomManager_SpawnStriker = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager_SpawnStriker>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            spawnerBulletCube.DestroyServerRpc(networkObjId);
            
            roomManager_SpawnStriker.IncreaseBulletPlayer1();
        }
        if(col.gameObject.tag == "Player2")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            spawnerBulletCube.DestroyServerRpc(networkObjId);

            roomManager_SpawnStriker.IncreaseBulletPlayer2();
        }
        if(col.gameObject.tag == "striker")
        {
            ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
            spawnerBulletCube.DestroyServerRpc(networkObjId);
        }
    }
}
