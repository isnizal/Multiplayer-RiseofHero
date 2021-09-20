using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using Mirror.Experimental;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{

	public static PlayerMovement instance;
	public static PlayerMovement PlayerMovementInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<PlayerMovement>();
			}
			return instance;
		}

	}

	//private Animator anim;
	private NetworkAnimator netAnim;
	private NetworkRigidbody2D netRigidbody2D;
	//private Rigidbody2D myRigidbody;
	private Vector3 change;
	public float moveSpeed;
	public bool canMove;
	[HideInInspector]
	public float rightPos, frontPos;
	public int attackRight, attackFront, attackBack, attackLeft;
	public int currePos;
	public bool attacking;

	public FixedJoystick fixedJoystick;

	[Header("Action Buttons Info")]
	public TextMeshProUGUI actionText;
	public bool canTalkNPCTeleport;
	public bool canTalkNPCCrafter;
	public bool canTalkNPCShop;
	public bool canReadSign;

	private GameClothes gameClothes;
	private PlayerClothes playerClothes;
	[SyncVar]
	public int helmetValue = -1;
	[SyncVar]
	public int torsoValue = -1;
	[SyncVar]
	public int armValue = -1;
	[SyncVar]
	public int bootValue = -1;
	[SyncVar]
	public int swordValue = -1;
	[SyncVar]
	public int shieldValue = -1;
	[SyncVar]
	public int hairValue = -1;

	public void Initialize()
	{
		attacking = false;
		waitAttack = true;
		rightPos = 0;
		frontPos = -1;


		attackRight = 0;
		attackFront = 1;
		attackBack = 0;
		attackLeft = 0;
		if (base.hasAuthority)
		{

			netAnim.animator.SetBool("DeadFront", false);
			netAnim.animator.SetBool("DeadBack", false);
			netAnim.animator.SetBool("DeadRight", false);
			netAnim.animator.SetBool("DeadLeft", false);

			netAnim.animator.SetBool("IdleFront", true);
			netAnim.animator.Play("Base Layer.FrontIdleAnimation");

		}


		currePos = 0;
		canMove = true;

	}
	public void CheckValueClothes()
	{
		if (helmetValue == -1)
			helmetAvatar.SetActive(false);
		else
			helmetAvatar.SetActive(true);

		if (torsoValue == -1)
			torsoAvatar.SetActive(false);
		else
			torsoAvatar.SetActive(true);

		if (armValue == -1) { leftarmAvatar.SetActive(false); rightarmAvatar.SetActive(false); }
		else { leftarmAvatar.SetActive(true); rightarmAvatar.SetActive(true); }

		if (bootValue == -1) { leftBootAvatar.gameObject.SetActive(false); rightBootAvatar.gameObject.SetActive(false); }
		else { leftBootAvatar.SetActive(true); rightBootAvatar.SetActive(true); }

		if (shieldValue == -1)
			shieldAvatar.SetActive(false);
		else
			shieldAvatar.SetActive(true);

		if (swordValue == -1)
			swordAvatar.SetActive(false);
		else
			swordAvatar.SetActive(true);

		if (hairValue == -1)
			hairAvatar.SetActive(false);
		else
			hairAvatar.SetActive(true);
		//RpcCheckValueClothes();
	}
	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();


	}
	public override void OnStartClient()
	{
		base.OnStartClient();


		Debug.Log("starting player client");
		OwnClientStart();

	}
	public void OwnClientStart()
	{
		gameClothes = FindObjectOfType<GameClothes>();
		CmdSetClothesValue(gameClothes.helmetValue, gameClothes.torsoValue, gameClothes.armValue, gameClothes.swordValue,
			gameClothes.shieldValue, gameClothes.bootValue, gameClothes.hairValue);

		CheckValueClothes();
		InitializeAwake();
		//start client initialize
		Initialize();
		CmdSetSpriteFront();
	}
	[Command(requiresAuthority =false)]
	private void CmdSetClothesValue(int helmet, int torso, int arm, int sword, int shiedl, int boot, int hair)
	{
		helmetValue = helmet;
		torsoValue = torso;
		armValue = arm;
		bootValue = boot;
		swordValue = sword;
		shieldValue = shiedl;
		hairValue = hair;
	}
	public override void OnStartAuthority()
	{
		base.OnStartAuthority();

		Debug.Log("player start authority");

		//InstantiateObjectCanvas();
		string findName = FindObjectOfType<GameObserver>().LocalPlayerName;
		InvokeCanvasObjects(findName);

	}

	[SyncVar(hook = nameof(OnNameChanged))]
	private string localPlayerName = "M";

	[SyncVar(hook = nameof(OnMessageChanged))]
	private string localPlayerMessage;

	private void InvokeCanvasObjects(string findName)
	{
		CmdSetName(findName);
	}
	public void SetMessageForPlayer(string Message)
	{
		CmdSentMessage(Message);
	}
	[Command(requiresAuthority = false)]
	public void CmdSetName(string name)
	{
		localPlayerName = name;
	}

	public void OnNameChanged(string oldName, string newName)
	{
		TextMeshProUGUI nameText = GetComponentInChildren<NameCanvas>().nameText;
		nameText.text = localPlayerName;
	}

	private TextMeshProUGUI dialogText;
	private GameObject dialogObject;
	private GameObject nameObject;
	public void OnMessageChanged(string oldMessage, string newMessage)
	{
		if (DialogDisable != null)
			StopCoroutine(DialogDisable);
		dialogText = GetComponentInChildren<NameCanvas>().dialogText;
		dialogObject = GetComponentInChildren<NameCanvas>().dialogObject;
		nameObject = GetComponentInChildren<NameCanvas>().nameObject;

		dialogText.text = localPlayerMessage;
		onDialog = true;
		dialogObject.SetActive(true);
		nameObject.SetActive(false);
		DialogDisable = StartDisableDialog(5f);
		StartCoroutine(DialogDisable);
	}

	[Command(requiresAuthority = false)]
	public void CmdSentMessage(string message)
	{
		localPlayerMessage = message;

	}
	private bool onDialog = false;
	public IEnumerator DialogDisable;
	private IEnumerator StartDisableDialog(float value)
	{
		while (onDialog)
		{
			yield return new WaitForSeconds(value);
			onDialog = false;
			dialogObject.SetActive(false);
			nameObject.SetActive(true);
		}
	}
	private Character _character;
	private Button _actionBtn,_spellBtn,_attackBtn;
	public void InitializeAwake()
	{
		_character = GetComponent<Character>();
		GetComponent<PlayerCombat>().FindRespawnWindow();
		netAnim = GetComponent<NetworkAnimator>();
		netRigidbody2D = GetComponent<NetworkRigidbody2D>();
		netRigidbody2D.target.simulated = base.hasAuthority;
		gameClothes = FindObjectOfType<GameClothes>();
		playerClothes = GetComponent<PlayerClothes>();

		//Mobile
		fixedJoystick = FindObjectOfType<FixedJoystick>();
		actionText = GameObject.Find("ActionText").GetComponent<TextMeshProUGUI>();
		_actionBtn = GameObject.Find("ActionButton").GetComponent<Button>();
		_actionBtn.onClick.AddListener(ActionButton);
		_spellBtn = GameObject.Find("SpellButton").GetComponent<Button>();
		_spellBtn.onClick.AddListener(this.GetComponent<PlayerCombat>().CastSpell);
		_attackBtn = GameObject.Find("AttackButton").GetComponent<Button>();
		_attackBtn.onClick.AddListener(ActionMobileAttack);
		GetComponent<Character>().FindObjects();

	}

    //call from animation
    public void EnableDamage()
	{
		attacking = false;
		waitAttack = true;
	}
	private void Update()
	{
		if (isLocalPlayer)
		{
			if (_character is null)
				return;
			if (!_character.onInput)
			{
				if (!PlayerCombat.CombatInstance.playerDied)
				{
					if (GameManager.GameManagerInstance.isHandheld)
					{
						change = Vector3.zero;
						change.x = fixedJoystick.Horizontal;
						change.y = fixedJoystick.Vertical;

						if (base.hasAuthority)
						{
							UpdateAnimation();
							AttackActionButton();
						}
					}

					if (GameManager.GameManagerInstance.isDesktop)
					{

						change = Vector3.zero;
						if (!attacking)
						{
							change.x = Input.GetAxisRaw("Horizontal");
							change.y = Input.GetAxisRaw("Vertical");
						}
						if (base.hasAuthority)
						{
							UpdateAnimation();
							AttackActionButton();
						}
					}
				}
			}
		}


	}
	public void ActionMobileAttack()
	{
		if (base.hasAuthority)
		{
			if (attackRight == 1)
			{
				if (waitAttack)
				{
					waitAttack = false;
					netAnim.animator.SetTrigger("AttackRight");
					attacking = true;
				}
			}
			else if (attackLeft == 1)
			{
				if (waitAttack)
				{
					waitAttack = false;
					netAnim.animator.SetTrigger("AttackLeft");
					attacking = true;
				}
			}
			else if (attackBack == 1)
			{
				if (waitAttack)
				{
					waitAttack = false;
					netAnim.animator.SetTrigger("AttackBack");
					attacking = true;
				}

			}
			else if (attackFront == 1)
			{
				if (waitAttack)
				{
					waitAttack = false;
					netAnim.animator.SetTrigger("AttackFront");
					attacking = true;
				}
			}
		}
	}
	public void AttackActionButton()
	{
		if (base.hasAuthority)
		{
			//from calculate pos
			if (attackRight == 1)
			{
				if (waitAttack)
				{
					if (Input.GetKeyDown(KeyCode.F))
					{
						waitAttack = false;
						netAnim.animator.SetTrigger("AttackRight");
						attacking = true;
					}
				}
			}
			else if (attackLeft == 1)
			{
				if (waitAttack)
				{
					if (Input.GetKeyDown(KeyCode.F))
					{
						waitAttack = false;
						netAnim.animator.SetTrigger("AttackLeft");
						attacking = true;
					}
				}
			}
			else if (attackBack == 1)
			{
				if (waitAttack)
				{
					if (Input.GetKeyDown(KeyCode.F))
					{
						waitAttack = false;
						netAnim.animator.SetTrigger("AttackBack");
						frontPos = 1;
						attacking = true;
					}
				}

			}
			else if (attackFront == 1)
			{
				if (waitAttack)
				{
					if (Input.GetKeyDown(KeyCode.F))
					{
						waitAttack = false;
						netAnim.animator.SetTrigger("AttackFront");
						attacking = true;
					}
				}
			}
		}
	}

	private bool waitAttack = true;

	[Header("Avatar")]
	[SerializeField] private GameObject hairAvatar;
	[SerializeField] private GameObject helmetAvatar;
	[SerializeField] private GameObject torsoAvatar;
	[SerializeField] private GameObject shieldAvatar;
	[SerializeField] private GameObject leftarmAvatar;
	[SerializeField] private GameObject rightarmAvatar;
	[SerializeField] private GameObject leftBootAvatar;
	[SerializeField] private GameObject rightBootAvatar;
	[SerializeField] private GameObject swordAvatar;

	private void CheckMovePos()
	{
			if (change.x == 1)
			{
				CmdSetSpriteRight();
				rightPos = 1;
				frontPos = 0;

				attackRight = 1;
				attackLeft = 0;
				attackFront = 0;
				attackBack = 0;

				currePos = 1;
			}
			else if (change.x == -1)
			{

				CmdSetSpriteLeft();
				rightPos = -1;
				frontPos = 0;

				attackLeft = 1;
				attackRight = 0;
				attackBack = 0;
				attackFront = 0;

				currePos = 3;
			}
			else if (change.y == 1)
			{

				CmdSetSpriteBack();
				rightPos = 0;
				frontPos = 1;

				attackBack = 1;
				attackFront = 0;
				attackLeft = 0;
				attackRight = 0;

				currePos = 2;
			}
			else if (change.y == -1)
			{
				CmdSetSpriteFront();
				rightPos = 0;
				frontPos = -1;

				attackFront = 1;
				attackLeft = 0;
				attackRight = 0;
				attackBack = 0;

				currePos = 0;
			}
		
	}



	[Command(requiresAuthority = false)]
	private void CmdSetSpriteFront()
	{

		if (isServer)
			RpcSetSpriteFront();
	}
	[ClientRpc]
	public void RpcSetSpriteFront()
	{
		if (playerClothes is null)
			playerClothes = GetComponent<PlayerClothes>();
		if (playerClothes is null)
			return;
		if (helmetAvatar.activeInHierarchy)
		{
			if (helmetValue == -1)
				return;
			this.helmetAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[helmetValue].helmetSprite[0];
		}
		if (torsoAvatar.activeInHierarchy)
		{
			if (torsoValue == -1)
				return;
			this.torsoAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[torsoValue].torsoSprite[0];
		}
		if (leftarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.leftarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].leftArmSprite[0];
		}
		if (rightarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.rightarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].rightArmSprite[0];
		}
		if (leftBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.leftBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].leftBootSprite[0];
		}
		if (rightBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.rightBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].rightBootSprite[0];
		}
		if (swordAvatar.activeInHierarchy)
		{
			if (swordValue == -1)
				return;
			this.swordAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[swordValue].swordSprite[0];
		}
		if (shieldAvatar.activeInHierarchy)
		{
			if (shieldValue == -1)
				return;
			this.shieldAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[shieldValue].shieldSprite[0];
		}
		if (hairAvatar.activeInHierarchy)
		{
			if (hairValue == -1)
				return;
			this.hairAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[hairValue].hairSprite[0];
		}
	}

	[Command(requiresAuthority = false)]
	private void CmdSetSpriteLeft()
	{	

		if (isServer)
			RpcSetSpriteLeft();
	}


	[ClientRpc]
	private void RpcSetSpriteLeft()
	{
		if (playerClothes is null)
			playerClothes = GetComponent<PlayerClothes>();
		if (playerClothes is null)
			return;
		if (helmetAvatar.activeInHierarchy)
		{
			if (helmetValue == -1)
				return;
			this.helmetAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[helmetValue].helmetSprite[1];
		}
		if (torsoAvatar.activeInHierarchy)
		{
			if (torsoValue == -1)
				return;
			this.torsoAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[torsoValue].torsoSprite[1];
		}
		if (leftarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.leftarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].leftArmSprite[1];
		}
		if (rightarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.rightarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].rightArmSprite[1];
		}
		if (leftBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.leftBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].leftBootSprite[1];
		}
		if (rightBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.rightBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].rightBootSprite[1];
		}
		if (swordAvatar.activeInHierarchy)
		{
			if (swordValue == -1)
				return;
			this.swordAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[swordValue].swordSprite[1];
		}
		if (shieldAvatar.activeInHierarchy)
		{
			if (shieldValue == -1)
				return;
			this.shieldAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[shieldValue].shieldSprite[1];
		}
		if (hairAvatar.activeInHierarchy)
		{
			if (hairValue == -1)
				return;
			this.hairAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[hairValue].hairSprite[1];
		}
	}

	[Command(requiresAuthority = false)]
	private void CmdSetSpriteRight()
	{
		if (isServer)
			RpcSetSpriteRight();
	}

	[ClientRpc]
	private void RpcSetSpriteRight()
	{
		if (playerClothes is null)
			playerClothes = GetComponent<PlayerClothes>();
		if (playerClothes is null)
			return;
		if (helmetAvatar.activeInHierarchy)
		{
			if (helmetValue == -1)
				return;
			this.helmetAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[helmetValue].helmetSprite[2];
		}
		if (torsoAvatar.activeInHierarchy)
		{
			if (torsoValue == -1)
				return;
			this.torsoAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[torsoValue].torsoSprite[2];
		}
		if (leftarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.leftarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].leftArmSprite[1];
		}
		if (rightarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.rightarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].rightArmSprite[1];
		}
		if (leftBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.leftBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].leftBootSprite[2];
		}
		if (rightBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.rightBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].rightBootSprite[2];
		}
		if (swordAvatar.activeInHierarchy)
		{
			if (swordValue == -1)
				return;
			this.swordAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[swordValue].swordSprite[2];
		}
		if (shieldAvatar.activeInHierarchy)
		{
			if (shieldValue == -1)
				return;
			this.shieldAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[shieldValue].shieldSprite[2];
		}
		if (hairAvatar.activeInHierarchy)
		{
			if (hairValue == -1)
				return;
			this.hairAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[hairValue].hairSprite[2];
		}
	}
	

	[Command(requiresAuthority = false)]
	private void CmdSetSpriteBack()
	{
		if (isServer)
			RpcSetSpriteBack();
	}

	[ClientRpc]
	private void RpcSetSpriteBack()
	{
		if (playerClothes is null)
			playerClothes = GetComponent<PlayerClothes>();
		if (playerClothes is null)
			return;
		if (helmetAvatar.activeInHierarchy)
		{
			if (helmetValue == -1)
				return;
			this.helmetAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[helmetValue].helmetSprite[3];
		}
		if (torsoAvatar.activeInHierarchy)
		{
			if (torsoValue == -1)
				return;
			this.torsoAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[torsoValue].torsoSprite[3];
		}
		if (leftarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.leftarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].leftArmSprite[2];
		}
		if (rightarmAvatar.activeInHierarchy)
		{
			if (armValue == -1)
				return;
			this.rightarmAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[armValue].rightArmSprite[2];
		}
		if (leftBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.leftBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].leftBootSprite[3];
		}
		if (rightBootAvatar.activeInHierarchy)
		{
			if (bootValue == -1)
				return;
			this.rightBootAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[bootValue].rightBootSprite[3];
		}
		if (swordAvatar.activeInHierarchy)
		{
			if (swordValue == -1)
				return;
			this.swordAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[swordValue].swordSprite[3];
		}
		if (shieldAvatar.activeInHierarchy)
		{
			if (shieldValue == -1)
				return;
			this.shieldAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[shieldValue].shieldSprite[3];
		}
		if (hairAvatar.activeInHierarchy)
		{
			if (hairValue == -1)
				return;
			this.hairAvatar.GetComponent<SpriteRenderer>().sprite = playerClothes.playerClothes[hairValue].hairSprite[3];
		}
	}



	
    public void SetPositionDead()
	{
		if (rightPos == 1)
		{
			rightPos = 1;
			netAnim.animator.SetBool("DeadRight", true);
			netAnim.animator.SetBool("IdleRight", false);
			CmdSetSpriteRight();
		}
		else if (rightPos == -1)
		{
			rightPos = -1;
			netAnim.animator.SetBool("DeadLeft", true);
			netAnim.animator.SetBool("IdleLeft", false);
			CmdSetSpriteRight();
		}
		else if (frontPos == -1)
		{
			frontPos = -1;
			netAnim.animator.SetBool("DeadFront", true);
			netAnim.animator.SetBool("IdleFront", false);
			CmdSetSpriteFront();
		}
		else if (frontPos == 1)
		{
			frontPos = 1;
			netAnim.animator.SetBool("DeadBack", true);
			netAnim.animator.SetBool("IdleBack", false);
			CmdSetSpriteBack();
		}
	}
	private void UpdateAnimation()
	{
		CheckMovePos();
		if (change != Vector3.zero)
		{
			netAnim.animator.SetBool("IdleLeft", false);
			netAnim.animator.SetBool("IdleFront", false);
			netAnim.animator.SetBool("IdleBack", false);
			netAnim.animator.SetBool("IdleRight", false);

			if (rightPos == 1)
			{
				rightPos = 1;
				netAnim.animator.SetBool("MoveRight", true);
				netAnim.animator.SetBool("MoveLeft", false);
				netAnim.animator.SetBool("MoveFront", false);
				netAnim.animator.SetBool("MoveBack", false);
				Movement();
					
			}
			else if (rightPos == -1)
			{
				rightPos = -1;
				netAnim.animator.SetBool("MoveLeft", true);
				netAnim.animator.SetBool("MoveFront", false);
				netAnim.animator.SetBool("MoveBack", false);
				netAnim.animator.SetBool("MoveRight", false);
				Movement();

			}
			else if (frontPos == -1)
			{
				frontPos = -1;
				netAnim.animator.SetBool("MoveFront", true);
				netAnim.animator.SetBool("MoveBack", false);
				netAnim.animator.SetBool("MoveRight", false);
				netAnim.animator.SetBool("MoveLeft", false);
				Movement();

			}
			else if (frontPos == 1)
			{
				frontPos = 1;
				netAnim.animator.SetBool("MoveBack", true);
				netAnim.animator.SetBool("MoveRight", false);
				netAnim.animator.SetBool("MoveLeft", false);
				netAnim.animator.SetBool("MoveFront", false);
				Movement();
			}
		}
		else
		{
			//netRigidbody2D.target.velocity = Vector3.zero;
			if (!PlayerCombat.CombatInstance.playerDied)
			{
				if (rightPos == 1)
				{
					rightPos = 1;
					CmdSetSpriteRight();
					netAnim.animator.SetBool("IdleRight", true);

					netAnim.animator.SetBool("MoveBack", false);
					netAnim.animator.SetBool("MoveFront", false);
					netAnim.animator.SetBool("MoveLeft", false);
					netAnim.animator.SetBool("MoveRight", false);
				}
				else if (rightPos == -1)
				{
					rightPos = -1;
					CmdSetSpriteLeft();
					netAnim.animator.SetBool("IdleLeft", true);

					netAnim.animator.SetBool("MoveBack", false);
					netAnim.animator.SetBool("MoveFront", false);
					netAnim.animator.SetBool("MoveLeft", false);
					netAnim.animator.SetBool("MoveRight", false);
				}
				else if (frontPos == -1)
				{
					frontPos = -1;
					CmdSetSpriteFront();
					netAnim.animator.SetBool("IdleFront", true);

					netAnim.animator.SetBool("MoveBack", false);
					netAnim.animator.SetBool("MoveFront", false);
					netAnim.animator.SetBool("MoveLeft", false);
					netAnim.animator.SetBool("MoveRight", false);
				}
				else if (frontPos == 1)
				{
					frontPos = 1;
					CmdSetSpriteBack();
					netAnim.animator.SetBool("IdleBack", true);

					netAnim.animator.SetBool("MoveBack", false);
					netAnim.animator.SetBool("MoveFront", false);
					netAnim.animator.SetBool("MoveLeft", false);
					netAnim.animator.SetBool("MoveRight", false);
				}
			}
		}

	}
	private void Movement()
	{
		if (!canMove)
		{

			netRigidbody2D.target.velocity = Vector3.zero;
			return;
		}

		change.Normalize();
		netRigidbody2D.target.MovePosition(transform.position + change * moveSpeed * Time.fixedDeltaTime);
		netRigidbody2D.target.velocity = Vector3.zero;

	}
	public bool pressRead = false;
	public void ActionButton()
	{
		if (canTalkNPCTeleport)
			NPCTeleport.instance.OpenTeleportChat();
		if (canTalkNPCCrafter)
			NPCCrafter.instance.TalkToNPCCrafter();
		if (canTalkNPCShop)
			NPCGeneralShop.instance.TalkToNPCShop();
		if (canReadSign)
		{
			pressRead = true;
		}
	}
	public void AllSignRead(string message)
	{
		pressRead = false;
		GameManager.GameManagerInstance.dialogBox.SetActive(true);
		GameManager.GameManagerInstance.DialogBox(message);
	}

}
