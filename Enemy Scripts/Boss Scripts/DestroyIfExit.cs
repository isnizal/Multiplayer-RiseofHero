using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DestroyIfExit : NetworkBehaviour
{
	public static DestroyIfExit instance;

	private void Update()
	{
		instance = this;
	}

	[Server]
	public void DestroyObject()
	{
		if (isServer)
		{
			NetworkServer.Destroy(this.gameObject);
		}
	}
}
