using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class BulletState : NetworkBehaviour
{
    TMP_Text p1Bullet;
    TMP_Text p2Bullet;
    
    public NetworkVariable<int> bulletP1 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> bulletP2 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        p1Bullet = GameObject.Find("P1Bullet (TMP)").GetComponent<TMP_Text>();
        p2Bullet = GameObject.Find("P2Bullet (TMP)").GetComponent<TMP_Text>();
    }

    private void UpdatePlayerNameAndScore()
    {
        if(IsOwnedByServer)
        {
            p1Bullet.text = $"Bullet: {bulletP1.Value}";
        }
        else
        {
            p2Bullet.text = $"Bullet: {bulletP2.Value}";
        }

    }

    void Update()
    {
        UpdatePlayerNameAndScore();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(!IsLocalPlayer)  return;

        if(col.gameObject.tag == "GunCube")
        {
            if(IsOwnedByServer)
            {
                if(bulletP1.Value <= 6)
                {
                    bulletP1.Value++;
                }
            }
            else
            {
                if(bulletP2.Value <= 6)
                {
                    bulletP2.Value++;
                }
            }
        }
    }
}
