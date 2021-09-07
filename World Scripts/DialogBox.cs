using UnityEngine;

public class DialogBox : MonoBehaviour
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

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space) && inRange && !GameManager.GameManagerInstance.isHandheld)
		{
			if (!GameManager.GameManagerInstance.dialogBox.activeInHierarchy)
			{
				if (profession == Profession.Patrol)
				{
					GetComponent<NPCMovement>().ActivateTalkCondition();
					GameManager.GameManagerInstance.dialogBox.SetActive(true);
					GameManager.GameManagerInstance.DialogBox(dialogMessage);
				}
				if (profession == Profession.Sign)
				{

					GameManager.GameManagerInstance.dialogBox.SetActive(true);
					GameManager.GameManagerInstance.DialogBox(dialogMessage);
				}
			}
			else
			{
				if (profession == Profession.Patrol)
				{
					GetComponent<NPCMovement>().DeactivateTalkCondition();
					GameManager.GameManagerInstance.dialogBox.SetActive(false);
				}
				if (profession == Profession.Sign)
				{
					GameManager.GameManagerInstance.dialogBox.SetActive(false);
				}
			}

		}

	}
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player") && !other.isTrigger)
		{
			inRange = true;
			if (GameManager.GameManagerInstance.isHandheld)
			{
				PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
				playerMovement.canReadSign = true;
				playerMovement.actionText.text = "Read";
				if (playerMovement.pressRead)
				{
					playerMovement.AllSignRead(dialogMessage);
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player") && !other.isTrigger)
		{
			inRange = false;
			if (GameManager.GameManagerInstance.isHandheld)
			{
				PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
				playerMovement.canReadSign = false;
				playerMovement.actionText.text = null;
			}
			GameManager.GameManagerInstance.dialogBox.SetActive(false);
		}
	}

}
