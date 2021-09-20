using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopItemManager : MonoBehaviour
{

	public ShopUseItemSO[] shopUseItemSO;
	public ShopUseItemTemplate[] shopUseItemPanels;
	public GameObject[] shopPanelsGO;
	public Button[] myPurchaseButtons;

	private Character _character;
	private Inventory _inventory;
	public void InitializeShopItemManager(Character character)
	{
		_character = character;
		if (_inventory == null)
			_inventory = FindObjectOfType<Inventory>();

		for (int i = 0; i < shopUseItemSO.Length; i++)
		{
			shopPanelsGO[i].SetActive(true);
		}
		LoadPanels();
		CheckPurchaseable();
	}
	private void Update()
	{
		if (_character is null)
			return;

		CheckPurchaseable();
	}
	private void Start()
	{

	}

	public void LoadPanels()
	{
		for (int i = 0; i < shopUseItemSO.Length; i++)
		{
			shopUseItemPanels[i].itemName.text = shopUseItemSO[i].itemName;
			shopUseItemPanels[i].itemIcon.sprite = shopUseItemSO[i].itemIcon;
			shopUseItemPanels[i].thisItemSO = shopUseItemSO[i].itemSO;
			shopUseItemPanels[i].description.text = shopUseItemSO[i].description;
			shopUseItemPanels[i].cost.text = "Cost: " + shopUseItemSO[i].cost.ToString();

		}
	}

	public void CheckPurchaseable()
	{
		for (int i = 0; i < shopUseItemSO.Length; i++)
		{
			if (_character.copperCurrency >= shopUseItemSO[i].cost)
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
		if (_character.copperCurrency >= shopUseItemSO[buttonNo].cost)
		{
			_character.copperCurrency = _character.copperCurrency - shopUseItemSO[buttonNo].cost;
			_inventory.AddItem(shopUseItemSO[buttonNo].itemSO.GetCopy());
			CheckPurchaseable();
		}
	}
}
