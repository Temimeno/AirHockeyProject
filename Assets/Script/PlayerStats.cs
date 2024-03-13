using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerStats : NetworkBehaviour
{
    TMP_Text p1Text;
    TMP_Text p2Text;

    public NetworkVariable<int> scoreP1 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> scoreP2 = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Start is called before the first frame update
    void Start()
    {
        p1Text = GameObject.Find("HostScore (TMP)").GetComponent<TMP_Text>();
        p2Text = GameObject.Find("ClientScore (TMP)").GetComponent<TMP_Text>();
    }

    private void UpdatePlayerNameAndScore()
    {
        p1Text.text = $"{scoreP1.Value}";
        p2Text.text = $"{scoreP2.Value}";
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerNameAndScore();
    }
}
