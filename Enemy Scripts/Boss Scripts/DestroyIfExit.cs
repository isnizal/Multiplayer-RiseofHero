using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfExit : MonoBehaviour
{
	public static DestroyIfExit instance;

	private void Update()
	{
		instance = this;
	}

	public void DestroyObject()
	{
		Destroy(this.gameObject);
	}
}
