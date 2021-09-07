using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace DailyRewardSystem
{
    public enum RewardType
	{
        CopperCoins,
        Experience,
        Gems,
        Item
	}
    [Serializable] public struct Reward
	{
        public RewardType Type;
        public int Amount;
        [Header("Item Rewards")]
        public Sprite itemImage;
        public Item itemSO;
	}

	public class DailyRewards : MonoBehaviour
    {
        [Header("Rewards UI")]
        [SerializeField] GameObject rewardsCanvas;
        [SerializeField] Button openButton;
        [SerializeField] Button closeButton;
        [SerializeField] Image rewardImage;
        [SerializeField] TextMeshProUGUI rewardAmount;
        [SerializeField] Button claimButton;
        [SerializeField] GameObject rewardsNotification;
        [SerializeField] GameObject noMoreRewardsPanel;
        [SerializeField] GameObject rewardsPanel;
        [SerializeField] TextMeshProUGUI rewardLeftTime;

        [Space]
        [Header("Rewards Images")]
        [SerializeField] Sprite iconCopperSprite;
        [SerializeField] Sprite iconExperienceSprite;
        [SerializeField] Sprite iconGemSprite;

        [Space]
        [Header("Rewards Database")]
        [SerializeField] RewardsDatabase rewardsDB;
        [SerializeField] Item rewardItemSO;

        [Space]
        [Header("Timing")]
        [SerializeField] double nextRewardDelay = 3600f;
        [SerializeField] float checkForRewardDelay = 1f;

        private Inventory inventory;
        private int nextRewardIndex;
        private bool isRewardReady = false;

        void Start()
        {
            Initialize();
            StopAllCoroutines();
            StartCoroutine(CheckForRewards());
            if (inventory == null)
                inventory = FindObjectOfType<Inventory>();
        }

        void Initialize()
		{
            nextRewardIndex = PlayerPrefs.GetInt("Next_Reward_Index", 0);

            openButton.onClick.RemoveAllListeners();
            openButton.onClick.AddListener(OnOpenButtonClick);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnCloseButtonClick);

            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(OnClaimButtonClick);

            if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Claim_Datetime")))
                PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());
		}

        IEnumerator CheckForRewards()
		{
            while (true)
			{
                if(!isRewardReady)
				{
                    DateTime currentDatetime = DateTime.Now;
                    DateTime rewardClaimDatetime = DateTime.Parse(PlayerPrefs.GetString("Reward_Claim_Datetime", currentDatetime.ToString()));

                    double elapsedSeconds = (currentDatetime - rewardClaimDatetime).TotalSeconds;
                    float timeUntilReward = (float)nextRewardDelay - (float)elapsedSeconds;
                    timeUntilReward -= Time.deltaTime;
                    int hours = Mathf.FloorToInt(timeUntilReward / 3600);
                    int minutes = Mathf.FloorToInt(timeUntilReward / 60) % 60;
                    int seconds = Mathf.FloorToInt(timeUntilReward % 60);
                    string rewardTimer = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
                    rewardLeftTime.text = "Next Reward in " + rewardTimer;
                    if (elapsedSeconds >= nextRewardDelay)
                        ActivateReward();
                    else
                        DeactivateReward();
                }
                yield return new WaitForSeconds(checkForRewardDelay);
			}
		}

        void ActivateReward()
		{
            isRewardReady = true;
            rewardsPanel.SetActive(true);
            noMoreRewardsPanel.SetActive(false);
            rewardsNotification.SetActive(true);
            Reward reward = rewardsDB.GetReward(nextRewardIndex);

            if (reward.Type == RewardType.CopperCoins)
                rewardImage.sprite = iconCopperSprite;
            else if (reward.Type == RewardType.Experience)
                rewardImage.sprite = iconExperienceSprite;
            else if (reward.Type == RewardType.Gems)
                rewardImage.sprite = iconGemSprite;
            else if (reward.Type == RewardType.Item)
            {
                rewardImage.sprite = reward.itemImage;
                rewardItemSO = reward.itemSO;
            }

            rewardAmount.text = string.Format("+{0}", reward.Amount);
        }

        void DeactivateReward()
		{
            isRewardReady = false;
            rewardsPanel.SetActive(false);
            noMoreRewardsPanel.SetActive(true);
            rewardsNotification.SetActive(false);
		}

        void OnClaimButtonClick()
		{
            Reward reward = rewardsDB.GetReward(nextRewardIndex);
            if (reward.Type == RewardType.CopperCoins)
            {
                Debug.Log(reward.Type.ToString() + reward.Amount);
                GameData.CopperCoins += reward.Amount;
                Character.MyInstance.copperCurrency += reward.Amount;
                isRewardReady = false;
            }
            else if (reward.Type == RewardType.Experience)
            {
                Debug.Log(reward.Type.ToString() + reward.Amount);
                GameData.Experience += reward.Amount;
                LevelSystem.LevelInstance.AddExp(reward.Amount);
                isRewardReady = false;
            }
            else if(reward.Type == RewardType.CopperCoins)
            {
                Debug.Log(reward.Type.ToString() + reward.Amount);
                GameData.Gems += reward.Amount;
                Character.MyInstance.premiumCurrency += reward.Amount;

                isRewardReady = false;
            }
            else if (reward.Type == RewardType.Item)
            {
                Debug.Log(reward.Type.ToString() + reward.Amount);
                GameData.Item += reward.Amount;
                inventory.AddItem(rewardItemSO.GetCopy());
                isRewardReady = false;
            }
            //nextRewardIndex++;
            nextRewardIndex = UnityEngine.Random.Range(0, rewardsDB.rewardsCount);
            if (nextRewardIndex >= rewardsDB.rewardsCount)
                nextRewardIndex = 0;

            PlayerPrefs.SetInt("Next_Reward_Index", nextRewardIndex);

            PlayerPrefs.SetString("Reward_Claim_Datetime", DateTime.Now.ToString());

            DeactivateReward();
        }

        void OnOpenButtonClick()
		{
            rewardsCanvas.SetActive(true);
		}

        void OnCloseButtonClick()
		{
            rewardsCanvas.SetActive(false);
		}
    }
}
