
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [Header("---> Level Values <---")]
    [FormerlySerializedAs("levelText")]
    public TextMeshProUGUI levelValue;
    [Space]
    [Header("---> Currency Values <---")]
    [FormerlySerializedAs("premiumCurrencyValue")]
    public TextMeshProUGUI premiumValue;
    [FormerlySerializedAs("copperCurrencyValue")]
    public TextMeshProUGUI copperValue;
    [Space]
    [Header("---> HP Values <---")]
    [FormerlySerializedAs("hpBar")]
    public Slider hpSlider;
    [FormerlySerializedAs("hpText")]
    public TextMeshProUGUI hpValue;
    [Space]
    [Header("---> MP Values <---")]
    [FormerlySerializedAs("mpBar")]
    public Slider mpSlider;
    [FormerlySerializedAs("mpText")]
    public TextMeshProUGUI mpValue;
    [Space]
    [Header("---> XP Values <---")]
    [FormerlySerializedAs("xpBar")]
    public Slider xpSlider;
    [FormerlySerializedAs("xpText")]
    public TextMeshProUGUI xpValue;

    private static UIManager instance;
    private TMP_InputField chatInput;
    [HideInInspector]public GameManager _gameManager;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }

    }
    private Character _character;
    public void InitializeUIVariable(Character player)
    {
        if (player.isLocalPlayer)
        {
            GetComponent<AchievementManager>().AchievementManagerLoad(player);
            GetComponent<InventoryInput>().LoadInventoryInput(player);
            _character = player;
            levelValue = GameObject.Find("LevelValue").GetComponent<TextMeshProUGUI>();
            premiumValue = GameObject.Find("PremiumValue").GetComponent<TextMeshProUGUI>();
            copperValue = GameObject.Find("CopperValue").GetComponent<TextMeshProUGUI>();
            hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
            hpValue = GameObject.Find("HPValue").GetComponent<TextMeshProUGUI>();
            mpSlider = GameObject.Find("MPSlider").GetComponent<Slider>();
            mpValue = GameObject.Find("MPValue").GetComponent<TextMeshProUGUI>();
            xpSlider = GameObject.Find("XPSlider").GetComponent<Slider>();
            xpValue = GameObject.Find("XPValue").GetComponent<TextMeshProUGUI>();
            chatInput = GameObject.Find("ChatText").GetComponentInChildren<TMP_InputField>();
            PlayerMovement playerMovement = _character.gameObject.GetComponent<PlayerMovement>();
            chatInput.onEndEdit.AddListener(playerMovement.SetMessageForPlayer);
        }
    }
    public void InitializeAwake(Character player)
    {
        InitializeUIVariable(player);
        _gameManager = GetComponent<GameManager>();
        _gameManager.InitializeGameManagerVariable(player, this);
    }

    void Update()
    {
        
        if (_character == null)
            return;
        if (_character.hasAuthority)
        {
            UpdateHealth();
            UpdateMP();
            xpSlider.maxValue = _character.gameObject.GetComponent<LevelSystem>().toLevelUp[_character.gameObject.GetComponent<LevelSystem>().currentLevel];
            xpSlider.value = _character.gameObject.GetComponent<LevelSystem>().currentExp;
            xpValue.text = "" + _character.gameObject.GetComponent<LevelSystem>().currentExp + "/" + _character.gameObject.GetComponent<LevelSystem>().expToLevel;

            levelValue.text = "" + _character.GetComponent<LevelSystem>().currentLevel;

            premiumValue.text = "" + _character.premiumCurrency;
            copperValue.text = "" + _character.copperCurrency;
        }
        if (chatInput == null)
            return;
        if (chatInput.isFocused)
        {
            //disable player keyboard if on desktop
            _character.onInput = true;
        }
        else
            _character.onInput = false;
    }
    public void UpdateHealth()
    {

        hpSlider.maxValue = _character.MaxHealth;
        hpSlider.value = _character.Health;
        hpValue.text = "" + _character.Health + "/" + _character.MaxHealth;
    }

    public void UpdateMP()
	{
        mpSlider.maxValue = _character.MaxMP;
        mpSlider.value = _character.Mana;
        mpValue.text = "" + _character.Mana + "/" + _character.MaxMP;
	}
}
