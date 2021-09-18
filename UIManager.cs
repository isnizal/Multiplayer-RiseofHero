
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
    private Character Player;
    public void InitializeUIVariable(Character player)
    {
        GetComponent<AchievementManager>().AchievementManagerLoad(player);
        GetComponent<InventoryInput>().LoadInventoryInput(player);
        Player = player;
        if (Player is null)
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
        PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();
        chatInput.onEndEdit.AddListener(playerMovement.SetMessageForPlayer);

    }
    public void InitializeAwake(Character player)
    {
        InitializeUIVariable(player);
        GetComponent<GameManager>().InitializeGameManagerVariable();
    }

    void Update()
    {
        
        if (Player is null)
            return;
        if (Player.isLocalPlayer)
        {
            if (LevelSystem.LevelInstance is null)
                return;
            UpdateHealth();
            UpdateMP();
            xpSlider.maxValue = LevelSystem.LevelInstance.toLevelUp[LevelSystem.LevelInstance.currentLevel];
            xpSlider.value = LevelSystem.LevelInstance.currentExp;
            xpValue.text = "" + LevelSystem.LevelInstance.currentExp + "/" + LevelSystem.LevelInstance.expToLevel;

            levelValue.text = "" + LevelSystem.LevelInstance.currentLevel;

            premiumValue.text = "" + Character.MyInstance.premiumCurrency;
            copperValue.text = "" + Character.MyInstance.copperCurrency;
        }
        if (chatInput is null)
            return;
        if (chatInput.isFocused)
        {
            //disable player keyboard if on desktop
            Player.GetComponent<Character>().onInput = true;
        }
        else
            Player.GetComponent<Character>().onInput = false;
    }
    public void UpdateHealth()
    {
        if (Character.MyInstance is null)
            return;
        hpSlider.maxValue = Character.MyInstance.MaxHealth;
        hpSlider.value = Character.MyInstance.Health;
        hpValue.text = "" + Character.MyInstance.Health + "/" + Character.MyInstance.MaxHealth;
    }

    public void UpdateMP()
	{
        if (Character.MyInstance is null)
            return;
        mpSlider.maxValue = Character.MyInstance.MaxMP;
        mpSlider.value = Character.MyInstance.Mana;
        mpValue.text = "" + Character.MyInstance.Mana + "/" + Character.MyInstance.MaxMP;
	}
}
