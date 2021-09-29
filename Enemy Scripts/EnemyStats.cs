using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Experimental;

[RequireComponent(typeof(NetworkAnimator))]
[RequireComponent(typeof(NetworkRigidbody2D))]
public class EnemyStats : NetworkBehaviour
{
    public static EnemyStats instance;
    public static EnemyStats EnemyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyStats>();
            }
            return instance;
        }

    }

    [Header("Enemy Stats")]
    public int areaSpawnNumber;
    public string enemyName;
    [SyncVar(hook = nameof(OnEnemyHpChanged))]public int enemyCurrentHP;
    [SyncVar]public int enemyMaxHP;
    [SyncVar]public int enemyAttackPower;
    [SyncVar]public int enemyDefense;
    [SyncVar]public float attackCoolDownTime = 1f;
    [Space]
    public int expToGive;

    public GameObject damageNumbers;
    [Space]
    public LootTables lootTables;

    private Spawner _spawner;

    public void InitializeEnemyStats()
    {
        _spawner = FindObjectOfType<Spawner>();
        enemyCurrentHP = enemyMaxHP;
    }
    void FixedUpdate()
    {
        if (isServer)
        {
            EHealthCheck();
        }
    }
    //set on the server
    private LevelSystem _levelSystem;
    public void OnEnemyHpChanged(int oldHp, int newHp)
    {
        this.enemyCurrentHP = newHp;
    }
    public void ETakeDamage(int eDamageToGive,GameObject player)
	{
        if (isClient)
        {
            CmdETakeDamage(eDamageToGive,player);
        }
        SoundManager.PlaySound(SoundManager.Sound.EnemyHit);
    }
    [Command(requiresAuthority = false)]
    public void CmdETakeDamage(int eDamageToGive,GameObject player)
    {
        enemyCurrentHP -= eDamageToGive;
        _levelSystem = player.GetComponent<LevelSystem>();
    }

    [Server]
    void EHealthCheck()
	{
        if (enemyCurrentHP <= 0)
            Death();
	}
    private IEnumerator EnemyAttackTimeCorou;
    [SyncVar]public bool isAttack = false;
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
            if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                return;
            isAttack = true;
            EnemyAttackTimeCorou = EnemyAttack(other);
            StartCoroutine(EnemyAttackTimeCorou);
        }
 	}
    #region"playerattack"
    private Character _character;
    private AchievementManager _achievementManager;
    private IEnumerator EnemyAttack(Collision2D other)
    {
        GameObject player = other.gameObject;
        _character = player.GetComponent<Character>();
        _achievementManager = _character.uiManager.gameObject.GetComponent<AchievementManager>();
        while (isAttack)
        {
            int enemyDamage = enemyAttackPower;
            int calcTotalDefense = Mathf.RoundToInt(_character.Defense.Value + _character.Defense.BaseValue);
            int calcDefense = Mathf.RoundToInt(calcTotalDefense * .125f);
            int totalDamage = Random.Range(enemyDamage - calcDefense, enemyDamage * 2 - calcDefense);

            if (totalDamage <= 0)
            {
                CmdPlayerNoDamage(_character, totalDamage);
            }
            else if (totalDamage > 0)
            {
                CmdPlayerTakeDamage(player, totalDamage,_character);

            }
            yield return new WaitForSeconds(attackCoolDownTime);
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdPlayerNoDamage(Character _character, int totalDamage)
    {
        var missclone = Instantiate(damageNumbers, _character.transform.position, Quaternion.Euler(Vector3.zero));
        NetworkServer.Spawn(missclone);
        RpcPlayerNoDamage(missclone,totalDamage);
    }
    [TargetRpc]
    public void RpcPlayerNoDamage(GameObject missclone,int totalDamage)
    {
        missclone.GetComponent<DamageNumbers>().isMiss = true;
        missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
    }
    [Command(requiresAuthority = false)]
    public void CmdPlayerTakeDamage(GameObject player, int totalDamage,Character _character)
    {
        var missclone = Instantiate(damageNumbers, _character.transform.position, Quaternion.Euler(Vector3.zero));
        NetworkServer.Spawn(missclone);
        RpcPlayerTakeDamage(player.GetComponent<NetworkIdentity>().connectionToClient, totalDamage,missclone);
    }
    [TargetRpc]
    public void  RpcPlayerTakeDamage(NetworkConnection player, int totalDamage,GameObject missclone)
    {
        player.identity.gameObject.GetComponent<PlayerCombat>().TakeDamage(totalDamage);
        missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
    }
    #endregion

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                return;
            isAttack = false;
            StopCoroutine(EnemyAttackTimeCorou);
        }
    }


    protected GameObject lootItem;
    [Server]
    public void DropLoot()
	{
        if(lootTables != null)
		{
            GameObject current = lootTables.LootItems();
            lootItem = current;
            SpawnLoot(lootItem);
        }
	}
    [Server]
    public void SpawnLoot(GameObject lootItem)
	{
        if (lootItem == null)
        {
            NetworkServer.Destroy(this.gameObject);
            return;
        }
        var objectSpawn = Instantiate(lootItem, this.transform.position, Quaternion.identity);
		NetworkServer.Spawn(objectSpawn);
        NetworkServer.Destroy(this.gameObject);
	}

    [Server]
    public void Death()
	{
        SoundManager.PlaySound(SoundManager.Sound.EnemyDie);
        _levelSystem.AddExp(expToGive);
        CheckAchDeath();
        if(areaSpawnNumber == 1)//Asea Area
		{
            _spawner.aseaEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 2)//Efos Area
		{
            _spawner.efosEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 3)//Newlow Caves
		{
            _spawner.newlowCaveEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 4)//Diregarde Castle
		{
            _spawner.diregardeCastleEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 5)//Efos Pass
		{
            _spawner.efosPassEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 6)//Astad Area
		{
            _spawner.astadEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 7)//Diregarde Area
		{
            _spawner.diregardeEnemyCounter -= 1;
		}
        DropLoot();
    }

    [Server]
    public void CheckAchDeath()
	{
        if (_achievementManager is null)
            return;
        if (_achievementManager.killSlime1Active == 1)
        {
            if (enemyName == "Red Slime")
                _achievementManager.killSlimeCountAch++;
        }
        if(_achievementManager.kill50AchActive == 1)
		{
            if(gameObject.CompareTag("Enemy"))
			{
                _achievementManager.kill50CountAch++;
			}
		}
    }
}
