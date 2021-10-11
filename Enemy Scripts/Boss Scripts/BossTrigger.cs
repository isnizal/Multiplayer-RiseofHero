using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BossTrigger : MonoBehaviour
{
	public static BossTrigger instance;
	public static BossTrigger BossTriggerInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<BossTrigger>();
			}
			return instance;
		}

	}

	//public GameObject bossPrefab;
	//public Transform bossHolder;
	public Transform bossStart;
	//public float bossTimerTrigger = 120f;

	private ServerManager _serverManager;
	private PlayerMovement _playerMovement;
	private void Update()
	{
		//bossTimerTrigger -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			_playerMovement = other.gameObject.GetComponent<PlayerMovement>();
			_serverManager = _playerMovement.serverManager;
			if (!_serverManager.devilQueenSpawned)
			{
				if (_serverManager.bossTimerTrigger <= 0)
				{
					_serverManager.devilQueenCanSpawn = true;
					BossSpawn();
				}
			}


		}
	}

	public void BossSpawn()
	{
		if (_serverManager.devilQueenCanSpawn && !_serverManager.devilQueenSpawned)
		{
			_playerMovement.ContactServerManager(bossStart.transform.position);
		}
		
	}
}
