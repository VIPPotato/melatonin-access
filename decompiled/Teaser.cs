using System.Collections;
using UnityEngine;

public class Teaser : Custom
{
	public static Teaser dir;

	private bool isEnabled;

	private static bool isTeaserSeen;

	private void Awake()
	{
		dir = this;
	}

	private void Start()
	{
		isTeaserSeen = true;
		Technician.mgr.ToggleVsync(toggle: true);
		Technician.mgr.SetAudioListener(1f);
		Interface.env.Disable();
		Interface.env.FeatherBorder.Show();
		Interface.env.Letterbox.Show();
		CallToAction.env.Fader.Show();
		CallToAction.env.Fader.SetColor(1);
		CallToAction.env.Fader.SetSpeed(0.5f);
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		yield return new WaitForSeconds(0.5f);
		Interface.env.FeatherBorder.Deactivate();
		CallToAction.env.Fader.Deactivate();
		yield return new WaitForSeconds(0.5f);
		isEnabled = true;
	}

	private void Update()
	{
		if (isEnabled && (ControlHandler.mgr.CheckIsActionPressed() || ControlHandler.mgr.CheckIsStartPressed() || ControlHandler.mgr.CheckIsCancelPressed()))
		{
			CallToAction.env.PlayExitSound();
			Interface.env.ExitTo("Chapter_1");
			isEnabled = false;
		}
	}

	public static bool CheckIsTeaserSeen()
	{
		return isTeaserSeen;
	}
}
