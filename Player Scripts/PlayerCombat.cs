using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EasyUI.Toast;

public class PlayerCombat : MonoBehaviour
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
	public bool playerDied;
    private void Awake()
    {

    }

	public void FindRespawnWindow()
	{
		respawnWindow = GameObject.Find("RespawnWindow");
		respawnWindow.SetActive(false);
		playerMovement = GetComponent<PlayerMovement>();
	}
	void HealthCheck()
	{
		if (Character.MyInstance.Health <= 0)
		{
			Character.MyInstance.DisableAllRegen();
			playerDied = true;
			GameManager.instance.autoSave = false;
			StopCoroutine(GameManager.instance.saveTimer);
			Character.MyInstance.Health = 0;
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
			GameManager.GameManagerInstance.BGM.Stop();
			SoundManager.PlaySound(SoundManager.Sound.PlayerDie);
		}
	}

	public void TakeDamage(int damageToGive)
	{
		if (!playerDied)
		{
			Character.MyInstance.notInCombat = false;
			DisableSelfRegenHp();
			DisableSelfRegenMana();
			//set enemy hit to true
			Character.MyInstance.enemyHit = true;
			SoundManager.PlaySound(SoundManager.Sound.PlayerHit);
			Character.MyInstance.Health -= damageToGive;

			UIManager.Instance.UpdateHealth();
			UIManager.Instance.UpdateMP();

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
		if (Character.MyInstance.ResetSelfRegenHp != null)
			StopCoroutine(Character.MyInstance.ResetSelfRegenHp);
		if (Character.MyInstance.RestoreHealth != null)
			StopCoroutine(Character.MyInstance.RestoreHealth);

		Character.MyInstance.ResetSelfRegenHp = null;
		Character.MyInstance.RestoreHealth = null;

		EnableSelfRegenHp();
	}
	public void EnableSelfRegenHp()
	{
		Character.MyInstance.ResetSelfRegenHp = Character.MyInstance.SetSelfRegenHp();
		StartCoroutine(Character.MyInstance.ResetSelfRegenHp);
	}
	public void DisableSelfRegenMana()
	{
		if (Character.MyInstance.ResetSelfRegenMana != null)
			StopCoroutine(Character.MyInstance.ResetSelfRegenMana);
		if (Character.MyInstance.RestoreMana != null)
			StopCoroutine(Character.MyInstance.RestoreMana);

		Character.MyInstance.ResetSelfRegenMana = null;
		Character.MyInstance.RestoreMana = null;
		EnableSelfRegenMana();
	}
	public void EnableSelfRegenMana()
	{
		Character.MyInstance.ResetSelfRegenMana = Character.MyInstance.SetSelfRegenMana();
		StartCoroutine(Character.MyInstance.ResetSelfRegenMana);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftControl) && canCastSpells)
		{
			CheckSpellCost();
		}
	}
	public void MeleeAttack(Collider2D other)
	{
		float value = GetComponent<Character>().Strength.Value;
		float playerAttackPower = 0;
		//if critical intialize
		bool critical = false;
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
		if (Character.MyInstance.Intelligence.BaseValue > 0)
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
		else if(Character.MyInstance.Intelligence.BaseValue == 0)
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
				SpellTree.SpellInstance.fireballSpellImage.color = IconDisabled;
				SpellTree.SpellInstance.icicleSpellImage.color = IconDisabled;
				SpellTree.SpellInstance.arcticBlastSpellImage.color = IconDisabled;
				break;
			case 1: //Fireball Spell
				currentProjectile = fireballProjectile;
				fireballActive = true;
				SpellTree.SpellInstance.fireballSpellImage.color = IconEnabled;
				SpellTree.SpellInstance.icicleSpellImage.color = IconDisabled;
				SpellTree.SpellInstance.arcticBlastSpellImage.color = IconDisabled;
				icicleActive = false;
				arcticBlastActive = false;
				break;

			case 2: //Icicle Spell
				currentProjectile = icicleProjectile;
				icicleActive = true;
				SpellTree.SpellInstance.icicleSpellImage.color = IconEnabled;
				SpellTree.SpellInstance.fireballSpellImage.color = IconDisabled;
				SpellTree.SpellInstance.arcticBlastSpellImage.color = IconDisabled;
				fireballActive = false;
				arcticBlastActive = false;
				break;
			case 3: //ArcticBlast Spell
				currentProjectile = arcticBlastProjectile;
				arcticBlastActive = true;
				SpellTree.SpellInstance.arcticBlastSpellImage.color = IconEnabled;
				SpellTree.SpellInstance.icicleSpellImage.color = IconDisabled;
				SpellTree.SpellInstance.fireballSpellImage.color = IconDisabled;
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
		if (Character.MyInstance.Intelligence.BaseValue > 0)
		{
			if (fireballActive && canCastSpells)
			{
				if (Character.MyInstance.Mana > fireballMPCost)
				{
					if (GameManager.GameManagerInstance.fireball1Active)
					{
						CastSpell();
						Character.MyInstance.Mana -= fireballMPCost;
					}
					if (GameManager.GameManagerInstance.fireball2Active)
					{
						CastSpell();
						Character.MyInstance.Mana -= fireballMPCost * 2;
					}

				}
				else if (Character.MyInstance.Mana < fireballMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
			if (icicleActive && canCastSpells)
			{
				if (Character.MyInstance.Mana > icicleMPCost)
				{
					CastSpell();
					Character.MyInstance.Mana -= icicleMPCost;
				}
				else if (Character.MyInstance.Mana < fireballMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
			if (arcticBlastActive && canCastSpells)
			{
				if (Character.MyInstance.Mana > arcticMPCost)
				{
					CastSpell();
					Character.MyInstance.Mana -= arcticMPCost;
				}
				else if (Character.MyInstance.Mana < arcticMPCost)
				{
					Toast.Show("You need more MP", 2f, ToastPosition.MiddleCenter);
					SetCurrentSpell(0);
					print("You dont have enough MP for this!");
				}
			}
		}
		else if(Character.MyInstance.Intelligence.BaseValue == 0)
		{
			Toast.Show("You need Intelligence to use this", 2f, ToastPosition.MiddleCenter);
		}
	}
}
