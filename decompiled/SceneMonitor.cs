using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMonitor : Custom
{
	public static SceneMonitor mgr;

	private bool isLoadWaiting;

	private string activeSceneName;

	private AsyncOperation sceneLoaded;

	private void Awake()
	{
		mgr = this;
		activeSceneName = SceneManager.GetActiveScene().name;
	}

	public void PreloadScene(string sceneName)
	{
		if (SaveManager.mgr.CheckIsSavingPlayerDataStacked())
		{
			SaveManager.mgr.SavePlayerData();
		}
		if (SaveManager.mgr.CheckIsSavingEditorDataStacked())
		{
			SaveManager.mgr.SaveEditorData();
		}
		Interface.env.Disable();
		sceneLoaded = SceneManager.LoadSceneAsync(sceneName);
		sceneLoaded.allowSceneActivation = false;
	}

	public void LoadScene()
	{
		StartCoroutine(LoadingScene());
	}

	private IEnumerator LoadingScene()
	{
		if (sceneLoaded.progress < 0.9f)
		{
			isLoadWaiting = true;
			Interface.env.Spinner.Activate();
			yield return new WaitUntil(() => sceneLoaded.progress >= 0.9f);
			yield return new WaitForSecondsRealtime(0.333f);
			Interface.env.Spinner.Deactivate();
			yield return new WaitForSecondsRealtime(0.333f);
			if (isLoadWaiting)
			{
				isLoadWaiting = false;
			}
		}
		sceneLoaded.allowSceneActivation = true;
	}

	public bool CheckIsSceneInBuild(string sceneNameChecked)
	{
		for (int i = 3; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
			int num = scenePathByBuildIndex.LastIndexOf("/");
			if (scenePathByBuildIndex.Substring(num + 1, scenePathByBuildIndex.LastIndexOf(".") - num - 1) == sceneNameChecked)
			{
				return true;
			}
		}
		return false;
	}

	public string GetActiveSceneName()
	{
		return activeSceneName;
	}
}
