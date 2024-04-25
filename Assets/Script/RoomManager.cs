using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RoomManager : MonoBehaviour
{
    public GameObject joinPanel;
    public GameObject startButton;
    public GameObject scorePanel;
    public GameObject playerStats;
    public GameObject BulletP1;
    public GameObject BulletP2;
    public GameObject HpP1;
    public GameObject HpP2;
    public List<uint> AltPlayerPrefab;


    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleServerStarted()
    {
        Debug.Log("HandleServerStarted");
    }

    private void HandleClientConnected(ulong clientId)
    {
        Debug.Log("clientId = " + clientId);

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            joinPanel.SetActive(false);
            //startButton.SetActive(true);
            scorePanel.SetActive(true);
            playerStats.SetActive(true);
            BulletP1.SetActive(true);
            BulletP2.SetActive(true);
            HpP1.SetActive(true);
            HpP2.SetActive(true);
        }
    }

    public async void Host()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        Debug.Log("Start Host");
    }

    public void Client()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Join");
    }

    private void setSpawnLocation(ulong clientId, NetworkManager.ConnectionApprovalResponse response)
    {
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            spawnPos = new Vector3(-5.6f, 0f, 0f);
            spawnRot = Quaternion.Euler(0f, 0f, 0f);
        }

        else
        {
            spawnPos = new Vector3(5.6f, 0f, 0f);
            spawnRot = Quaternion.Euler(0f, 0f, 0f);
        }

        response.Position = spawnPos;
        response.Rotation = spawnRot;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
        int playerSelectNum = 1;

        if (clientId == 0)
        {
            playerSelectNum = 0;
        }
        else
        {
            playerSelectNum = 1;
        }

        response.PlayerPrefabHash = AltPlayerPrefab[playerSelectNum];

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        setSpawnLocation(clientId, response);
        NetworkLog.LogInfoServer("Spawn pos of " + clientId + " is " + response.Position.ToString());

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }


}
