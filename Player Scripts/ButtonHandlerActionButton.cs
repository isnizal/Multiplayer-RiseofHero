using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandlerActionButton : MonoBehaviour, IPointerClickHandler
{
    private PlayerMovement _playerMovement;
    public void InitializeButtonHandler(PlayerMovement player)
    {
        _playerMovement = player;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_playerMovement != null)
        {
            _playerMovement.ActionButton();
        }
    }
}
