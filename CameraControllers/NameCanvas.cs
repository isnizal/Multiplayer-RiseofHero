using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NameCanvas : MonoBehaviour
{
    public Canvas canvas;
    public GameObject nameObject;
    public GameObject dialogObject;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = FindObjectOfType<Camera>();
        canvas.GetComponent<Canvas>();
        canvas.worldCamera = cam;
        //_playerMovement = GetComponentInParent<PlayerMovement>();
    }


}
