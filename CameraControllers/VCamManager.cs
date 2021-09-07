using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamManager : MonoBehaviour
{
    public GameObject EnterCamera;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !other.isTrigger)
		{
			EnterCamera.SetActive(true);
			
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Player") && !other.isTrigger)
		{
			EnterCamera.SetActive(false);
		}
	}
}
