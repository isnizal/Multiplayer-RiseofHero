using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EasyUI.Toast;
using Mirror;

public class PlayerCombat : NetworkBehaviour
{
	private static PlayerCombat instance;
	public static PlayerCombat CombatInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<PlayerCombat>();
			}
			return instance;
		}

	}
	public TextMeshProUGUI nameText;
	public GameObject respawnWindow;
	[Header("---> Damage Numbers <---")]
	public GameObject damageNumbers;

	[Tooltip("Where the spell is casting from & Speed")]
	[Header("---> Cast Points/Speed <---")]
	public Transform castPoint;
	public Transform areaCastPoint;
	public bool canCastSpells;
	public float speed;
	public GameObject currentProjectile;

	[Header("---> Fireball Spell <---")]
	public bool fireballActive;
	public GameObject fireballProjectile;
	public int fireballMPCost;

	[Header("---> Icicle Spell <---")]
	public bool icicleActive;
	public GameObject icicleProjectile;
	public int icicleMPCost;

	[Header("---> ArcticBlast Spell <---")]
	public bool arcticBlastActive;
	public GameObject arcticBlastProjectile;
	public int arcticMPCost;

	private PlayerMovement playerMovement;
	private Character _character;
	public bool playerDied;
	private GameManager _gameManager;
	private SpellTree _spellTree;
	public UIManager _uiManager;
	public void InitializePlayerCombat()
	{
		if (isLocalPlayer)
		{
			_character = GetComponent<Character>();
			_gameManager = FindObjectOfType<GameManager>();
			_spellTree = FindObjectOfType<SpellTree>();
			_uiManager = FindObjectOfType<UIManager>();
			respawnWindow = GameObject.Find("RespawnWindow");
			respawnWindow.SetActive(false);
			playerMovement = GetComponent<PlayerMovement>();
		}
	}
	[Command(requiresAuthority = true)]
	public void CmdSetHealth()
	{
		_character.Health = 0;
	}
	void HealthCheck()
	{
		if (isClient)
		{
			if (_character.Health <= 0)
			{
				_character.DisableAllRegen();
				playerDied = true;
				_gameManager.autoSave = false;
				StopCoroutine(_gameManager.saveTimer);
				CmdSetHealth();
				playerMovement.SetPositionDead();
				GetComponent<BoxCollider2D>().enabled = false;
				CheckPlayerDeath();
			}
		}
	}
	public void CheckPlayerDeath()
	{
		if (playerDied)
		{
			playerMovement.canMove = false;
			respawnWindow.SetActive(true);
			_gameManager.BGM.Stop();
			SoundManager.PlaySound(SoundManager.Sound.PlayerDie);
		}
	}
	[Command(requiresAuthority = true)]
	public void CmdCharacterDamage(int damageToGive,Character _character)
	{
		_character.Health -= damageToGive;
	}
	public void TakeDamage(int damageToGive)
	{
		if (hasAuthority)
		{
			if (!playerDied)
			{
				if (isClient)
				{
					_character.notInCombat = false;
					DisableSelfRegenHp();
					DisableSelfRegenMana();
					//set enemy hit to true
					_character.enemyHit = true;
					SoundManager.PlaySound(SoundManager.Sound.PlayerHit);
					CmdCharacterDamage(damageToGive,_character);

					_uiManager.UpdateHealth();
					_uiManager.UpdateMP();

					HealthCheck();
				}
			}
		}
	}

	public void DisplayInformation(Item item)
	{
		Toast.Show("You picked up: <color=red><b>" + item.ItemName + "</b></color>", 2f, ToastPosition.MiddleCenter);
	}

	public void DisplayCoin(int coin)
	{
		Toast.Show("You picked up: <color=red><b>" + coin.ToString() + " </b></color>Copper", 2f, ToastPosition.MiddleCenter);
	}
	public void DisableSelfRegenHp()
	{      
		//check self regen active to stop 
		if (_character.ResetSelfRegenHp != null)
			StopCoroutine(_character.ResetSelfRegenHp);
		if (_character.RestoreHealth != null)
			StopCoroutine(_character.RestoreHealth);

		_character.ResetSelfRegenHp = null;
		_character.RestoreHealth = null;

		EnableSelfRegenHp();
	}
	public void EnableSelfRegenHp()
	{
		_character.ResetSelfRegenHp = _character.SetSelfRegenHp();
		StartCoroutine(_character.ResetSelfRegenHp);
	}
	public void DisableSelfRegenMana()
	{
		if (_character.ResetSelfRegenMana != null)
			StopCoroutine(_character.ResetSelfRegenMana);
		if (_character.RestoreMana != null)
			StopCoroutine(_character.RestoreMana);

		_character.ResetSelfRegenMana = null;
		_character.RestoreMana = null;
		EnableSelfRegenMana();
	}
	public void EnableSelfRegenMana()
	{
		_character.ResetSelfRegenMana = Character.MyInstance.SetSelfRegenMana();
		StartCoroutine(_character.ResetSelfRegenMana);
	}

	private void Update()
	{
		if (isLocalPlayer)
		{
			if (_character is null)
				return;
			if (!_character.onInput)
			{
				if (Input.GetKeyDown(KeyCode.LeftControl) && canCastSpells)
				{
					CheckSpellCost();
				}
			}
			//if (_getPlayerName == string.Empty)
			//	return;
			//if (nameText != null)
			//	nameText.text = _getPlayerName.ToString();
		}
	}
    #region"NetworkAttack"
    [Command(requiresAuthority = false)]
	public void CmdCriticalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcCriticalAttack(clone,normalAttack,enemy);
	}
	[ClientRpc]
	public void RpcCriticalAttack(GameObject clone,float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		clone.GetComponent<DamageNumbers>().isRedAttack = true;
		enemy.GetComponent<EnemyStats>().ETakeDamage((int)normalAttack, this.gameObject);
	}
	[Command(requiresAuthority = false)]
	public void CmdNormalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcNormalAttack(clone, normalAttack,enemy);
	}
	[ClientRpc]
	public void RpcNormalAttack(GameObject clone, float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		enemy.GetComponent<EnemyStats>().ETakeDamage((int)normalAttack, this.gameObject);
	}



	[Command(requiresAuthority = false)]
	public void CmdBossCriticalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcBossCriticalAttack(clone, normalAttack,enemy);
	}
	[ClientRpc]
	public void RpcBossCriticalAttack(GameObject clone, float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		clone.GetComponent<DamageNumbers>().isRedAttack = true;
		enemy.gameObject.GetComponent<BossStats>().ETakeDamage((int)normalAttack, this.gameObject);
	}
	[Command(requiresAuthority = false)]
	public void CmdBossNormalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcBossNormalAttack(clone, normalAttack,enemy);
	}
	[ClientRpc]
	public void RpcBossNormalAttack(GameObject clone, float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		clone.GetComponent<DamageNumbers>().isRedAttack = true;
		enemy.gameObject.GetComponent<BossStats>().ETakeDamage((int)normalAttack, this.gameObject);
	}
	[Command(requiresAuthority = false)]
	public void CmdDestroyObjects(GameObject item)
	{
		NetworkServer.Destroy(item);
		//RpcDestroyObject(item);
	}
	[ClientRpc]
	public void RpcDestroyObject(GameObject item)
	{
		Destroy(item.gameObject);
	}
	[Command(requiresAuthority = false)]
	public void CmdSetTarget(GameObject enemy)
	{
		enemy.gameObject.GetComponent<EnemyAI>().target = this.transform;
	}
    #endregion
    public void MeleeAttack(Collider2D enemy)
	{
		if (isServer) return;
		float value = GetComponent<Character>().Strength.Value;
		float playerAttackPower = 0;
		//if critical intialize
		//bool critical = false;
		float normalAttack;
		if (enemy.gameObject.CompareTag("Enemy"))
		{
			//int randomDamageRange = Random.Range(GetComponent<Character>().Strength.Value,)
			CmdSetTarget(enemy.gameObject);
			//check current attack
			if (value != 0)
			{
				playerAttackPower = 1 * value;
				normalAttack = Random.Range(playerAttackPower / 2 , playerAttackPower * 1.5f);
			}
			else 
			{
				normalAttack = 0;
			}
			//float playerAttackPower = Random.Range(GetComponent<Character>().Strength.Value / 5f, GetComponent<Character>().Strength.Value * 1.5f);
			//critical attack
			//attack more than base attack to show red color
			if (normalAttack > value)
			{
				CmdCriticalAttack(enemy.gameObject, normalAttack);
				//DamageNumbers.DamageInstance.displayNumber.text = "<color=red><b>" + normalAttack.ToString() + "</color>";
			}
			else
			{
				CmdNormalAttack(enemy.gameObject, normalAttack);
			}
		}
		else if (enemy.gameObject.CompareTag("Boss"))
		{
			//int randomDamageRange = Random.Range(GetComponent<Character>().Strength.Value,)
			//check current attack
			if (value != 0)
			{
				playerAttackPower = 1 * value;
				normalAttack = Random.Range(playerAttackPower / 2, playerAttackPower * 1.5f);
			}
			else
			{

				normalAttack = 0;

			}
			//float playerAttackPower = Random.Range(GetComponent<Character>().Strength.Value / 5f, GetComponent<Character>().Strength.Value * 1.5f);
			//attack more than base attack to show red color
			if (normalAttack > value)
			{
				//DamageNumbers.DamageInstance.displayNumber.text = "<color=red><b>" + normalAttack.ToString() + "</color>";
				CmdBossCriticalAttack(enemy.gameObject, normalAttack);
			}
			else
			{
				CmdBossNormalAttack(enemy.gameObject, normalAttack);
			}
		}
		SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
	}
	public void CastSpell()
	{
		if (_character.Intelligence.BaseValue > 0)
		{
			DisableSelfRegenMana();
			SoundManager.PlaySound(SoundManager.Sound.SpellCast);

			if (playerMovement.currePos == 0)
			{
				CmdSpawnSpellDown();
			}
			else if (playerMovement.currePos == 1)
			{
				CmdSpawnSpellRight();
			}
			else if (playerMovement.currePos == 2)
			{
				CmdSpawnSpellUp();
			}
			else if (playerMovement.currePos == 3)
			{
				CmdSpawnSpellLeft();
			}
		}
		else if(_character.Intelligence.BaseValue == 0)
		{
			Toast.Show("You need intelligence to use this", 2f, ToastPosition.MiddleCenter);
		}
	}
	[Command(requiresAuthority = false)]
	public void CmdSpawnSpellDown()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, -90));
		NetworkServer.Spawn(obj);
		obj.GetComponent<Mirror.Experimental.NetworkRigidbody2D>().target.velocity = new Vector2(0, -3 + -speed * Time.deltaTime);
	}
	[Command(requiresAuthority = false)]
	public void CmdSpawnSpellRight()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.identity);
		NetworkServer.Spawn(obj);
		obj.GetComponent<Mirror.Experimental.NetworkRigidbody2D>().target.velocity = new Vector2(3 + speed * Time.deltaTime, 0);
	}
	[Command(requiresAuthority = false)]
	public void CmdSpawnSpellUp()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, 90));
		NetworkServer.Spawn(obj);
		obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3 + speed * Time.deltaTime);
	}
	[Command(requiresAuthority = false)]
	public void CmdSpawnSpellLeft()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, -180));
		NetworkServer.Spawn(obj);
		obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-3 + -speed * Time.deltaTime, 0);
	}

	public void SetCurrentSpell(int spellID)
	{
		Color IconEnabled = new Color(1, 1, 1, 1f);
		Color IconDisabled = new Color(1, 1, 1, 0.5f);
		switch (spellID)
		{
			case 0: //if you run out of mp
				currentProjectile = null;
				fireballActive = false;
				icicleActive = false;
				arcticBlastActive = false;
				_spellTree.fireballSpellImage.color = IconDisabled;
				_spellTree.icicleSpellImage.color = IconDisabled;
				_spellTree.arcticBlastSpellImage.color = IconDisabled;
				break;
			case 1: //Fireball Spell
				currentProjectile = fireballProjectile;
				fireballActive = true;
				_spellTree.fireballSpellImage.color = IconEnabled;
				_spellTree.icicleSpellImage.color = IconDisabled;
				_spellTree.arcticBlastSpellImage.color = IconDisabled;
				icicleActive = false;
				arcticBlastActive = false;
				break;

			case 2: //Icicle Spell
				currentProjectile = icicleProjectile;
				icicleActive = true;
				_spellTree.icicleSpellImage.color = IconEnabled;
				_spellTree.fireballSpellImage.color = IconDisabled;
				_spellTree.arcticBlastSpellImage.color = IconDisabled;
				fireballActive = false;
				arcticBlastActive = false;
				break;
			case 3: //ArcticBlast Spell
				currentProjectile = arcticBlastProjectile;
				arcticBlastActive = true;
				_spellTree.arcticBlastSpellImage.color = IconEnabled;
				_spellTree.icicleSpellImage.color = IconDisabled;
				_spellTree.fireballSpellImage.color = IconDisabled;
				fireballActive = false;
				icicleActive = false;
				break;
		}
	}
	public void ActivateFireball()
	{
		SetCurrentSpell(1);
	}
	public void ActivateIcicle()
	{
		SetCurrentSpell(2);
	}
	public void ActivateArcticBlast()
	{
		SetCurrentSpell(3);
	}
	[Command(requiresAuthority = false)]
	public void CmdEnemyRigidbody2D(GameObject enemy,Vector2 newPos)
	{
		if (enemy == null)
			return;
		EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
		enemyAI._netRigidbody2D.syncVelocity = false;
		enemyAI._netRigidbody2D.clearVelocity = true;
		enemyAI._netRigidbody2D.target.constraints = RigidbodyConstraints2D.FreezeAll;
		enemyAI.target = this.gameObject.transform;
		
		RpcEnemyRigidbody2D(enemy,newPos);
	}
	[ClientRpc]
	public void RpcEnemyRigidbody2D(GameObject enemy,Vector2 newPos)
	{
		if (enemy == null)
			return;
		EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
		enemyAI._netRigidbody2D.syncVelocity = false;
		enemyAI._netRigidbody2D.clearVelocity = true;
		enemyAI._netRigidbody2D.target.constraints = RigidbodyConstraints2D.FreezeAll;
		enemyAI.target = this.gameObject.transform;
	}
	[Command(requiresAuthority = false)]
	public void CmdUnEnemyRigidbody2D(GameObject enemy,Vector2 newPos)
	{
		if (enemy == null)
			return;
		EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
		enemyAI._netRigidbody2D.syncVelocity = true;
		enemyAI._netRigidbody2D.clearVelocity = false;
		enemyAI._netRigidbody2D.target.constraints = RigidbodyConstraints2D.None;
		RpcUnEnemyRigidbody2D(enemy,newPos);
	}
	[ClientRpc]
	public void RpcUnEnemyRigidbody2D(GameObject enemy,Vector2 newPos)
	{
		if (enemy == null)
			return; 
		EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
		enemyAI._netRigidbody2D.syncVelocity = true;
		enemyAI._netRigidbody2D.clearVelocity = false;
		enemyAI._netRigidbody2D.target.constraints = RigidbodyConstraints2D.None;
	}


	[Command(requiresAuthority = false)]
	public void CmdMoveToThis(GameObject enemy,float moveSpeed)
	{
		if (enemy == null)
			return;
		Mirror.NetworkTransform enemyPos = enemy.GetComponent<NetworkTransform>();
		enemyPos.transform.position = Vector3.MoveTowards(enemyPos.transform.position, transform.position, moveSpeed * Time.deltaTime);
		//RpcMoveToThis(enemyPos, moveSpeed);
	}
	[ClientRpc]
	public void RpcMoveToThis(NetworkTransform enemy,float moveSpeed)
	{
		if (enemy == null)
			return;
		enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, transform.position, moveSpeed * Time.deltaTime);
	}
	public void CheckSpellCost()
	{
		if (_character.Intelligence.BaseValue > 0)
		{
			if (fireballActive && canCastSpells)
			{
				if (_character.Mana > fireballMPCost)
				{
					if (_gameManager.fireball1Active)
					{
						CastSpell();
						_character.Mana -= fireballMPCost;
					}
					if (_gameManager.fireball2Active)
					{
						CastSpell();
						_character.Mana -= fireballMPCost * 2;
					}

				}
				else if (_character.Mana < fireballMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
			if (icicleActive && canCastSpells)
			{
				if (_character.Mana > icicleMPCost)
				{
					CastSpell();
					_character.Mana -= icicleMPCost;
				}
				else if (_character.Mana < fireballMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
			if (arcticBlastActive && canCastSpells)
			{
				if (_character.Mana > arcticMPCost)
				{
					CastSpell();
					_character.Mana -= arcticMPCost;
				}
				else if (_character.Mana < arcticMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
		}
		else if(_character.Intelligence.BaseValue == 0)
		{
			Toast.Show("You need Intelligence to use this", 2f, ToastPosition.MiddleCenter);
		}
	}
}
