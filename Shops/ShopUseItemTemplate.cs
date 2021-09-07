using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUseItemTemplate : MonoBehaviour
{
	public TMP_Text itemName;
	public TMP_Text description;

	public TMP_Text cost;
	[Space]
	public Image itemIcon;
	public Item thisItemSO;
}
