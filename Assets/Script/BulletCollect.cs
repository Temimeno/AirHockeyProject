using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletCollect : NetworkBehaviour
{
    PlayerStats playerStats;
    public void Start()
    {

        playerStats = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerStats>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GunCube")
        {
            if (IsOwnedByServer)
            {
                if (playerStats.bulletP1.Value <= 6)
                {
                    playerStats.bulletP1.Value++;
                }
            }

            else
            {
                if (playerStats.bulletP1.Value <= 6)
                {
                    playerStats.bulletP2.Value++;
                }
            }
        }
    }
}
