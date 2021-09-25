using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
public class Spawner : NetworkBehaviour
{
    public static Spawner instance;
    public static Spawner MySpawner
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Spawner>();
            }
            return instance;
        }

    }

    public Transform[] aseaSpawnPoints;
    public Transform[] efosSpawnPoints;
    public Transform[] newlowCaveSpawnPoints;
    public Transform[] diregardeCastleSpawnPoints;
    public Transform[] efosPassSpawnPoints;
    public Transform[] astadSpawnPoints;
    public Transform[] diregardeSpawnPoints;
    [Space]
    public Transform enemyHolder;
    [Space]
    public GameObject[] aseaEnemyPrefab;
    public GameObject[] efosEnemyPrefab;
    public GameObject[] newlowCaveEnemyPrefab;
    public GameObject[] diregardeCastleEnemyPrefab;
    public GameObject[] efosPassEnemyPrefab;
    public GameObject[] astadEnemyPrefab;
    public GameObject[] diregardeEnemyPrefab;
    private GameObject newEnemyClone;
    [Space]
    public float timeBetweenSpawn, spawnDelay;
    [SyncVar]
    public int aseaEnemyCounter, aseaMaxEnemyCounter;
    [SyncVar]
    public int efosEnemyCounter, efosMaxEnemyCounter;
    [SyncVar]
    public int newlowCaveEnemyCounter, newlowCaveMaxEnemyCounter;
    [SyncVar]
    public int diregardeCastleEnemyCounter, diregardeCastleMaxEnemyCounter;
    [SyncVar]
    public int efosPassEnemyCounter, maxEfosPassEnemyCounter;
    [SyncVar]
    public int astadEnemyCounter, astadMaxEnemyCounter;
    [SyncVar]
    public int diregardeEnemyCounter, diregardeMaxEnemyCounter;

    private void Awake()
    {



    }
    public override void OnStartServer()
    {
        DontDestroyOnLoad(gameObject);
        base.OnStartServer();
        Debug.Log("spawner start sserver");
        if (isServer)
        {
            InvokeRepeating(nameof(AseaEnemySpawn), timeBetweenSpawn, spawnDelay);
            InvokeRepeating(nameof(EfosEnemySpawn), timeBetweenSpawn, spawnDelay);
            InvokeRepeating(nameof(NewlowCaveEnemySpawn), timeBetweenSpawn, spawnDelay);
            InvokeRepeating(nameof(DiregardeCastleEnemySpawn), timeBetweenSpawn, spawnDelay);
            InvokeRepeating(nameof(EfosPassEnemySpawn), timeBetweenSpawn, spawnDelay);
            InvokeRepeating(nameof(AstadEnemySpawn), timeBetweenSpawn, spawnDelay);
        }

    }
    void AseaEnemySpawn()
    {
        if (aseaEnemyCounter == aseaMaxEnemyCounter)
            return;
        if (aseaEnemyCounter < aseaMaxEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, aseaEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, aseaSpawnPoints.Length);

            RpcAseaEnemySpawn(spawnEnemy, spawnLocation);
        }
    }

    public void RpcAseaEnemySpawn(int spawnEnemy,int spawnLocation)
    {
        newEnemyClone = Instantiate(aseaEnemyPrefab[spawnEnemy], aseaSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(newEnemyClone);
        
        aseaEnemyCounter++;
    }
    void EfosEnemySpawn()
    {
        if (efosEnemyCounter == efosMaxEnemyCounter)
            return;
        if (efosEnemyCounter < efosMaxEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, efosEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, efosSpawnPoints.Length);
            CmdEfosEnemySpawn(spawnEnemy, spawnLocation);
        }
    }
    public void CmdEfosEnemySpawn(int spawnEnemy, int spawnLocation)
    {
        newEnemyClone = Instantiate(efosEnemyPrefab[spawnEnemy], efosSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        newEnemyClone.transform.parent = enemyHolder;
        efosEnemyCounter++;
    }

    void NewlowCaveEnemySpawn()
	{
        if (newlowCaveEnemyCounter == newlowCaveMaxEnemyCounter)
            return;
        if (newlowCaveEnemyCounter < newlowCaveMaxEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, newlowCaveEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, newlowCaveSpawnPoints.Length);
            CmdNewLowCaveEnemySpawn(spawnEnemy, spawnLocation);
        }
    }
    public void CmdNewLowCaveEnemySpawn(int spawnEnemy, int spawnLocation)
    {
        newEnemyClone = Instantiate(newlowCaveEnemyPrefab[spawnEnemy], newlowCaveSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        newEnemyClone.transform.parent = enemyHolder;
        newlowCaveEnemyCounter++;
    }

    void DiregardeCastleEnemySpawn()
    {
        if (diregardeCastleEnemyCounter == diregardeCastleMaxEnemyCounter)
            return;
        if (diregardeCastleEnemyCounter < diregardeCastleMaxEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, diregardeCastleEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, diregardeCastleSpawnPoints.Length);
            CmdDiregardeCastleEnemySpawn(spawnEnemy, spawnLocation);
        }
    }
    public void CmdDiregardeCastleEnemySpawn(int spawnEnemy, int spawnLocation)
    {
        newEnemyClone = Instantiate(diregardeCastleEnemyPrefab[spawnEnemy], diregardeCastleSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        newEnemyClone.transform.parent = enemyHolder;
        diregardeCastleEnemyCounter++;
    }
    void EfosPassEnemySpawn()
    {
        if (efosPassEnemyCounter == maxEfosPassEnemyCounter)
            return;
        if (efosPassEnemyCounter < maxEfosPassEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, efosPassEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, efosPassSpawnPoints.Length);
            CmdEfosPassEnemySpawn(spawnEnemy, spawnLocation);
        }
    }
    public void CmdEfosPassEnemySpawn(int spawnEnemy, int spawnLocation)
    {
        newEnemyClone = Instantiate(efosPassEnemyPrefab[spawnEnemy], efosPassSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        newEnemyClone.transform.parent = enemyHolder;
        efosPassEnemyCounter++;
    }
    void AstadEnemySpawn()
    {
        if (astadEnemyCounter == astadMaxEnemyCounter)
            return;
        if (astadEnemyCounter < astadMaxEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, astadEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, astadSpawnPoints.Length);
            CmdAstadEnemySpawn(spawnEnemy, spawnLocation);
        }
    }
    public void CmdAstadEnemySpawn(int spawnEnemy, int spawnLocation)
    {
        newEnemyClone = Instantiate(astadEnemyPrefab[spawnEnemy], astadSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        newEnemyClone.transform.parent = enemyHolder;
        astadEnemyCounter++;
    }
    void DiregardeEnemySpawn()
    {
        if (diregardeEnemyCounter == diregardeMaxEnemyCounter)
            return;
        if (diregardeEnemyCounter < diregardeMaxEnemyCounter)
        {
            var spawnEnemy = Random.Range(0, diregardeEnemyPrefab.Length);
            var spawnLocation = Random.Range(0, diregardeSpawnPoints.Length);
            CmdDiregardEnemySpawn(spawnEnemy, spawnLocation);
        }
    }
    public void CmdDiregardEnemySpawn(int spawnEnemy,int spawnLocation)
    {
        newEnemyClone = Instantiate(diregardeEnemyPrefab[spawnEnemy], diregardeSpawnPoints[spawnLocation].position, Quaternion.identity) as GameObject;
        newEnemyClone.transform.parent = enemyHolder;
        diregardeEnemyCounter++;
    }
}
