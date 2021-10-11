using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
	public SpriteRenderer doorSprite;

	private PlayerMovement _playerMovement;
	public void InitializeBossDoor(PlayerMovement playerMovement)
	{
		_playerMovement = playerMovement;
	}
	private void Update()
	{
		if (_playerMovement != null && _playerMovement.serverManager.devilQueenDefeated)
		{
			doorSprite.enabled = false;
			doorSprite.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
