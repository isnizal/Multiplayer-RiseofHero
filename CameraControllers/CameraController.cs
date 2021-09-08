using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraController : MonoBehaviour
{
    public GameObject thePlayer;
    public Transform lookAtTarget;
    private CinemachineVirtualCamera vcam;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        AttachCam();
    }
    void AttachCam()
	{
        if (thePlayer == null)
		{
            thePlayer = FindObjectOfType<PlayerMovement>().gameObject;
            if(thePlayer != null)
			{
                lookAtTarget = thePlayer.transform;
                vcam.Follow = lookAtTarget;
			}
		}
	}
}
