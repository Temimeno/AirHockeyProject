using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Gun : NetworkBehaviour
{
    public GameObject bullet;
    public List<GameObject> spawnerBullet = new List<GameObject>();

    BulletState bulletState;
    
    void Start()
    {
        bulletState = GetComponent<BulletState>();
    }

    void Update()
    {
        if(!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(bulletState.bulletP1.Value >= 1)
            {
                SpawnBulletServerRpc();
                bulletState.bulletP1.Value--;
            }
        }
    }

    [ServerRpc]
    public void SpawnBulletServerRpc()
    {
        Vector3 spawnPos = transform.position + (transform.right * 1f);
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
