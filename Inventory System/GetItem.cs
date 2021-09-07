using UnityEngine;
using UnityEditor;

public class GetItem : MonoBehaviour, ISerializationCallbackReceiver
{
	public static GetItem GetItemInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GetItem>();
			}
			return instance;
		}
	}
	public Inventory inventory;
	public Sprite uiDisplay;
	public Item itemToPickUp;
	public bool inRange;
	[Space]
	public int itemCost;

	public static GetItem instance;

	private void Start()
	{
		instance = this;
	}

	private void OnValidate()
	{
		if (inventory == null)
			inventory = FindObjectOfType<Inventory>();
	}
	private void Update()
	{
		if (inventory == null)
			inventory = FindObjectOfType<Inventory>();

	}

	public void PickupItem()
	{
		if (itemToPickUp != null && inRange == true)
		{
			SoundManager.PlaySound(SoundManager.Sound.PickupItem);
			inventory.AddItem(itemToPickUp.GetCopy());
			PlayerCombat.CombatInstance.DisplayInformation(itemToPickUp.GetCopy());
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PickupItem();
		}
		
		inRange = true;
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			inRange = false;
		}
	}

	public void OnBeforeSerialize()
	{
#if UNITY_EDITOR
		uiDisplay = itemToPickUp.Icon;
		GetComponent<SpriteRenderer>().sprite = uiDisplay;
		EditorUtility.SetDirty(GetComponent<SpriteRenderer>());
#endif
	}

	public void OnAfterDeserialize()
	{

	}
}
