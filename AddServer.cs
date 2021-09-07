using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddServer : MonoBehaviour
{
    [SerializeField] private GameObject addServer;

    private NewNetworkManager _netWorkManager;
    private CharacterCustomize _characterCustomize;
    private List<string> serverList = new List<string>();

    private void Awake()
    {
        _netWorkManager = FindObjectOfType<NewNetworkManager>();

        CheckServer();
    }
    // Start is called before the first frame update

    private void CheckServer()
    {
        string server = _netWorkManager.networkAddress;
        serverList.Add(server);

        var serverBtn = Instantiate(addServer);
        serverBtn.transform.SetParent(this.transform);
        serverBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        serverBtn.GetComponentInChildren<TextMeshProUGUI>().text = serverList[0];
        _characterCustomize = FindObjectOfType<CharacterCustomize>();
        serverBtn.GetComponent<Button>().onClick.AddListener(_characterCustomize.EnterWorld);
    }
}
