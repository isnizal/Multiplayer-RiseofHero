using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Mirror.Experimental.NetworkRigidbody2D))]
public class FireballDamage : NetworkBehaviour
{
	//private Rigidbody2D _rb;
	private Mirror.Experimental.NetworkRigidbody2D _netrbd2D;
	public float speed;

	[SyncVar]private float totalFireballDamage;
	[SyncVar]private PlayerCombat _playerCombat;
	[SyncVar]private Character _character;
	[SyncVar]private float timeToDestroy = 1f;

	private GameManager _gameManager;
	private SpellTree _spellTree;

	[ClientRpc]
	public void RpcInitializeBallDamage(PlayerCombat _playerCombat)
	{
		_netrbd2D = GetComponent<Mirror.Experimental.NetworkRigidbody2D>();
		this._playerCombat = _playerCombat;
		this._character = this._playerCombat.character;
		this._gameManager = _character.GetComponent<PlayerMovement>()._gameManager;
		this._spellTree = _gameManager._spellTree;
		CalcFireballDamage();
	}
	private void Update()
	{
		if (hasAuthority)
		{
			CmdMoveFireBall();
			CmdDestoryOverTime();
		}

	}
	[Command(requiresAuthority = true)]
	private void CmdDestoryOverTime()
	{
		timeToDestroy -= Time.deltaTime;
		if (timeToDestroy <= 0)
		{
			if (this.gameObject == null)
				return;
			NetworkServer.Destroy(this.gameObject);
		}
	}
	[Command(requiresAuthority = true)]
	private void CmdMoveFireBall()
	{
		RpcMoveFireBall();
	}
	[ClientRpc]
	private void RpcMoveFireBall()
	{
		_netrbd2D.target.MovePosition(transform.TransformPoint(speed * Time.deltaTime * Vector3.right));
	}
	[Command(requiresAuthority = true)]
	private void CmdDestroySelf()
	{
		if (this.gameObject == null)
			return;
		NetworkServer.Destroy(this.gameObject);
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Enemy"))
		{
			_playerCombat.CmdEnemyNormalAttack(other.gameObject, totalFireballDamage);
			CmdDestroySelf();
		}
		if(other.CompareTag("Boss"))
		{
			_playerCombat.CmdBossNormalAttack(other.gameObject, totalFireballDamage);
			CmdDestroySelf();
		}
	}

	[Command(requiresAuthority = true)]
	private void CmdCalCulateFireBallDamage(float fireSpellValue, float levelMultiplier)
	{
		float totalFireballDamage = Random.Range((int)fireSpellValue * levelMultiplier / .5f, (int)fireSpellValue * levelMultiplier * .5f);
		this.totalFireballDamage = totalFireballDamage;
	}

	[Client]
	private void CalcFireballDamage()
	{
		if (_gameManager.fireball1Active)
		{
			if (_spellTree.fireball1Level <= _spellTree.fireball1LevelMax)
			{
				float calcfireballSpellValue = _character.Intelligence.BaseValue + _character.Intelligence.Value;
				float calcLevelMultiplier = _gameManager._spellTree.fireball1Level * 0.125f;
				CmdCalCulateFireBallDamage(calcfireballSpellValue, calcLevelMultiplier);
			}
		}
		if (_gameManager.fireball2Active)
		{
			if (_spellTree.fireball2Level <= _spellTree.fireball2LevelMax)
			{
				float calcfireballSpellValue2 = _character.Intelligence.BaseValue + _character.Intelligence.Value;
				float calcLevelMultiplier2 = _gameManager._spellTree.fireball2Level * .5f;
				CmdCalCulateFireBallDamage(calcfireballSpellValue2, calcLevelMultiplier2);
			}
		}
	}
}
