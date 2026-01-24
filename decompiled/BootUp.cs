using System.Collections;
using UnityEngine;

public class BootUp : Custom
{
	private void Start()
	{
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		SaveManager.mgr.LoadPlayerData();
		Technician.mgr.SetMixerPrefs();
		Application.targetFrameRate = 400;
		QualitySettings.maxQueuedFrames = 1;
		Fragment.SetAudioOffset((float)SaveManager.mgr.GetAudioOffsetMs() / 1000f);
		if (Builder.mgr.GetOperatingSystemNum() == 1 || Builder.mgr.GetOperatingSystemNum() == 2)
		{
			Fragment.SetAudioSync(0.025f);
		}
		Interface.env.Disable();
		SceneMonitor.mgr.PreloadScene("TitleScreen");
		if (Builder.mgr.GetOperatingSystemNum() == 1)
		{
			yield return new WaitForSecondsRealtime(3f);
			Interface.env.Spinner.Activate();
			yield return new WaitForSecondsRealtime(3f);
		}
		else
		{
			yield return new WaitForSecondsRealtime(1f);
		}
		if (Builder.mgr.GetOperatingSystemNum() != 2)
		{
			Technician.mgr.InitiateResolution();
		}
		if (Builder.mgr.GetOperatingSystemNum() == 1)
		{
			Interface.env.Spinner.Deactivate();
			yield return new WaitForSecondsRealtime(3f);
		}
		else
		{
			yield return new WaitForSecondsRealtime(1f);
		}
		if (ControlHandler.mgr.GetGamepad() != null)
		{
			ControlHandler.mgr.ConfigureGamepadType();
		}
		SceneMonitor.mgr.LoadScene();
	}
}
