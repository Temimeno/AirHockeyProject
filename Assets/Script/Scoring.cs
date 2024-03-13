using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Scoring : NetworkBehaviour
{
    public GameObject scoreManager;
    PlayerStats playerStats;
    // Start is called before the first frame update

    private void Update()
    {
        if(scoreManager.activeInHierarchy)
        {
            playerStats = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<PlayerStats>();
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "GoalLeft")
        {
            playerStats.scoreP2.Value++;
            StartCoroutine(StrikerDestroy());
        }

        if (collider.gameObject.tag == "GoalRight")
        {
            playerStats.scoreP1.Value++;
            StartCoroutine(StrikerDestroy());
        }
    }

    IEnumerator StrikerDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
