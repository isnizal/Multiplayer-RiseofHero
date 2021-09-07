using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop System", menuName = "Shop Items/New Use Item", order = 1)]
public class ShopUseItemSO : ScriptableObject
{
	public string itemName;
	[TextArea]
	public string description;
	public int cost;
	[Space]
	public Item itemSO;
	public Sprite itemIcon;
}
