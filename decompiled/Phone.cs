using System.Collections;
using UnityEngine;

public class Phone : Wrapper
{
	[Header("Children")]
	public DatingApp DatingApp;

	public HomeScreen HomeScreen;

	[Header("Fragments")]
	public Fragment thumber;

	public Fragment frame;

	private bool isSmall;

	protected override void Awake()
	{
		thumber.Awake();
		frame.Awake();
		RenderChildren(toggle: false);
	}

	public void Show(int state)
	{
		RenderChildren(toggle: true);
		switch (state)
		{
		case 0:
			isSmall = false;
			frame.TriggerAnim("0");
			thumber.TriggerAnim("idled");
			HomeScreen.Show();
			break;
		case 1:
			isSmall = false;
			frame.TriggerAnim("0");
			thumber.TriggerAnim("idled");
			DatingApp.Show();
			DatingApp.SetToCropped();
			break;
		}
		if (state == 2)
		{
			isSmall = true;
			frame.TriggerAnim("1");
			thumber.TriggerAnim("hidden");
			DatingApp.Show();
			DatingApp.SetToCentered();
		}
	}

	public void LaunchApp()
	{
		StartCoroutine(LaunchingApp());
	}

	private IEnumerator LaunchingApp()
	{
		thumber.TriggerAnim("tap");
		yield return new WaitForSeconds(19f / 60f);
		DatingApp.Activate();
		HomeScreen.Deactivate();
		Interface.env.Cam.Sway();
	}

	public void ThumbLeft()
	{
		if (!isSmall)
		{
			thumber.TriggerAnim("left");
		}
	}

	public void ThumbRight()
	{
		if (!isSmall)
		{
			thumber.TriggerAnim("right");
		}
	}

	public void SmallRotateDelayed(float timeStarted, int direction)
	{
		StartCoroutine(SmallRotatingDelayed(timeStarted, direction));
	}

	private IEnumerator SmallRotatingDelayed(float timeStarted, int direction)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isSmall = true;
		thumber.TriggerAnim("hidden");
		DatingApp.SetToCentered();
		switch (direction)
		{
		case 1:
			frame.TriggerAnim("1to5");
			break;
		case 0:
			frame.TriggerAnim("5to1");
			break;
		}
	}

	public void SetSmallRotationDelayed(float timeStarted, int value)
	{
		StartCoroutine(SettingSmallRotationDelayed(timeStarted, value));
	}

	private IEnumerator SettingSmallRotationDelayed(float timeStarted, int value)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		frame.TriggerAnim(value.ToString() ?? "");
		switch (value)
		{
		case 0:
			isSmall = false;
			thumber.TriggerAnim("slideIn", LoveLand.env.GetSpeed());
			DatingApp.Crop();
			break;
		case 1:
			if (isSmall)
			{
				thumber.TriggerAnim("hidden");
				DatingApp.SetToCentered();
			}
			else
			{
				isSmall = true;
				thumber.TriggerAnim("slideOut", LoveLand.env.GetSpeed());
				DatingApp.Center();
			}
			break;
		default:
			isSmall = true;
			thumber.TriggerAnim("hidden");
			DatingApp.SetToCentered();
			break;
		}
	}

	public bool CheckIsSmall()
	{
		return isSmall;
	}
}
