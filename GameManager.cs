using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.Toast;
using TMPro;
using Mirror;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager :MonoBehaviour
{
	public static GameManager instance;
	public static GameManager GameManagerInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}

	}

	[Header("Dialog")]
	public GameObject dialogBox;
	[FormerlySerializedAs("dialogMessageText")]
	public TextMeshProUGUI dialogText;

	[Space]
	[Header("Game Sounds")]
	public SoundAudioClip[] soundAudioClipArray;

	[System.Serializable]
	public class SoundAudioClip
	{
		public SoundManager.Sound sound;
		public AudioClip audioClip;
	}

	[Header("Misc Variables")]
	public int firstTimePlaying = 0;

	[Header("Device Tracker")]
	public bool isHandheld = false;
	public bool isDesktop = false;
	public GameObject joystickCanvas;

	public AudioSource BGM;

	[Header("Player Respawn Info")]
	public GameObject _player;
	public GameObject toLocation;
	public Animator transition;
	public float teleportDelayTime;
	public float fadeDelay;

	[Header("Spell Tracker")]
	public bool fireball1Active;
	public bool fireball2Active;
	public bool icicle1Active;
	public bool icicle2Active;
	public bool arcticBlast1Active;
	public bool arcticBlast2Active;
	[Header("Boss Data")]
	public int devilQueenDefeated = 0;
	public bool devilQueenSpawned;
	public bool devilQueenCanSpawn;

	public IEnumerator saveTimer;

    public void InitializeGameManagerVariable(Character player)
	{
		if (player.isLocalPlayer)
		{
			FindObjectOfType<ShopEquipmentManager>().InitializeShopEquipmentManger(player);
			FindObjectOfType<ShopItemManager>().InitializeShopItemManager(player);
			FindObjectOfType<SpellTree>().InitializeSpell(player);
			FindObjectOfType<StatPanel>().InitializeStatPanel(player);
			FindObjectOfType<StatsModifier>().InitializeStatModifier(player);

			FindObjectOfType<DialogBox>().InitializeDialogBox(this);

			//respawn click
			GetComponent<AudioSource>().Play();
			yesRespawnBtn.onClick.AddListener(ClickYesRespawn);
			noRespawnBtn.onClick.AddListener(ClickNoRespawn);

			this._player = player.gameObject;

			joystickCanvas = GameObject.Find("JoystickCanvas");
			//player hud
			dialogText = GameObject.Find("DialogText").GetComponent<TextMeshProUGUI>();
			dialogBox = GameObject.Find("DialogBox");
			dialogBox.SetActive(false);
			//castle
			toLocation = GameObject.Find("PlayerStartPoint");
			//transition
			transition = GameObject.Find("CrossFade").GetComponent<Animator>();

			if (SystemInfo.deviceType == DeviceType.Handheld)
			{
				joystickCanvas.SetActive(true);
				isHandheld = true;
			}
			else
			{
				joystickCanvas.SetActive(false);
				isDesktop = true;
			}

			autoSave = true;
			saveTimer = StartSaveTimer();
			StartCoroutine(saveTimer);

		}
	}
    private void Update()
	{
		if (SpellTree.SpellInstance is null)
			return;

		CheckFireballSpellActive();
		CheckIcicleSpellActive();
		CheckArcticBlastSpellActive();
	}

	void CheckFireballSpellActive()
	{
		if (SpellTree.SpellInstance.fireball1Level < SpellTree.SpellInstance.fireball1LevelMax && SpellTree.SpellInstance.fireball1Level >= SpellTree.SpellInstance.fireball1LevelReq)
		{
			fireball1Active = true;
		}
		else if (SpellTree.SpellInstance.fireball1Level == SpellTree.SpellInstance.fireball1LevelMax && SpellTree.SpellInstance.fireball2Level < SpellTree.SpellInstance.fireball2LevelMax)
		{
			fireball1Active = false;
			fireball2Active = true;
		}
	}

	void CheckIcicleSpellActive()
	{
		if(SpellTree.SpellInstance.icicle1Level < SpellTree.SpellInstance.icicle1LevelMax && SpellTree.SpellInstance.icicle1Level >= SpellTree.SpellInstance.icicle1LevelReq)
		{
			icicle1Active = true;
		}
		else if (SpellTree.SpellInstance.icicle1Level == SpellTree.SpellInstance.icicle1LevelMax && SpellTree.SpellInstance.icicle2Level < SpellTree.SpellInstance.icicle2LevelMax)
		{
			icicle1Active = false;
			icicle2Active = true;
		}
	}

	void CheckArcticBlastSpellActive()
	{
		if(SpellTree.SpellInstance.arcticBlast1Level < SpellTree.SpellInstance.arcticBlast1LevelMax && SpellTree.SpellInstance.arcticBlast1Level >= SpellTree.SpellInstance.arcticBlast1LevelReq)
		{
			arcticBlast1Active = true;
		}
		else if(SpellTree.SpellInstance.arcticBlast1Level == SpellTree.SpellInstance.arcticBlast1LevelMax && SpellTree.SpellInstance.arcticBlast2Level < SpellTree.SpellInstance.arcticBlast2LevelMax)
		{
			arcticBlast1Active = false;
			arcticBlast2Active = true;
		}
	}

	IEnumerator RespawnPlayer()
	{
		_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		transition.SetBool("Exit", true);
		Debug.Log("Respawn step 1");
		yield return new WaitForSeconds(teleportDelayTime);
		_player.transform.position = new Vector2(toLocation.transform.position.x, toLocation.transform.position.y);

		LevelSystem.LevelInstance.currentExp = LevelSystem.LevelInstance.currentExp / 2;

		Debug.Log("Respawn step 2");
		yield return new WaitForSeconds(fadeDelay);
		Character.MyInstance.newHealth = Character.MyInstance.MaxHealth;
		Character.MyInstance.Health = Character.MyInstance.newHealth;
		Character.MyInstance.newMana = Character.MyInstance.MaxMP;
		Character.MyInstance.Mana = Character.MyInstance.newMana;

		UIManager.Instance.UpdateHealth();

		transition.SetBool("Exit", false);
		PlayerCombat.CombatInstance.playerDied = false;
		//respawn initialize
		PlayerMovement.instance.Initialize();
		_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		PlayerCombat.CombatInstance.GetComponent<BoxCollider2D>().enabled = true;
		PlayerCombat.CombatInstance.EnableSelfRegenHp();
		PlayerCombat.CombatInstance.EnableSelfRegenMana();

		autoSave = true;
		saveTimer = StartSaveTimer();
		StartCoroutine(saveTimer);
		Debug.Log("Respawn step 3");
	}

	[SerializeField]private Button yesRespawnBtn, noRespawnBtn;
	public void ClickYesRespawn()
	{
		PlayerCombat.CombatInstance.respawnWindow.SetActive(false);
		StartCoroutine(RespawnPlayer());
	}
	public void ClickNoRespawn()
	{
		Toast.Show("<b>Na Na Na, You didn't say the magic words!</b>", 2f, ToastPosition.MiddleCenter);
	}

	public void ChangeBGM(AudioClip music)
	{
		if (BGM.clip.name == music.name)
			return;
		else if (BGM.clip.name != music.name)
		{
			BGM.Stop();
			BGM.clip = music;
			BGM.Play();
		}
	}
	public bool autoSave = true;
	IEnumerator StartSaveTimer()
	{

		do
		{
			yield return new WaitForSeconds(30f);
			if (!PlayerCombat.CombatInstance.playerDied)
			{
				Character.MyInstance.Save();
				Debug.LogWarning("Game Save");
			}
		}
		while (autoSave);

	}

	public void DialogBox(string message)
	{
		dialogText.text = message;
	}

	public void SaveGame()
	{
		Toast.Show("Game has been saved!", 2f, ToastPosition.MiddleCenter);
		Character.MyInstance.Save();
	}
	public void QuitGame()
	{
		StartCoroutine(QuitGameCo());
	}
	IEnumerator QuitGameCo()
	{
		Toast.Show("Now Exiting, Thanks for Playing!", 4f, ToastPosition.MiddleCenter);
		yield return new WaitForSeconds(4f);
		Application.Quit();
	}
}


