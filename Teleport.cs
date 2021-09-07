using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Toast;
using Mirror;

public class Teleport : MonoBehaviour
{
	public static Teleport instance;

	public static Teleport TeleportInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Teleport>();
			}
			return instance;
		}

	}

	private void OnValidate()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

    public GameObject player;
    public GameObject toLocation;
	public Animator transition;
	public float teleportDelayTime;
	public float fadeDelay;
	public int requiredLevel;


	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player") && !other.isTrigger)
		{
			if(LevelSystem.LevelInstance.currentLevel < requiredLevel)
			{
				Toast.Show("You arent high enough level for this area", 2f, ToastPosition.MiddleCenter);
			}
			else if(LevelSystem.LevelInstance.currentLevel >= requiredLevel)
			{
				StartCoroutine(EnterArea());
			}
		}
	}

	//this script need to be modified
	IEnumerator EnterArea()
	{
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		transition.SetBool("Exit", true);
		yield return new WaitForSeconds(teleportDelayTime);
		player.transform.position = new Vector2(toLocation.transform.position.x, toLocation.transform.position.y);
		yield return new WaitForSeconds(fadeDelay);
		transition.SetBool("Exit", false);
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
