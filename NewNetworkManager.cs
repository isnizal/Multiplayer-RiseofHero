
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
        Debug.Log("server" + conn.identity);
    }

}
