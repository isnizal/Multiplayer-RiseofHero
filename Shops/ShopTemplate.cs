using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopTemplate : MonoBehaviour
{
	public TMP_Text itemName;
	public TMP_Text strValue;
	public TMP_Text defValue;
	public TMP_Text intValue;
	public TMP_Text vitValue;
	public TMP_Text cost;
	[Space]
	public Image itemIcon;
	public Item thisItemSO;
}
