using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerManager : NetworkBehaviour
{
    public bool devilQueenDefeated = false;
    public bool devilQueenSpawned = false;
    public bool devilQueenCanSpawn = false;

   [SyncVar] public float bossTimerTrigger = 120f;
    public GameObject bossQueen;

    private ServerManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

    }
    private void Update()
    {
        if(bossTimerTrigger >= 0)
            bossTimerTrigger -= Time.deltaTime;
    }

    public void SpawnBossDevilQueen(Vector2 position)
    {
        var clone = Instantiate(bossQueen, new Vector2(position.x, position.y),Quaternion.identity);
        NetworkServer.Spawn(clone);

       // clone.transform.parent = bossHolder;
        devilQueenSpawned = true;
        bossTimerTrigger = 120f;
    }
}
