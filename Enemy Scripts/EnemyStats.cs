using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
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
    public int enemyCurrentHP;
    public int enemyMaxHP;
    public int enemyAttackPower;
    public int enemyDefense;
    public float attackCoolDownTime = 1f;
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
        SoundManager.PlaySound(SoundManager.Sound.EnemyHit);
    }

    void EHealthCheck()
	{
        if (enemyCurrentHP <= 0)
            Death();
	}
    private IEnumerator EnemyAttackTimeCorou;
    public bool isAttack = false;
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
            isAttack = true;
            EnemyAttackTimeCorou = EnemyAttack(other);
            StartCoroutine(EnemyAttackTimeCorou);
        }
 	}
    private IEnumerator EnemyAttack(Collision2D other)
    {
        GameObject player = other.gameObject;
        while (isAttack)
        {
            int enemyDamage = GetComponent<EnemyStats>().enemyAttackPower;
            int calcTotalDefense = Mathf.RoundToInt(Character.MyInstance.Defense.Value + Character.MyInstance.Defense.BaseValue);
            int calcDefense = Mathf.RoundToInt(calcTotalDefense * .125f);
            int totalDamage = Random.Range(enemyDamage - calcDefense, enemyDamage * 2 - calcDefense);
            var missclone = Instantiate(damageNumbers, Character.MyInstance.transform.position, Quaternion.Euler(Vector3.zero));
            if (totalDamage <= 0)
            {
                missclone.GetComponent<DamageNumbers>().isMiss = true;
                missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
            }
            else if (totalDamage > 0)
            {
                player.GetComponent<PlayerCombat>().TakeDamage(totalDamage);
                missclone.GetComponent<DamageNumbers>().damageNumber = totalDamage;
            }
            yield return new WaitForSeconds(attackCoolDownTime);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isAttack = false;
            StopCoroutine(EnemyAttackTimeCorou);
        }
    }



    public void DropLoot()
	{
        if(lootTables != null)
		{
            GameObject current = lootTables.LootItems();
            if(current != null)
			{
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
			}
		}
	}

    public void Death()
	{
        SoundManager.PlaySound(SoundManager.Sound.EnemyDie);
        LevelSystem.LevelInstance.AddExp(expToGive);
        DropLoot();
        CheckAchDeath();
        if(areaSpawnNumber == 1)//Asea Area
		{
            Spawner.MySpawner.aseaEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 2)//Efos Area
		{
            Spawner.MySpawner.efosEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 3)//Newlow Caves
		{
            Spawner.MySpawner.newlowCaveEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 4)//Diregarde Castle
		{
            Spawner.MySpawner.diregardeCastleEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 5)//Efos Pass
		{
            Spawner.MySpawner.efosPassEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 6)//Astad Area
		{
            Spawner.MySpawner.astadEnemyCounter -= 1;
		}
        if(areaSpawnNumber == 7)//Diregarde Area
		{
            Spawner.MySpawner.diregardeEnemyCounter -= 1;
		}
        Destroy(this.gameObject);
	}

    public void CheckAchDeath()
	{
        if (AchievementManager.AchievementInstance.killSlime1Active == 1)
        {
            if (enemyName == "Red Slime")
                AchievementManager.AchievementInstance.killSlimeCountAch++;
        }
        if(AchievementManager.AchievementInstance.kill50AchActive == 1)
		{
            if(gameObject.CompareTag("Enemy"))
			{
                AchievementManager.AchievementInstance.kill50CountAch++;
			}
		}
    }
}
