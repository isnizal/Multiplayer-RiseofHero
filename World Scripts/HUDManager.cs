using UnityEngine;

public class HUDManager : MonoBehaviour
{
	public static HUDManager instance;

	[SerializeField] GameObject equipmentPanelWindow;
	[SerializeField] GameObject skillWindow;
	[SerializeField] GameObject achievementWindow;

	private void Start()
	{
		instance = this;
	}
	public void ToggleSkillWindow(CanvasGroup canvas)
	{
		if (canvas.alpha == 0)
		{
			skillWindow.GetComponent<CanvasGroup>().alpha = 1;
			skillWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			skillWindow.GetComponent<CanvasGroup>().alpha = 0;
			skillWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
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
			equipmentPanelWindow.GetComponent<CanvasGroup>().alpha = 1;
			equipmentPanelWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			equipmentPanelWindow.GetComponent<CanvasGroup>().alpha = 0;
			equipmentPanelWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
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
