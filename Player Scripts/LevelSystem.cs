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

	[SyncVar]public int currentLevel;
    [SyncVar]public int currentExp;
    [SyncVar]public int expToLevel;
	[SyncVar]public bool canLevelUp = false;
	[SyncVar]public float expMultiplier;
	public int[] toLevelUp;

	private Character _character;
	private PlayerCombat _playerCombat;
	private SpellTree _spellTree;
	public void InitializeLevelSystem()
	{
		_character = GetComponent<Character>();
		_playerCombat = GetComponent<PlayerCombat>();
		_spellTree = _playerCombat._spellTree;
	}
	private void Update()
	{
		if (isLocalPlayer)
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
	[Client]
	public void ExperienceReward(int expToAdd)
	{
		CmdAddExperienceReward(expToAdd);
	}
	[Command(requiresAuthority = true)]
	public void CmdAddExperienceReward(int expToAdd)
	{
		AddExp(connectionToClient, expToAdd);
	}
	[TargetRpc]
	public void AddExp(NetworkConnection conn, int expToAdd)
	{
		conn.identity.GetComponent<LevelSystem>().currentExp += expToAdd;
		//NetworkConnection net = GetComponent<NetworkIdentity>().connectionToClient;
		//TargetAddExp(net, expToAdd);
	}
	
	[TargetRpc]
	public void TargetAddExp(NetworkConnection target, int expToAdd)
	{
		//target.identity.gameObject.GetComponent<LevelSystem>().currentExp += expToAdd;
	}

	[Client]
	void StatPoints()
	{
		if (canLevelUp)
		{
			_character.newStatPoints = UnityEngine.Random.Range(1, 5);
			_character.statPoints += _character.newStatPoints;
			_character.newStatPoints = 0;
			SpellPoints();
			CmdResetPlayerHealthAndMp();
			canLevelUp = false;
		}
	}
	[Command(requiresAuthority = true)]
	public void CmdResetPlayerHealthAndMp()
	{
		TargetRpcResetPlayerHealthAndMp(this.netIdentity.connectionToClient);
	}
	[TargetRpc]
	public void TargetRpcResetPlayerHealthAndMp(NetworkConnection player)
	{
		Character character = player.identity.gameObject.GetComponent<Character>();
		PlayerCombat playerCombat = player.identity.gameObject.GetComponent<PlayerCombat>();
		playerCombat.DisableSelfRegenHp();
		character.ExecuteHealth(character.MaxHealth);
		playerCombat.DisableSelfRegenMana();
		character.ExecuteMana(character.MaxMP);
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
