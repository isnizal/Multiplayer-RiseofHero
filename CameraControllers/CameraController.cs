using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraController : MonoBehaviour
{
    public GameObject thePlayer;
    public Transform lookAtTarget;
    private CinemachineVirtualCamera vcam;

    public void EnterArea(GameObject player)
    {
        thePlayer = player;
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (thePlayer != null)
        {
            lookAtTarget = thePlayer.transform;
            vcam.Follow = lookAtTarget;
        }
    }
    public void LeaveArea(GameObject player)
    {
        thePlayer = player;
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (thePlayer != null)
        {
            lookAtTarget = null;
            vcam.Follow = null;
        }

    }
}
