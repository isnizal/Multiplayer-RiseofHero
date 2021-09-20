using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using EasyUI.Toast;

public class StatsModifier : MonoBehaviour
{
    public static StatsModifier instance;
    public static StatsModifier StatsInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StatsModifier>();
            }
            return instance;
        }

    }

    [Header("Stat Points Manager")]
    public TextMeshProUGUI statPointsValue;

    [Header("Base Stats")]
    public int currentStr;
    public int currentDef;
    public int currentInt;
    public int currentVit;
    [Space]
    [HideInInspector] public int currentStrValue;
    [HideInInspector] public int currentDefValue;
    [HideInInspector] public int currentIntValue;
    [HideInInspector] public int currentVitValue;

    [Space]
    [Header("Stats Text Areas")]
    public TextMeshProUGUI currentStrAmountValueText;
    public TextMeshProUGUI addStrAmountValueText;
    private int tempStrPointHolder;

    public TextMeshProUGUI currentDefAmountValueText;
    public TextMeshProUGUI addDefAmountValueText;
    private int tempDefPointHolder;

    public TextMeshProUGUI currentIntAmountValueText;
    public TextMeshProUGUI addIntAmountValueText;
    private int tempIntPointHolder;

    public TextMeshProUGUI currentVitAmountValueText;
    public TextMeshProUGUI addVitAmountValueText;
    private int tempVitPointHolder;

    public bool canUpgrade;
    private Character _character;

    public void InitializeStatModifier(Character character)
    {
        _character = character;
    }
    private void Update()
	{
        if (_character is null)
            return;

         if (tempStrPointHolder > 0 || tempDefPointHolder > 0 || tempIntPointHolder > 0 || tempVitPointHolder > 0)
         {
             canUpgrade = true;
         }
         else if (tempStrPointHolder == 0 || tempDefPointHolder == 0 || tempIntPointHolder == 0 || tempVitPointHolder == 0)
         {
             canUpgrade = false;
         }


         _character.Strength.BaseValue = currentStrValue;
         _character.Defense.BaseValue = currentDefValue;
         _character.Intelligence.BaseValue = currentIntValue;
         _character.Vitality.BaseValue = currentVitValue;
	}


	void FixedUpdate()
    {
        if (_character is null)
            return;

        statPointsValue.text = _character.statPoints.ToString();

        currentStrAmountValueText.text = "" + _character.Strength.BaseValue.ToString();
        addStrAmountValueText.text = "" + tempStrPointHolder;
        currentStr = (int)_character.Strength.Value;

        currentDefAmountValueText.text = "" + _character.Defense.BaseValue.ToString();
        addDefAmountValueText.text = "" + tempDefPointHolder;
        currentDef = (int)_character.Defense.Value;

        currentIntAmountValueText.text = "" + _character.Intelligence.BaseValue.ToString();
        addIntAmountValueText.text = "" + tempIntPointHolder;
        currentInt = (int)_character.Intelligence.Value;

        currentVitAmountValueText.text = "" + _character.Vitality.BaseValue.ToString();
        addVitAmountValueText.text = "" + tempVitPointHolder;
        currentVit = (int)_character.Vitality.Value;
    }

    public void AddStrStat()
    {
        if (_character.statPoints > 0)
        {
            tempStrPointHolder++;
            _character.statPoints--;
        }
        else if (_character.statPoints == 0)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveStrStat()
    {
        if (tempStrPointHolder > 0)
        {
            tempStrPointHolder--;
            _character.statPoints++;
        }
        else if (tempStrPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void AddDefStat()
    {
        if (_character.statPoints > 0)
        {
            tempDefPointHolder++;
            _character.statPoints--;
        }
        else if (_character.statPoints < 1)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveDefStat()
    {
        if (tempDefPointHolder > 0)
        {
            tempDefPointHolder--;
            _character.statPoints++;
        }
        else if (tempDefPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void AddIntStat()
    {
        if (_character.statPoints > 0)
        {
            tempIntPointHolder++;
            _character.statPoints--;
        }
        else if (_character.statPoints < 1)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveIntStat()
    {
        if (tempIntPointHolder > 0)
        {
            tempIntPointHolder--;
            _character.statPoints++;
        }
        else if (tempIntPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void AddVitStat()
    {
        if (_character.statPoints > 0)
        {
            tempVitPointHolder++;
            _character.statPoints--;
        }
        else if (_character.statPoints < 1)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveVitStat()
    {
        if (tempVitPointHolder > 0)
        {
            tempVitPointHolder--;
            _character.statPoints++;
        }
        else if (tempVitPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void ConfirmSelection()
    {
        if (canUpgrade)
		{

			_character.Strength.BaseValue += tempStrPointHolder;
            currentStrValue += tempStrPointHolder;
            _character.statPointsAllocated += tempStrPointHolder;
            tempStrPointHolder = 0;

            _character.Defense.BaseValue += tempDefPointHolder;
            currentDefValue += tempDefPointHolder;
            _character.statPointsAllocated += tempDefPointHolder;
            tempDefPointHolder = 0;

            _character.Intelligence.BaseValue += tempIntPointHolder;
            currentIntValue += tempIntPointHolder;
            _character.statPointsAllocated += tempIntPointHolder;
            tempIntPointHolder = 0;

            _character.Vitality.BaseValue += tempVitPointHolder;
            currentVitValue += tempVitPointHolder;
            var healthIncrease = tempVitPointHolder;
            var newHealth = healthIncrease * 2;
            _character.statPointsAllocated += tempVitPointHolder;
            tempVitPointHolder = 0;

            _character.MaxHealth += newHealth;
        }
        else if(!canUpgrade)
		{
            Toast.Show("You haven't placed any stat points", 2f, ToastPosition.MiddleCenter);
		}
    }
}
