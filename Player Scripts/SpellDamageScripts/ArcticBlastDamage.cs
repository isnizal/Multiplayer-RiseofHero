using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcticBlastDamage : MonoBehaviour
{
	public float totalArcticDamage;

	public float rotatingSpeed = 200f;
	public float radius;
	public float speed;
	Rigidbody2D rb;


	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Awake()
	{
		CalcArcticDamage();
	}

	private void Update()
	{
		ProjectileHoming();

	}


	public void ProjectileHoming()
	{
		float distanceToClosestEnemy = Mathf.Infinity;
		EnemyStats target = null;
		EnemyStats[] allTargets = GameObject.FindObjectsOfType<EnemyStats>();

		foreach (EnemyStats currentEnemy in allTargets)
		{
			float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			if (distanceToEnemy < distanceToClosestEnemy)
			{
				distanceToClosestEnemy = distanceToEnemy;
				target = currentEnemy;
			}
		}

		if (target == null)
		{
			print("No Target");
		}
		else
		{
			Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, radius);
			foreach (Collider2D collider2D in colliderArray)
			{
				if (collider2D.GetComponent<EnemyStats>())
				{
					Vector2 pointToTarget = (Vector2)transform.position - (Vector2)target.transform.position;
					pointToTarget.Normalize();
					float value = Vector3.Cross(pointToTarget, transform.right).z;
					rb.angularVelocity = rotatingSpeed * value;
					rb.velocity = transform.right * speed;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyStats>().ETakeDamage((int)totalArcticDamage);
			var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			clone.GetComponent<DamageNumbers>().damageNumber = (int)totalArcticDamage;
			Destroy(this.gameObject);
		}
		if(other.CompareTag("Boss"))
		{
			other.GetComponent<BossStats>().ETakeDamage((int)totalArcticDamage);
			var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			clone.GetComponent<DamageNumbers>().damageNumber = (int)totalArcticDamage;
			Destroy(this.gameObject);
		}
	}

	public void CalcArcticDamage()
	{
		if(GameManager.GameManagerInstance.arcticBlast1Active)
		{
			if(SpellTree.SpellInstance.arcticBlast1Level <= SpellTree.SpellInstance.arcticBlast1LevelMax)
			{
				float calcarcticSpellValue = Character.MyInstance.Intelligence.BaseValue + Character.MyInstance.Intelligence.Value;
				float calcLevelMultiplier = SpellTree.SpellInstance.arcticBlast1Level * 0.25f;
				totalArcticDamage = Random.Range((int)calcarcticSpellValue * calcLevelMultiplier / .5f, (int)calcarcticSpellValue * calcLevelMultiplier * .5f);
			}
		}	
		if(GameManager.GameManagerInstance.arcticBlast2Active)
		{
			if(SpellTree.SpellInstance.arcticBlast2Level <= SpellTree.SpellInstance.arcticBlast2LevelMax)
			{
				float calcarcticSpellValue2 = Character.MyInstance.Intelligence.BaseValue + Character.MyInstance.Intelligence.Value;
				float calcLevelMultiplier2 = SpellTree.SpellInstance.arcticBlast1Level * .8f;
				totalArcticDamage = Random.Range((int)calcarcticSpellValue2 * calcLevelMultiplier2 / .5f, (int)calcarcticSpellValue2 * calcLevelMultiplier2 * .5f);
			}
		}
	}
}
