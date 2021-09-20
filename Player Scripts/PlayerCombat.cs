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
	private UIManager _uiManager;
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
	void HealthCheck()
	{
		if (_character.Health <= 0)
		{
			_character.DisableAllRegen();
			playerDied = true;
			_gameManager.autoSave = false;
			StopCoroutine(_gameManager.saveTimer);
			_character.Health = 0;
			playerMovement.SetPositionDead();
			GetComponent<BoxCollider2D>().enabled = false;
			CheckPlayerDeath();
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

	public void TakeDamage(int damageToGive)
	{
		if (!playerDied)
		{
			_character.notInCombat = false;
			DisableSelfRegenHp();
			DisableSelfRegenMana();
			//set enemy hit to true
			_character.enemyHit = true;
			SoundManager.PlaySound(SoundManager.Sound.PlayerHit);
			_character.Health -= damageToGive;

			_uiManager.UpdateHealth();
			_uiManager.UpdateMP();

			HealthCheck();
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
	public void MeleeAttack(Collider2D other)
	{
		float value = GetComponent<Character>().Strength.Value;
		float playerAttackPower = 0;
		//if critical intialize
		//bool critical = false;
		float normalAttack;
		if (other.gameObject.CompareTag("Enemy"))
		{
			//int randomDamageRange = Random.Range(GetComponent<Character>().Strength.Value,)
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
			//attack more than base attack to show red color
			if (normalAttack > value)
			{
				other.gameObject.GetComponent<EnemyStats>().ETakeDamage((int)normalAttack);
				var clone = (GameObject)Instantiate(damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
				clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
				clone.GetComponent<DamageNumbers>().isRedAttack = true;
				//DamageNumbers.DamageInstance.displayNumber.text = "<color=red><b>" + normalAttack.ToString() + "</color>";
			}
			else
			{
				other.gameObject.GetComponent<EnemyStats>().ETakeDamage((int)normalAttack);
				var clone = (GameObject)Instantiate(damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
				clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
			}
		}
		else if (other.gameObject.CompareTag("Boss"))
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
				other.gameObject.GetComponent<BossStats>().ETakeDamage((int)normalAttack);
				var clone = (GameObject)Instantiate(damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
				clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
				clone.GetComponent<DamageNumbers>().isRedAttack = true;
				//DamageNumbers.DamageInstance.displayNumber.text = "<color=red><b>" + normalAttack.ToString() + "</color>";
			}
			else
			{
				other.gameObject.GetComponent<BossStats>().ETakeDamage((int)normalAttack);
				var clone = (GameObject)Instantiate(damageNumbers, other.transform.position, Quaternion.Euler(Vector3.zero));
				clone.GetComponent<DamageNumbers>().damageNumber = (int)normalAttack;
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
				var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, -90));
				obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -3 + -speed * Time.deltaTime);
			}
			else if (playerMovement.currePos == 1)
			{
				var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.identity);
				obj.GetComponent<Rigidbody2D>().velocity = new Vector2(3 + speed * Time.deltaTime, 0);
			}
			else if (playerMovement.currePos == 2)
			{
				var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, 90));
				obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3 + speed * Time.deltaTime);
			}
			else if (playerMovement.currePos == 3)
			{
				var obj = Instantiate(currentProjectile, castPoint.position, Quaternion.Euler(0, 0, -180));
				obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-3 + -speed * Time.deltaTime, 0);
			}
		}
		else if(_character.Intelligence.BaseValue == 0)
		{
			Toast.Show("You need intelligence to use this", 2f, ToastPosition.MiddleCenter);
		}
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
