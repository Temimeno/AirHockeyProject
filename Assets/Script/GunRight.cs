using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GunRight : NetworkBehaviour
{
    public GameObject bulletRight;
    public List<GameObject> spawnerBulletRight = new List<GameObject>();
    PlayerStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if(!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(playerStats.bulletP2.Value >= 1)
            {
                SpawnBulletRightServerRpc();
                playerStats.bulletP2.Value--;
            }
        }
    }

    [ServerRpc]
    public void SpawnBulletRightServerRpc()
    {
        Vector3 spawnPos = transform.position + (-transform.right * 1f);
        Quaternion spawnRot = transform.rotation;
        GameObject bulletRightNew = Instantiate(bulletRight, spawnPos, spawnRot);
        spawnerBulletRight.Add(bulletRightNew);
        bulletRightNew.GetComponent<BulletRight>().gunRight = this;
        bulletRightNew.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc(ulong netWorkObjId)
    {
        GameObject obj = findspawnerBulletRight(netWorkObjId);
        if(obj == null) return;
        obj.GetComponent<NetworkObject>().Despawn();
        spawnerBulletRight.Remove(obj);
        Destroy(obj);
    }

    private GameObject findspawnerBulletRight(ulong netWorkObjId)
    {
        foreach (GameObject bulletRightNew in spawnerBulletRight)
        {
            ulong bulletRightId = bulletRightNew.GetComponent<NetworkObject>().NetworkObjectId;
            if(bulletRightId == netWorkObjId)  { return bulletRightNew; }
        }
        return null;
    }
}
