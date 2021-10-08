using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NPCCrafter : MonoBehaviour
{
	public static NPCCrafter instance;

    [SerializeField] GameObject craftingCanvas;
	[SerializeField] GameObject craftingDialog;
    [SerializeField] GameObject craftContextClue;
    [SerializeField] bool craftingDialogIsOpen;
	[SerializeField] bool craftingDisplay;
    [SerializeField] bool inRange;
    [SerializeField] SpriteRenderer craftingContextClueSprite;

	private void OnValidate()
	{
        craftingContextClueSprite.enabled = false;
	}

	private void Start()
	{
		instance = this;
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Q) && inRange)
		{
			if (craftingDialogIsOpen)
			{
				print("Crafter is already open");
			}
			else if (!craftingDialogIsOpen)
			{
				craftingDialog.SetActive(true);
				craftingDialogIsOpen = true;
			}
		}
	}

	public void TalkToNPCCrafter()
	{
		if (inRange)
		{
			if (craftingDialogIsOpen)
			{
				print("Crafter is already open");
			}
			else if (!craftingDialogIsOpen)
			{
				craftingDialog.SetActive(true);
				craftingDialogIsOpen = true;
			}
		}
	}
	private GameManager _gameManager;
	private PlayerMovement _playerMovement;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_gameManager = other.gameObject.GetComponent<PlayerMovement>()._gameManager;
			_playerMovement = other.gameObject.GetComponent<PlayerMovement>();
			CheckCraftContextClue(true);
			inRange = true;
			if (_gameManager.isHandheld)
			{
				_playerMovement.canTalkNPCCrafter = true;
				_playerMovement.actionText.text = "Talk";
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			CheckCraftContextClue(false);
			inRange = false;
			craftingDialogIsOpen = false;
			craftingDisplay = false;
			craftingDialog.SetActive(false);
			craftingCanvas.SetActive(false);
			if (_gameManager.isHandheld)
			{
				_playerMovement.canTalkNPCCrafter = false;
				_playerMovement.actionText.text = null;
			}
		}
	}

	private void CheckCraftContextClue(bool state)
	{
		craftingContextClueSprite.enabled = state;
	}

	public void CloseCraftWindow()
	{
		craftingCanvas.SetActive(false);
		craftingDialog.SetActive(false);
		craftingDialogIsOpen = false;
		craftingDisplay = false;
	}

	public void OpenCrafting()
	{
		if (craftingDisplay)
		{
			craftingCanvas.SetActive(false);
			craftingDialogIsOpen = false;
			craftingDisplay = false;
		}
		else
		{
			craftingDialog.SetActive(false);
			craftingCanvas.SetActive(true);
			craftingDisplay = true;
		}
	}
}
