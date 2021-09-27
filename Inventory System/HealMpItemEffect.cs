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

			character.ExecuteMana(HealMpAmount);
			if (character.Mana > character.MaxMP)
			{
				character.ExecuteMana(character.MaxMP);
			}
		}
		else if (character.Mana == character.MaxMP)
		{
			character.ExecuteMana(character.MaxMP);
		}
	}

	public override string GetDescription()
	{
		return "Heals MP for " + HealMpAmount + " MP.";
	}
}
