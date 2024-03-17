using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnerBulletCube : NetworkBehaviour
{
    public GameObject GunCube;
    public GameObject scoreManager;
    public float DelayspawnGuncube;
    public float time = 0f;
    public float timespawn = 5f;
    public List<GameObject> spawnerGunCube = new List<GameObject>();
    
    PlayerStats playerStats;

    public void Update()
    {
        if(scoreManager.activeInHierarchy)
        {
            time += Time.deltaTime;
            if(time >= timespawn)
            {
                StartCoroutine(SpawnGunCube());
                time = 0f;
                StopCoroutine(SpawnGunCube());
            }
        }
    }
    
    [ServerRpc]
    public void SpawnGunCubeServerRpc()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-6,7), 0, Random.Range(-3,3));
        Quaternion spawnRot = transform.rotation;
        GameObject GunCubeNew = Instantiate(GunCube, spawnPos, spawnRot);
        spawnerGunCube.Add(GunCubeNew);
        GunCubeNew.GetComponent<GunCube>().spawnerBulletCube = this;
        GunCubeNew.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc(ulong networkObjId)
    {
        GameObject obj = findSpawnerGunCube(networkObjId);
        if(obj == null) return;
        obj.GetComponent<NetworkObject>().Despawn();
        spawnerGunCube.Remove(obj);
        Destroy(obj);
    }

    private GameObject findSpawnerGunCube(ulong netWorkObjId)
    {
        foreach (GameObject GunCubeNew in spawnerGunCube)
        {
            ulong GunCubeID = GunCubeNew.GetComponent<NetworkObject>().NetworkObjectId;
            if(GunCubeID == netWorkObjId) { return GunCubeNew; }
        }
        return null;
    }

    IEnumerator SpawnGunCube()
    {
        yield return new WaitForSeconds(DelayspawnGuncube);
        SpawnGunCubeServerRpc();
    }
}
