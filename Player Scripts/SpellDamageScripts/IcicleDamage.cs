using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleDamage : MonoBehaviour
{
	public float totalIcicleDamage;
	public float speed;
	private Rigidbody2D _rb;

	private void Awake()
	{
		
	}

	private void Start()
	{
		CalcIcicleDamage();
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		
		_rb.MovePosition(transform.TransformPoint(speed * Time.deltaTime * Vector3.right));
	}



	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
			enemyAI.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			enemyAI.freezing = true;
			enemyAI.StartFreeze(2f);
			other.GetComponent<EnemyStats>().ETakeDamage((int)totalIcicleDamage,other.gameObject);
			var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			clone.GetComponent<DamageNumbers>().damageNumber = (int)totalIcicleDamage;
			Destroy(this.gameObject);
		}
		if(other.CompareTag("Boss"))
		{
			EnemyAI enemyAI = other.GetComponent<EnemyAI>();
			enemyAI.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			enemyAI.freezing = true;
			enemyAI.StartFreeze(1f);
			other.GetComponent<BossStats>().ETakeDamage((int)totalIcicleDamage,other.gameObject);
			var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			clone.GetComponent<DamageNumbers>().damageNumber = (int)totalIcicleDamage;
			Destroy(this.gameObject);
		}
	}

	public void CalcIcicleDamage()
	{
		if(GameManager.GameManagerInstance.icicle1Active)
		{
			if (SpellTree.SpellInstance.icicle1Level <= SpellTree.SpellInstance.icicle1LevelMax)
			{
				float calcicicleSpellValue = Character.MyInstance.Intelligence.BaseValue + Character.MyInstance.Intelligence.Value;
				float calcLevelMultiplier = SpellTree.SpellInstance.icicle1Level * 0.2f;
				totalIcicleDamage = Random.Range((int)calcicicleSpellValue * calcLevelMultiplier / .5f, (int)calcicicleSpellValue * calcLevelMultiplier * .5f);
			}

		}
		if(GameManager.GameManagerInstance.icicle2Active)
		{
			if(SpellTree.SpellInstance.icicle2Level <= SpellTree.SpellInstance.icicle2LevelMax)
			{
				float calcicicleSpellValue2 = Character.MyInstance.Intelligence.BaseValue + Character.MyInstance.Intelligence.Value;
				float calcLevelMultiplier2 = SpellTree.SpellInstance.icicle2Level * .5f;
				totalIcicleDamage = Random.Range((int)calcicicleSpellValue2 * calcLevelMultiplier2 / .5f, (int)calcicicleSpellValue2 * calcLevelMultiplier2 * .5f);
			}
		}

	}
}
