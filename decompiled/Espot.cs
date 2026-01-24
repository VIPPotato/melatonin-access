using System.Collections;
using UnityEngine;

public class Espot : Wrapper
{
	public static Espot env;

	[Header("Children")]
	public Feedback[] Feedbacks;

	public Sweat Sweat;

	public UfoMachine UfoMachine;

	public Arcade Arcade;

	private bool isActivated;

	private bool isDollying = true;

	private Coroutine cueing;

	private Coroutine cueingSound;

	private const float animTempo = 80f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false, 3);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true, 3);
		Interface.env.Cam.SetPosition(0f, 0f);
		UfoMachine.Show();
		Arcade.Show();
		DreamWorld.env.SetFeedbacks(Feedbacks);
	}

	public void Hide()
	{
		isActivated = false;
		UfoMachine.Hide();
		Arcade.Hide();
		CancelCoroutine(cueing);
		CancelCoroutine(cueingSound);
		RenderChildren(toggle: false, 3);
	}

	public void CueFastDelayed(float timeStarted, int breaks)
	{
		CancelCoroutine(cueing);
		cueing = StartCoroutine(CueingFastDelayed(timeStarted, breaks));
	}

	private IEnumerator CueingFastDelayed(float timeStarted, int breaks)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDollying)
		{
			gears[1].TriggerAnim("left", env.GetSpeed() * 2f);
		}
		UfoMachine.JoystickRight();
		UfoMachine.Claw.SlideRight();
		float timeDelayed = MusicBox.env.GetSecsPerBeat() * 0.5f;
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		UfoMachine.JoystickRight();
		switch (breaks)
		{
		case 0:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 1.5f;
			break;
		case 1:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 7.5f;
			break;
		case 2:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 13.5f;
			break;
		}
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDollying)
		{
			gears[1].TriggerAnim("right", env.GetSpeed() * 2f);
		}
		UfoMachine.JoystickLeft();
		UfoMachine.Claw.SlideLeft();
		timeDelayed = MusicBox.env.GetSecsPerBeat() * 0.5f;
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		UfoMachine.JoystickLeft();
	}

	public void CueFastSoundDelayed(float timeStarted, int breaks)
	{
		CancelCoroutine(cueingSound);
		cueingSound = StartCoroutine(CueingFastSoundDelayed(timeStarted, breaks));
	}

	private IEnumerator CueingFastSoundDelayed(float timeStarted, int breaks)
	{
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 2);
		float timeDelayed = MusicBox.env.GetSecsPerBeat() * 0.5f;
		float checkpoint = timeStarted + timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 0);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		switch (breaks)
		{
		case 0:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 1.5f;
			break;
		case 1:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 7.5f;
			break;
		case 2:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 13.5f;
			break;
		}
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 3);
		timeDelayed = MusicBox.env.GetSecsPerBeat() * 0.5f;
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 3);
	}

	public void CueSlowDelayed(float timeStarted, int breaks)
	{
		CancelCoroutine(cueing);
		cueing = StartCoroutine(CueingSlowDelayed(timeStarted, breaks));
	}

	private IEnumerator CueingSlowDelayed(float timeStarted, int breaks)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDollying)
		{
			gears[1].TriggerAnim("left", env.GetSpeed());
		}
		UfoMachine.JoystickRight();
		UfoMachine.Claw.NudgeRight(0);
		float timeDelayed = MusicBox.env.GetSecsPerBeat() * 1f;
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		UfoMachine.JoystickRight();
		UfoMachine.Claw.NudgeRight(1);
		switch (breaks)
		{
		case 0:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 3f;
			break;
		case 1:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 7f;
			break;
		case 2:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 133f;
			break;
		}
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDollying)
		{
			gears[1].TriggerAnim("right", env.GetSpeed());
		}
		UfoMachine.JoystickLeft();
		UfoMachine.Claw.NudgeLeft(2);
		timeDelayed = MusicBox.env.GetSecsPerBeat() * 1f;
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		UfoMachine.JoystickLeft();
		UfoMachine.Claw.NudgeLeft(1);
	}

	public void CueSlowSoundDelayed(float timeStarted, int breaks)
	{
		CancelCoroutine(cueingSound);
		cueingSound = StartCoroutine(CueingSlowSoundDelayed(timeStarted, breaks));
	}

	private IEnumerator CueingSlowSoundDelayed(float timeStarted, int breaks)
	{
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 2);
		float timeDelayed = MusicBox.env.GetSecsPerBeat() * 1f;
		float checkpoint = timeStarted + timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 0);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		switch (breaks)
		{
		case 0:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 3f;
			break;
		case 1:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 7f;
			break;
		case 2:
			timeDelayed = MusicBox.env.GetSecsPerBeat() * 133f;
			break;
		}
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		speakers[0].TriggerSoundDelayedTimeStarted(checkpoint, 3);
		timeDelayed = MusicBox.env.GetSecsPerBeat() * 1f;
		checkpoint += timeDelayed;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		speakers[1].TriggerSoundDelayedTimeStarted(checkpoint, 3);
	}

	public void PlayGrabSfx()
	{
		speakers[2].TriggerSound(0);
	}

	public void PlayReleaseSfx()
	{
		if (UfoMachine.Claw.GetMultiplier() == 2f)
		{
			speakers[2].TriggerSound(1);
		}
		else
		{
			speakers[2].TriggerSound(2);
		}
	}

	public void ScaleBobble(float multiplier)
	{
		gears[0].TriggerAnim("scale", env.GetSpeed() * 0.125f * multiplier);
	}

	public void TrailerOutro()
	{
		gears[0].TriggerAnim("trailerOutro", env.GetSpeed());
	}

	public void ToggleIsDollying(bool toggle)
	{
		isDollying = toggle;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 80f;
	}
}
