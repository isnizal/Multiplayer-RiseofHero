using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
public class BossStats : NetworkBehaviour
{
    public static BossStats instance;
    public static BossStats BossStatsInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BossStats>();
            }
            return instance;
        }

    }
    [Header("Enemy Stats")]
    public string enemyName;
    [SyncVar]
    public int enemyCurrentHP;
    public int enemyMaxHP;
    public int enemyAttackPower;
    public int enemyDefense;
    [Space]
    public int expToGive;

    public GameObject damageNumbers;
    [Space]
    public LootTables lootTables;

    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
    }

    void FixedUpdate()
    {
        if (base.hasAuthority)
        {
            EHealthCheck();
        }
    }
    private LevelSystem _levelSystem;
    public void ETakeDamage(int eDamageToGive,GameObject other)
    {
        enemyCurrentHP -= eDamageToGive;
        _levelSystem = other.GetComponent<LevelSystem>();
    }

    void EHealthCheck()
    {
        if(enemyCurrentHP < enemyMaxHP / 2)
		{
            GetComponent<BossAI>().speed = 10;
            GetComponent<BossAI>().startWaitTime = 1;
        }
        if (enemyCurrentHP <= 0)
            Death();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
                return;
            int enemyDamage = GetComponent<BossStats>().enemyAttackPower;
            int calcTotalDefense = Mathf.RoundToInt(other.gameObject.GetComponent<Character>().Defense.Value
                + other.gameObject.GetComponent<Character>().Defense.BaseValue);
            int calcDefense = Mathf.RoundToInt(calcTotalDefense * .5f);
            int totalDamage = enemyDamage - calcDefense;
            if (totalDamage <= 0)
            {
                totalDamage = 0;
                CmdNoDamage(totalDamage,other.gameObject.GetComponent<Character>());

            }
            else if (totalDamage > 0)
            {
                CmdNormalDamage(totalDamage, other.gameObject.GetComponent<Character>());
            }
        }
    }
    [Command]
    public void CmdNoDamage(int totalDamage, Character other)
    {
        var missclone = Instantiate(damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
        NetworkServer.Spawn(missclone);
        RpcNoDamage(missclone, totalDamage);
    }
    [ClientRpc]
    public void RpcNoDamage(GameObject missclone,int totalDamage)
    {
        missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
    }
    [Command(requiresAuthority = false)]
    public void CmdNormalDamage(int totalDamage, Character other)
    {
        other.gameObject.GetComponent<PlayerCombat>().TakeDamage(totalDamage);
        var clone = Instantiate(damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
        NetworkServer.Spawn(clone);
        RpcNormalDamage(clone, totalDamage);
    }
    [ClientRpc]
    public void RpcNormalDamage(GameObject clone, int totalDamage)
    {
        clone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
    }



    public void DropLoot()
    {
        if (lootTables != null)
        {
            GameObject current = lootTables.LootItems();
            if (current != null)
            {
                CmdDropLoot(current);
            }
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdDropLoot(GameObject current)
    {
        var loot = Instantiate(current.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(loot);
    }
    public void Death()
    {
        _levelSystem.gameObject.GetComponent<LevelSystem>().AddExp(expToGive);
        DropLoot();
        GameManager.GameManagerInstance.devilQueenDefeated = 1;
        //GameManager.GameManagerInstance.devilQueenSpawned = false;
        _levelSystem.gameObject.GetComponent<PlayerCombat>().CmdDestroyObjects(this.gameObject);
    }

}
