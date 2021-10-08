using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;
using Mirror;

public class Character : NetworkBehaviour
{
	private static Character instance;
	public static Character MyInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Character>();
			}
			return instance;
		}

	}

	public void Save()
	{
		if (achievement ==  null)
			return;

		SaveLoadManager.SavePlayerData(this, GetComponent<LevelSystem>());
		SaveLoadManager.SavePlayerAchievement(achievement);
		itemSaveManager.SaveEquipment(this);
		itemSaveManager.SaveInventory(this);
		itemSaveManager.SaveItemStash(this);
	}

	//also call from button
	public void Load()
	{
		LoadPlayerData();

	}
	private void LoadPlayerData()
	{
		int[] loadedStats = SaveLoadManager.LoadPlayer();
		int[] loadedStatsAchievement = SaveLoadManager.LoadAchievement();
		Health = loadedStats[0];
		MaxHealth = loadedStats[1];
		Mana = loadedStats[2];
		MaxMP = loadedStats[3];
		copperCurrency = loadedStats[4];
		LevelSystem.LevelInstance.currentLevel = loadedStats[5];
		LevelSystem.LevelInstance.currentExp = loadedStats[6];
		statPoints = loadedStats[7];
		statPointsAllocated = loadedStats[8];
		StatsModifier.StatsInstance.currentStrValue = loadedStats[9];
		StatsModifier.StatsInstance.currentDefValue = loadedStats[10];
		StatsModifier.StatsInstance.currentIntValue = loadedStats[11];
		StatsModifier.StatsInstance.currentVitValue = loadedStats[12];
		premiumCurrency = loadedStats[13];
		SpellTree.SpellInstance.spellPointsAvailable = loadedStats[14];
		SpellTree.SpellInstance.spellPointsAllocated = loadedStats[15];
		SpellTree.SpellInstance.fireball1Level = loadedStats[16];
		SpellTree.SpellInstance.icicle1Level = loadedStats[17];
		SpellTree.SpellInstance.arcticBlast1Level = loadedStats[18];

		UIManager.Instance.UpdateHealth();
		UIManager.Instance.UpdateMP();
		itemSaveManager.LoadEquipment(this);
		itemSaveManager.LoadInventory(this);
		itemSaveManager.LoadItemStash(this);
		transform.position = new Vector2(loadedStats[19], loadedStats[20]);
		playerMovement.helmetValue = loadedStats[21];
		playerMovement.torsoValue = loadedStats[22];
		playerMovement.armValue = loadedStats[23];
		playerMovement.bootValue = loadedStats[24];
		playerMovement.swordValue = loadedStats[25];
		playerMovement.shieldValue = loadedStats[26];
		playerMovement.hairValue = loadedStats[27];
		GameManager.GameManagerInstance.devilQueenDefeated = loadedStats[28];
		GameManager.GameManagerInstance.firstTimePlaying = loadedStats[29];

		Health = MaxHealth;
		Mana = MaxMP;
		PlayerCombat.CombatInstance.EnableSelfRegenHp();
		PlayerCombat.CombatInstance.EnableSelfRegenMana();

		playerMovement.CheckValueClothes();
		//load game initialize
		playerMovement.Initialize();
		LoadPlayerAchievement(loadedStatsAchievement);
	}
	private void LoadPlayerAchievement(int[] loadedStats)
	{
		achievement.killSlimeCountAch = loadedStats[0];
		achievement.killSlime1Active = loadedStats[1];
		achievement.killSlime1Claimed = loadedStats[2];
		achievement.killSlime1Done = loadedStats[3];

		achievement.collectCopperCountAch = loadedStats[4];
		achievement.collectCopper1Active = loadedStats[5];
		achievement.collectCopper1Claimed = loadedStats[6];
		achievement.collectCopper1Done = loadedStats[7];
		achievement.kill50AchActive = loadedStats[8];
		achievement.kill50CountAch = loadedStats[9];
		achievement.kill50AchClaimed = loadedStats[10];
		achievement.kill50AchDone = loadedStats[11];
		achievement.killDevilQueenActive = loadedStats[12];
		achievement.killDevilQueenClaimed = loadedStats[13];
		achievement.killDevilQueenCountAch = loadedStats[14];
		achievement.killDevilQueenDone = loadedStats[15];
		achievement.collectCopper2Active = loadedStats[16];
		achievement.collectCopper2Claimed = loadedStats[17];
		achievement.collectCopper2Done = loadedStats[18];
		achievement.kill500AchActive = loadedStats[19];
		achievement.kill500AchClaimed = loadedStats[20];
		achievement.kill500AchDone = loadedStats[21];
	}


	[Header("------> Variable Stats <------")]
	[SyncVar] public int Health;
	[SyncVar] public int Mana;
	public int MaxHealth;
	public float hpRegenTime;
	public int MaxMP;
	public float mpRegenTime;
	public float playerPosX;
	public float playerPosY;


	[Space]

	[Header("------> Currency <------")]
	public int premiumCurrency;
	public int copperCurrency;
	[Space]

	[Header("------> Points Manager <------")]
	public int statPoints;
	public int newStatPoints;
	public int statPointsAllocated;
	[Space]

	[Header("------> Attributes <------")]
	public CharacterStat Strength;
	public CharacterStat Defense;
	public CharacterStat Intelligence;
	public CharacterStat Vitality;
	[Space]

	[Header("------> Public <------")]
	public Inventory Inventory;
	public EquipmentPanel EquipmentPanel;
	[FormerlySerializedAs("ItemStashPanel")]
	public ItemStash itemStash;
	[Space]

	[Header("------> Serialize Fields <------")]
	[SerializeField] CraftingWindow craftingWindow;
	[SerializeField] StatPanel statPanel;
	[SerializeField] ItemTooltip itemTooltip;
	[SerializeField] Image draggableItem;
	[SerializeField] DropItemArea dropItemArea;
	[SerializeField] DropSellArea sellItemArea;
	[SerializeField] QuestionDialog dropItemDialog;
	[SerializeField] QuestionDialog sellItemDialog;
	[SerializeField] ItemSaveManager itemSaveManager;

	private BaseItemSlot dragItemSlot;
	private AchievementManager achievement;
	private GameObject _shopWindow;

	public PlayerMovement playerMovement;

	[Tooltip("UI Manager contain game manager initialize")]
	[HideInInspector] public UIManager uiManager;
	private void OnValidate()
	{
		if (itemTooltip == null)
			itemTooltip = FindObjectOfType<ItemTooltip>();
	}

	public void InitializeCharacter()
	{
		if (hasAuthority)
		{
			ExecuteHealth(MaxHealth);
			ExecuteMana(MaxMP);

			sellItemArea = FindObjectOfType<DropSellArea>();
			dropItemArea = FindObjectOfType<DropItemArea>();
			uiManager = FindObjectOfType<UIManager>();
			uiManager.InitializeAwake(this);
			achievement = FindObjectOfType<AchievementManager>();
			Inventory = FindObjectOfType<Inventory>();
			EquipmentPanel = FindObjectOfType<EquipmentPanel>();
			statPanel = FindObjectOfType<StatPanel>();
			itemTooltip = FindObjectOfType<ItemTooltip>();
			itemTooltip.gameObject.SetActive(false);
			draggableItem = GameObject.Find("Draggable Item").GetComponent<Image>();
			dropItemDialog = GameObject.Find("DropItemDialog").GetComponent<QuestionDialog>();
			dropItemDialog.gameObject.SetActive(false);
			playerMovement = gameObject.GetComponent<PlayerMovement>();
			itemSaveManager = FindObjectOfType<ItemSaveManager>();
			craftingWindow = FindObjectOfType<CraftingWindow>();
			craftingWindow.GetComponent<CraftingWindow>().InitializeCraftingWindow();
			craftingWindow.gameObject.SetActive(false);
			//_shopWindow = sellItemArea.gameObject.transform.parent.gameObject;
			//sellItemArea.gameObject.transform.parent.gameObject.SetActive(false);
			sellItemDialog = GameObject.Find("SellItemDialog").GetComponent<QuestionDialog>();
			sellItemDialog.gameObject.SetActive(false);
			SetupEvents();
		}
	}
	public bool onInput = false;
	private void SetupEvents()
	{
		statPanel.SetStats(Strength, Defense, Intelligence, Vitality);
		statPanel.UpdateStatValues();

		// Setup Events:
		// Right Click
		Inventory.OnRightClickEvent += InventoryRightClick;
		EquipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
		// Pointer Enter
		Inventory.OnPointerEnterEvent += ShowTooltip;
		EquipmentPanel.OnPointerEnterEvent += ShowTooltip;
		craftingWindow.OnPointerEnterEvent += ShowTooltip;
		// Pointer Exit
		Inventory.OnPointerExitEvent += HideTooltip;
		EquipmentPanel.OnPointerExitEvent += HideTooltip;
		craftingWindow.OnPointerExitEvent += HideTooltip;
		// Begin Drag
		Inventory.OnBeginDragEvent += BeginDrag;
		EquipmentPanel.OnBeginDragEvent += BeginDrag;
		// End Drag
		Inventory.OnEndDragEvent += EndDrag;
		EquipmentPanel.OnEndDragEvent += EndDrag;
		// Drag
		Inventory.OnDragEvent += Drag;
		EquipmentPanel.OnDragEvent += Drag;
		// Drop
		Inventory.OnDropEvent += Drop;
		EquipmentPanel.OnDropEvent += Drop;
		dropItemArea.OnDropEvent += DropItemOutsideUI;

		sellItemArea.OnDropEvent += DropItemSellArea;
	}

	private void Update()
	{
		if (isLocalPlayer)
		{
			if (!onInput)
				baseMaxHealth = 50 + Vitality.BaseValue;
			VitalityMonitor();
			playerPosX = transform.position.x;
			playerPosY = transform.position.y;
		}
	}
	public void DisableAllRegen()
	{
		ResetSelfRegenHp = null;
		ResetSelfRegenMana = null;
	}
	[HideInInspector] public bool notInCombat = true;
	private bool currentlyHealing = false;
	private bool currentlyMana = false;
	private void IdleRegen()
	{
		if (notInCombat)
		{
			notInCombat = false;
			if (!currentlyHealing)
			{
				isSelfHPRegen = true;
				onRestoreHealth = false;
				enemyHit = false;
				HpRegen();
			}
			if (!currentlyMana)
			{
				isSelfManaRegen = true;
				onRestoreMana = false;
				enemyHit = false;
				MpRegen();
			}
		}
	}
	public void ExecuteNewHealth(int newHealth)
	{
		if (isClient)
		{
			CmdExecuteNewHealth(newHealth);

		}
	}
	[Command(requiresAuthority = true)]
	public void CmdExecuteNewHealth(int newHealth)
	{
		this.newHealth = this.Health;
		this.newHealth += newHealth;
		this.Health = this.newHealth;
		if (this.newHealth > MaxHealth)
			this.newHealth = MaxHealth;
	}
	public void ExecuteHealth(int health)
	{
		if (isClient)
		{
			CmdExecuteHealth(health);

		}
	}
	[Command(requiresAuthority = true)]
	public void CmdExecuteHealth(int health)
	{
		
		this.Health += health;
		this.newHealth = this.Health;

		if (this.Health > MaxHealth)
			this.Health = MaxHealth;


	}
	[SyncVar] public int newHealth;
	[HideInInspector] public bool isSelfHPRegen = false;
	[HideInInspector] public bool enemyHit = false;
	[HideInInspector] public bool onRestoreHealth = false;
	[HideInInspector] public IEnumerator RestoreHealth;
	[HideInInspector] public IEnumerator ResetSelfRegenHp;

	public IEnumerator SetSelfRegenHp()
	{
		yield return new WaitForSeconds(hpRegenTime);
		notInCombat = true;
		IdleRegen();
	}
	private IEnumerator RestorePotion(float Time)
	{
		newHealth = Health;
		onRestoreHealth = true;
		while (newHealth < MaxHealth)
		{
			currentlyHealing = true;
			ExecuteNewHealth(1);
			//break if enemy hit, another restore health and self regen happening
			//more than the max health
			if (newHealth > MaxHealth || enemyHit | RestoreHealth == null || ResetSelfRegenHp == null)
				break;


			yield return new WaitForSeconds(Time);
		}
		currentlyHealing = false;
	}
	public void HpRegen()
	{
		if (isSelfHPRegen)
		{
			if (!enemyHit)
			{
				isSelfHPRegen = false;
				if (!onRestoreHealth)
				{
					RestoreHealth = RestorePotion(hpRegenTime);
					StartCoroutine(RestoreHealth);
				}
			}
		}

	}
	public void ExecuteMana(int mana)
	{
		CmdExecuteMana(mana);
	}
	[Command(requiresAuthority = true)]
	public void CmdExecuteMana(int mana)
	{
		this.Mana += mana;
		this.newMana = this.Mana;
		if (this.Mana > MaxMP)
			this.Mana = MaxMP;

	}
	public void ExecuteNewMana(int mana)
	{
		CmdExecuteNewMana(mana);
	}
	[Command(requiresAuthority = true)]
	public void CmdExecuteNewMana(int mana)
	{
		this.newMana = Mana;
		this.newMana += mana;
		this.Mana = this.newMana;
		if (this.Mana > MaxMP)
			this.Mana = MaxMP;
	}
	[HideInInspector] public bool isSelfManaRegen = false;
	[HideInInspector] public bool onRestoreMana = false;
	[SyncVar][HideInInspector] public int newMana;
	[HideInInspector]public IEnumerator RestoreMana;
	[HideInInspector] public IEnumerator ResetSelfRegenMana;
	public void MpRegen()
	{
		if (isSelfManaRegen)
		{
			if (!enemyHit)
			{
				isSelfManaRegen = false;
				if (!onRestoreMana)
				{
					RestoreMana = RestoreMP(mpRegenTime);
					StartCoroutine(RestoreMana);
				}
			}
		}

	}
	public IEnumerator SetSelfRegenMana()
	{
		yield return new WaitForSeconds(mpRegenTime);
		notInCombat = true;
		IdleRegen();
	}
	private IEnumerator RestoreMP(float Time)
	{
		newMana = Mana;
		onRestoreMana = true;
		while (newMana < MaxMP)
		{
			currentlyMana = true;
			ExecuteNewMana(1);

			//more than the max health
			if (newMana > MaxMP || enemyHit || RestoreMana == null || ResetSelfRegenMana == null)
				break;
			yield return new WaitForSeconds(Time);
		}
		currentlyMana = false;
	}


	//private void OnDestroy()
	//{
	//	itemSaveManager.SaveEquipment(this);
	//	itemSaveManager.SaveInventory(this);
	//	itemSaveManager.SaveItemStash(this);
	//}

	private void InventoryRightClick(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item is EquippableItem)
		{
			Equip((EquippableItem)itemSlot.Item);
			//Debug.Log("Added Item");//added item / on click
		}
		else if (itemSlot.Item is UsableItem)
		{
			UsableItem usableItem = (UsableItem)itemSlot.Item;
			usableItem.Use(this);

			if (usableItem.IsConsumable)
			{
				itemSlot.Amount--;
				usableItem.Destroy();
				//Debug.Log("Item used");
			}
		}
	}

	private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item is EquippableItem)
		{
			Unequip((EquippableItem)itemSlot.Item);
			//Debug.Log("Remove on Click");
		}
	}

	private void ShowTooltip(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item != null)
		{
			itemTooltip.ShowTooltip(itemSlot.Item);
		}
	}

	private void HideTooltip(BaseItemSlot itemSlot)
	{
		if (itemTooltip.gameObject.activeSelf)
		{
			itemTooltip.HideTooltip();
		}
	}

	private void BeginDrag(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item != null)
		{
			dragItemSlot = itemSlot;
			draggableItem.sprite = itemSlot.Item.Icon;
			draggableItem.transform.position = Input.mousePosition;
			draggableItem.gameObject.SetActive(true);
			//Debug.Log("Dragging");
		}
	}

	private void Drag(BaseItemSlot itemSlot)
	{
		draggableItem.transform.position = Input.mousePosition;
	}

	private void EndDrag(BaseItemSlot itemSlot)
	{
		dragItemSlot = null;
		draggableItem.gameObject.SetActive(false);
		
		//Debug.Log("End Drag");
	}

	private void Drop(BaseItemSlot dropItemSlot)
	{
		if (dragItemSlot == null) return;

		if (dropItemSlot.CanAddStack(dragItemSlot.Item))
		{
			AddStacks(dropItemSlot);
		}
		else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
		{
			SwapItems(dropItemSlot);
			//Debug.Log("Drop / Swap Item");
		}
	}

	private void AddStacks(BaseItemSlot dropItemSlot)
	{
		int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
		int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

		dropItemSlot.Amount += stacksToAdd;
		dragItemSlot.Amount -= stacksToAdd;
	}

	private void SwapItems(BaseItemSlot dropItemSlot)
	{
		EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
		EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

		if (dropItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null) dragEquipItem.Equip(this);
			if (dropEquipItem != null) dropEquipItem.Unequip(this);
			//Debug.Log("Drop Item Slot" + "Dragged" + dragEquipItem + "Dropped" + dropEquipItem);//drop item to eq panel
		}
		if (dragItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null) dragEquipItem.Unequip(this);
			if (dropEquipItem != null) dropEquipItem.Equip(this);
			//Debug.Log("Drag Item Slot" + "Dragged" + dragEquipItem + "Dropped" + dropEquipItem);
		}
		statPanel.UpdateStatValues();

		Item draggedItem = dragItemSlot.Item;
		int draggedItemAmount = dragItemSlot.Amount;

		dragItemSlot.Item = dropItemSlot.Item;
		dragItemSlot.Amount = dropItemSlot.Amount;

		dropItemSlot.Item = draggedItem;
		dropItemSlot.Amount = draggedItemAmount;
	}

	private void DropItemOutsideUI()
	{
		if (dragItemSlot == null) return;

		dropItemDialog.Show();
		BaseItemSlot slot = dragItemSlot;
		if (slot.Amount == 1)
		{
			if (slot is EquipmentSlot)
			{
				EquippableItem equippableItem = (EquippableItem)slot.Item;
				equippableItem.Unequip(this);
			}
			currentValue = 1;
			Slider newSlider = dropItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>();
			if (newSlider.gameObject.activeInHierarchy)
				newSlider.gameObject.SetActive(false);

			dropItemDialog.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
			= "Do you want to drop this item?";
			dropItemDialog.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentValue.ToString();
			dropItemDialog.OnYesEvent += () => DestroyItemInSlot(slot,currentValue);
		}
		else if (slot.Amount >= 2)
		{
			currentSlot = slot;
			currentValue = 0;
			Slider newSlider = dropItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>();
			if (!newSlider.gameObject.activeInHierarchy)
				newSlider.gameObject.SetActive(true);

			newSlider.maxValue = slot.Amount;
			currentValue = (int)newSlider.maxValue;

			dropItemDialog.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentValue.ToString();
			newSlider.onValueChanged.AddListener(DropReduceAmountItem);
			newSlider.value = currentValue;

			dropItemDialog.OnYesEvent += () => DestroyItemInSlot(slot, 
				(int)dropItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>().value);
		}

	}

	private void DropItemSellArea()
	{
		if (dragItemSlot == null) return;

		BaseItemSlot slot = dragItemSlot;
		sellItemDialog.Show();
		if (slot.Amount == 1)
		{
			if (slot is EquipmentSlot)
			{
				EquippableItem equippableItem = (EquippableItem)slot.Item;
				equippableItem.Unequip(this);
			}
			Slider newSlider = sellItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>();
			if (newSlider.gameObject.activeInHierarchy)
				newSlider.gameObject.SetActive(false);

			currentSlot = slot;
			currentValue = 1;
			sellItemDialog.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
			= "Sell <color=red><b>" + slot.Item.ItemName + "</color></b> \n for <color=yellow><b>" + slot.Item.itemCost + "</color></b> Copper?";
			sellItemDialog.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentValue.ToString();
			sellItemDialog.OnYesEvent += () => SellItemInSlot(slot,currentValue);
		}
		else if (slot.Amount >= 2)
		{
			currentSlot = slot;
			currentValue = 0;
			Slider newSlider = sellItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>();
			if (!newSlider.gameObject.activeInHierarchy)
				newSlider.gameObject.SetActive(true);

			newSlider.maxValue = slot.Amount;
			currentValue = (int)newSlider.maxValue;
			sellItemDialog.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
			= "Sell <color=red><b>" + slot.Item.ItemName + "</color></b> \n for <color=yellow><b>" + slot.Item.itemCost * slot.Amount + "</color></b> Copper?";
			sellItemDialog.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentValue.ToString();

			newSlider.onValueChanged.AddListener(SellReduceAmountItem);
			newSlider.value = currentValue;
			sellItemDialog.OnYesEvent += () => SellItemInSlot(slot, 
				(int)sellItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>().value);

		}

	}
	private int currentValue = 0;
	private BaseItemSlot currentSlot;
	private void DropReduceAmountItem(float value)
	{
		currentValue = (int)value;
		dropItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>().value = currentValue;
		dropItemDialog.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
		= "Do you want to drop this item?";
		dropItemDialog.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentValue.ToString();
	}
	private void SellReduceAmountItem(float value)
	{
		currentValue = (int)value;
		int newAmount = currentValue * currentSlot.Item.itemCost;
		sellItemDialog.transform.GetChild(1).transform.GetChild(1).GetComponent<Slider>().value = currentValue;
		sellItemDialog.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
		= "Sell <color=red><b>" + currentSlot.Item.ItemName + "</color></b> \n for <color=yellow><b>" + newAmount + "</color></b> Copper?";
		sellItemDialog.transform.GetChild(1).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentValue.ToString();

	}
	private void SellItemInSlot(BaseItemSlot itemSlot, int value)
	{
		for (int i = 0; i < value; i++)
		{
			copperCurrency += itemSlot.Item.itemCost;
			itemSlot.Amount -= 1;
		}
		if (itemSlot.Amount == 0)
			itemSlot.Item = null;

		currentSlot = null;
	}
	private void DestroyItemInSlot(BaseItemSlot itemSlot, int value)
	{
		for (int i = 0; i < value; i++)
		{
			itemSlot.Amount -= 1;
		}
		if (itemSlot.Amount == 0)
			itemSlot.Item = null;

		currentSlot = null;
	}


	public void Equip(EquippableItem item)
	{
		if (Inventory.RemoveItem(item))
		{
			EquippableItem previousItem;
			if (EquipmentPanel.AddItem(item, out previousItem))
			{
				if (previousItem != null)
				{
					Inventory.AddItem(previousItem);
					previousItem.Unequip(this);
					statPanel.UpdateStatValues();
					//Debug.Log("Added"); // added on click

				}
				item.Equip(this);
				statPanel.UpdateStatValues();

					//Debug.Log("Step2");
			}
			else
			{
				Inventory.AddItem(item);
				//Debug.Log("removed + swap");
			}
		}
	}

	public void Unequip(EquippableItem item)
	{
		if (Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))
		{
			item.Unequip(this);
			statPanel.UpdateStatValues();
			Inventory.AddItem(item);
			//Debug.Log("Removed"); // removed


		}
	}

	private ItemContainer openItemContainer;

	private void TransferToItemContainer(BaseItemSlot itemSlot)
	{
		Item item = itemSlot.Item;
		if (item != null && openItemContainer.CanAddItem(item))
		{
			Inventory.RemoveItem(item);
			openItemContainer.AddItem(item);
			//Debug.Log("Transfered to Container" + item);
		}
	}

	private void TransferToInventory(BaseItemSlot itemSlot)
	{
		Item item = itemSlot.Item;
		if (item != null && Inventory.CanAddItem(item))
		{
			openItemContainer.RemoveItem(item);
			Inventory.AddItem(item);
			//Debug.Log("Transfered to Inventory" + item);
		}
	}

	public void OpenItemContainer(ItemContainer itemContainer)
	{
		openItemContainer = itemContainer;

		Inventory.OnRightClickEvent -= InventoryRightClick;
		Inventory.OnRightClickEvent += TransferToItemContainer;

		itemContainer.OnRightClickEvent += TransferToInventory;

		itemContainer.OnPointerEnterEvent += ShowTooltip;
		itemContainer.OnPointerExitEvent += HideTooltip;
		itemContainer.OnBeginDragEvent += BeginDrag;
		itemContainer.OnEndDragEvent += EndDrag;
		itemContainer.OnDragEvent += Drag;
		itemContainer.OnDropEvent += Drop;
	}

	public void CloseItemContainer(ItemContainer itemContainer)
	{
		openItemContainer = null;

		Inventory.OnRightClickEvent += InventoryRightClick;
		Inventory.OnRightClickEvent -= TransferToItemContainer;

		itemContainer.OnRightClickEvent -= TransferToInventory;

		itemContainer.OnPointerEnterEvent -= ShowTooltip;
		itemContainer.OnPointerExitEvent -= HideTooltip;
		itemContainer.OnBeginDragEvent -= BeginDrag;
		itemContainer.OnEndDragEvent -= EndDrag;
		itemContainer.OnDragEvent -= Drag;
		itemContainer.OnDropEvent -= Drop;
	}

	public void UpdateStatValues()
	{
		statPanel.UpdateStatValues();

	}
	public void AddCurrency(int amount)
	{
		copperCurrency += amount;
		if (achievement.collectCopper1Active == 1)
		{
			achievement.collectCopperCountAch += amount;
		}
	}
	public float currentVitality;
	public float amountToAddMaxHealth;
	public float baseMaxHealth;
	public float maxHealthValue;
	public void VitalityMonitor()
	{
		currentVitality = Vitality.Value - Vitality.BaseValue; // Gets Vitality Value From Equipment
		amountToAddMaxHealth = currentVitality; // Puts Value in Holder
		maxHealthValue = amountToAddMaxHealth + baseMaxHealth; // Adds Combined Value to MaxHealth for Calculations
		MaxHealth = (int)maxHealthValue; // Applies Value to MaxHealth
		if (Health > MaxHealth)
			Health = MaxHealth;
	}
}
