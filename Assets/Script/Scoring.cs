using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Scoring : NetworkBehaviour
{
    public GameObject scoreManager; 
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
        
        if (collider.gameObject.tag == "OutGround")
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                StartCoroutine(StrikerDestroy());
                StartCoroutine(DelaySpawnStrikerRight());
            }
            if(random == 1)
            {
                StartCoroutine(StrikerDestroy());
                StartCoroutine(DelaySpawnStrikerLeft());
            }
            else
            {   
                StartCoroutine(StrikerDestroy());
                StartCoroutine(DelaySpawnStrikerLeft());
            }
            Debug.Log(random);
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
