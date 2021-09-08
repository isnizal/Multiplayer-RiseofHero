
using UnityEngine;
using Mirror;
using System;

public class NewNetworkManager : NetworkManager
{
    private GameObject _attributesWindow, _spellsWindow;
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
       // Debug.Log("add player");
        base.OnServerAddPlayer(conn);
        var equipmentPanel = Instantiate(NetworkManager.singleton.spawnPrefabs[0]);
        equipmentPanel.name = equipmentPanel.name + "" + conn;
        NetworkServer.Spawn(equipmentPanel, conn);
        Debug.Log("sucess spawn");
    }
}
