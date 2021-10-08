using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal MP")]
public class HealMpItemEffect : UsableItemEffect
{
	public int HealMpAmount;

	public override void ExecuteEffect(UsableItem usableItem, Character character)
	{
		PlayerCombat _playerCombat = character.gameObject.GetComponent<PlayerCombat>();
		_playerCombat.DisableSelfRegenMana();
		if (character.Mana < character.MaxMP)
		{
			character.ExecuteNewMana(HealMpAmount);
			//character.ExecuteMana(HealMpAmount);
			character.uiManager.UpdateMP();
			if (character.Mana > character.MaxMP)
			{
				character.ExecuteNewMana(character.MaxMP);
				//character.ExecuteMana(character.MaxMP);
				character.uiManager.UpdateMP();
			}
		}
		else if (character.Mana == character.MaxMP)
		{
			character.ExecuteNewMana(character.MaxMP);
			//character.ExecuteMana(character.MaxMP);
			character.uiManager.UpdateMP();
		}
	}

	public override string GetDescription()
	{
		return "Heals MP for " + HealMpAmount + " MP.";
	}
}
