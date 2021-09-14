using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameCanvas : MonoBehaviour
{
    public Canvas canvas;
    public GameObject nameText;
    public GameObject dialogText;
    private PlayerMovement _playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = FindObjectOfType<Camera>();
        canvas.GetComponent<Canvas>();
        canvas.worldCamera = cam;
        _playerMovement = GetComponentInParent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovement.rightPos == -1 || _playerMovement.frontPos == 1)
        {
            nameText.gameObject.transform.localScale = new Vector2(-.2f, .2f);
            dialogText.gameObject.transform.localScale = new Vector2(-.2f, .2f);
        }
        else
        {
            nameText.gameObject.transform.localScale = new Vector2(.2f, .2f);
            dialogText.gameObject.transform.localScale = new Vector2(.2f, .2f);
        }
    }
}
