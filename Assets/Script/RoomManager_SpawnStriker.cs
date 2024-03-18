using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RoomManager_SpawnStriker : NetworkBehaviour
{
    public GameObject Striker;
    public GameObject scoreManager;
    public List<GameObject> spawnerStriker = new List<GameObject>();

    PlayerStats playerStats;

    public void Update()
    {
        if(scoreManager.activeInHierarchy)
        {
            playerStats = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerStats>();
        }
    }

    public void IncreaseScorePlayer1()
    {
        playerStats.scoreP1.Value++;
    }

    public void IncreaseScorePlayer2()
    {
        playerStats.scoreP2.Value++;
    }

    /*public void IncreaseBulletPlayer1()
    {
        if(playerStats.bulletP1.Value <= 6)
        {
            playerStats.bulletP1.Value++;
        }
    }

    public void IncreaseBulletPlayer2()
    {
        if(playerStats.bulletP2.Value <= 6)
        {
            playerStats.bulletP2.Value++;
        }
    }*/

    [ServerRpc]
    public void SpawnStrikerLeftServerRpc()
    {
        Vector3 spawnPos = new Vector3(-4f, 0f, 0f);
        Quaternion spawnRot = Quaternion.Euler(0f, 0f, 0f);
        GameObject strikerNew = Instantiate(Striker, spawnPos, spawnRot);
        spawnerStriker.Add(strikerNew);
        strikerNew.GetComponent<Scoring>().roomManager_SpawnStriker = this;
        strikerNew.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    public void SpawnStrikerRightServerRpc()
    {
        Vector3 spawnPos = new Vector3(4f, 0f, 0f);
        Quaternion spawnRot = Quaternion.Euler(0f, 0f, 0f);
        GameObject strikerNew = Instantiate(Striker, spawnPos, spawnRot);
        spawnerStriker.Add(strikerNew);
        strikerNew.GetComponent<Scoring>().roomManager_SpawnStriker = this;
        strikerNew.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc(ulong networkObjId)
    {
        GameObject obj = findSpawnerStriker(networkObjId);
        if(obj == null) return;
        obj.GetComponent<NetworkObject>().Despawn();
        spawnerStriker.Remove(obj);
        Destroy(obj);
        
    }

    private GameObject findSpawnerStriker(ulong netWorkObjId)
    {
        foreach (GameObject strikerNew in spawnerStriker)
        {
            ulong StrikerID = strikerNew.GetComponent<NetworkObject>().NetworkObjectId;
            if(StrikerID == netWorkObjId)
            {
                return strikerNew;
            }
        }
        return null;
    }
}
