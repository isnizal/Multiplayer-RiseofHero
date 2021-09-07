using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal MP")]
public class HealMpItemEffect : UsableItemEffect
{
	public int HealMpAmount;

	public override void ExecuteEffect(UsableItem usableItem, Character character)
	{
		PlayerCombat.CombatInstance.DisableSelfRegenMana();
		if (character.Mana < character.MaxMP)
		{

			character.Mana += HealMpAmount;
			if (character.Mana > character.MaxMP)
			{
				character.Mana = character.MaxMP;
			}
		}
		else if (character.Mana == character.MaxMP)
		{
			character.Mana = character.MaxMP;
		}
	}

	public override string GetDescription()
	{
		return "Heals MP for " + HealMpAmount + " MP.";
	}
}
