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

	private PlayerCombat _playerCombat;
	private Character _character;
	private PlayerMovement _playerMovement;
	private LevelSystem _levelSystem;
	private UIManager _uiManager;
    public void InitializeGameManagerVariable(Character player, UIManager uiManager)
	{
		if (player.isLocalPlayer)
		{
			_playerCombat = player.gameObject.GetComponent<PlayerCombat>();
			_character = player;
			_playerMovement = player.gameObject.GetComponent<PlayerMovement>();
			_levelSystem = player.gameObject.GetComponent<LevelSystem>();
			_uiManager = uiManager;

			var shopEquip = FindObjectOfType<ShopEquipmentManager>();
			shopEquip.InitializeShopEquipmentManger(player);
			var shopItem = FindObjectOfType<ShopItemManager>();
			shopItem.InitializeShopItemManager(player);
			shopItem.gameObject.SetActive(false);
			shopItem.transform.parent.gameObject.SetActive(false);
			FindObjectOfType<SpellTree>().InitializeSpell(player);
			FindObjectOfType<StatPanel>().InitializeStatPanel(player);
			FindObjectOfType<StatsModifier>().InitializeStatModifier(player);
			FindObjectOfType<DailyRewardSystem.DailyRewards>().InitializeDailyRewards(player);


			_spellTree = FindObjectOfType<SpellTree>();
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
	private SpellTree _spellTree;
    private void Update()
	{
		if (_spellTree  == null)
			return;

		CheckFireballSpellActive();
		CheckIcicleSpellActive();
		CheckArcticBlastSpellActive();
	}

	void CheckFireballSpellActive()
	{
		if (_spellTree.fireball1Level < _spellTree.fireball1LevelMax && _spellTree.fireball1Level >= _spellTree.fireball1LevelReq)
		{
			fireball1Active = true;
		}
		else if (_spellTree.fireball1Level == _spellTree.fireball1LevelMax && _spellTree.fireball2Level < _spellTree.fireball2LevelMax)
		{
			fireball1Active = false;
			fireball2Active = true;
		}
	}

	void CheckIcicleSpellActive()
	{
		if(_spellTree.icicle1Level < _spellTree.icicle1LevelMax && _spellTree.icicle1Level >= _spellTree.icicle1LevelReq)
		{
			icicle1Active = true;
		}
		else if (_spellTree.icicle1Level == _spellTree.icicle1LevelMax && _spellTree.icicle2Level < _spellTree.icicle2LevelMax)
		{
			icicle1Active = false;
			icicle2Active = true;
		}
	}

	void CheckArcticBlastSpellActive()
	{
		if(_spellTree.arcticBlast1Level < _spellTree.arcticBlast1LevelMax && _spellTree.arcticBlast1Level >= _spellTree.arcticBlast1LevelReq)
		{
			arcticBlast1Active = true;
		}
		else if(_spellTree.arcticBlast1Level == _spellTree.arcticBlast1LevelMax && _spellTree.arcticBlast2Level < _spellTree.arcticBlast2LevelMax)
		{
			arcticBlast1Active = false;
			arcticBlast2Active = true;
		}
	}

	IEnumerator RespawnPlayer()
	{
		_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		transition.SetBool("Exit", true);
		yield return new WaitForSeconds(teleportDelayTime);
		_player.GetComponent<NetworkTransform>().transform.position = new Vector2(toLocation.transform.position.x, toLocation.transform.position.y);

		_levelSystem.currentExp = LevelSystem.LevelInstance.currentExp / 2;

		yield return new WaitForSeconds(fadeDelay);
		_character.newHealth = _character.MaxHealth;
		_character.Health = _character.newHealth;
		_character.newMana = _character.MaxMP;
		_character.Mana = _character.newMana;

		_uiManager.UpdateHealth();

		transition.SetBool("Exit", false);
		_playerCombat.playerDied = false;
		//respawn initialize
		_playerMovement.Initialize();
		_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		_player.GetComponent<BoxCollider2D>().enabled = true;
		_playerCombat.EnableSelfRegenHp();
		_playerCombat.EnableSelfRegenMana();

		autoSave = true;
		saveTimer = StartSaveTimer();
		StartCoroutine(saveTimer);
	}

	[SerializeField]private Button yesRespawnBtn, noRespawnBtn;
	public void ClickYesRespawn()
	{
		_playerCombat.respawnWindow.SetActive(false);
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
			if (!_playerCombat.playerDied)
			{
				_character.Save();
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
		_character.Save();
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


