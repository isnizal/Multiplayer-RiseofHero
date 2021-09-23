
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
    private Character _Character;
    public void InitializeUIVariable(Character player)
    {
        if (player.isLocalPlayer)
        {
            GetComponent<AchievementManager>().AchievementManagerLoad(player);
            GetComponent<InventoryInput>().LoadInventoryInput(player);
            _Character = player;
            if (_Character is null)
                return;
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
            PlayerMovement playerMovement = _Character.gameObject.GetComponent<PlayerMovement>();
            chatInput.onEndEdit.AddListener(playerMovement.SetMessageForPlayer);
        }
    }
    public void InitializeAwake(Character player)
    {
        InitializeUIVariable(player);
        GetComponent<GameManager>().InitializeGameManagerVariable(player,this);

    }

    void Update()
    {
        
        if (_Character is null)
            return;
        if (_Character.isLocalPlayer)
        {
            if (hpSlider is null)
                return;
            UpdateHealth();
            UpdateMP();
            xpSlider.maxValue = _Character.gameObject.GetComponent<LevelSystem>().toLevelUp[_Character.gameObject.GetComponent<LevelSystem>().currentLevel];
            xpSlider.value = _Character.gameObject.GetComponent<LevelSystem>().currentExp;
            xpValue.text = "" + _Character.gameObject.GetComponent<LevelSystem>().currentExp + "/" + _Character.gameObject.GetComponent<LevelSystem>().expToLevel;

            levelValue.text = "" + _Character.GetComponent<LevelSystem>().currentLevel;

            premiumValue.text = "" + _Character.premiumCurrency;
            copperValue.text = "" + _Character.copperCurrency;
        }
        if (chatInput is null)
            return;
        if (chatInput.isFocused)
        {
            //disable player keyboard if on desktop
            _Character.onInput = true;
        }
        else
            _Character.onInput = false;
    }
    public void UpdateHealth()
    {
        hpSlider.maxValue = _Character.MaxHealth;
        hpSlider.value = _Character.Health;
        hpValue.text = "" + _Character.Health + "/" + _Character.MaxHealth;
    }

    public void UpdateMP()
	{
        mpSlider.maxValue = _Character.MaxMP;
        mpSlider.value = _Character.Mana;
        mpValue.text = "" + _Character.Mana + "/" + _Character.MaxMP;
	}
}
