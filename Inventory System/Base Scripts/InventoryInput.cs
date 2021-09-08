using UnityEngine;
using Mirror;


public class InventoryInput : NetworkBehaviour
{
	public CanvasGroup _characterPanelObject;
	public CanvasGroup _skillWindowGameObject;
	public GameObject _settingsWindowGameObject;
	[SerializeField] KeyCode[] toggleInventoryKeys;
	[SerializeField] KeyCode[] toggleSkillWindowKeys;
	[SerializeField] KeyCode[] toggleSettingsWindowKeys;

    public override void OnStartClient()
    {
        base.OnStartClient();
		//_characterPanelObject = GameObject.Find("Character Panel").GetComponent<CanvasGroup>();
		//_skillWindowGameObject = GameObject.Find("SkillPanelWindow").GetComponent<CanvasGroup>();
		//_settingsWindowGameObject = GameObject.Find("SettingsPanel");
		//_settingsWindowGameObject.SetActive(false);
	}
    private void Update()
	{
		for (int i = 0; i < toggleInventoryKeys.Length; i++)
		{
			if(Input.GetKeyDown(toggleInventoryKeys[i]))
			{
				HUDManager.instance.ToggleEquipmentWindow(_characterPanelObject);
				break;
			}
			if(Input.GetKeyDown(toggleSkillWindowKeys[i]))
			{
				HUDManager.instance.ToggleSkillWindow(_skillWindowGameObject);
			}
			if(Input.GetKeyDown(toggleSettingsWindowKeys[i]))
			{
				HUDManager.instance.ToggleSettingsWindow(_settingsWindowGameObject);
			}
		}
	}


}
