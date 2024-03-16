using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Scoring : NetworkBehaviour
{
    public GameObject scoreManager; 
    PlayerStats playerStats;
    public RoomManager_SpawnStriker roomManager_SpawnStriker;

    private void Start()
    {
        roomManager_SpawnStriker = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager_SpawnStriker>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "GoalLeft")
        {
            //playerStats.scoreP2.Value++
            roomManager_SpawnStriker.IncreaseScorePlayer2();
            StartCoroutine(StrikerDestroy());
            StartCoroutine(DelaySpawnStrikerLeft());
        }

        if (collider.gameObject.tag == "GoalRight")
        {
            //playerStats.scoreP1.Value++;
            roomManager_SpawnStriker.IncreaseScorePlayer1();
            StartCoroutine(StrikerDestroy());
            StartCoroutine(DelaySpawnStrikerRight());
        }
    }

    IEnumerator StrikerDestroy()
    {
        yield return new WaitForSeconds(3f);
        ulong networkObjId = GetComponent<NetworkObject>().NetworkObjectId;
        roomManager_SpawnStriker.DestroyServerRpc(networkObjId);
        Destroy(gameObject);
    }

    IEnumerator DelaySpawnStrikerLeft()
    {
        yield return new WaitForSeconds(3f);
        roomManager_SpawnStriker.SpawnStrikerLeftServerRpc();
    }

    IEnumerator DelaySpawnStrikerRight()
    {
        yield return new WaitForSeconds(3f);
        roomManager_SpawnStriker.SpawnStrikerRightServerRpc();
    }
}
