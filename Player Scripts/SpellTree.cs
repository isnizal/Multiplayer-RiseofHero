using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyUI.Toast;
using Mirror;

public class SpellTree : MonoBehaviour
{
    private static SpellTree instance;
    public static SpellTree SpellInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellTree>();
            }
            return instance;
        }

    }

    [Header("---> Spell Points <---")]
    public int spellPointsAvailable;
    public int spellPointsAllocated;
    [SerializeField] TextMeshProUGUI spellPointsValue;
    [Space]

    [Header("---> Fireball Spell <---")]
    public GameObject fireballHotbarSlot;
    public GameObject fireball1SkillSlot;
    public GameObject fireball2SkillSlot;
    public int fireball1LevelReq;
    public int fireball2LevelReq;
    public int fireball1Level;
    public int fireball2Level;
    [SerializeField] TextMeshProUGUI fireball1Value;
    [SerializeField] TextMeshProUGUI fireball2Value;
    public int fireball1LevelMax;
    public int fireball2LevelMax;
    public Image fireballSpellImage;
    [SerializeField] private Sprite fireball1Image;
    [SerializeField] private Sprite fireball2Image;
    [SerializeField] private Sprite fireball3Image;
    [Space]
    [Header("---> Icicle Spell <---")]
    public GameObject icicleBallHotbarSlot;
    public GameObject icicle1SkillSlot;
    public GameObject icicle2SkillSlot;
    public int icicle1LevelReq;
    public int icicle2LevelReq;
    public int icicle1Level;
    public int icicle2Level;
    [SerializeField] TextMeshProUGUI icicle1Value;
    [SerializeField] TextMeshProUGUI icicle2Value;
    public int icicle1LevelMax;
    public int icicle2LevelMax;
    public Image icicleSpellImage;
    [SerializeField] private Sprite icicle1Image;
    [SerializeField] private Sprite icicle2Image;
    [SerializeField] private Sprite icicle3Image;
    [Space]
    [Header("---> Arctic Blast <---")]
    public GameObject arcticBlastHotBarSlot;
    public GameObject arcticBlast1SkillSlot;
    public GameObject arcticBlast2SkillSlot;
    public int arcticBlast1LevelReq;
    public int arcticBlast2LevelReq;
    public int arcticBlast1Level;
    public int arcticBlast2Level;
    [SerializeField] TextMeshProUGUI arcticBlast1Value;
    [SerializeField] TextMeshProUGUI arcticBlast2Value;
    public int arcticBlast1LevelMax;
    public int arcticBlast2LevelMax;
    public Image arcticBlastSpellImage;
    [SerializeField] private Sprite arcticBlast1Image;
    [SerializeField] private Sprite arcticBlast2Image;
    [SerializeField] private Sprite arcticBlast3Image;

    private Character _character;
    private PlayerCombat _playerCombat;
    private LevelSystem _levelSystem;
    public void InitializeSpell(Character character)
    {
        _character = character;
        _playerCombat = _character.gameObject.GetComponent<PlayerCombat>();
        _levelSystem = _character.gameObject.GetComponent<LevelSystem>();


        fireballHotbarSlot.transform.parent.gameObject.SetActive(true);
        fireballHotbarSlot.GetComponent<Image>().sprite = fireball1Image;
        fireballHotbarSlot.GetComponent<Button>().onClick.AddListener(_playerCombat.ActivateFireball);
        fireballHotbarSlot.transform.parent.gameObject.SetActive(false);

        icicleBallHotbarSlot.transform.parent.gameObject.SetActive(true);
        icicleBallHotbarSlot.GetComponent<Image>().sprite = icicle1Image;
        icicleBallHotbarSlot.GetComponent<Button>().onClick.AddListener(_playerCombat.ActivateIcicle);
        icicleBallHotbarSlot.transform.parent.gameObject.SetActive(false);

        arcticBlastHotBarSlot.transform.parent.gameObject.SetActive(true);
        arcticBlastHotBarSlot.GetComponent<Image>().sprite = arcticBlast1Image;
        arcticBlastHotBarSlot.GetComponent<Button>().onClick.AddListener(_playerCombat.ActivateArcticBlast);
        arcticBlastHotBarSlot.transform.parent.gameObject.SetActive(false);
    }
	void Update()
    {
        if (_character == null)
            return;
        if (!_character.isLocalPlayer)
            return;
        if (!NetworkClient.active)
            return;
        spellPointsValue.text = "" + spellPointsAvailable;

        if (fireball1Level != fireball1LevelMax)
            fireball1Value.text = "" + fireball1Level + "/" + fireball1LevelMax;
        else if (fireball1Level == fireball1LevelMax)
            fireball1Value.text = "Max";

        if (fireball2Level != fireball2LevelMax)
            fireball2Value.text = "" + fireball2Level + "/" + fireball2LevelMax;
        else if (fireball2Level == fireball2LevelMax)
            fireball2Value.text = "Max";

        if (icicle1Level != icicle1LevelMax)
            icicle1Value.text = "" + icicle1Level + "/" + icicle1LevelMax;
        else if (icicle1Level == icicle1LevelMax)
            icicle1Value.text = "Max";

        if (icicle2Level != icicle2LevelMax)
            icicle2Value.text = "" + icicle2Level + "/" + icicle2LevelMax;
        else if (icicle2Level == icicle2LevelMax)
            icicle2Value.text = "Max";

        if (arcticBlast1Level != arcticBlast1LevelMax)
            arcticBlast1Value.text = "" + arcticBlast1Level + "/" + arcticBlast1LevelMax;
        else if (arcticBlast1Level == arcticBlast1LevelMax)
            arcticBlast1Value.text = "Max";

        if (arcticBlast2Level != arcticBlast2LevelMax)
            arcticBlast2Value.text = "" + arcticBlast2Level + "/" + arcticBlast2LevelMax;
        else if (arcticBlast2Level == arcticBlast2LevelMax)
            arcticBlast2Value.text = "Max";

        SpellHotbar();
        LevelSpellCheck();
        CheckSpellButton();
    }

    [Client]
    private void SpellHotbar()
	{
        if(fireball1Level >= 1)
		{
            _playerCombat.canCastSpells = true;
            fireballHotbarSlot.transform.parent.gameObject.SetActive(true);
            fireballSpellImage.sprite = fireball1Image;
            if(!_playerCombat.fireballActive)
                fireballSpellImage.color = new Color(1, 1, 1, .5f);
		}
        if(fireball2Level >= 1)
		{
            _playerCombat.canCastSpells = true;
            fireballSpellImage.sprite = fireball2Image;
            if (!_playerCombat.fireballActive)
                fireballSpellImage.color = new Color(1, 1, 1, .5f);
		}
        if(icicle1Level >= 1)
		{
            _playerCombat.canCastSpells = true;
            icicleBallHotbarSlot.transform.parent.gameObject.SetActive(true);
            icicleSpellImage.sprite = icicle1Image;
            if(!_playerCombat.icicleActive)
                icicleSpellImage.color = new Color(1, 1, 1, .5f);
		}
        if(icicle2Level >= 1)
		{
            _playerCombat.canCastSpells = true;
            icicleSpellImage.sprite = icicle2Image;
            if (!_playerCombat.icicleActive)
                icicleSpellImage.color = new Color(1, 1, 1, .5f);
        }
        if(arcticBlast1Level >= 1)
		{
            _playerCombat.canCastSpells = true;
            arcticBlastHotBarSlot.transform.parent.gameObject.SetActive(true);
            arcticBlastSpellImage.sprite = arcticBlast1Image;
            if (!_playerCombat.arcticBlastActive)
                arcticBlastSpellImage.color = new Color(1, 1, 1, .5f);
		}
        if(arcticBlast2Level >= 1)
		{
            _playerCombat.canCastSpells = true;
            arcticBlastSpellImage.sprite = arcticBlast2Image;
            if (!_playerCombat.arcticBlastActive)
                arcticBlastSpellImage.color = new Color(1, 1, 1, .5f);
        }
	}

    public void FireballSpell()
	{
        if (fireball1Level != fireball1LevelMax && spellPointsAvailable > 0)
        {
            fireball1Level++;
            spellPointsAvailable--;
            spellPointsAllocated++;
        }
        if(fireball1Level == fireball1LevelMax && fireball2Level != fireball2LevelMax && spellPointsAvailable > 0)
		{
            fireball2Level++;
            spellPointsAvailable--;
            spellPointsAllocated++;
		}
		else if(fireball1Level == fireball1LevelMax)
		{
            print("This has been maxxed out!");
            Toast.Show("This spell is at max!", 2f, ToastPosition.MiddleCenter);
        }
        else if(fireball2Level == fireball2LevelMax)
        {
            print("This has been maxxed out!");
            Toast.Show("This spell is at max!", 2f, ToastPosition.MiddleCenter);
        }
        else if (spellPointsAvailable == 0)
        {
            Toast.Show("You don't have enough spell points!", 2f, ToastPosition.MiddleCenter);
            print("You dont have any spell points");
        }
    }

    public void IcicleSpell()
	{
        if(icicle1Level != icicle1LevelMax && spellPointsAvailable > 0)
		{
            icicle1Level++;
            spellPointsAvailable--;
            spellPointsAllocated++;
		}
        if (icicle1Level == icicle1LevelMax && icicle2Level != icicle2LevelMax && spellPointsAvailable > 0)
        {
            icicle2Level++;
            spellPointsAvailable--;
            spellPointsAllocated++;
        }
        else if (spellPointsAvailable == 0)
        {
            Toast.Show("You don't have enough spell points!", 2f, ToastPosition.MiddleCenter);
            print("You dont have any spell points");
        }
        else if(icicle1Level == icicle1LevelMax)
		{
            Toast.Show("This spell is at max!", 2f, ToastPosition.MiddleCenter);
            print("This has been maxxed out!");
		}
        else if(icicle2Level == icicle2LevelMax)
		{
            Toast.Show("This spell is at max!", 2f, ToastPosition.MiddleCenter);
            print("This has been maxxed out!");
        }
	}

    public void ArcticBlastSpell()
	{
        if(arcticBlast1Level != arcticBlast1LevelMax && spellPointsAvailable > 0)
		{
            arcticBlast1Level++;
            spellPointsAvailable--;
            spellPointsAllocated++;
		}
        if (arcticBlast1Level == arcticBlast1LevelMax && arcticBlast2Level != arcticBlast2LevelMax && spellPointsAvailable > 0)
        {
            arcticBlast2Level++;
            spellPointsAvailable--;
            spellPointsAllocated++;
        }
        else if (spellPointsAvailable == 0)
        {
            Toast.Show("You don't have enough spell points!", 2f, ToastPosition.MiddleCenter);
            print("You dont have any spell points");
        }
        else if(arcticBlast1Level == arcticBlast1LevelMax)
		{
            Toast.Show("This spell is at max!", 2f, ToastPosition.MiddleCenter);
            print("This has been maxxed out!");
		}
        else if (arcticBlast2Level == arcticBlast2LevelMax)
        {
            Toast.Show("This spell is at max!", 2f, ToastPosition.MiddleCenter);
            print("This has been maxxed out!");
        }
    }

    void LevelSpellCheck()
	{
        if(fireball1LevelReq <= _levelSystem.currentLevel)
		{
            fireball1SkillSlot.SetActive(true);
        }
        if(fireball1Level == fireball1LevelMax && fireball2LevelReq <= _levelSystem.currentLevel)
		{
            fireball2SkillSlot.SetActive(true);
		}
        if(icicle1LevelReq <= _levelSystem.currentLevel)
		{
            icicle1SkillSlot.SetActive(true);
		}
        if(icicle1Level == icicle1LevelMax && icicle2LevelReq <= _levelSystem.currentLevel)
		{
            icicle2SkillSlot.SetActive(true);
		}
        if (arcticBlast1LevelReq <= _levelSystem.currentLevel)
		{
            arcticBlast1SkillSlot.SetActive(true);
		}
        if(arcticBlast1Level == arcticBlast1LevelMax && arcticBlast2LevelReq <= _levelSystem.currentLevel)
		{
            arcticBlast2SkillSlot.SetActive(true);
		}
	}

    public void CheckSpellButton()
	{
        if (fireball1Level == fireball1LevelMax)
        {
            fireball1SkillSlot.GetComponentInChildren<Button>().interactable = false;
        }
        if (fireball2Level == fireball2LevelMax)
        {
            fireball2SkillSlot.GetComponentInChildren<Button>().interactable = false;
        }
        if (icicle1Level == icicle1LevelMax)
        {
            icicle1SkillSlot.GetComponentInChildren<Button>().interactable = false;
        }
        if (icicle2Level == icicle2LevelMax)
        {
            icicle2SkillSlot.GetComponentInChildren<Button>().interactable = false;
        }
        if (arcticBlast1Level == arcticBlast1LevelMax)
        {
            arcticBlast1SkillSlot.GetComponentInChildren<Button>().interactable = false;
        }
        if (arcticBlast2Level == arcticBlast2LevelMax)
        {
            arcticBlast2SkillSlot.GetComponentInChildren<Button>().interactable = false;
        }
    }
}
