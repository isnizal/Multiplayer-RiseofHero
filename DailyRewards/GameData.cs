using UnityEngine;

public static class GameData
{
	private static int _copperCoins = 0;
	private static int _experience = 0;
	private static int _gems = 0;
	private static string _item;

	static GameData()
	{
		_copperCoins = PlayerPrefs.GetInt("CopperCoins", 0);
		_experience = PlayerPrefs.GetInt("Experience", 0);
		_gems = PlayerPrefs.GetInt("Gems", 0);
		_item = PlayerPrefs.GetString("Item", _item);
	}

	public static int CopperCoins
	{
		get { return _copperCoins; }
		set { PlayerPrefs.SetInt("CopperCoins", (_copperCoins = value)); }
	}

	public static int Experience
	{
		get { return _experience; }
		set { PlayerPrefs.SetInt("Experience", (_experience = value)); }
	}

	public static int Gems
	{
		get { return _gems; }
		set { PlayerPrefs.SetInt("Gems", (_gems = value)); }
	}

	public static string Item
	{
		get { return _item; }
		set { PlayerPrefs.SetString("Item", (_item = value)); }
	}
}
