
using UnityEngine;
using Mirror;

public class SwitchMusicTrigger : MonoBehaviour
{
    public AudioClip newTrack;
    private GameManager gameManager;

    void Start()
    {
		gameManager = FindObjectOfType<GameManager>();
	}

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
			return;

		//Debug.Log(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
		if ( other.CompareTag("Player"))
		{
			//for (int i = 0; i < registerPlayerForMusic.Length; i++)
			//{
			//	if (registerPlayerForMusic[i] == null)
			//		registerPlayerForMusic[i] = other.gameObject;
			//	else if(registerPlayerForMusic[i] == other.gameObject)
			//		break;
			//}
			//for (int a = 0; a < registerPlayerForMusic.Length; a++)
			//{
			//	Debug.Log(registerPlayerForMusic[a].GetComponent<NetworkIdentity>().connectionToClient);
			//	Debug.Log(other.gameObject.GetComponent<NetworkIdentity>().connectionToClient);
			//
			//	if (registerPlayerForMusic[a].GetComponent<NetworkIdentity>().connectionToClient
			//		!= other.gameObject.GetComponent<NetworkIdentity>().connectionToClient)
			//	{
			//		break;
			//	}
			//}
			if (newTrack != null)
				if (gameManager == null)
				{
					gameManager = FindObjectOfType<GameManager>();
					gameManager.ChangeBGM(newTrack);
				}else
					gameManager.ChangeBGM(newTrack);
			if (newTrack == null)
				gameManager.ChangeBGM(newTrack);
		}
	}
}
