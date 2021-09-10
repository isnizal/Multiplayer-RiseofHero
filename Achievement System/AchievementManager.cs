using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EasyUI.Toast;
using Mirror;
using UnityEngine.Serialization;

public class AchievementManager : NetworkBehaviour
{
	public static AchievementManager instance;
	public static AchievementManager AchievementInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<AchievementManager>();
			}
			return instance;
		}

	}
	//General Notification Variables
	[Header("Notifications Panel")]
	[FormerlySerializedAs("achNoticeGO")]
	public GameObject achivementNotice;
	public TextMeshProUGUI achNoticeTitle;
	public TextMeshProUGUI achNoticeDesc;
	public Sprite achDoneImage;
	public Sprite achNotDoneImage;

    public override void OnStartClient()
    {
        base.OnStartClient();
		achivementNotice = GameObject.Find("AchivementNotice");
		achNoticeTitle = GameObject.Find("AchNoticeTitle").GetComponent<TextMeshProUGUI>();
		achNoticeDesc = GameObject.Find("AchNoticeDesc").GetComponent<TextMeshProUGUI>();
		achivementNotice.SetActive(false);
		LoadAchievementPanel();
	}
    [Space]
	[Header("Kill Red Slimes - KS1")]
	public int killSlimeCountAch;

	public int killSlime1Done = 0;
	public int killSlime1Active = 0;
	public int killSlime1Claimed = 0;

	[Space]
	[Header("Collect Copper - CC1")]
	public int collectCopperCountAch;

	public int collectCopper1Done = 0;
	public int collectCopper1Active = 0;
	public int collectCopper1Claimed = 0;

	[Space]
	[Header("Collect Copper - CC2")]
	public int collectCopper2Done = 0;
	public int collectCopper2Active = 0;
	public int collectCopper2Claimed = 0;

	[Space]
	[Header("Kill 50 Enemmies")]
	public int kill50CountAch;
	public int kill50AchDone = 0;
	public int kill50AchActive = 0;
	public int kill50AchClaimed = 0;

	[Space]
	[Header("Kill 500 Enemies")]
	public int kill500AchDone = 0;
	public int kill500AchActive = 0;
	public int kill500AchClaimed = 0;

	[Space]
	[Header("Kill DevilQueen")]
	public int killDevilQueenCountAch;
	public int killDevilQueenDone = 0;
	public int killDevilQueenActive = 0;
	public int killDevilQueenClaimed = 0;

	[Header("Achievement Panel")]
	[FormerlySerializedAs("achGO")]
	public GameObject[] achievementSlot = new GameObject[6];
	[FormerlySerializedAs("achPanel")]
	public AchievementDisplay[] achievementDisplay = new AchievementDisplay[6];
	public AchievementSlot[] achSO = new AchievementSlot[6];
	public Button[] claimButton = new Button[6];

	private void LoadAchievementPanel()
	{
		for (int i = 0; i < achievementSlot.Length; i++)
		{
			achievementSlot[i] = GameObject.Find($"AchievementSlot({i})");
		}
		for (int a = 0; a < achievementSlot.Length; a++)
		{
			achievementDisplay[a] = GameObject.Find($"AchievementSlot({a})").GetComponent<AchievementDisplay>();
		}
		for (int s = 0; s < claimButton.Length; s++)
		{
			claimButton[s] = GameObject.Find($"AchievementSlot({s})").gameObject.transform.GetChild(1).GetChild(7).GetComponent<Button>();
		}
		//look like already load on the prefab
		//for (int z = 0; z < achSO.Length; z++)
		//{
		//	achSO[z] = Resources.Load<AchievementSlot>("Achievements SO/Achievements" + "" + z);
		//}

		//disable after initialize
		//for (int f = 0; f < achievementSlot.Length; f++)
		//{
		//	achievementSlot[f].SetActive(false);
		//}
	}
	private void OnValidate()
	{
		for (int s = 0; s < claimButton.Length; s++)
		{
			if (claimButton[s] is null)
				return;
		}

		for (int i = 0; i < achSO.Length; i++)
		{
			for (int j = 0; j < claimButton.Length; j++)
			{
				claimButton[0].gameObject.name = achSO[0].achName + " Button";
				claimButton[1].gameObject.name = achSO[1].achName + " Button";
				claimButton[2].gameObject.name = achSO[2].achName + " Button";
				claimButton[3].gameObject.name = achSO[3].achName + " Button";
				claimButton[4].gameObject.name = achSO[4].achName + " Button";
				claimButton[5].gameObject.name = achSO[5].achName + " Button";
			}
		}
	}

	private void Update()
	{
		if (hasAuthority)
		{
			Debug.Log("achievement local player");
			ActiveAch();
			CheckRequirements();
		}
	}
	public void ActiveAch()
	{
		for (int i = 0; i < achSO.Length; i++)
		{
			if (LevelSystem.LevelInstance.currentLevel >= achSO[i].achLevelRequired)
			{
				achievementSlot[i].SetActive(true);
				if (achSO[i].achID == 1)
				{
					killSlime1Active = 1;
				}
				if (achSO[i].achID == 2)
				{
					collectCopper1Active = 1;
				}
				if (achSO[i].achID == 3)
				{
					kill50AchActive = 1;
				}
				if (achSO[i].achID == 4)
				{
					killDevilQueenActive = 1;
				}
				if (achSO[i].achID == 5)
				{
					collectCopper2Done = 1;
				}
				if (achSO[i].achID == 6)
				{
					kill500AchActive = 1;
				}
			}
		}
		LoadAchievement();
		CheckAchCompletion();
	}

	private void Start()
	{
		CheckAchCompletionUIUpdate();
	}

	public void CheckAchCompletionUIUpdate()
	{
		if (killSlime1Done == 1 && killSlime1Claimed == 1)
		{
			claimButton[0].interactable = false;
			achievementDisplay[0].achPanelIcon.sprite = achDoneImage;
		}
		if (collectCopper1Done == 1 && collectCopper1Claimed == 1)
		{
			claimButton[1].interactable = false;
			achievementDisplay[1].achPanelIcon.sprite = achDoneImage;
		}
		if (kill50AchDone == 1 && kill50AchClaimed == 1)
		{
			claimButton[2].interactable = false;
			achievementDisplay[2].achPanelIcon.sprite = achDoneImage;
		}
		if (killDevilQueenDone == 1 && killDevilQueenClaimed == 1)
		{
			claimButton[3].interactable = false;
			achievementDisplay[3].achPanelIcon.sprite = achDoneImage;
		}
		if (collectCopper2Done == 1 && collectCopper2Claimed == 1)
		{
			claimButton[4].interactable = false;
			achievementDisplay[4].achPanelIcon.sprite = achDoneImage;
		}
		if (kill500AchDone == 1 && kill500AchClaimed == 1)
		{
			claimButton[5].interactable = false;
			achievementDisplay[5].achPanelIcon.sprite = achDoneImage;
		}
	}


	public void LoadAchievement()
	{
		for (int i = 0; i < achSO.Length; i++)
		{
			achievementDisplay[i].achLevelReq.text = "Lv: " + achSO[i].achLevelRequired.ToString();
			achievementDisplay[i].achPanelTitle.text = achSO[i].achPanelTitle;
			achievementDisplay[i].achPanelDesc.text = achSO[i].achPanelDesc;
			achievementDisplay[i].achPanelRewards.text = achSO[i].achReward;
			achievementDisplay[i].achPanelReqAmount.text = "Req:" + achSO[i].achAmountRequired.ToString();
		}
	}

	public void CheckAchCompletion()
	{
		if (achSO[0].achID == 1)
		{
			if (killSlime1Claimed == 0)
			{
				if (killSlimeCountAch >= achSO[0].achAmountRequired)
				{

					if (killSlime1Done == 1)
					{
						achievementDisplay[0].achPanelIcon.sprite = achDoneImage;
						achievementDisplay[0].achPanelCompletedImage.SetActive(true);
					}
				}
				else
				{

					if (killSlime1Done == 0)
					{
						achievementDisplay[0].achPanelIcon.sprite = achNotDoneImage;
						achievementDisplay[0].achPanelCompletedImage.SetActive(false);
						claimButton[0].interactable = false;
					}
				}
			}


		}
		if (achSO[1].achID == 2)
		{
			if (collectCopper1Claimed == 0)
			{
				if (collectCopperCountAch >= achSO[1].achAmountRequired)
				{
					if (collectCopper1Done == 1)
					{
						achievementDisplay[1].achPanelIcon.sprite = achDoneImage;
						achievementDisplay[1].achPanelCompletedImage.SetActive(true);
					}
				}
				else
				{
					if (collectCopper1Done == 0)
					{
						achievementDisplay[1].achPanelIcon.sprite = achNotDoneImage;
						achievementDisplay[1].achPanelCompletedImage.SetActive(false);
						claimButton[1].interactable = false;
					}
				}
			}
		}
		if (achSO[2].achID == 3)
		{
			if (kill50AchClaimed == 0)
			{
				if (kill50CountAch >= achSO[2].achAmountRequired)
				{
					if (kill50AchDone == 1)
					{
						achievementDisplay[2].achPanelIcon.sprite = achDoneImage;
						achievementDisplay[2].achPanelCompletedImage.SetActive(true);
					}
				}
				else
				{
					if (kill50AchDone == 0)
					{
						achievementDisplay[2].achPanelIcon.sprite = achNotDoneImage;
						achievementDisplay[2].achPanelCompletedImage.SetActive(false);
						claimButton[2].interactable = false;
					}
				}
			}
		}
		if (achSO[3].achID == 4)
		{
			if (killDevilQueenClaimed == 0)
			{
				if (killDevilQueenCountAch >= achSO[3].achAmountRequired)
				{
					if (killDevilQueenDone == 1)
					{
						achievementDisplay[3].achPanelIcon.sprite = achDoneImage;
						achievementDisplay[3].achPanelCompletedImage.SetActive(true);
					}
				}
				else
				{
					if (killDevilQueenDone == 0)
					{
						achievementDisplay[3].achPanelIcon.sprite = achNotDoneImage;
						achievementDisplay[3].achPanelCompletedImage.SetActive(false);
						claimButton[3].interactable = false;
					}
				}
			}
		}
		if (achSO[4].achID == 5)
		{
			if (collectCopper2Claimed == 0)
			{
				if (collectCopperCountAch >= achSO[4].achAmountRequired)
				{
					if (collectCopper2Done == 1)
					{
						achievementDisplay[4].achPanelIcon.sprite = achDoneImage;
						achievementDisplay[4].achPanelCompletedImage.SetActive(true);
					}
				}
				else
				{
					if (collectCopper2Done == 0)
					{
						achievementDisplay[4].achPanelIcon.sprite = achNotDoneImage;
						achievementDisplay[4].achPanelCompletedImage.SetActive(false);
						claimButton[4].interactable = false;
					}
				}
			}
		}
		if (achSO[5].achID == 6)
		{
			if (kill500AchClaimed == 0)
			{
				if (kill50CountAch >= achSO[5].achAmountRequired)
				{
					if (kill500AchDone == 1)
					{
						achievementDisplay[5].achPanelIcon.sprite = achDoneImage;
						achievementDisplay[5].achPanelCompletedImage.SetActive(true);
					}
				}
				else
				{
					if (kill500AchDone == 0)
					{
						achievementDisplay[5].achPanelIcon.sprite = achNotDoneImage;
						achievementDisplay[5].achPanelCompletedImage.SetActive(false);
						claimButton[5].interactable = false;
					}
				}
			}
		}
	}

	public void CheckRequirements()
	{
		if (killSlimeCountAch >= achSO[0].achAmountRequired && killSlime1Done == 0)
		{
			claimButton[0].interactable = true;
			killSlime1Done = 1;
			StartCoroutine(KillSkime1AchCO());
		}


		if (collectCopperCountAch >= achSO[1].achAmountRequired && collectCopper1Done == 0)
		{
			claimButton[1].interactable = true;
			collectCopper1Done = 1;
			StartCoroutine(CollectCopper1AchCO());
		}

		if (kill50CountAch >= achSO[2].achAmountRequired && kill50AchDone == 0)
		{
			claimButton[2].interactable = true;
			kill50AchDone = 1;
			StartCoroutine(Kill50AchCO());
		}

		if (killDevilQueenCountAch >= achSO[3].achAmountRequired && killDevilQueenDone == 0)
		{
			claimButton[3].interactable = true;
			killDevilQueenDone = 1;
			StartCoroutine(KillDevilQueenCO());
		}

		if (collectCopperCountAch >= achSO[4].achAmountRequired && collectCopper2Done == 0)
		{
			claimButton[4].interactable = true;
			collectCopper2Done = 1;
			StartCoroutine(CollectCopper2CO());
		}

		if (kill50CountAch >= achSO[5].achAmountRequired && kill500AchDone == 0)
		{
			claimButton[5].interactable = true;
			kill500AchDone = 1;
			StartCoroutine(Kill500AchCO());
		}
	}

	public void ClaimButton(int buttonNo)
	{
		if (buttonNo == 0)
		{
			if (killSlime1Done == 1)
			{
				Toast.Show("You gained " + "<color=red>" + achSO[0].achReward + "</color>", 3f, ToastPosition.MiddleCenter);
				Character.MyInstance.copperCurrency += 100;
				killSlime1Claimed = 1;
				claimButton[buttonNo].interactable = false;
			}
			else
			{
				Toast.Show("Task is not done yet" + "<color=red> " + achSO[0].achPanelTitle + " </color>", 3f, ToastPosition.MiddleCenter);
			}
		}
		if (buttonNo == 1)
		{
			if (collectCopper1Done == 1)
			{
				Toast.Show("You gained " + "<color=red>" + achSO[1].achReward + "</color>", 3f, ToastPosition.MiddleCenter);
				LevelSystem.LevelInstance.AddExp(50);
				collectCopper1Claimed = 1;
				claimButton[buttonNo].interactable = false;
			}
			else
			{
				Toast.Show("Task is not done yet" + "<color=red> " + achSO[1].achPanelTitle + " </color>", 3f, ToastPosition.MiddleCenter);
			}
		}
		if (buttonNo == 2)
		{
			if (kill50AchDone == 1)
			{
				Toast.Show("You gained " + "<color=red>" + achSO[2].achReward + "</color>", 3f, ToastPosition.MiddleCenter);
				Character.MyInstance.copperCurrency += 300;
				kill50AchClaimed = 1;
				claimButton[buttonNo].interactable = false;
			}
			else
			{
				Toast.Show("Task is not done yet" + "<color=red> " + achSO[2].achPanelTitle + " </color>", 3f, ToastPosition.MiddleCenter);
			}
		}
		if (buttonNo == 3)
		{
			if (killDevilQueenDone == 1)
			{
				Toast.Show("You gained " + "<color=red>" + achSO[3].achReward + "</color>", 3f, ToastPosition.MiddleCenter);
				LevelSystem.LevelInstance.AddExp(250);
				killDevilQueenClaimed = 1;
				claimButton[buttonNo].interactable = false;
			}
			else
			{
				Toast.Show("Task is not done yet" + "<color=red> " + achSO[3].achPanelTitle + " </color>", 3f, ToastPosition.MiddleCenter);
			}
		}
		if (buttonNo == 4)
		{
			if (collectCopper2Done == 1)
			{
				Toast.Show("You gained " + "<color=red>" + achSO[4].achReward + "</color>", 3f, ToastPosition.MiddleCenter);
				LevelSystem.LevelInstance.AddExp(250);
				collectCopper2Claimed = 1;
				claimButton[buttonNo].interactable = false;
			}
			else
			{
				Toast.Show("Task is not done yet" + "<color=red> " + achSO[4].achPanelTitle + " </color>", 3f, ToastPosition.MiddleCenter);
			}
		}
		if (buttonNo == 5)
		{
			if (kill500AchDone == 1)
			{
				Toast.Show("You gained " + "<color=red>" + achSO[5].achReward + "</color>", 3f, ToastPosition.MiddleCenter);
				Character.MyInstance.copperCurrency += 300;
				kill500AchClaimed = 1;
				claimButton[buttonNo].interactable = false;
			}
			else
			{
				Toast.Show("Task is not done yet" + "<color=red> " + achSO[5].achPanelTitle + " </color>", 3f, ToastPosition.MiddleCenter);
			}
		}

	}

	IEnumerator KillSkime1AchCO()
	{
		if (killSlime1Done == 1)
		{
			//activate UI Notification
			achNoticeTitle.text = achSO[0].achPanelTitle;
			achNoticeDesc.text = achSO[0].achPanelDesc + "(" + "<color=green>" + achSO[0].achAmountRequired + "</color>" + ")";
			achivementNotice.SetActive(true);
			SoundManager.PlaySound(SoundManager.Sound.Achievement);
			//killSlime1Done = true;
			yield return new WaitForSeconds(4);
			achivementNotice.SetActive(false);
		}
	}

	IEnumerator CollectCopper1AchCO()
	{
		if (collectCopper1Done == 1)
		{
			//activate UI Notification
			achNoticeTitle.text = achSO[1].achPanelTitle;
			achNoticeDesc.text = achSO[1].achPanelDesc + "(" + "<color=green>" + achSO[1].achAmountRequired + "</color>" + ")";
			achivementNotice.SetActive(true);
			SoundManager.PlaySound(SoundManager.Sound.Achievement);
			//collectCopper1Done = true;
			yield return new WaitForSeconds(4);
			achivementNotice.SetActive(false);
		}
	}

	IEnumerator Kill50AchCO()
	{
		if (kill50AchDone == 1)
		{
			//activate UI Notification
			achNoticeTitle.text = achSO[2].achPanelTitle;
			achNoticeDesc.text = achSO[2].achPanelDesc + "(" + "<color=green>" + achSO[2].achAmountRequired + "</color>" + ")";
			achivementNotice.SetActive(true);
			SoundManager.PlaySound(SoundManager.Sound.Achievement);
			//killSlime1Done = true;
			yield return new WaitForSeconds(4);
			achivementNotice.SetActive(false);
		}
	}

	IEnumerator KillDevilQueenCO()
	{
		if (killDevilQueenDone == 1)
		{
			//activate UI Notification
			achNoticeTitle.text = achSO[3].achPanelTitle;
			achNoticeDesc.text = achSO[3].achPanelDesc + "(" + "<color=green>" + achSO[3].achAmountRequired + "</color>" + ")";
			achivementNotice.SetActive(true);
			SoundManager.PlaySound(SoundManager.Sound.Achievement);
			//killSlime1Done = true;
			yield return new WaitForSeconds(4);
			achivementNotice.SetActive(false);
		}
	}

	IEnumerator CollectCopper2CO()
	{
		if (collectCopper2Done == 1)
		{
			//activate UI Notification
			achNoticeTitle.text = achSO[4].achPanelTitle;
			achNoticeDesc.text = achSO[4].achPanelDesc + "(" + "<color=green>" + achSO[4].achAmountRequired + "</color>" + ")";
			achivementNotice.SetActive(true);
			SoundManager.PlaySound(SoundManager.Sound.Achievement);
			//killSlime1Done = true;
			yield return new WaitForSeconds(4);
			achivementNotice.SetActive(false);
		}
	}

	IEnumerator Kill500AchCO()
	{
		if (kill500AchDone == 1)
		{
			//activate UI Notification
			achNoticeTitle.text = achSO[5].achPanelTitle;
			achNoticeDesc.text = achSO[5].achPanelDesc + "(" + "<color=green>" + achSO[6].achAmountRequired + "</color>" + ")";
			achivementNotice.SetActive(true);
			SoundManager.PlaySound(SoundManager.Sound.Achievement);
			//killSlime1Done = true;
			yield return new WaitForSeconds(4);
			achivementNotice.SetActive(false);
		}
	}
}
	
