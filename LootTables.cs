using UnityEngine;

[System.Serializable]
public class Loot
{
	public GameObject thisLoot;
	public int lootChance;
}

[CreateAssetMenu]
public class LootTables : ScriptableObject
{
	public Loot[] lootItems;

	public GameObject LootItems()
	{
		int probability = 0;
		int currentProb = Random.Range(0, 100);
		for (int i = 0; i < lootItems.Length; i++)
		{
			probability += lootItems[i].lootChance;
			if(currentProb <= probability)
			{
				return lootItems[i].thisLoot;
			}
		}
		return null;
	}
}
