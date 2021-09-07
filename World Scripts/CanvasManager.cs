using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
	public static CanvasManager instance;
	public static CanvasManager CanvasInstance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<CanvasManager>();
			}
			return instance;
		}

	}

	public GameObject attributesWindow;
	public GameObject spellWindow;

	public void ToggleAttributesWindow(CanvasGroup canvas)
	{
		if (canvas.alpha == 0)
		{
			attributesWindow.GetComponent<CanvasGroup>().alpha = 1;
			attributesWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
			spellWindow.GetComponent<CanvasGroup>().alpha = 0;
			spellWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		//else
		//{
		//	attributesWindow.GetComponent<CanvasGroup>().alpha = 0;
		//	attributesWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
		//}
	}

	public void ToggleSpellWindow(CanvasGroup canvas)
	{
		if (canvas.alpha == 0)
		{
			spellWindow.GetComponent<CanvasGroup>().alpha = 1;
			spellWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
			attributesWindow.GetComponent<CanvasGroup>().alpha = 0;
			attributesWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		//else
		//{
		//	spellWindow.GetComponent<CanvasGroup>().alpha = 0;
		//	spellWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;

		//}
	}
}
