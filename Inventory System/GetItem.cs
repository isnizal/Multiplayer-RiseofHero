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
	private Inventory inventory;
	public Sprite uiDisplay;
	public Item itemToPickUp;
	public bool inRange;
	[Space]
	//public int itemCost;

	public static GetItem instance;
	
	private void Start()
	{
		instance = this;
	}
	//public void MobilePickUp(GameObject _character)
	//{
	//	inventory = _character.gameObject.GetComponent<Character>().Inventory;
	//}
    private void Update()
    {
		//if (Input.GetKey(KeyCode.P) && notPickUp || mobilePickUp && notPickUp)
		//{
		//	PickupItem();
		//}
    }

    [Client]
	public void PickupItem()
	{
		if (itemToPickUp != null)
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
	private PlayerMovement _playerMovement;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_playerCombat = other.gameObject.GetComponent<PlayerCombat>();
			inventory = _playerCombat.gameObject.GetComponent<Character>().Inventory;
			_playerMovement = _playerCombat.GetComponent<PlayerMovement>();
			if (_playerMovement._gameManager.isHandheld)
				_playerMovement.actionText.text = "Pick";
			_playerCombat.GetComponent<PlayerMovement>().SetItemToPickTrue(this.gameObject);

		}
		

	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			if(_playerCombat != null)
				_playerCombat.GetComponent<PlayerMovement>().SetItemToPickFalse();

			if(_playerMovement._gameManager.isHandheld)
				_playerMovement.actionText.text = " ";
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
