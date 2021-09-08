using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror.Experimental;

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

	private void OnValidate()
	{
	}

	private void Start()
	{
		if (inventory == null)
			inventory = FindObjectOfType<Inventory>();

		instance = this;
		transition = GameObject.Find("CrossFade").GetComponent<Animator>();
		toLocation = GameObject.Find("Efos StartPoint");
	}

    private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q) && inRange)
		{
			npcDialog.SetActive(true);
			player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeAll;
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
		player.GetComponent<NetworkRigidbody2D>().target.constraints = RigidbodyConstraints2D.FreezeAll;
		transition.SetBool("Exit", true);
		yield return new WaitForSeconds(teleportDelayTime);
		player.transform.position = new Vector2(toLocation.transform.position.x, toLocation.transform.position.y);
		yield return new WaitForSeconds(fadeDelay);
		transition.SetBool("Exit", false);
		if (GameManager.GameManagerInstance.firstTimePlaying == 0)
		{
			firstTimeDialogBox.SetActive(true);
			inventory.AddItem(startItemSO.GetCopy());
			GameManager.GameManagerInstance.firstTimePlaying = 1;
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
