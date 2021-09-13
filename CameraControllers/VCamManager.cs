using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VCamManager : MonoBehaviour
{
    public GameObject EnterCamera;
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
	{

		if (other.CompareTag("Player") && !other.isTrigger)
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;

			EnterCamera.SetActive(true);
			GetComponentInChildren<CameraController>().EnterArea(other.gameObject);

		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !other.isTrigger)
		{
			if (!other.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
				return;


			GetComponentInChildren<CameraController>().LeaveArea(other.gameObject);
			EnterCamera.SetActive(false);
		}
	}
}
