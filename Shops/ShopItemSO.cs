using UnityEngine;

[CreateAssetMenu(fileName = "Shop System", menuName = "Shop Items/New Item", order = 1)]
public class ShopItemSO : ScriptableObject
{
	public string itemName;
	public int strValue;
	public int defValue;
	public int intValue;
	public int vitValue;
	[Space]
	public Item itemSO;
	public Sprite itemIcon;
}
