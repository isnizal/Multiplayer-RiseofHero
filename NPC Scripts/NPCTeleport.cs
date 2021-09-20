using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Experimental;
using Mirror;

public class NPCTeleport : MonoBehaviour
{
	public static NPCTeleport instance;

	public GameObject npcDialog;
	public GameObject player;
	public GameObject toLocation;
	public Animator transition;
	public float teleportDelayTime;
	public float fadeDelay;
	public bool inRange;
	public GameObject firstTimeDialogBox;
	public Item startItemSO;

	private Inventory inventory;
	private Character _character;

	private void Start()
	{
		instance = this;
		transition = GameObject.Find("CrossFade").GetComponent<Animator>();
		toLocation = GameObject.Find("Efos StartPoint");
		if (inventory == null)
			inventory = FindObjectOfType<Inventory>();
	}

    private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q) && inRange)
		{
			if (!_character.onInput)
			{
				npcDialog.SetActive(true);
				player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeAll;
			}
		}
	}

	public void OpenTeleportChat()
	{
		npcDialog.SetActive(true);
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player") && !other.isTrigger)
		{
			if (!other.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_character = other.gameObject.GetComponent<Character>();
			player = other.gameObject;
			inRange = true;
			if (GameManager.GameManagerInstance.isHandheld)
			{
				PlayerMovement.PlayerMovementInstance.canTalkNPCTeleport = true;
				PlayerMovement.PlayerMovementInstance.actionText.text = "Talk";
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			if (!other.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			inRange = false;
			if (GameManager.GameManagerInstance.isHandheld)
			{
				PlayerMovement.PlayerMovementInstance.canTalkNPCTeleport = false;
				PlayerMovement.PlayerMovementInstance.actionText.text = null;
			}
		}
	}

	IEnumerator EnterArea()
	{
		GameManager gameManager = FindObjectOfType<GameManager>();
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeAll;
		transition.SetBool("Exit", true);
		yield return new WaitForSeconds(teleportDelayTime);
		player.GetComponent<Mirror.NetworkTransform>().transform.position = new Vector2(toLocation.transform.position.x, toLocation.transform.position.y);
		yield return new WaitForSeconds(fadeDelay);
		transition.SetBool("Exit", false);
		if (gameManager.firstTimePlaying == 0)
		{
			if (inventory is null)
				inventory = FindObjectOfType<Inventory>();
			firstTimeDialogBox.SetActive(true);
			inventory.AddItem(startItemSO.GetCopy());
			gameManager.firstTimePlaying = 1;
		}
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.None;
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeRotation;

	}

	public void CloseWindow()
	{
		npcDialog.SetActive(false);
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.None;
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void LeaveArea()
	{
		npcDialog.SetActive(false);
		inRange = false;
		StartCoroutine(EnterArea());
	}
}
