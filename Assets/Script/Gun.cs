using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Gun : NetworkBehaviour
{
    public GameObject bullet;
    public List<GameObject> spawnerBullet = new List<GameObject>();
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
            if(playerStats.bulletP1.Value >= 1)
            {
                SpawnBulletServerRpc();
                playerStats.bulletP1.Value--;
            }
        }
    }

    [ServerRpc]
    public void SpawnBulletServerRpc()
    {
        Vector3 spawnPos = transform.position + (transform.forward * -1.5f) + (transform.up * 1.5f);
        Quaternion spawnRot = transform.rotation;
        GameObject bulletNew = Instantiate(bullet, spawnPos, spawnRot);
        spawnerBullet.Add(bulletNew);
        bulletNew.GetComponent<Bullet>().gun = this;
        bulletNew.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc(ulong networkObjId)
    {
        GameObject obj = findspawnerBullet(networkObjId);
        if(obj == null) return;
        obj.GetComponent<NetworkObject>().Despawn();
        spawnerBullet.Remove(obj);
        Destroy(obj);
    }

    private GameObject findspawnerBullet(ulong netWorkObjId)
    {
        foreach (GameObject bulletNew in spawnerBullet)
        {
            ulong bulletID = bulletNew.GetComponent<NetworkObject>().NetworkObjectId;
            if(bulletID == netWorkObjId) { return bulletNew; }
        }
        return null;
    }
}
