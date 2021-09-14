using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Toast;
using Mirror;

public class GameObserver : MonoBehaviour
{
    public static GameObserver instance;
    public string LocalPlayerName;
    public static GameObserver MyGameObserver
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameObserver>();
            }
            return instance;
        }
    }
	private void Awake()
	{
            DontDestroyOnLoad(this.gameObject);
	}

    public bool newCharacter;
    [HideInInspector]public bool loadCharacter = false;
	void Start()
    {
        loadCharacter = false;
    }

    void Update()
    {

        if(newCharacter)
		{
            newCharacter = false;
		}
    }
    public void LoadCharacter()
    {
        if (loadCharacter)
        {
            Destroy(GetComponentInChildren<GameClothes>().gameObject);
            StartCoroutine(LoadCoroutine());
        }
        else
        {
            GetComponentInChildren<GameClothes>().LoadClothesOnStart();
        }
    }

    private IEnumerator LoadCoroutine()
	{
        LoadGame();
        yield return new WaitForSeconds(1f);
        Toast.Show("Character Loaded", 3f, ToastPosition.MiddleCenter);
	}
    public void LoadGame()
    {
        Character.MyInstance.Load();
        loadCharacter = false;
    }
    public void EnableLoad()
    {
        loadCharacter = true;
    }


}
