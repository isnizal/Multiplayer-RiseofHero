using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal HP")]
public class HealItemEffect : UsableItemEffect
{
	public int HealAmount;

	public override void ExecuteEffect(UsableItem usableItem, Character character)
	{
		PlayerCombat _playerCombat = character.gameObject.GetComponent<PlayerCombat>();
		_playerCombat.DisableSelfRegenHp();
		if (character.newHealth < character.MaxHealth)
		{
			character.ExecuteNewHealth(HealAmount);
			//character.ExecuteHealth(HealAmount);

			character.uiManager.UpdateHealth();
			if (character.newHealth > character.MaxHealth || character.Health > character.MaxHealth)
			{
				character.ExecuteNewHealth(character.MaxHealth);
				//character.ExecuteHealth(character.MaxHealth);
				character.uiManager.UpdateHealth();
			}
		}
		else if (character.newHealth == character.MaxHealth || character.Health == character.MaxHealth)
		{
			character.ExecuteNewHealth(character.MaxHealth);
			//character.ExecuteHealth(character.MaxHealth);
			character.uiManager.UpdateHealth();
		}
	}
	
	public override string GetDescription()
	{
		return "Heals for " + HealAmount + " health.";
	}
}