using UnityEngine;

public enum EquipmentType
{
	Helmet,
	Chest,
	Boots,
	Weapon,
	Shield,
}

[CreateAssetMenu(menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{

	public int StrengthBonus;
	public int DefenseBonus;
	public int IntelligenceBonus;
	public int VitalityBonus;
	[Space]
	public float StrengthPercentBonus;
	public float DefensePercentBonus;
	public float IntelligencePercentBonus;
	public float VitalityPercentBonus;
	[Space]
	public EquipmentType EquipmentType;



	public override Item GetCopy()
	{
		return Instantiate(this);
	}

	public override void Destroy()
	{
		Destroy(this);
	}

	public void Equip(Character c)
	{
		if (StrengthBonus != 0)
			c.Strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this));
		if (DefenseBonus != 0)
			c.Defense.AddModifier(new StatModifier(DefenseBonus, StatModType.Flat, this));
		if (IntelligenceBonus != 0)
			c.Intelligence.AddModifier(new StatModifier(IntelligenceBonus, StatModType.Flat, this));
		if (VitalityBonus != 0)
			c.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModType.Flat, this));

		if (StrengthPercentBonus != 0)
			c.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
		if (DefensePercentBonus != 0)
			c.Defense.AddModifier(new StatModifier(DefensePercentBonus, StatModType.PercentMult, this));
		if (IntelligencePercentBonus != 0)
			c.Intelligence.AddModifier(new StatModifier(IntelligencePercentBonus, StatModType.PercentMult, this));
		if (VitalityPercentBonus != 0)
			c.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModType.PercentMult, this));

		PlayerCombat.CombatInstance.EnableSelfRegenHp();
		PlayerCombat.CombatInstance.EnableSelfRegenMana();
	}

	public void Unequip(Character c)
	{
		c.Strength.RemoveAllModifiersFromSource(this);
		c.Defense.RemoveAllModifiersFromSource(this);
		c.Intelligence.RemoveAllModifiersFromSource(this);
		c.Vitality.RemoveAllModifiersFromSource(this);

		PlayerCombat.CombatInstance.EnableSelfRegenHp();
		PlayerCombat.CombatInstance.EnableSelfRegenMana();
	}

	public override string GetItemType()
	{
		return EquipmentType.ToString();
	}

	public override string GetDescription()
	{
		sb.Length = 0;
		AddStat(StrengthBonus, "Strength");
		AddStat(DefenseBonus, "Defense");
		AddStat(IntelligenceBonus, "Intelligence");
		AddStat(VitalityBonus, "Vitality");

		AddStat(StrengthPercentBonus, "Strength", isPercent: true);
		AddStat(DefensePercentBonus, "Defense", isPercent: true);
		AddStat(IntelligencePercentBonus, "Intelligence", isPercent: true);
		AddStat(VitalityPercentBonus, "Vitality", isPercent: true);

		return sb.ToString();
	}

	private void AddStat(float value, string statName, bool isPercent = false)
	{
		if (value != 0)
		{
			if (sb.Length > 0)
				sb.AppendLine();

			if (value > 0)
				sb.Append("+");

			if (isPercent)
			{
				sb.Append(value * 100);
				sb.Append("% ");
			}
			else
			{
				sb.Append(value);
				sb.Append(" ");
			}
			sb.Append(statName);
		}
	}
}
