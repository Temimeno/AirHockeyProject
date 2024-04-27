using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;
using Mono.CSharp.Linq;
using UnityEngine.SceneManagement;

public class QuickJoinLobbyScript : MonoBehaviour
{
    string lobbyName = "MyLobby";
    private Lobby joinedLobby;
    private bool gameHasStarted;
    [SerializeField] private string characterSelectSceneName = "CharacterSelect";
    public Dictionary<ulong, ClientData> ClientData { get; private set; }

    public async void CreateOrJoinLobby()
    {
        joinedLobby = await QuickJoinLobby() ?? await CreateLobby();
    }

    private async Task<Lobby> CreateLobby()
    {
        try
        {
            const int maxPlayers = 2;

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Data = new Dictionary<string, DataObject>
                {
                    {"JoinCodeKey", new DataObject(DataObject.VisibilityOptions.Public, joinCode)}
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            // Start the room immediately (or can wait for the lobby to fill up)

            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

            ClientData = new Dictionary<ulong, ClientData>();

            NetworkManager.Singleton.StartHost();

            Debug.Log("Join Code = " + joinCode);
            return lobby;
        }
        catch (Exception e)
        {
            Debug.LogFormat("Failed creating a lobby");
            return null;
        }
    }

    private async Task<Lobby> FindRandomLobby()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"1",QueryFilter.OpOptions.GT)
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.Log("Lobbby found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                return lobby;
            }
            return null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await FindRandomLobby();

            if (lobby == null) return null;
            Debug.Log(lobby.Name + " , " + lobby.AvailableSlots);

            if (lobby.Data["JoinCodeKey"].Value != null)
            {
                string joinCode = lobby.Data["JoinCodeKey"].Value;
                Debug.Log("Join Code = " + joinCode);
                if (joinCode == null) return null;

                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient();
                return lobby;
            }

            return null;
        }
        catch (Exception e)
        {
            Debug.Log("No lobbies avaliable via quick join");
            return null;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (ClientData.Count >= 2 || gameHasStarted)
        {
            response.Approved = false;
            return;
        }

        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;

        ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);

        Debug.Log($"Added client {request.ClientNetworkId}");
    }

    private void OnNetworkReady()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        NetworkManager.Singleton.SceneManager.LoadScene(characterSelectSceneName, LoadSceneMode.Single);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (ClientData.ContainsKey(clientId))
        {
            if (ClientData.Remove(clientId))
            {
                Debug.Log($"Removed client {clientId}");
            }
        }
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
