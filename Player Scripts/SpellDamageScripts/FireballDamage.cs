using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDamage : MonoBehaviour
{
	private Rigidbody2D _rb;
	public float speed;
	public float totalFireballDamage;


	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		CalcFireballDamage();
	}

	private void Update()
	{
		_rb.MovePosition(transform.TransformPoint(speed * Time.deltaTime * Vector3.right));
	}



	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Enemy"))
		{
			other.GetComponent<EnemyStats>().ETakeDamage((int)totalFireballDamage, other.gameObject);
			var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			clone.GetComponent<DamageNumbers>().damageNumber = (int)totalFireballDamage;
			Destroy(this.gameObject);
		}
		if(other.CompareTag("Boss"))
		{
			other.GetComponent<BossStats>().ETakeDamage((int)totalFireballDamage,other.gameObject);
			var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			clone.GetComponent<DamageNumbers>().damageNumber = (int)totalFireballDamage;
			Destroy(this.gameObject);
		}
	}

	public void CalcFireballDamage()
	{
		if (GameManager.GameManagerInstance.fireball1Active)
		{
			if (SpellTree.SpellInstance.fireball1Level <= SpellTree.SpellInstance.fireball1LevelMax)
			{
				float calcfireballSpellValue = Character.MyInstance.Intelligence.BaseValue + Character.MyInstance.Intelligence.Value;
				float calcLevelMultiplier = SpellTree.SpellInstance.fireball1Level * 0.125f;
				totalFireballDamage = Random.Range((int)calcfireballSpellValue * calcLevelMultiplier / .5f, (int)calcfireballSpellValue * calcLevelMultiplier * .5f);
			}
		}
		if (GameManager.GameManagerInstance.fireball2Active)
		{
			if (SpellTree.SpellInstance.fireball2Level <= SpellTree.SpellInstance.fireball2LevelMax)
			{
				float calcfireballSpellValue2 = Character.MyInstance.Intelligence.BaseValue + Character.MyInstance.Intelligence.Value;
				float calcLevelMultiplier2 = SpellTree.SpellInstance.fireball2Level * .5f;
				totalFireballDamage = Random.Range((int)calcfireballSpellValue2 * calcLevelMultiplier2 / .5f, (int)calcfireballSpellValue2 * calcfireballSpellValue2 * .5f);
			}
		}
	}
}
