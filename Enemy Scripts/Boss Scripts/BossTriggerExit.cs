using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerExit : MonoBehaviour
{
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log(other.gameObject.name);
			if (GameManager.GameManagerInstance.devilQueenSpawned)
			{
				DestroyIfExit.instance.DestroyObject();
				GameManager.GameManagerInstance.devilQueenSpawned = false;
			}
		}
	}
}
