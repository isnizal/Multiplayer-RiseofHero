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

	private void Update()
	{
        if (Character.MyInstance is null)
            return;

         if (tempStrPointHolder > 0 || tempDefPointHolder > 0 || tempIntPointHolder > 0 || tempVitPointHolder > 0)
         {
             canUpgrade = true;
         }
         else if (tempStrPointHolder == 0 || tempDefPointHolder == 0 || tempIntPointHolder == 0 || tempVitPointHolder == 0)
         {
             canUpgrade = false;
         }


         Character.MyInstance.Strength.BaseValue = currentStrValue;
         Character.MyInstance.Defense.BaseValue = currentDefValue;
         Character.MyInstance.Intelligence.BaseValue = currentIntValue;
         Character.MyInstance.Vitality.BaseValue = currentVitValue;
	}


	void FixedUpdate()
    {
        if (Character.MyInstance is null)
            return;

        statPointsValue.text = Character.MyInstance.statPoints.ToString();

        currentStrAmountValueText.text = "" + Character.MyInstance.Strength.BaseValue.ToString();
        addStrAmountValueText.text = "" + tempStrPointHolder;
        currentStr = (int)Character.MyInstance.Strength.Value;

        currentDefAmountValueText.text = "" + Character.MyInstance.Defense.BaseValue.ToString();
        addDefAmountValueText.text = "" + tempDefPointHolder;
        currentDef = (int)Character.MyInstance.Defense.Value;

        currentIntAmountValueText.text = "" + Character.MyInstance.Intelligence.BaseValue.ToString();
        addIntAmountValueText.text = "" + tempIntPointHolder;
        currentInt = (int)Character.MyInstance.Intelligence.Value;

        currentVitAmountValueText.text = "" + Character.MyInstance.Vitality.BaseValue.ToString();
        addVitAmountValueText.text = "" + tempVitPointHolder;
        currentVit = (int)Character.MyInstance.Vitality.Value;
    }

    public void AddStrStat(Character c)
    {
        if (c.statPoints > 0)
        {
            tempStrPointHolder++;
            c.statPoints--;
        }
        else if (c.statPoints == 0)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveStrStat(Character c)
    {
        if (tempStrPointHolder > 0)
        {
            tempStrPointHolder--;
            c.statPoints++;
        }
        else if (tempStrPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void AddDefStat(Character c)
    {
        if (c.statPoints > 0)
        {
            tempDefPointHolder++;
            c.statPoints--;
        }
        else if (c.statPoints < 1)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveDefStat(Character c)
    {
        if (tempDefPointHolder > 0)
        {
            tempDefPointHolder--;
            c.statPoints++;
        }
        else if (tempDefPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void AddIntStat(Character c)
    {
        if (c.statPoints > 0)
        {
            tempIntPointHolder++;
            c.statPoints--;
        }
        else if (c.statPoints < 1)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveIntStat(Character c)
    {
        if (tempIntPointHolder > 0)
        {
            tempIntPointHolder--;
            c.statPoints++;
        }
        else if (tempIntPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void AddVitStat(Character c)
    {
        if (c.statPoints > 0)
        {
            tempVitPointHolder++;
            c.statPoints--;
        }
        else if (c.statPoints < 1)
        {
            print("You dont have enough stat points");
            Toast.Show("You don't have enough stat points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void RemoveVitStat(Character c)
    {
        if (tempVitPointHolder > 0)
        {
            tempVitPointHolder--;
            c.statPoints++;
        }
        else if (tempVitPointHolder < 1)
        {
            print("You cant remove any more points");
            Toast.Show("You can't remove any more points!", 2f, ToastPosition.MiddleCenter);
        }
    }

    public void ConfirmSelection(Character c)
    {
		if (canUpgrade)
		{

			c.Strength.BaseValue += tempStrPointHolder;
            currentStrValue += tempStrPointHolder;
            c.statPointsAllocated += tempStrPointHolder;
            tempStrPointHolder = 0;

            c.Defense.BaseValue += tempDefPointHolder;
            currentDefValue += tempDefPointHolder;
            c.statPointsAllocated += tempDefPointHolder;
            tempDefPointHolder = 0;

            c.Intelligence.BaseValue += tempIntPointHolder;
            currentIntValue += tempIntPointHolder;
            c.statPointsAllocated += tempIntPointHolder;
            tempIntPointHolder = 0;

            c.Vitality.BaseValue += tempVitPointHolder;
            currentVitValue += tempVitPointHolder;
            var healthIncrease = tempVitPointHolder;
            var newHealth = healthIncrease * 2;
            c.statPointsAllocated += tempVitPointHolder;
            tempVitPointHolder = 0;

            Character.MyInstance.MaxHealth += newHealth;
        }
        else if(!canUpgrade)
		{
            Toast.Show("You haven't placed any stat points", 2f, ToastPosition.MiddleCenter);
		}
    }
}
