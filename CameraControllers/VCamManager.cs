using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VCamManager : MonoBehaviour
{
    public GameObject EnterCamera;

	private GameObject[] registerPlayerOnTrigger = new GameObject[100];
    private void OnTriggerEnter2D(Collider2D other)
	{

		if (!other.CompareTag("Player"))
			return;
		if (other is null)
			return;
		Debug.Log(other.name);
		if (!other.isTrigger && other.CompareTag("Player"))
		{
			//for (int i = 0; i < registerPlayerOnTrigger.Length; i++)
			//{
			//	if (registerPlayerOnTrigger[i] is null)
			//	{
			//		registerPlayerOnTrigger[i] = other.gameObject;
			//	}
			//	else if(registerPlayerOnTrigger[i] == other.gameObject)
			//		break;
			//
			//	Debug.Log(registerPlayerOnTrigger[i].gameObject.name);
			//}
			//for (int a = 0; a < registerPlayerOnTrigger.Length; a++)
			//{
			//	Debug.Log("vcammanager" + other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
			//	Debug.Log("vcammanager" + registerPlayerOnTrigger[a].GetComponent<NetworkIdentity>().connectionToClient);
			//	if (other.gameObject.GetComponent<NetworkIdentity>().connectionToClient 
			//		== registerPlayerOnTrigger[a].gameObject.GetComponent<NetworkIdentity>().connectionToClient)
			//	{
			//
			//		break;
			//	}
			//}
			EnterCamera.SetActive(true);
			GetComponentInChildren<CameraController>().EnterArea(other.gameObject);
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
			return;

		if( !other.isTrigger && other.CompareTag("Player"))
		{
			//for (int s = 0; s < registerPlayerOnTrigger.Length; s++)
			//{
			//	if (other.gameObject.GetComponent<NetworkIdentity>().connectionToClient == 
			//		registerPlayerOnTrigger[s].gameObject.GetComponent<NetworkIdentity>().connectionToClient)
			//	{
			//
			//		break;
			//	}
			//}
			if (GetComponentInChildren<CameraController>() is null)
				return;
			GetComponentInChildren<CameraController>().LeaveArea(other.gameObject);
			EnterCamera.SetActive(false);
		}
	}
	private GameObject _Player;
	public void StorePlayerPreference(GameObject newPlayer)
    {
		Debug.Log("Store reference" + newPlayer.GetComponent<NetworkIdentity>().connectionToClient);
		_Player = newPlayer;
    }
}
