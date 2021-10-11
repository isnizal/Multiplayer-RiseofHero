using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager
{
	public static void SavePlayerData(Character character, LevelSystem levelSystem)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.ppp", FileMode.Create);

		PlayerData data = new PlayerData(character, levelSystem);

		bf.Serialize(stream, data);
		stream.Close();
	}
	public static void SavePlayerAchievement(AchievementManager achievement)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerAchievementData.ppp", FileMode.Create);

		PlayerData data = new PlayerData(achievement);

		bf.Serialize(stream, data);
		stream.Close();
	}
	public static int[] LoadPlayer()
	{
		if(File.Exists(Application.persistentDataPath + "/PlayerData.ppp"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.ppp", FileMode.Open);

			PlayerData data = bf.Deserialize(stream) as PlayerData;

			stream.Close();
			return data.playerIntStats;
		}
		else
		{
			Debug.Log("File does not exist");
			return null;
		}
	}
	public static int[] LoadAchievement()
	{
		if (File.Exists(Application.persistentDataPath + "/PlayerAchievementData.ppp"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerAchievementData.ppp", FileMode.Open);

			PlayerData data = bf.Deserialize(stream) as PlayerData;

			stream.Close();
			return data.playerAchievementStats;
		}
		else
		{
			Debug.Log("File does not exist");
			return null;
		}
	}
}

[Serializable]
public class PlayerData
{
	public int[] playerIntStats;
	public int[] playerAchievementStats;
	public PlayerData(Character character, LevelSystem levelSystem)
	{
		playerIntStats = new int[30];
		playerIntStats[0] = character.Health;
		playerIntStats[1] = character.MaxHealth;
		playerIntStats[2] = character.Mana;
		playerIntStats[3] = character.MaxMP;
		playerIntStats[4] = character.copperCurrency;
		playerIntStats[5] = levelSystem.currentLevel;
		playerIntStats[6] = levelSystem.currentExp;
		playerIntStats[7] = character.statPoints;
		playerIntStats[8] = character.statPointsAllocated;
		playerIntStats[9] =	(int)character.Strength.BaseValue;
		playerIntStats[10] = (int)character.Defense.BaseValue;
		playerIntStats[11] = (int)character.Intelligence.BaseValue;
		playerIntStats[12] = (int)character.Vitality.BaseValue;
		playerIntStats[13] = character.premiumCurrency;
		playerIntStats[14] = character.playerMovement._gameManager._spellTree.spellPointsAvailable;
		playerIntStats[15] = character.playerMovement._gameManager._spellTree.spellPointsAllocated;
		playerIntStats[16] = character.playerMovement._gameManager._spellTree.fireball1Level;
		playerIntStats[17] = character.playerMovement._gameManager._spellTree.icicle1Level;
		playerIntStats[18] = character.playerMovement._gameManager._spellTree.arcticBlast1Level;
		playerIntStats[19] = (int)character.playerPosX;
		playerIntStats[20] = (int)character.playerPosY;
		playerIntStats[21] = character.playerMovement.helmetValue;
		playerIntStats[22] = character.playerMovement.torsoValue;
		playerIntStats[23] = character.playerMovement.armValue;
		playerIntStats[24] = character.playerMovement.bootValue;
		playerIntStats[25] = character.playerMovement.swordValue;
		playerIntStats[26] = character.playerMovement.shieldValue;
		playerIntStats[27] = character.playerMovement.hairValue;
		//playerIntStats[28] = GameManager.GameManagerInstance.devilQueenDefeated;
		playerIntStats[28] = character.playerMovement._gameManager.firstTimePlaying;
	}
	public PlayerData(AchievementManager achievementManager)
	{
		playerAchievementStats = new int[22];
		playerAchievementStats[0] = achievementManager.killSlimeCountAch;
		playerAchievementStats[1] = achievementManager.killSlime1Active ;
		playerAchievementStats[2] = achievementManager.killSlime1Claimed;
		playerAchievementStats[3] = achievementManager.killSlime1Done;
		playerAchievementStats[4] = achievementManager.collectCopperCountAch;
		playerAchievementStats[5] = achievementManager.collectCopper1Active;
		playerAchievementStats[6] = achievementManager.collectCopper1Claimed;
		playerAchievementStats[7] = achievementManager.collectCopper1Done;
		playerAchievementStats[8] = achievementManager.kill50CountAch;
		playerAchievementStats[9] = achievementManager.kill50AchClaimed;
		playerAchievementStats[10] = achievementManager.kill50AchDone;
		playerAchievementStats[11] = achievementManager.kill50AchActive;
		playerAchievementStats[12] = achievementManager.killDevilQueenActive;
		playerAchievementStats[13] = achievementManager.killDevilQueenClaimed;
		playerAchievementStats[14] = achievementManager.killDevilQueenCountAch;
		playerAchievementStats[15] = achievementManager.killDevilQueenDone;
		playerAchievementStats[16] = achievementManager.collectCopper2Active;
		playerAchievementStats[17] = achievementManager.collectCopper2Claimed;
		playerAchievementStats[18] = achievementManager.collectCopper2Done;
		playerAchievementStats[19] = achievementManager.kill500AchActive;
		playerAchievementStats[20] = achievementManager.kill500AchClaimed;
		playerAchievementStats[21] = achievementManager.kill500AchDone;
	}
}

