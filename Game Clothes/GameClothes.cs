using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameClothes : MonoBehaviour
{

    public int helmetValue;
    public int torsoValue;
    public int armValue;
    public int swordValue;
    public int shieldValue;
    public int bootValue;
    public int hairValue;

    private PlayerMovement playerMovement;

    //load on player load game to load clothe value
    /*
     * for network this will be disable
     * */
    public void LoadClothesOnStart()
    {
        
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement is null)
            return;
        playerMovement.SetClothesValue();
        playerMovement.CheckValueClothes();
        playerMovement.Initialize();
        
    }
    public void CurrentPlayerClothes(int value2)
    {
        torsoValue = value2;
    }
    public void CurrentPlayerHelmet(int value2)
    {
        helmetValue = value2;
    }
    public void CurrentPlayerArm(int value3)
    {
        armValue = value3;
    }

    public void CurrentPlayerBoot(int value3)
    {
        bootValue = value3;
    }
    public void CurrentPlayerSword( int value2)
    {
        swordValue = value2;
    }
    public void CurrentPlayerShield( int value2)
    {
        shieldValue = value2;
    }

    public void CurrentPlayerHair(int value)
    {
        hairValue = value;
    }
}
