using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSellArea : MonoBehaviour, IDropHandler
{
	public event Action OnDropEvent;
	public void OnDrop(PointerEventData eventData)
	{
		if (OnDropEvent != null)
			OnDropEvent();
	}
}
