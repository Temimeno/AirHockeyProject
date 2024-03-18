using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class BulletState : NetworkBehaviour
{
    public GameEnding gameEnding;
    TMP_Text p1Bullet;
    TMP_Text p2Bullet;
    TMP_Text p1Hp;
    TMP_Text p2Hp;
    
    public NetworkVariable<int> bulletP1 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> bulletP2 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> HpP1 = new NetworkVariable<int>(3,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> HpP2 = new NetworkVariable<int>(3,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        p1Bullet = GameObject.Find("P1Bullet (TMP)").GetComponent<TMP_Text>();
        p2Bullet = GameObject.Find("P2Bullet (TMP)").GetComponent<TMP_Text>();
        p1Hp = GameObject.Find("HpP1Text(TMP)").GetComponent<TMP_Text>();
        p2Hp = GameObject.Find("HpP2Text(TMP)").GetComponent<TMP_Text>();
        gameEnding = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<GameEnding>();
    }

    private void UpdatePlayerNameAndScore()
    {
        if(IsOwnedByServer)
        {
            p1Bullet.text = $"Bullet: {bulletP1.Value}";
            p1Hp.text = $"P1: {HpP1.Value}";
        }
        else
        {
            p2Bullet.text = $"Bullet: {bulletP2.Value}";
            p2Hp.text = $"P2: {HpP2.Value}";
        }

    }

    void Update()
    {
        UpdatePlayerNameAndScore();

        if (HpP1.Value == 0)
        {
            gameEnding.P2Win();
        }

        if (HpP2.Value == 0)
        {
            gameEnding.P1Win();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(!IsLocalPlayer)  return;

        if(col.gameObject.tag == "GunCube")
        {
            if(IsOwnedByServer)
            {
                if(bulletP1.Value < 6)
                {
                    bulletP1.Value++;
                }
            }
            else
            {
                if(bulletP2.Value < 6)
                {
                    bulletP2.Value++;
                }
            }
        }

        if(col.gameObject.tag == "Bullet")
        {
            if(IsOwnedByServer)
            {
                HpP1.Value--;
                Debug.Log(HpP1.Value);
            }
            else
            {
                HpP2.Value--;
                Debug.Log(HpP2.Value);
            }
        }
    }
}
