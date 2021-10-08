using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NPCGeneralShop : MonoBehaviour
{
	public static NPCGeneralShop instance;

    [SerializeField] GameObject npcShopCanvas;
	[SerializeField] GameObject npcShopWelcome;
    [SerializeField] bool shopIsOpen;
	[SerializeField] bool inRange;
	[SerializeField] SpriteRenderer npcContextClue;
	[SerializeField] GameObject characterPanel;
	[SerializeField] GameObject dropItemArea;

	private void OnValidate()
	{
		npcContextClue.enabled = false;
	}

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q) && inRange)
		{
			if (!shopIsOpen)
			{
				npcShopWelcome.SetActive(true);
				dropItemArea.SetActive(false);
				GetComponentInChildren<Canvas>().sortingOrder = 510;
			}
		}
	}

	public void TalkToNPCShop()
	{
		if(inRange)
		{
			if (!shopIsOpen)
			{
				npcShopWelcome.SetActive(true);
				dropItemArea.SetActive(false);
			}
		}
	}
	private GameManager _gameManager;
	private PlayerMovement _playerMovement;
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_gameManager = FindObjectOfType<GameManager>();
			_playerMovement = other.gameObject.GetComponent<PlayerMovement>();
			dropItemArea.SetActive(false);
			CheckContextClue(true);
			inRange = true;
			if (_gameManager.isHandheld)
			{
				_playerMovement.canTalkNPCShop = true;
				_playerMovement.actionText.text = "Talk";
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			CheckContextClue(false);
			inRange = false;
			npcShopWelcome.SetActive(false);
			npcShopCanvas.SetActive(false);
			shopIsOpen = false;
			characterPanel.GetComponent<CanvasGroup>().alpha = 0;
			characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
			dropItemArea.SetActive(true);
			if (_gameManager.isHandheld)
			{
				_playerMovement.canTalkNPCShop = false;
				_playerMovement.actionText.text = null;
			}
		}
	}

	private void CheckContextClue(bool state)
	{
		npcContextClue.enabled = state;
	}

	public void OpenShop()
	{
		if(shopIsOpen)
		{
			npcShopCanvas.SetActive(false);
			shopIsOpen = false;
		}
		else
		{
			npcShopWelcome.SetActive(false);
			npcShopCanvas.SetActive(true);
			characterPanel.GetComponent<CanvasGroup>().alpha = 1;
			characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			shopIsOpen = true;
			GetComponentInChildren<Canvas>().sortingOrder = 490;
		}

	}

	//calling from outside btn UI
	public void CloseShopWindow()
	{
		npcShopWelcome.SetActive(false);
		npcShopCanvas.SetActive(false);
		characterPanel.GetComponent<CanvasGroup>().alpha = 0;
		characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		shopIsOpen = false;
	}

}
