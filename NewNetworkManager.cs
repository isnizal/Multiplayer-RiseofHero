
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class NewNetworkManager : NetworkManager
{
    private GameObject _spawner;
    public override void OnStartServer()
    {
        base.OnStartServer();
        _spawner = Instantiate(spawnPrefabs[0]);
        NetworkServer.Spawn(_spawner);
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
       // Debug.Log("add player");
        base.OnServerAddPlayer(conn);
        //_spawner.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        Debug.Log("server" + conn.identity);
    }
    public override void OnServerChangeScene(string newSceneName)
    {
        if (string.IsNullOrEmpty(newSceneName))
        {
            Debug.LogError("ServerChangeScene empty scene name");
            return;
        }
        
        // Debug.Log("ServerChangeScene " + newSceneName);
        NetworkServer.SetAllClientsNotReady();
        networkSceneName = newSceneName;
        
        // Let server prepare for scene change
        //OnServerChangeScene(newSceneName);
        
        // set server flag to stop processing messages while changing scenes
        // it will be re-enabled in FinishLoadScene.
        NetworkServer.isLoadingScene = true;
        loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
        if (FindObjectOfType<MainMenu>() is null)
            return;

        FindObjectOfType<MainMenu>().LoadNewGame(loadingSceneAsync);

        // ServerChangeScene can be called when stopping the server
        // when this happens the server is not active so does not need to tell clients about the change
        if (NetworkServer.active)
        {
            // notify all clients about the new scene
            NetworkServer.SendToAll(new SceneMessage { sceneName = newSceneName });
        }
        
        startPositionIndex = 0;
        startPositions.Clear();
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        if (string.IsNullOrEmpty(newSceneName))
        {
            Debug.LogError("ClientChangeScene empty scene name");
            return;
        }

        // Debug.Log("ClientChangeScene newSceneName:" + newSceneName + " networkSceneName:" + networkSceneName);

        // Let client prepare for scene change
        //OnClientChangeScene(newSceneName, sceneOperation, customHandling);

        // After calling OnClientChangeScene, exit if server since server is already doing
        // the actual scene change, and we don't need to do it for the host client
        if (NetworkServer.active)
            return;

        // set client flag to stop processing messages while loading scenes.
        // otherwise we would process messages and then lose all the state
        // as soon as the load is finishing, causing all kinds of bugs
        // because of missing state.
        // (client may be null after StopClient etc.)
        // Debug.Log("ClientChangeScene: pausing handlers while scene is loading to avoid data loss after scene was loaded.");
        NetworkClient.isLoadingScene = true;

        // Cache sceneOperation so we know what was requested by the
        // Scene message in OnClientChangeScene and OnClientSceneChanged
        clientSceneOperation = sceneOperation;

        // scene handling will happen in overrides of OnClientChangeScene and/or OnClientSceneChanged
        // Do not call FinishLoadScene here. Custom handler will assign loadingSceneAsync and we need
        // to wait for that to finish. UpdateScene already checks for that to be not null and isDone.
        if (customHandling)
            return;

        switch (sceneOperation)
        {
            case SceneOperation.Normal:
                loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
                if (FindObjectOfType<MainMenu>() is null)
                    break;
                FindObjectOfType<MainMenu>().LoadNewGame(loadingSceneAsync);
                break;
            case SceneOperation.LoadAdditive:
                // Ensure additive scene is not already loaded on client by name or path
                // since we don't know which was passed in the Scene message
                if (!SceneManager.GetSceneByName(newSceneName).IsValid() && !SceneManager.GetSceneByPath(newSceneName).IsValid())
                {
                    loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
                    FindObjectOfType<MainMenu>().LoadNewGame(loadingSceneAsync);
                }
                else
                {
                    Debug.LogWarning($"Scene {newSceneName} is already loaded");

                    // Reset the flag that we disabled before entering this switch
                    NetworkClient.isLoadingScene = false;
                }
                break;
            case SceneOperation.UnloadAdditive:
                // Ensure additive scene is actually loaded on client by name or path
                // since we don't know which was passed in the Scene message
                if (SceneManager.GetSceneByName(newSceneName).IsValid() || SceneManager.GetSceneByPath(newSceneName).IsValid())
                {
                    loadingSceneAsync = SceneManager.UnloadSceneAsync(newSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    FindObjectOfType<MainMenu>().LoadNewGame(loadingSceneAsync);
                }
                else
                {
                    Debug.LogWarning($"Cannot unload {newSceneName} with UnloadAdditive operation");

                    // Reset the flag that we disabled before entering this switch
                    NetworkClient.isLoadingScene = false;
                }
                break;
        }
        // don't change the client's current networkSceneName when loading additive scene content
        if (sceneOperation == SceneOperation.Normal)
            networkSceneName = newSceneName;
    }

}
