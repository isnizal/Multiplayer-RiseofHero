using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Heal HP")]
public class HealItemEffect : UsableItemEffect
{
	public int HealAmount;

	public override void ExecuteEffect(UsableItem usableItem, Character character)
	{
		PlayerCombat.CombatInstance.DisableSelfRegenHp();
		if (character.newHealth < character.MaxHealth)
		{
			character.newHealth += HealAmount;
			character.Health += HealAmount;

			UIManager.Instance.UpdateHealth();
			if (character.newHealth > character.MaxHealth || character.Health > character.MaxHealth)
			{
				character.newHealth = character.MaxHealth;
				character.Health = character.MaxHealth;
				UIManager.Instance.UpdateHealth();
			}
		}
		else if (character.newHealth == character.MaxHealth || character.Health == character.MaxHealth)
		{
			character.Health = character.MaxHealth;
			character.newHealth = character.MaxHealth;
			UIManager.Instance.UpdateHealth();
		}
	}
	
	public override string GetDescription()
	{
		return "Heals for " + HealAmount + " health.";
	}
}