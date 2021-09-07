using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoad : MonoBehaviour
{
    public SpriteRenderer helmetSprite;
    public SpriteRenderer torsoSprite;
    public SpriteRenderer leftArmSprite;
    public SpriteRenderer rightArmSprite;
    public SpriteRenderer leftBootSprite;
    public SpriteRenderer rightBootSprite;
    public SpriteRenderer hairSprite;
    public SpriteRenderer swordSprite;
    public SpriteRenderer shieldSprite;
    [SerializeField] private PlayerClothes playerClothes;
    [SerializeField] private GameObject ERROR_NOFOUND_CHARACTER;


    public void LoadCharacter()
    {
        int[] loadedStats = SaveLoadManager.LoadPlayer();
        if (loadedStats == null)
            ERROR_NOFOUND_CHARACTER.SetActive(true);
        else
        {
            if (loadedStats[21] != -1)
            {
                helmetSprite.sprite = playerClothes.playerClothes[loadedStats[21]].helmetSprite[0];
                helmetSprite.gameObject.SetActive(true);
            }
            else
                helmetSprite.gameObject.SetActive(false);
            if (loadedStats[22] != -1)
            {
                torsoSprite.sprite = playerClothes.playerClothes[loadedStats[22]].torsoSprite[0];
                torsoSprite.gameObject.SetActive(true);
            }
            else
                torsoSprite.gameObject.SetActive(false);
            if (loadedStats[23] != -1)
            {
                leftArmSprite.sprite = playerClothes.playerClothes[loadedStats[23]].leftArmSprite[0];
                leftArmSprite.gameObject.SetActive(true);
            }
            else
                leftArmSprite.gameObject.SetActive(false);
            if (loadedStats[24] != -1)
            {
                rightArmSprite.sprite = playerClothes.playerClothes[loadedStats[23]].rightArmSprite[0];
                rightArmSprite.gameObject.SetActive(true);
            }
            else
                rightArmSprite.gameObject.SetActive(false);
            if (loadedStats[24] != -1)
            {
                leftBootSprite.sprite = playerClothes.playerClothes[loadedStats[24]].leftBootSprite[0];
                leftBootSprite.gameObject.SetActive(true);
            }
            else
                leftBootSprite.gameObject.SetActive(false);
            if (loadedStats[24] != -1)
            {
                rightBootSprite.sprite = playerClothes.playerClothes[loadedStats[24]].rightBootSprite[0];
                rightBootSprite.gameObject.SetActive(true);
            }
            else
                rightBootSprite.gameObject.SetActive(false);
            if (loadedStats[25] != -1)
            {
                swordSprite.sprite = playerClothes.playerClothes[loadedStats[25]].swordSprite[0];
                swordSprite.gameObject.SetActive(true);
            }
            else
                swordSprite.gameObject.SetActive(false);
            if (loadedStats[26] != -1)
            {
                shieldSprite.sprite = playerClothes.playerClothes[loadedStats[26]].shieldSprite[0];
                shieldSprite.gameObject.SetActive(true);
            }
            else
                shieldSprite.gameObject.SetActive(false);
            if (loadedStats[27] != -1)
            {
                hairSprite.sprite = playerClothes.playerClothes[loadedStats[27]].hairSprite[0];
                hairSprite.gameObject.SetActive(true);
            }
            else
                hairSprite.gameObject.SetActive(false);
        }

    }

    public void DisableCanvasError()
    {
        gameObject.SetActive(false);
        ERROR_NOFOUND_CHARACTER.SetActive(false);
    }
}
