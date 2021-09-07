
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.Serialization;

public class UIManager : NetworkBehaviour
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
    private void InitializeVariable()
    {
        levelValue = GameObject.Find("LevelValue").GetComponent<TextMeshProUGUI>();
        premiumValue = GameObject.Find("PremiumValue").GetComponent<TextMeshProUGUI>();
        copperValue = GameObject.Find("CopperValue").GetComponent<TextMeshProUGUI>();
        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpValue = GameObject.Find("HPValue").GetComponent<TextMeshProUGUI>();
        mpSlider = GameObject.Find("MPSlider").GetComponent<Slider>();
        mpValue = GameObject.Find("MPValue").GetComponent<TextMeshProUGUI>();
        xpSlider = GameObject.Find("XPSlider").GetComponent<Slider>();
        xpValue = GameObject.Find("XPValue").GetComponent<TextMeshProUGUI>();
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("local authority");
        base.OnStartLocalPlayer();
        InitializeVariable();
    }
    
    void Update()
    {
        if (hasAuthority)
        {
            UpdateHealth();
            UpdateMP();
            xpSlider.maxValue = LevelSystem.LevelInstance.toLevelUp[LevelSystem.LevelInstance.currentLevel];
            xpSlider.value = LevelSystem.LevelInstance.currentExp;
            xpValue.text = "" + LevelSystem.LevelInstance.currentExp + "/" + LevelSystem.LevelInstance.expToLevel;

            levelValue.text = "" + LevelSystem.LevelInstance.currentLevel;

            premiumValue.text = "" + Character.MyInstance.premiumCurrency;
            copperValue.text = "" + Character.MyInstance.copperCurrency;
        }
    }
    public void UpdateHealth()
    {
        hpSlider.maxValue = Character.MyInstance.MaxHealth;
        hpSlider.value = Character.MyInstance.Health;
        hpValue.text = "" + Character.MyInstance.Health + "/" + Character.MyInstance.MaxHealth;
    }

    public void UpdateMP()
	{
        mpSlider.maxValue = Character.MyInstance.MaxMP;
        mpSlider.value = Character.MyInstance.Mana;
        mpValue.text = "" + Character.MyInstance.Mana + "/" + Character.MyInstance.MaxMP;
	}
}
