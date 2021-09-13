using UnityEngine;
using UnityEngine.Serialization;

public class HUDManager : MonoBehaviour
{
	public static HUDManager instance;

	[FormerlySerializedAs("equipmentPanelWindow")]
	[SerializeField] GameObject characterPanel;
	[FormerlySerializedAs("playerSkillCanvas")]
	[SerializeField] GameObject skillPanelWindow;
	[SerializeField] GameObject achievementWindow;

	private void Start()
	{
		instance = this;
	}
	public void ToggleSkillWindow(CanvasGroup canvas)
	{
		if (canvas.alpha == 0)
		{
			skillPanelWindow.GetComponent<CanvasGroup>().alpha = 1;
			skillPanelWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			skillPanelWindow.GetComponent<CanvasGroup>().alpha = 0;
			skillPanelWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
			CanvasManager.CanvasInstance.attributesWindow.GetComponent<CanvasGroup>().alpha = 0;
			CanvasManager.CanvasInstance.spellWindow.GetComponent<CanvasGroup>().alpha = 0;
			CanvasManager.CanvasInstance.attributesWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
			CanvasManager.CanvasInstance.spellWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}


	public void ToggleEquipmentWindow(CanvasGroup canvas)
	{
		if (canvas.alpha == 0)
		{
			characterPanel.GetComponent<CanvasGroup>().alpha = 1;
			characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			characterPanel.GetComponent<CanvasGroup>().alpha = 0;
			characterPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	public void ToggleAchievementWindow(CanvasGroup canvas)
	{
		if(canvas.alpha == 0)
		{
			achievementWindow.GetComponent<CanvasGroup>().alpha = 1;
			achievementWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			achievementWindow.GetComponent<CanvasGroup>().alpha = 0;
			achievementWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	public void ToggleSettingsWindow(GameObject settingsWindow)
	{
		if (!settingsWindow.activeInHierarchy)
			settingsWindow.SetActive(true);
		else if (settingsWindow.activeInHierarchy)
			settingsWindow.SetActive(false);
	}
}
