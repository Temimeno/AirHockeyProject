using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnCharacter : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        foreach (var client in HostManager.Instance.ClientData)
        {
            var character = characterDatabase.GetCharacterById(client.Value.characterId);
            if (character != null && character.Id == 1)
            {
                var spawnPos = new Vector3(-5.6f, 0f, 0f);
                var spawnRot = Quaternion.Euler(0f, 0f, 0f);
                StartCoroutine(WaitforMap());
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, spawnRot);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
            }
            if (character != null && character.Id == 2)
            {
                var spawnPos = new Vector3(5.6f, 0f, 0f);
                var spawnRot = Quaternion.Euler(0f, 0f, 0f);
                StartCoroutine(WaitforMap());
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, spawnRot);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
            }
        }
    }

    IEnumerator WaitforMap()
    {
        yield return new WaitForSeconds(50f);
    }
}
