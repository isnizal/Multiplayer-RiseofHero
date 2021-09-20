using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ShopEquipmentManager : MonoBehaviour
{
	private Inventory inventory;
    public ShopItemSO[] shopItemSO;
    public ShopTemplate[] shopPanels;
	public GameObject[] shopPanelsGO;
	public Button[] myPurchaseButtons;

	private Character _character;
	public void InitializeShopEquipmentManger(Character character)
	{
		_character = character;
		if (inventory == null)
			inventory = FindObjectOfType<Inventory>();

		for (int i = 0; i < shopItemSO.Length; i++)
		{
			shopPanelsGO[i].SetActive(true);
		}
		LoadPanels();
	}
	private void Update()
	{
		if (_character is null)
			return;

		CheckPurchaseable();
	}

	public void LoadPanels()
	{
		for (int i = 0; i < shopItemSO.Length; i++)
		{
			shopPanels[i].itemName.text = shopItemSO[i].itemName;
			shopPanels[i].itemIcon.sprite = shopItemSO[i].itemIcon;
			shopPanels[i].thisItemSO = shopItemSO[i].itemSO;
			shopPanels[i].strValue.text = "Strength: " + shopItemSO[i].strValue.ToString();
			shopPanels[i].defValue.text = "Defense: " + shopItemSO[i].defValue.ToString();
			shopPanels[i].intValue.text = "Intelligence: " + shopItemSO[i].intValue.ToString();
			shopPanels[i].vitValue.text = "Vitality: " + shopItemSO[i].vitValue.ToString();
			shopPanels[i].cost.text = "Cost: " + shopItemSO[i].itemSO.shopCost.ToString();

		}
	}

	public void CheckPurchaseable()
	{
		for (int i = 0; i < shopItemSO.Length; i++)
		{
			if (_character.copperCurrency >= shopItemSO[i].itemSO.shopCost)
			{
				myPurchaseButtons[i].interactable = true;
			}
			else
			{
				myPurchaseButtons[i].interactable = false;
			}
		}
	}

	public void PurchaseItem(int buttonNo)
	{
		if (_character.copperCurrency >= shopItemSO[buttonNo].itemSO.shopCost)
		{
			_character.copperCurrency = _character.copperCurrency - shopItemSO[buttonNo].itemSO.shopCost;
			inventory.AddItem(shopItemSO[buttonNo].itemSO.GetCopy());
			CheckPurchaseable();
		}
	}
}
