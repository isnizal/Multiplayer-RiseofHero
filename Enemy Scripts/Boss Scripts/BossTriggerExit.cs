using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BossTriggerExit : MonoBehaviour
{
	private ServerManager _serverManager;
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_serverManager = other.gameObject.GetComponent<PlayerMovement>().serverManager;
			if (_serverManager.devilQueenSpawned)
			{
				DestroyIfExit.instance.DestroyObject();
				_serverManager.devilQueenSpawned = false;
			}
		}
	}
}
