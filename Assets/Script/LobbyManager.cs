using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private Lobby hostLobby;

    [Command]
    private async void CreateLobby()
    
    {
        try
        {
            string lobbyName = "new lobby";
            int maxPlayers = 2; //จำนวนของผู้เล่นเข้าสู่ห้อง Lobby
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false    
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            hostLobby = lobby;
            StartCoroutine(HeartbeatLobbyCoroutine(hostLobby.Id, 15));
            Debug.Log("Create Lobby : " + lobby.Name + " , " + lobby.MaxPlayers + " , "+ lobby.Id + " , " + lobby.LobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    [Command]
    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,

                // Filter for open lobbies only
                Filters = new List<QueryFilter>()
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots , "0", QueryFilter.OpOptions.GT)
                },

                // Order by newest lobbies first
                Order = new List<QueryOrder>()
                {
                    new QueryOrder(asc: false, QueryOrder.FieldOptions.Created)
                }
            };    

            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " , " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    [Command]
    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            Debug.Log(queryResponse.Results[0].Name + " , " + queryResponse.Results[0].AvailableSlots);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    [Command]
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined Lobby with code : " + lobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    [Command]
    private async void QuickJoinInLobby()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log(lobby.Name + " , " + lobby.AvailableSlots);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
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
