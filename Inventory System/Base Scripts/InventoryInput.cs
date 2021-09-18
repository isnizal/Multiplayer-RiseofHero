using UnityEngine;
using Mirror;


public class InventoryInput : MonoBehaviour
{
	public CanvasGroup _characterPanelObject;
	public CanvasGroup _skillWindowGameObject;
	public GameObject _settingsWindowGameObject;
	[SerializeField] KeyCode[] toggleInventoryKeys;
	[SerializeField] KeyCode[] toggleSkillWindowKeys;
	[SerializeField] KeyCode[] toggleSettingsWindowKeys;

	private Character _character;

    public void LoadInventoryInput(Character character)
    {
		_characterPanelObject = GameObject.Find("Character Panel").GetComponent<CanvasGroup>();
		_skillWindowGameObject = GameObject.Find("SkillPanelWindow").GetComponent<CanvasGroup>();
		_settingsWindowGameObject = GameObject.Find("SettingsPanel");
		_settingsWindowGameObject.SetActive(false);
		_character = character;
	}
    private void Update()
	{
		if (_character is null)
			return;

		if (!_character.onInput)
		{
			for (int i = 0; i < toggleInventoryKeys.Length; i++)
			{
				if (Input.GetKeyDown(toggleInventoryKeys[i]))
				{
					HUDManager.instance.ToggleEquipmentWindow(_characterPanelObject);
					break;
				}
				if (Input.GetKeyDown(toggleSkillWindowKeys[i]))
				{
					HUDManager.instance.ToggleSkillWindow(_skillWindowGameObject);
				}
				if (Input.GetKeyDown(toggleSettingsWindowKeys[i]))
				{
					HUDManager.instance.ToggleSettingsWindow(_settingsWindowGameObject);
				}
			}
		}
	}


}
