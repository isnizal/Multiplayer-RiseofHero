using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignLevelWarning : MonoBehaviour
{
	public TextMeshProUGUI levelReqText;
	public int levelRequired;


	private void OnValidate()
	{
		levelReqText = GetComponentInChildren<TextMeshProUGUI>();
	}

	void Update()
    {
		levelReqText.text = "" + levelRequired;
    }
}
