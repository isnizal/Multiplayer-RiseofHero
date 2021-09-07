using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public GameObject bossPrefab;
	public Transform bossHolder;
	public Transform bossStart;
	public float bossTimerTrigger = 120f;

	private void Update()
	{
		bossTimerTrigger -= Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player") && !GameManager.GameManagerInstance.devilQueenSpawned)
		{
			if (bossTimerTrigger <= 0)
			{
				GameManager.GameManagerInstance.devilQueenCanSpawn = true;
				BossSpawn();
			}
		}
	}

	public void BossSpawn()
	{
		if (GameManager.GameManagerInstance.devilQueenCanSpawn && !GameManager.GameManagerInstance.devilQueenSpawned)
		{
			var clone = Instantiate(bossPrefab, bossStart.position, Quaternion.identity);
			clone.transform.parent = bossHolder;
			GameManager.GameManagerInstance.devilQueenSpawned = true;
			bossTimerTrigger = 120f;
		}
		
	}
}
