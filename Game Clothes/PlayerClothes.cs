using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerClothes : MonoBehaviour, ISerializationCallbackReceiver
{
    [Serializable]
    public class Clothes
    {
        public Sprite[] helmetSprite = new Sprite[4];
        public Sprite[] torsoSprite = new Sprite[4];
        public Sprite[] leftArmSprite = new Sprite[3];
        public Sprite[] rightArmSprite = new Sprite[3];
        public Sprite[] leftBootSprite = new Sprite[4];
        public Sprite[] rightBootSprite = new Sprite[4];
        public Sprite[] swordSprite = new Sprite[4];
        public Sprite[] shieldSprite = new Sprite[4];
        public Sprite[] hairSprite = new Sprite[4];

    }

    public Clothes[] playerClothes = new Clothes[2];


    public void OnBeforeSerialize()
    {
        for (int i = 0; i < playerClothes.Length; i++)
        {
            for (int b = 0; b < playerClothes[i].helmetSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes"+i+"/Helmet/Hat" + b);
                playerClothes[i].helmetSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].torsoSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes"+i+ "/Torso/Body_clothes" + b);
                playerClothes[i].torsoSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].leftArmSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Arm/Left_Arm_clothes" + b);
                playerClothes[i].leftArmSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].rightArmSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Arm/Right_Arm_clothes" + b);
                playerClothes[i].rightArmSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].leftBootSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Boot/Left_Shoes" + b);
                playerClothes[i].leftBootSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].rightBootSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Boot/Right_Shoes" + b);
                playerClothes[i].rightBootSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].swordSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Sword/Sword" + b);
                playerClothes[i].swordSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].shieldSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Shield/Shiled" + b);
                playerClothes[i].shieldSprite[b] = clothes1;
            }
            for (int b = 0; b < playerClothes[i].shieldSprite.Length; b++)
            {
                Sprite clothes1 = Resources.Load<Sprite>("Clothes/Clothes" + i + "/Hair/HairMustacheBeard" + b);
                playerClothes[i].hairSprite[b] = clothes1;
            }
        }

    }

    public void OnAfterDeserialize()
    {
        
    }


}
