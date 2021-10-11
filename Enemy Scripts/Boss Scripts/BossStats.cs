using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkAnimator))]
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
    [SyncVar] public int enemyCurrentHP;
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
        //EHealthCheck();
    }
    [Client]
    public void ETakeDamage(NetworkConnection conn,int eDamageToGive)
    {

        _playerMovement = conn.identity.gameObject.GetComponent<PlayerMovement>();
    }

    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(int eDamageToGive)
    {
        enemyCurrentHP -= eDamageToGive;
        if (enemyCurrentHP < enemyMaxHP / 2)
        {
            GetComponent<BossAI>().speed = 10;
            GetComponent<BossAI>().startWaitTime = 1;
        }
        if (enemyCurrentHP <= 0)
        {
            Death();
        }
    }

   // [Server]
   // void EHealthCheck()
   // {
   //
   // }

    private PlayerMovement _playerMovement;
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
    #region "EnemyAttack"
    [Command(requiresAuthority = false)]
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
    #endregion

    [Server]
    public void DropLoot()
    {
        if (lootTables != null)
        {
            GameObject current = lootTables.LootItems();
            if (current != null)
            {
                Destroy(current);
            }
        }
    }
    [Server]
    public void Destroy(GameObject current)
    {
        var loot = Instantiate(current.gameObject, transform.position, Quaternion.identity);
        NetworkServer.Spawn(loot);
        NetworkServer.Destroy(this.gameObject);
    }
    [Server]
    public void Death()
    {
        _playerMovement.levelSystem.AddExp(connectionToClient,expToGive);
        DropLoot();

        _playerMovement.serverManager.devilQueenDefeated = true;
        //GameManager.GameManagerInstance.devilQueenSpawned = false;
    }

}
