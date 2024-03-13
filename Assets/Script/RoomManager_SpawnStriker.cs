using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RoomManager_SpawnStriker : NetworkBehaviour
{
    public GameObject Striker;
    public GameObject scoreManager;

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

    [ServerRpc]
    public void SpawnStrikerLeftServerRpc()
    {
        Vector3 spawnPos = new Vector3(-4f, 0f, 0f);
        Quaternion spawnRot = Quaternion.Euler(0f, 0f, 0f);
        GameObject strikerNew = Instantiate(Striker, spawnPos, spawnRot);
        strikerNew.GetComponent<NetworkObject>().Spawn(true);
    }

    [ServerRpc]
    public void SpawnStrikerRightServerRpc()
    {
        Vector3 spawnPos = new Vector3(4f, 0f, 0f);
        Quaternion spawnRot = Quaternion.Euler(0f, 0f, 0f);
        GameObject strikerNew = Instantiate(Striker, spawnPos, spawnRot);
        strikerNew.GetComponent<NetworkObject>().Spawn(true);
    }
}
