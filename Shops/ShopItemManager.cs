using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopItemManager : MonoBehaviour
{
	private Inventory inventory;
	public ShopUseItemSO[] shopUseItemSO;
	public ShopUseItemTemplate[] shopUseItemPanels;
	public GameObject[] shopPanelsGO;
	public Button[] myPurchaseButtons;

	private void Update()
	{

		CheckPurchaseable();
	}
	private void Start()
	{
		if (inventory == null)
			inventory = FindObjectOfType<Inventory>();

		for (int i = 0; i < shopUseItemSO.Length; i++)
		{
			shopPanelsGO[i].SetActive(true);
		}
		LoadPanels();
		CheckPurchaseable();
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
			if (Character.MyInstance.copperCurrency >= shopUseItemSO[i].cost)
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
		if (Character.MyInstance.copperCurrency >= shopUseItemSO[buttonNo].cost)
		{
			Character.MyInstance.copperCurrency = Character.MyInstance.copperCurrency - shopUseItemSO[buttonNo].cost;
			inventory.AddItem(shopUseItemSO[buttonNo].itemSO.GetCopy());
			CheckPurchaseable();
		}
	}
}
