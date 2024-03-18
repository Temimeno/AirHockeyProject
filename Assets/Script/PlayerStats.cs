using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerStats : NetworkBehaviour
{
    public GameEnding gameEnding;
    TMP_Text p1Text;
    TMP_Text p2Text;
    //TMP_Text p1Bullet;
    //TMP_Text p2Bullet;

    public NetworkVariable<int> scoreP1 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> scoreP2 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //public NetworkVariable<int> bulletP1 = new NetworkVariable<int>(0,
    //NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //public NetworkVariable<int> bulletP2 = new NetworkVariable<int>(0,
    //NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Start is called before the first frame update
    void Start()
    {
        p1Text = GameObject.Find("HostScore (TMP)").GetComponent<TMP_Text>();
        p2Text = GameObject.Find("ClientScore (TMP)").GetComponent<TMP_Text>();
        //p1Bullet = GameObject.Find("P1Bullet (TMP)").GetComponent<TMP_Text>();
        //p2Bullet = GameObject.Find("P2Bullet (TMP)").GetComponent<TMP_Text>();
        gameEnding = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<GameEnding>();
    }

    private void UpdatePlayerNameAndScore()
    {
        p1Text.text = $"{scoreP1.Value}";
        p2Text.text = $"{scoreP2.Value}";
        //p1Bullet.text = $"Bullet: {bulletP1.Value}";
        //p2Bullet.text = $"Bullet: {bulletP2.Value}";
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerNameAndScore();

        if (scoreP1.Value == 3)
        {
            gameEnding.P1Win();
        }

        if (scoreP2.Value == 3)
        {
            gameEnding.P2Win();
        }
    }
}
