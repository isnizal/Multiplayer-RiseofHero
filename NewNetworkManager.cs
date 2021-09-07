
using UnityEngine;
using Mirror;
using System;

public class NewNetworkManager : NetworkManager
{
    private GameObject _attributesWindow, _spellsWindow;
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("add player");
        base.OnServerAddPlayer(conn);
        var gameManager = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
        NetworkServer.Spawn(gameManager, conn);
    }
}
