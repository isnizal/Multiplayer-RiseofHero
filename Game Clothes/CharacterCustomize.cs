using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterCustomize : MonoBehaviour
{
    [Serializable]
    public class TypeOfClothes
    {
        public Sprite helmet;
        public Sprite torso;
        public Sprite leftArm;
        public Sprite rightArm;
        public Sprite sword;
        public Sprite shield;
        public Sprite leftBoot;
        public Sprite rightBoot;
        public Sprite hair;
    }
    public TypeOfClothes[] typeOfClothes;


    [Header("Avatar")]
    [SerializeField] private GameObject hairAvatar;
    [SerializeField] private GameObject helmetAvatar;
    [SerializeField] private GameObject torsoAvatar;
    [SerializeField] private GameObject shieldAvatar;
    [SerializeField] private GameObject leftarmAvatar;
    [SerializeField] private GameObject rightarmAvatar;
    [SerializeField] private GameObject leftBootAvatar;
    [SerializeField] private GameObject rightBootAvatar;
    [SerializeField] private GameObject swordAvatar;

    [SerializeField] private Button shieldBtn;
    [SerializeField] private Button swordBtn;
    private int currentSelectedHelmet = -1;
    private int currentSelectedTorso = -1;
    private int currentSelectedArm = -1;
    private int currentSelectedBoot = -1;
    private int currentSelectedShield = -1;
    private int currentSelectedSword = -1;
    private int currentSelectedHair = -1;

    private void Start()
    {
        helmetBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = helmetBtn;
    }
    public void RightArrow()
    {
        if (helmetSelected)
        {
            currentSelectedHelmet++;
            if (currentSelectedHelmet >= typeOfClothes.Length)
                currentSelectedHelmet = -1;

            if (currentSelectedHelmet >= 0)
                helmetAvatar.SetActive(true);

            if (currentSelectedHelmet == -1)
            {
                currentSelectedHelmet = -1;
                helmetAvatar.SetActive(false);

                return;
            }
            helmetAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedHelmet].helmet;
        }
        else if (torsoSelected)
        {
            currentSelectedTorso++;
            if (currentSelectedTorso >= typeOfClothes.Length)
                currentSelectedTorso = -1;

            if (currentSelectedTorso >= 0)
                torsoAvatar.SetActive(true);

            if (currentSelectedTorso == -1)
            {
                currentSelectedTorso = -1;
                torsoAvatar.SetActive(false);

                return;
            }
            torsoAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedTorso].torso;
        }
        else if (armSelected)
        {
            currentSelectedArm++;
            if (currentSelectedArm >= typeOfClothes.Length)
                currentSelectedArm = -1;

            if (currentSelectedArm >= 0)
            {
                leftarmAvatar.SetActive(true);
                rightarmAvatar.SetActive(true);
            }

            if (currentSelectedArm == -1)
            {
                currentSelectedArm = -1;
                leftarmAvatar.SetActive(false);
                rightarmAvatar.SetActive(false);
                return;
            }
            leftarmAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedArm].leftArm;
            rightarmAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedArm].rightArm;
        }
        else if (bootSelected)
        {
            currentSelectedBoot++;
            if (currentSelectedBoot >= typeOfClothes.Length)
                currentSelectedBoot = -1;

            if (currentSelectedBoot >= 0)
            {
                leftBootAvatar.SetActive(true);
                rightBootAvatar.SetActive(true);

            }

            if (currentSelectedBoot == -1)
            {
                currentSelectedBoot = -1;
                rightBootAvatar.SetActive(false);
                leftBootAvatar.SetActive(false);
                return;
            }
            leftBootAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedBoot].leftBoot;
            rightBootAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedBoot].rightBoot;
        }
        else if (shieldSelected)
        {
            currentSelectedShield++;
            if (currentSelectedShield >= typeOfClothes.Length)
                currentSelectedShield = -1;

            if (currentSelectedShield >= 0)
            {
                shieldAvatar.SetActive(true);

            }

            if (currentSelectedShield == -1)
            {
                currentSelectedShield = -1;
                shieldAvatar.SetActive(false);

                return;
            }
            shieldAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedShield].shield;
        }
        else if (swordSelected)
        {
            currentSelectedSword++;
            if (currentSelectedSword >= typeOfClothes.Length)
                currentSelectedSword = -1;

            if (currentSelectedSword >= 0)
            {
                swordAvatar.SetActive(true);

            }

            if (currentSelectedSword == -1)
            {
                currentSelectedSword = -1;
                swordAvatar.SetActive(false);

                return;
            }
            swordAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedSword].sword;
        }
        else if (hairSelected)
        {
            currentSelectedHair++;
            if (currentSelectedHair >= typeOfClothes.Length)
                currentSelectedHair = -1;

            if (currentSelectedHair >= 0)
            {
                hairAvatar.SetActive(true);

            }

            if (currentSelectedHair == -1)
            {
                currentSelectedHair = -1;
                hairAvatar.SetActive(false);

                return;
            }
            hairAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedHair].hair;
        }
    }
    public void LeftArrow()
    {
        if (helmetSelected)
        {
            currentSelectedHelmet--;

            if (currentSelectedHelmet >= 0)
            {
                helmetAvatar.SetActive(true);

            }

            if (currentSelectedHelmet <= -1)
            {
                currentSelectedHelmet = -1;
                helmetAvatar.SetActive(false);

                return;
            }
            helmetAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedHelmet].helmet;
        }
        else if (torsoSelected)
        {
            currentSelectedTorso--;

            if (currentSelectedTorso >= 0)
            {
                torsoAvatar.SetActive(true);

            }

            if (currentSelectedTorso <= -1)
            {
                currentSelectedTorso = -1;
                torsoAvatar.SetActive(false);

                return;
            }
            torsoAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedHelmet].torso;
        }
        else if (armSelected)
        {
            currentSelectedArm--;


            if (currentSelectedArm >= 0)
            {
                leftarmAvatar.SetActive(true);
                rightarmAvatar.SetActive(true);

            }

            if (currentSelectedArm <= -1)
            {
                currentSelectedArm = -1;
                leftarmAvatar.SetActive(false);
                rightarmAvatar.SetActive(false);
                return;
            }
            leftarmAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedArm].leftArm;
            rightarmAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedArm].rightArm;

        }
        else if (bootSelected)
        {
            currentSelectedBoot--;


            if (currentSelectedBoot >= 0)
            {
                leftBootAvatar.SetActive(true);
                leftBootAvatar.SetActive(true);

            }

            if (currentSelectedBoot <= -1)
            {
                currentSelectedBoot = -1;
                leftBootAvatar.SetActive(false);
                rightBootAvatar.SetActive(false);
                return;
            }
            leftBootAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedBoot].leftBoot;
            rightBootAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedBoot].rightBoot;
        }
        else if (shieldSelected)
        {
            currentSelectedShield--;


            if (currentSelectedShield >= 0)
            {
                shieldAvatar.SetActive(true);

            }

            if (currentSelectedShield <= -1)
            {
                currentSelectedShield = -1;
                shieldAvatar.SetActive(false);
                return;
            }
            shieldAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedShield].shield;
        }
        else if (swordSelected)
        {
            currentSelectedSword--;

            if (currentSelectedSword >= 0)
            {
                swordAvatar.SetActive(true);

            }

            if (currentSelectedSword <= -1)
            {
                currentSelectedSword = -1;
                swordAvatar.SetActive(false);

                return;
            }
            swordAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedSword].sword;
        }
        else if (hairSelected)
        {
            currentSelectedHair--;

            if (currentSelectedHair >= 0)
            {
                hairAvatar.SetActive(true);

            }

            if (currentSelectedHair <= -1)
            {
                currentSelectedHair = -1;
                hairAvatar.SetActive(false);

                return;
            }
            hairAvatar.GetComponent<SpriteRenderer>().sprite = typeOfClothes[currentSelectedHair].hair;
        }
    }

    private bool helmetSelected = true;
    private bool torsoSelected = false;
    private bool armSelected = false;
    private bool bootSelected = false;
    private bool shieldSelected = false;
    private bool swordSelected = false;
    private bool hairSelected = false;
    private Button selectedBtn;
    public Button helmetBtn;
    public Button torsoBtn;
    public Button armBtn;
    public Button bootBtn;
    public Button hairBtn;
    public Sprite selectedSprite;
    public Sprite lastSprite;
    public void HelmetUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        helmetSelected = true;
        torsoSelected = false;
        armSelected = false;
        bootSelected = false;
        shieldSelected = false;
        swordSelected = false;
        hairSelected = false;

        helmetBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = helmetBtn;
    }
    public void TorsoUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        helmetSelected = false;
        torsoSelected = true;
        armSelected = false;
        bootSelected = false;
        shieldSelected = false;
        swordSelected = false;
        hairSelected = false;

        torsoBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = torsoBtn;
    }
    public void ArmUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        helmetSelected = false;
        torsoSelected = false;
        armSelected = true;
        bootSelected = false;
        shieldSelected = false;
        swordSelected = false;
        hairSelected = false;

        armBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = armBtn;
    }

    public void BootUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        helmetSelected = false;
        torsoSelected = false;
        armSelected = false;
        bootSelected = true;
        shieldSelected = false;
        swordSelected = false;
        hairSelected = false;

        bootBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = bootBtn;
    }

    public void ShieldUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        shieldBtn.GetComponent<Image>().color = Color.white;
        helmetSelected = false;
        torsoSelected = false;
        armSelected = false;
        bootSelected = false;
        shieldSelected = true;
        swordSelected = false;
        hairSelected = false;

        shieldBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = shieldBtn;
    }
    public void SwordUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        swordBtn.GetComponent<Image>().color = Color.white;
        helmetSelected = false;
        torsoSelected = false;
        armSelected = false;
        bootSelected = false;
        shieldSelected = false;
        swordSelected = true;
        hairSelected = false;

        swordBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = swordBtn;
    }
    public void HairUISelected()
    {
        if (selectedBtn != null)
            selectedBtn.GetComponent<Image>().sprite = lastSprite;

        helmetSelected = false;
        torsoSelected = false;
        armSelected = false;
        bootSelected = false;
        shieldSelected = false;
        swordSelected = false;
        hairSelected = true;

        hairBtn.GetComponent<Image>().sprite = selectedSprite;
        selectedBtn = hairBtn;
    }

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private GameObject addServer;
    private string currentName;


    public void InputName(string value)
    {
        currentName = value;
        inputName.targetGraphic.color = Color.white;
    }
    public void SelectServer()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            inputName.targetGraphic.color = Color.red;
            return;
        }
        else if (currentSelectedShield == -1)
        {
            shieldBtn.GetComponent<Image>().color = Color.red;
            return;
        }
        else if (currentSelectedSword == -1)
        {
            swordBtn.GetComponent<Image>().color = Color.red;
            return;
        }
        else
        {
            SavePlayerClothes();
            addServer.SetActive(true);
        }
    }
    public void EnterWorld()
    {
        addServer.SetActive(false);
        FindObjectOfType<NewNetworkManager>().OnStartClient();
        //FindObjectOfType<MainMenu>().LoadNewGame("RiseofHeros");
    }

    public void SavePlayerClothes()
    {
        GameClothes playerClothes = FindObjectOfType<GameClothes>();

        playerClothes.CurrentPlayerHelmet(currentSelectedHelmet);
        playerClothes.CurrentPlayerClothes(currentSelectedTorso);
        playerClothes.CurrentPlayerArm(currentSelectedArm);
        playerClothes.CurrentPlayerBoot( currentSelectedBoot);
        playerClothes.CurrentPlayerSword( currentSelectedSword);
        playerClothes.CurrentPlayerShield( currentSelectedShield);
        playerClothes.CurrentPlayerHair(currentSelectedHair);

    }
}
