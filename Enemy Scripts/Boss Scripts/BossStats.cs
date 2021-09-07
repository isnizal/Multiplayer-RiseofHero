using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
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
        EHealthCheck();
    }

    public void ETakeDamage(int eDamageToGive)
    {
        enemyCurrentHP -= eDamageToGive;
    }

    void EHealthCheck()
    {
        if(enemyCurrentHP < enemyMaxHP / 2)
		{
            BossAI.BossInstance.speed = 10;
            BossAI.BossInstance.startWaitTime = 1;
        }
        if (enemyCurrentHP <= 0)
            Death();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int enemyDamage = GetComponent<BossStats>().enemyAttackPower;
            int calcTotalDefense = Mathf.RoundToInt(Character.MyInstance.Defense.Value + Character.MyInstance.Defense.BaseValue);
            int calcDefense = Mathf.RoundToInt(calcTotalDefense * .5f);
            int totalDamage = enemyDamage - calcDefense;
            if (totalDamage <= 0)
            {
                totalDamage = 0;
                var missclone = Instantiate(damageNumbers, Character.MyInstance.transform.position, Quaternion.Euler(Vector3.zero));
                missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
            }
            else if (totalDamage > 0)
            {
                other.gameObject.GetComponent<PlayerCombat>().TakeDamage(totalDamage);
                var clone = Instantiate(damageNumbers, Character.MyInstance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
            }
        }
    }



    public void DropLoot()
    {
        if (lootTables != null)
        {
            GameObject current = lootTables.LootItems();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    public void Death()
    {
        LevelSystem.LevelInstance.AddExp(expToGive);
        DropLoot();
        GameManager.GameManagerInstance.devilQueenDefeated = 1;
        //GameManager.GameManagerInstance.devilQueenSpawned = false;
        Destroy(this.gameObject);
    }
}
