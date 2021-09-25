using System;
using UnityEngine;
using Mirror;

public class LevelSystem : NetworkBehaviour
{
    public static LevelSystem instance;
	public static LevelSystem LevelInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<LevelSystem>();
			}
			return instance;
		}

	}
	public GameObject levelupParticle;
	public GameObject levelupBanner;
	public GameObject playerOverhead;
	public int currentLevel;
    public int currentExp;
    public int expToLevel;
	public bool canLevelUp = false;
	public float expMultiplier;
	public int[] toLevelUp;

	private Character _character;
	private PlayerCombat _playerCombat;
	private SpellTree _spellTree;
	public void InitializeLevelSystem()
	{
		_character = GetComponent<Character>();
		_playerCombat = GetComponent<PlayerCombat>();
		_spellTree = FindObjectOfType<SpellTree>();
	}
	private void Update()
	{
		if (hasAuthority)
		{
			if (_character is null)
				return;
			expToLevel = toLevelUp[currentLevel];
			CheckLevel();
		}
	}

	public void CheckLevel()
	{
		if(currentLevel == 1)
		{
			_character.statPoints = 0;
			canLevelUp = false;
		}

		if(currentExp >= toLevelUp[currentLevel])
		{
			canLevelUp = true;
		}
		if (canLevelUp)
		{
			LevelUp();
			canLevelUp = false;
			if(currentLevel > 1)
			{
				SoundManager.PlaySound(SoundManager.Sound.LevelUp);
				Instantiate(levelupParticle, playerOverhead.transform.position, Quaternion.identity);
				Instantiate(levelupBanner, playerOverhead.transform.position, Quaternion.identity);
			}
		}
	}

	private void LevelUp()
	{
		currentExp -= toLevelUp[currentLevel];
		currentLevel += 1;
		if(canLevelUp)
		{
			StatPoints();
		}
		else
		{
			canLevelUp = false;
		}
	}	

	[Server]
	public void AddExp(int expToAdd)
	{
		NetworkConnection net = GetComponent<NetworkIdentity>().connectionToClient;
		TargetAddExp(net, expToAdd);
	}
	[TargetRpc]
	public void TargetAddExp(NetworkConnection target, int expToAdd)
	{
		target.identity.gameObject.GetComponent<LevelSystem>().currentExp += expToAdd;
	}


	void StatPoints()
	{
		if (canLevelUp)
		{
			_character.newStatPoints = UnityEngine.Random.Range(1, 5);
			_character.statPoints += _character.newStatPoints;
			_character.newStatPoints = 0;
			SpellPoints();
			ResetPlayerHealthAndMP();
			canLevelUp = false;
		}
	}

	void ResetPlayerHealthAndMP()
	{
		_character.Health = _character.MaxHealth;
		_character.Mana = _character.MaxMP;
		_playerCombat.DisableSelfRegenHp();
		_playerCombat.DisableSelfRegenMana();
	}	

	void SpellPoints()
	{
		if(canLevelUp && currentLevel > 1)
		{
			_spellTree.spellPointsAvailable += 1;
		}
	}

	private void OnValidate()
	{
		CalculateLevels();
	}

	void CalculateLevels()
	{
		int step = (int)Mathf.Abs(currentLevel + (200.0f * expMultiplier / 2.5f));
		for (int i = 0; i < toLevelUp.Length; i++)
		{
			toLevelUp[i] = step * i;
		}
	}
}
