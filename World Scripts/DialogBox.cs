using UnityEngine;
using Mirror;
public class DialogBox : NetworkBehaviour
{
	public static DialogBox instance;
	public static DialogBox DialogInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<DialogBox>();
			}
			return instance;
		}

	}
	public enum Profession { Patrol, BlackSmith, Merchant, Sign, }
	public Profession profession;
    [TextArea(3,5)]
    public string dialogMessage;

	public bool inRange;

	private GameManager _gameManager;
    private void Awake()
    {
		_gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
        if (isClient) {
			if (Input.GetKeyUp(KeyCode.Space) && inRange && !_gameManager.isHandheld)
			{
				if (!_gameManager.dialogBox.activeInHierarchy)
				{
					if (profession == Profession.Patrol)
					{
						GetComponent<NPCMovement>().ActivateTalkCondition();
						_gameManager.dialogBox.SetActive(true);
						_gameManager.DialogBox(dialogMessage);
					}
					if (profession == Profession.Sign)
					{

						_gameManager.dialogBox.SetActive(true);
						_gameManager.DialogBox(dialogMessage);
					}
				}

			}
			else
			{
				if (playerMovement != null && _gameManager.isHandheld)
				{
					if (playerMovement.pressRead)
					{
						playerMovement.AllSignRead(dialogMessage);
					}
				}
			}

		}

	}
	PlayerMovement playerMovement;

	private void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.CompareTag("Player") && !other.collider.isTrigger)
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			inRange = true;
			if (_gameManager.isHandheld)
			{
				playerMovement = other.gameObject.GetComponent<PlayerMovement>();
				playerMovement.canReadSign = true;
				playerMovement.actionText.text = "Read";

			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player") && !other.collider.isTrigger)
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			inRange = false;
			if (_gameManager.isHandheld)
			{
				playerMovement = other.gameObject.GetComponent<PlayerMovement>();
				playerMovement.canReadSign = false;
				playerMovement.actionText.text = null;
			}
			if (profession == Profession.Patrol)
			{
				GetComponent<NPCMovement>().DeactivateTalkCondition();
				_gameManager.dialogBox.SetActive(false);
			}
			if (profession == Profession.Sign)
			{
				_gameManager.dialogBox.SetActive(false);
			}
		}
	}

}
