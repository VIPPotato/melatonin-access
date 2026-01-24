using System.Collections;
using UnityEngine;

public class Creditor : Custom
{
	public static Creditor dir;

	private void Awake()
	{
		dir = this;
	}

	private void Start()
	{
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		if (SaveManager.mgr.CheckIsGameComplete())
		{
			Credits.env.PlayCreditsMusic(1);
		}
		else
		{
			Credits.env.PlayCreditsMusic(0);
		}
		Technician.mgr.ToggleVsync(toggle: true);
		Technician.mgr.FadeInAudioListener();
		Credits.env.Show();
		yield return new WaitForSeconds(4.5f);
		Credits.env.TransitionLogoGameToLogoCompany();
		yield return new WaitForSeconds(4.5f);
		Credits.env.TransitionLogoCompanyToCreator();
		yield return new WaitForSeconds(4.5f);
		Credits.env.ScrollList();
		yield return new WaitForSeconds(Credits.env.GetScrollDuration() + 3f);
		ExitToTitle();
	}

	private void ExitToTitle()
	{
		StartCoroutine(ExitingToTitle());
	}

	private IEnumerator ExitingToTitle()
	{
		SceneMonitor.mgr.PreloadScene("TitleScreen");
		Technician.mgr.FadeOutAudioListener(0.125f);
		Credits.env.Faders[1].Activate();
		yield return new WaitForSeconds(10f);
		SceneMonitor.mgr.LoadScene();
	}
}
