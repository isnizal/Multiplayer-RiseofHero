using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Stat Buff")]
public class StatBuffItemEffect : UsableItemEffect
{
	public int DefenseBuff;
	public float Duration;

	public override void ExecuteEffect(UsableItem parentItem, Character character)
	{
		StatModifier statModifier = new StatModifier(DefenseBuff, StatModType.Flat, parentItem);
		character.Defense.AddModifier(statModifier);
		character.UpdateStatValues();
		character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
	}

	public override string GetDescription()
	{
		return "Grants " + DefenseBuff + " Defense for " + Duration + " seconds.";
	}

	private static IEnumerator RemoveBuff(Character character, StatModifier statModifier, float duration)
	{
		yield return new WaitForSeconds(duration);
		character.Defense.RemoveModifier(statModifier);
		character.UpdateStatValues();
	}
}