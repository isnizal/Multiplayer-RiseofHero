using UnityEngine;
using UnityEditor;
using Mirror;

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


	public void PickupItem()
	{
		if (itemToPickUp != null && inRange == true)
		{
			SoundManager.PlaySound(SoundManager.Sound.PickupItem);
			inventory.AddItem(itemToPickUp.GetCopy());
			_playerCombat.DisplayInformation(itemToPickUp.GetCopy());
			_playerCombat.CmdDestroyObjects(this.gameObject);
		}
	}
	public void RpcDestroyItem()
	{
		
	}
	private PlayerCombat _playerCombat;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_playerCombat = other.gameObject.GetComponent<PlayerCombat>();
			inRange = true;
			inventory = other.gameObject.GetComponent<Character>().Inventory;
			PickupItem();
		}
		

	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
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
