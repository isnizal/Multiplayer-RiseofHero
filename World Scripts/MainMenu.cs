using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject progressPanel;
	[SerializeField] private Slider progressBar;
	[SerializeField] private TextMeshProUGUI progressValue;
	[SerializeField] private GameObject characterCustomize;
	[SerializeField] private GameObject menuCanvas;

	[SerializeField] private GameObject aboutBox;
	[SerializeField] private GameObject characterLoad;
	[SerializeField] private TextMeshProUGUI versionText;


	public void Awake()
	{
		progressPanel.SetActive(false);
		aboutBox.SetActive(false);
		versionText.text = "Version: " + Application.version;
	}
	public void ActivateCharacterCustomize()
	{
		characterCustomize.SetActive(true);
		//menuCanvas.SetActive(false);
	}
	public void LoadNewGame(AsyncOperation newOperation)
	{
		progressPanel.SetActive(true);
		FindObjectOfType<GameObserver>().LoadCharacter();
		StartCoroutine(NewCharacterAsync(newOperation));
	}

	private IEnumerator NewCharacterAsync(AsyncOperation newOperation)
	{
		characterCustomize.SetActive(false);
		AsyncOperation operation = newOperation;
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress);
			progressBar.value = progress;
			progressValue.text = (progress * 100f).ToString("f0") + "%";
			yield return null;
		}
	}
	private IEnumerator LoadCharacterAsync(string levelName)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress);
			progressBar.value = progress;
			progressValue.text = (progress * 100f).ToString("f0") + "%";
			yield return null;
		}
	}
	public void FindSave()
	{
		characterLoad.SetActive(true);
		characterLoad.GetComponent<CharacterLoad>().LoadCharacter();
	}
	public void LoadCharacter(string levelName)
	{
		GameObserver gameObserver = FindObjectOfType<GameObserver>();
		characterLoad.SetActive(false);
		gameObserver.EnableLoad();
		progressPanel.SetActive(true);
		StartCoroutine(LoadCharacterAsync(levelName));
	}

	public void OpenDiscord(string url)
	{
		Application.OpenURL(url);
	}

	public void OpenAboutBox()
	{
		aboutBox.SetActive(true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
