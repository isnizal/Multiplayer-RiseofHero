using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Mirror.Experimental.NetworkRigidbody2D))]
public class IcicleDamage : NetworkBehaviour
{

	public float speed;
	//private Rigidbody2D _rb;
	private Mirror.Experimental.NetworkRigidbody2D _netRb2D;
	[SyncVar]private float timeToDestroy = 1f;
	[SyncVar]private PlayerCombat _playerCombat;
	[SyncVar]private Character _character;
	[SyncVar]private float totalIcicleDamage;
	private GameManager _gameManager;
	private SpellTree _spellTree;

	[ClientRpc]
	public void RpcInitializeIcicleDamage(PlayerCombat _playerCombat)
	{
		this._playerCombat = _playerCombat;
		_character = _playerCombat.GetComponent<Character>();
		_gameManager = _playerCombat.gameManager;
		_spellTree = _gameManager._spellTree;
		//_rb = GetComponent<Rigidbody2D>();
		_netRb2D = GetComponent<Mirror.Experimental.NetworkRigidbody2D>();
		CalcIcicleDamage();
	}
	[Command(requiresAuthority = true)]
	private void CmdMoveFireBall()
	{
		RpcMoveFireBall();
	}
	[ClientRpc]
	private void RpcMoveFireBall()
	{
		_netRb2D.target.MovePosition(transform.TransformPoint(speed * Time.deltaTime * Vector3.right));
	}

	private bool isDestroy = false;
	private void Update()
	{
		if (hasAuthority)
		{
			if (isDestroy)
				return;
			CmdMoveFireBall();
			CmdDestroySelf();
		}
	}
	[Command(requiresAuthority = true)]
	private void CmdDestroySelf()
	{
		timeToDestroy -= Time.deltaTime;
		if (timeToDestroy <= 0)
		{
			isDestroy = true;
			NetworkServer.Destroy(this.gameObject);
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			if (_playerCombat == null)
				return;
			EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();
			enemyAI.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			enemyAI.freezing = true;
			enemyAI.StartFreeze(2f);
			//other.GetComponent<EnemyStats>().ETakeDamage((int)totalIcicleDamage,other.gameObject);
			//var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			//clone.GetComponent<DamageNumbers>().damageNumber = (int)totalIcicleDamage;
			_playerCombat.CmdEnemyNormalAttack(other.gameObject, totalIcicleDamage);
			CmdDestroyThis();
		}
		if(other.CompareTag("Boss"))
		{
			if (_playerCombat == null)
				return;
			EnemyAI enemyAI = other.GetComponent<EnemyAI>();
			enemyAI.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
			enemyAI.freezing = true;
			enemyAI.StartFreeze(1f);
			//other.GetComponent<BossStats>().ETakeDamage((int)totalIcicleDamage,other.gameObject);
			//var clone = (GameObject)Instantiate(PlayerCombat.CombatInstance.damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
			//clone.GetComponent<DamageNumbers>().damageNumber = (int)totalIcicleDamage;
			_playerCombat.CmdEnemyNormalAttack(other.gameObject, totalIcicleDamage);
			CmdDestroyThis();
		}
	}
	[Command(requiresAuthority = true)]
	private void CmdCalCulateIcicleBallDamage(float fireSpellValue, float levelMultiplier)
	{
		totalIcicleDamage = Random.Range((int)fireSpellValue * levelMultiplier / .5f, (int)fireSpellValue * levelMultiplier * .5f);
		RpcCalculateIcicleBallDamage(totalIcicleDamage);
	}
	[ClientRpc]
	private void RpcCalculateIcicleBallDamage(float totalIcicleDamage)
	{
		this.totalIcicleDamage = totalIcicleDamage;
	}

	[Command(requiresAuthority = true)]
	private void CmdDestroyThis()
	{
		isDestroy = true;
		NetworkServer.Destroy(this.gameObject);
	}
	[Client]
	private void CalcIcicleDamage()
	{
		if(_gameManager.icicle1Active)
		{
			if (_spellTree.icicle1Level <= _spellTree.icicle1LevelMax)
			{
				float calcicicleSpellValue = _character.Intelligence.BaseValue + _character.Intelligence.Value;
				float calcLevelMultiplier = _spellTree.icicle1Level * 0.2f;
				CmdCalCulateIcicleBallDamage(calcicicleSpellValue, calcLevelMultiplier);
			}

		}
		if(_gameManager.icicle2Active)
		{
			if(_spellTree.icicle2Level <= _spellTree.icicle2LevelMax)
			{
				float calcicicleSpellValue2 = _character.Intelligence.BaseValue + _character.Intelligence.Value;
				float calcLevelMultiplier2 = _spellTree.icicle2Level * .5f;
				CmdCalCulateIcicleBallDamage(calcicicleSpellValue2, calcLevelMultiplier2);
			}
		}

	}
}
