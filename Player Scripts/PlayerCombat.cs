using EasyUI.Toast;
using Mirror;
using TMPro;
using UnityEngine;

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
	public bool playerDied;

	
	private PlayerMovement _playerMovement;
	public Character character;


	public GameManager gameManager;
	public SpellTree _spellTree;
	
	public UIManager _uiManager;
	public void InitializePlayerCombat()
	{
		if (isLocalPlayer)
		{
			character = GetComponent<Character>();
			_playerMovement = GetComponent<PlayerMovement>();
			gameManager = _playerMovement._gameManager;
			_uiManager = UIManager.Instance;
			_spellTree = SpellTree.SpellInstance;
			respawnWindow = GameObject.Find("RespawnWindow");
			respawnWindow.SetActive(false);

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
	[Command(requiresAuthority = true)]
	public void CmdSetHealth(Character character)
	{
		character.Health = 0;
	}
	void HealthCheck()
	{
		if (isClient)
		{
			if (character.Health <= 0)
			{
				character.DisableAllRegen();
				playerDied = true;
				gameManager.autoSave = false;
				StopCoroutine(gameManager.saveTimer);
				CmdSetHealth(character);
				_playerMovement.SetPositionDead();
				GetComponent<BoxCollider2D>().enabled = false;
				CheckPlayerDeath();
			}
		}
	}
	public void CheckPlayerDeath()
	{
		if (playerDied)
		{
			_playerMovement.canMove = false;
			respawnWindow.SetActive(true);
			gameManager.BGM.Stop();
			SoundManager.PlaySound(SoundManager.Sound.PlayerDie);
		}
	}
	[Command(requiresAuthority = true)]
	public void CmdCharacterDamage(int damageToGive,Character _character)
	{
		_character.Health -= damageToGive;
	}
	[Client]
	public void TakeDamage(int damageToGive)
	{
		if (hasAuthority)
		{
			if (!playerDied)
			{
				if (isClient)
				{
					character.notInCombat = false;
					DisableSelfRegenHp();
					DisableSelfRegenMana();
					//set enemy hit to true
					character.enemyHit = true;
					SoundManager.PlaySound(SoundManager.Sound.PlayerHit);
					CmdCharacterDamage(damageToGive,character);
					_uiManager.UpdateHealth();
					_uiManager.UpdateMP();
					HealthCheck();

				}
			}
		}
	}


	public void DisableSelfRegenHp()
	{
		//check self regen active to stop 
		if (character.ResetSelfRegenHp != null)
			StopCoroutine(character.ResetSelfRegenHp);
		if (character.RestoreHealth != null)
			StopCoroutine(character.RestoreHealth);

		character.ResetSelfRegenHp = null;
		character.RestoreHealth = null;

		EnableSelfRegenHp();
	}
	public void EnableSelfRegenHp()
	{
		character.ResetSelfRegenHp = character.SetSelfRegenHp();
		StartCoroutine(character.ResetSelfRegenHp);
	}
	public void DisableSelfRegenMana()
	{
		if (character.ResetSelfRegenMana != null)
			StopCoroutine(character.ResetSelfRegenMana);
		if (character.RestoreMana != null)
			StopCoroutine(character.RestoreMana);

		character.ResetSelfRegenMana = null;
		character.RestoreMana = null;
		EnableSelfRegenMana();
	}
	public void EnableSelfRegenMana()
	{
		character.ResetSelfRegenMana = character.SetSelfRegenMana();
		StartCoroutine(character.ResetSelfRegenMana);
	}
	private bool isChatOpen = false;
	public bool isMobileChat = false;

	public void MobileChat()
	{
		if (!isMobileChat)
			isMobileChat = true;
		else
			isMobileChat = false;
	}
	private void Update()
	{
		if (isLocalPlayer)
		{
			if (_uiManager == null)
				_uiManager = character.uiManager;
			if (Input.GetKeyDown(KeyCode.C))
			{
				if (isChatOpen)
				{
					_uiManager.chatInput.interactable = false;
					isChatOpen = false;
				}
				else
				{
					_uiManager.chatInput.interactable = true;
					isChatOpen = true;
				}
			}
			if (character == null)
				return;
			if (!character.onInput)
			{
				if (Input.GetKeyDown(KeyCode.LeftControl) && canCastSpells)
				{
					CheckSpellCost();
				}
			}
			if(gameManager == null)
				gameManager = _playerMovement._gameManager;
			if (gameManager != null)
			{
				if (gameManager.isHandheld)
				{
					if (isMobileChat)
						_uiManager.chatInput.interactable = true;
					else
						_uiManager.chatInput.interactable = false;
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
	public void CmdEnemyCriticalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcEnemyCriticalAttack(clone,normalAttack,enemy);
		NetworkConnection con = GetComponent<NetworkIdentity>().connectionToClient;
		TargetEnemyCriticalAttack(con,normalAttack, enemy);
	}
	[ClientRpc]
	public void RpcEnemyCriticalAttack(GameObject clone,float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		clone.GetComponent<DamageNumbers>().isRedAttack = true;

	}
	[TargetRpc]
	public void TargetEnemyCriticalAttack( NetworkConnection player, float normalAttack, GameObject enemy)
	{
		enemy.GetComponent<EnemyStats>().ETakeDamage((int)normalAttack, player);
	}
	[Command(requiresAuthority = false)]
	public void CmdEnemyNormalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcEnemyNormalAttack(clone, normalAttack,enemy);
		NetworkConnection con = GetComponent<NetworkIdentity>().connectionToClient;
		TargetEnemyNormalAttack(con, normalAttack, enemy);
	}
	[ClientRpc]
	public void RpcEnemyNormalAttack(GameObject clone, float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;

	}
	[TargetRpc]
	public void TargetEnemyNormalAttack(NetworkConnection player, float normalAttack, GameObject enemy)
	{
		enemy.GetComponent<EnemyStats>().ETakeDamage((int)normalAttack, player);
	}



	[Command(requiresAuthority = false)]
	public void CmdBossCriticalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcBossCriticalAttack(clone, normalAttack,enemy);
		TargetRpcBossCriticalAttack(connectionToClient, normalAttack, enemy);
	}
	[ClientRpc]
	public void RpcBossCriticalAttack(GameObject clone, float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		clone.GetComponent<DamageNumbers>().isRedAttack = true;

	}
	[TargetRpc]
	public void TargetRpcBossCriticalAttack(NetworkConnection player, float normalAttack, GameObject enemy)
	{
		enemy.gameObject.GetComponent<BossStats>().ETakeDamage(player,(int)normalAttack);
	}
	[Command(requiresAuthority = false)]
	public void CmdBossNormalAttack(GameObject enemy, float normalAttack)
	{
		if (enemy == null)
			return;
		var clone = (GameObject)Instantiate(damageNumbers, enemy.transform.position, Quaternion.Euler(Vector3.zero));
		NetworkServer.Spawn(clone);
		RpcBossNormalAttack(clone, normalAttack,enemy);
		TargetBossNormalAttack(connectionToClient, normalAttack, enemy);
	}
	[ClientRpc]
	public void RpcBossNormalAttack(GameObject clone, float normalAttack,GameObject enemy)
	{
		if (enemy == null)
			return;
		clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
		clone.GetComponent<DamageNumbers>().isRedAttack = true;

	}
	[TargetRpc]
	public void TargetBossNormalAttack(NetworkConnection player, float normalAttack, GameObject enemy)
	{
		enemy.gameObject.GetComponent<BossStats>().ETakeDamage(player,(int)normalAttack);
	}
	[Command(requiresAuthority = false)]
	public void CmdDestroyObjects(GameObject item)
	{
		NetworkServer.Destroy(item);
	}
	[Command(requiresAuthority = false)]
	public void CmdSetTarget(GameObject enemy)
	{
		if (enemy == null)
			return;
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
				CmdEnemyCriticalAttack(enemy.gameObject, normalAttack);
				//DamageNumbers.DamageInstance.displayNumber.text = "<color=red><b>" + normalAttack.ToString() + "</color>";
			}
			else
			{
				CmdEnemyNormalAttack(enemy.gameObject, normalAttack);
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
		if (character.Intelligence.BaseValue > 0)
		{
			DisableSelfRegenMana();
			SoundManager.PlaySound(SoundManager.Sound.SpellCast);

			if (_playerMovement.currePos == 0)
			{
				CmdSpawnSpellDown();
			}
			else if (_playerMovement.currePos == 1)
			{
				CmdSpawnSpellRight();
			}
			else if (_playerMovement.currePos == 2)
			{
				CmdSpawnSpellUp();
			}
			else if (_playerMovement.currePos == 3)
			{
				CmdSpawnSpellLeft();
			}
		}
		else if(character.Intelligence.BaseValue == 0)
		{
			Toast.Show("You need intelligence to use this", 2f, ToastPosition.MiddleCenter);
		}
	}
    #region"SpawnSpell"
    [Command(requiresAuthority = true)]
	public void CmdSpawnSpellDown()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, -90));
		NetworkServer.Spawn(obj,connectionToClient);
		obj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
		if (obj.TryGetComponent<FireballDamage>(out FireballDamage fire))
		{
			fire.RpcInitializeBallDamage(connectionToClient);
		}
		else if (obj.TryGetComponent<IcicleDamage>(out IcicleDamage ice))
		{
			ice.RpcInitializeIcicleDamage(this);
		}
		else if (obj.TryGetComponent<ArcticBlastDamage>(out ArcticBlastDamage arctic))
		{
			arctic.RpcInitializeBlastDamage(this);
		}

		obj.GetComponent<Mirror.Experimental.NetworkRigidbody2D>().target.velocity = new Vector2(0, -3 + -speed * Time.deltaTime);
	}
	[Command(requiresAuthority = true)]
	public void CmdSpawnSpellRight()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.identity);
		NetworkServer.Spawn(obj, connectionToClient);
		obj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
		if (obj.TryGetComponent<FireballDamage>(out FireballDamage fire))
		{
			fire.RpcInitializeBallDamage(connectionToClient);
		}
		else if (obj.TryGetComponent<IcicleDamage>(out IcicleDamage ice))
		{
			ice.RpcInitializeIcicleDamage(this);
		}
		else if (obj.TryGetComponent<ArcticBlastDamage>(out ArcticBlastDamage arctic))
		{
			arctic.RpcInitializeBlastDamage(this);
		}

		obj.GetComponent<Mirror.Experimental.NetworkRigidbody2D>().target.velocity = new Vector2(3 + speed * Time.deltaTime, 0);
	}
	[Command(requiresAuthority = true)]
	public void CmdSpawnSpellUp()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, 90));
		NetworkServer.Spawn(obj, connectionToClient);
		obj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
		if (obj.TryGetComponent<FireballDamage>(out FireballDamage fire))
		{
			fire.RpcInitializeBallDamage(connectionToClient);
		}
		else if (obj.TryGetComponent<IcicleDamage>(out IcicleDamage ice))
		{
			ice.RpcInitializeIcicleDamage(this);
		}
		else if (obj.TryGetComponent<ArcticBlastDamage>(out ArcticBlastDamage arctic))
		{
			arctic.RpcInitializeBlastDamage(this);
		}

		obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3 + speed * Time.deltaTime);
	}
	[Command(requiresAuthority = true)]
	public void CmdSpawnSpellLeft()
	{
		var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, -180));
		NetworkServer.Spawn(obj,connectionToClient);
		obj.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
		if (obj.TryGetComponent<FireballDamage>(out FireballDamage fire))
		{
			fire.RpcInitializeBallDamage(connectionToClient);
		}
		else if (obj.TryGetComponent<IcicleDamage>(out IcicleDamage ice))
		{
			ice.RpcInitializeIcicleDamage(this);
		}
		else if (obj.TryGetComponent<ArcticBlastDamage>(out ArcticBlastDamage arctic))
		{
			arctic.RpcInitializeBlastDamage(this);
		}

		obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-3 + -speed * Time.deltaTime, 0);
	}

	[ClientCallback]
	public void SetCurrentSpell(int spellID)
	{
		Color IconEnabled = new Color(1, 1, 1, 1f);
		Color IconDisabled = new Color(1, 1, 1, 0.5f);
		switch (spellID)
		{
			case 0: //if you run out of mp
				CmdSetCurrentProjectile(0);
				fireballActive = false;
				icicleActive = false;
				arcticBlastActive = false;
				_spellTree.fireballSpellImage.color = IconDisabled;
				_spellTree.icicleSpellImage.color = IconDisabled;
				_spellTree.arcticBlastSpellImage.color = IconDisabled;
				break;
			case 1: //Fireball Spell
				CmdSetCurrentProjectile(1);
				fireballActive = true;
				_spellTree.fireballSpellImage.color = IconEnabled;
				_spellTree.icicleSpellImage.color = IconDisabled;
				_spellTree.arcticBlastSpellImage.color = IconDisabled;
				icicleActive = false;
				arcticBlastActive = false;
				break;

			case 2: //Icicle Spell
				CmdSetCurrentProjectile(2);
				icicleActive = true;
				_spellTree.icicleSpellImage.color = IconEnabled;
				_spellTree.fireballSpellImage.color = IconDisabled;
				_spellTree.arcticBlastSpellImage.color = IconDisabled;
				fireballActive = false;
				arcticBlastActive = false;
				break;
			case 3: //ArcticBlast Spell
				CmdSetCurrentProjectile(3);
				arcticBlastActive = true;
				_spellTree.arcticBlastSpellImage.color = IconEnabled;
				_spellTree.icicleSpellImage.color = IconDisabled;
				_spellTree.fireballSpellImage.color = IconDisabled;
				fireballActive = false;
				icicleActive = false;
				break;
		}
	}
	[Command(requiresAuthority = true)]
	private void CmdSetCurrentProjectile(int id)
	{
		switch (id) {
			case 0:
				currentProjectile = null;
				break;
			case 1:
				currentProjectile = fireballProjectile;
				break;
			case 2:
				currentProjectile = icicleProjectile;
				break;
			case 3:
				currentProjectile = arcticBlastProjectile;
				break;
		}
		TargetSetCurrentProjectile(connectionToClient, id);
		
	}
	[TargetRpc]
	private void TargetSetCurrentProjectile(NetworkConnection conn,int id)
	{
		switch (id)
		{
			case 0:
				conn.identity.gameObject.GetComponent<PlayerCombat>().currentProjectile = null;
				break;
			case 1:
				conn.identity.gameObject.GetComponent<PlayerCombat>().currentProjectile = fireballProjectile;
				break;
			case 2:
				conn.identity.gameObject.GetComponent<PlayerCombat>().currentProjectile = icicleProjectile;
				break;
			case 3:
				conn.identity.gameObject.GetComponent<PlayerCombat>().currentProjectile = arcticBlastProjectile;
				break;
		}
	}
    #endregion
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
    #region"EnableRigidbodyEnemy"
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
	}
	[Command(requiresAuthority = true)]
    private void CmdReduceManaCost(int manaCost)
	{
		character = gameObject.GetComponent<Character>();
		character.Mana -= manaCost;
	}
    #endregion
    public void CheckSpellCost()
	{
		if (character.Intelligence.BaseValue > 0)
		{
			if (fireballActive && canCastSpells)
			{
				if (character.Mana > fireballMPCost)
				{
					if (gameManager.fireball1Active)
					{
						CastSpell();
						CmdReduceManaCost(fireballMPCost);
					}
					if (gameManager.fireball2Active)
					{
						CastSpell();
						CmdReduceManaCost(fireballMPCost * 2);
					}

				}
				else if (character.Mana < fireballMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
			if (icicleActive && canCastSpells)
			{
				if (character.Mana > icicleMPCost)
				{
					CastSpell();
					CmdReduceManaCost(icicleMPCost);
				}
				else if (character.Mana < fireballMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
			if (arcticBlastActive && canCastSpells)
			{
				if (character.Mana > arcticMPCost)
				{
					CastSpell();
					CmdReduceManaCost(arcticMPCost);
				}
				else if (character.Mana < arcticMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
		}
		else if(character.Intelligence.BaseValue == 0)
		{
			Toast.Show("You need Intelligence to use this", 2f, ToastPosition.MiddleCenter);
		}
	}
}
