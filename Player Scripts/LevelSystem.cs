using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
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

	private void Update()
	{
		expToLevel = toLevelUp[currentLevel];
		CheckLevel();
	}

	public void CheckLevel()
	{
		if(currentLevel == 1)
		{
			Character.MyInstance.statPoints = 0;
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

	public void AddExp(int expToAdd)
	{
		currentExp += expToAdd;
	}

	void StatPoints()
	{
		if (canLevelUp)
		{
			Character.MyInstance.newStatPoints = UnityEngine.Random.Range(1, 5);
			Character.MyInstance.statPoints += Character.MyInstance.newStatPoints;
			Character.MyInstance.newStatPoints = 0;
			SpellPoints();
			ResetPlayerHealthAndMP();
			canLevelUp = false;
		}
	}

	void ResetPlayerHealthAndMP()
	{
		Character.MyInstance.Health = Character.MyInstance.MaxHealth;
		Character.MyInstance.Mana = Character.MyInstance.MaxMP;
		PlayerCombat.CombatInstance.DisableSelfRegenHp();
		PlayerCombat.CombatInstance.DisableSelfRegenMana();
	}	

	void SpellPoints()
	{
		if(canLevelUp && currentLevel > 1)
		{
			SpellTree.SpellInstance.spellPointsAvailable += 1;
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
