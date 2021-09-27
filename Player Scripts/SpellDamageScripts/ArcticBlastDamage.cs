using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Mirror.Experimental.NetworkRigidbody2D))]
public class ArcticBlastDamage : NetworkBehaviour
{


	[SyncVar]public float rotatingSpeed = 200f;
	[SyncVar]public float radius;
	[SyncVar]public float speed;
	//Rigidbody2D rb;
	private Mirror.Experimental.NetworkRigidbody2D _netrgb2D;
	[SyncVar] private float totalArcticDamage;
	[SyncVar] private PlayerCombat _playerCombat;
	[SyncVar] private Character _character;

	private GameManager _gameManager;
	private SpellTree _spellTree;

	[ClientRpc]
	public void InitializeBlastDamage(PlayerCombat _playerCombat)
	{
		//rb = GetComponent<Rigidbody2D>();
		_netrgb2D = GetComponent<Mirror.Experimental.NetworkRigidbody2D>();

		this._playerCombat = _playerCombat;
		_character = this._playerCombat.GetComponent<Character>();
		_gameManager =_character.gameObject.GetComponent<PlayerMovement>()._gameManager;
		_spellTree = _gameManager._spellTree;
		CalcArcticDamage();
	}

	private void Update()
	{
		CmdProjectileHoming();

	}
	[Command(requiresAuthority = true)]
	public void CmdProjectileHoming()
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
					_netrgb2D.target.angularVelocity = rotatingSpeed * value;
					_netrgb2D.target.velocity = transform.right * speed;
				}
			}
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			_playerCombat.CmdEnemyNormalAttack(other.gameObject, totalArcticDamage);
			CmdDestroySelf();
		}
		if (other.CompareTag("Boss"))
		{
			_playerCombat.CmdBossNormalAttack(other.gameObject, totalArcticDamage);
			CmdDestroySelf();
		}
	}
	[Command(requiresAuthority = true)]
	public void CmdDestroySelf()
	{
		NetworkServer.Destroy(this.gameObject);
	}
	[Client]
	public void CalcArcticDamage()
	{
		if (_gameManager.arcticBlast1Active)
		{
			if (_spellTree.arcticBlast1Level <= _spellTree.arcticBlast1LevelMax)
			{
				float calcarcticSpellValue = _character.Intelligence.BaseValue + _character.Intelligence.Value;
				float calcLevelMultiplier = _spellTree.arcticBlast1Level * 0.25f;
				totalArcticDamage = Random.Range((int)calcarcticSpellValue * calcLevelMultiplier / .5f, (int)calcarcticSpellValue * calcLevelMultiplier * .5f);
			}
		}
		if (_gameManager.arcticBlast2Active)
		{
			if (_spellTree.arcticBlast2Level <= _spellTree.arcticBlast2LevelMax)
			{
				float calcarcticSpellValue2 = _character.Intelligence.BaseValue + _character.Intelligence.Value;
				float calcLevelMultiplier2 = _spellTree.arcticBlast1Level * .8f;
				CmdCalCulateFireBallDamage(calcarcticSpellValue2, calcLevelMultiplier2);
			}
		}
	}

	[Command(requiresAuthority = true)]
	public void CmdCalCulateFireBallDamage(float fireSpellValue, float levelMultiplier)
	{
		totalArcticDamage = Random.Range((int)fireSpellValue * levelMultiplier / .5f, (int)fireSpellValue * levelMultiplier * .5f);
	}

}
