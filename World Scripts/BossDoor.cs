using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
	public SpriteRenderer doorSprite;
	private void Update()
	{
		if (GameManager.GameManagerInstance is null)
			return;
		if (GameManager.GameManagerInstance.devilQueenDefeated == 1)
		{
			doorSprite.enabled = false;
			doorSprite.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
