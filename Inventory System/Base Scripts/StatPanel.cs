using UnityEngine;
using TMPro;
using Mirror;

public class StatPanel : MonoBehaviour
{
	[SerializeField] StatDisplay[] statDisplays;
	[SerializeField] string[] statNames;

	private CharacterStat[] stats;

	//[SerializeField] TextMeshProUGUI statsLevelText;

	private void OnValidate()
	{


    }
	private Character thePlayer;

	private void Awake()
	{
		thePlayer = FindObjectOfType<Character>();
		statDisplays = GetComponentsInChildren<StatDisplay>();
	}

	private void Update()
	{
			UpdateStatValues();
			UpdateStatNames();

	}

	public void SetStats(params CharacterStat[] charStats)
	{
		stats = charStats;

		if (stats.Length > statDisplays.Length)
		{
			Debug.LogError("Not Enough Stat Displays!");
			return;
		}
		for (int i = 0; i < statDisplays.Length; i++)
		{
			statDisplays[i].gameObject.SetActive(i < stats.Length);

			if (i < stats.Length)
			{
				statDisplays[i].Stat = stats[i];
			}
		}
	}

	public void UpdateStatValues()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].UpdateStatValue();
		}
	}

	public void UpdateStatNames()
	{
		for (int i = 0; i < statNames.Length; i++)
		{
			statDisplays[i].Name = statNames[i];
		}
	}
}
