using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameEnding : NetworkBehaviour
{
    public GameObject P1WinPanel;
    public GameObject P2WinPanel;

    public void P1Win()
    {
        Debug.Log("P1 Win");
        P1WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void P2Win()
    {
        Debug.Log("P2 Win");
        P2WinPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
