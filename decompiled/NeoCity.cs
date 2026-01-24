using System.Collections;
using UnityEngine;

public class NeoCity : Wrapper
{
	public static NeoCity env;

	[Header("Children")]
	public Cockpit Cockpit;

	public Lasers Lasers;

	public Targets Targets;

	public Feedback[] Feedbacks;

	public Sparkle Sparkle;

	public Sweat Sweat;

	[Header("Fragments")]
	public Fragment shootingStar;

	public Fragment[] beams;

	public Fragment fakeTargets;

	public Fragment movingLines;

	private int activeFeedback;

	private bool isActivated;

	private bool isAltFeedback;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		shootingStar.Awake();
		fakeTargets.Awake();
		movingLines.Awake();
		Fragment[] array = beams;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		RenderChildren(toggle: false, 3);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true, 3);
		Cockpit.Show();
		Lasers.Hide();
		Targets.Show();
		movingLines.TriggerAnim("loop", GetSpeed());
		DreamWorld.env.ClearFeedbacks();
		Interface.env.Cam.SetPosition(0f, 0f);
	}

	public void Hide()
	{
		isActivated = false;
		Cockpit.Hide();
		Lasers.Hide();
		Targets.Hide();
		RenderChildren(toggle: false, 3);
	}

	private void ShootStarDelayed(float delta)
	{
		Sparkle.SetLocalX(Random.Range(-10.5f, 10.5f));
		Sparkle.SetLocalY(Random.Range(2.5f, 6.5f));
		shootingStar.TriggerAnimDelayedDelta(delta, "shoot");
	}

	private void SparkleStarDelayed(float delta)
	{
		Sparkle.SetLocalX(Random.Range(-10.5f, 10.5f));
		Sparkle.SetLocalY(Random.Range(1f, 6.5f));
		Sparkle.CrossInDelayed(delta, 0);
	}

	public void Bobble(float delta, int bar, int beat)
	{
		movingLines.TriggerAnim("loop", GetSpeed());
		Cockpit.BobbleDelayed(delta, beat);
		if (beat == 2 || beat == 4)
		{
			SparkleStarDelayed(delta);
		}
		if (bar % 2 == 0 && beat == 3)
		{
			ShootStarDelayed(delta);
		}
	}

	public void TriggerCueDelayed(float timeStarted, bool isFullBeat, int soundNum)
	{
		StartCoroutine(TriggeringCueDelayed(timeStarted, isFullBeat, soundNum));
	}

	private IEnumerator TriggeringCueDelayed(float timeStarted, bool isFullBeat, int soundNum)
	{
		if (isFullBeat)
		{
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, soundNum);
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 4);
		}
		else
		{
			speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, soundNum);
			speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, 4);
		}
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		switch (soundNum)
		{
		case 0:
			beams[0].SetLocalX(-0.125f);
			beams[0].TriggerAnim("beam");
			break;
		case 1:
			beams[0].SetLocalX(0f);
			beams[0].TriggerAnim("beam");
			break;
		case 2:
			beams[0].SetLocalX(0.125f);
			beams[0].TriggerAnim("beam");
			break;
		case 3:
			beams[0].SetLocalX(-0.125f);
			beams[1].SetLocalX(0.125f);
			beams[0].TriggerAnim("beam");
			beams[1].TriggerAnim("beam");
			break;
		}
	}

	public void ShootCenter()
	{
		Cockpit.PressCenter();
		Lasers.ShootCenter();
	}

	public void ShootLeft()
	{
		Cockpit.PressLeft();
		Lasers.ShootLeft();
	}

	public void ShootRight()
	{
		Cockpit.PressRight();
		Lasers.ShootRight();
	}

	public void Hit(float accuracy, int hitType)
	{
		speakers[2].TriggerSoundStack(0);
		speakers[2].TriggerSoundStack(5);
		if (hitType == 3)
		{
			speakers[2].TriggerSoundStack(4);
			Feedbacks[activeFeedback].SetLocalPosition(-3f, 0f);
			if (accuracy == 1f)
			{
				Feedbacks[activeFeedback].CrossIn("perfect");
			}
			else if (accuracy == 0.332f)
			{
				Feedbacks[activeFeedback].CrossIn("early");
			}
			else if (accuracy == 0.333f)
			{
				Feedbacks[activeFeedback].CrossIn("late");
			}
			activeFeedback++;
			if (activeFeedback >= Feedbacks.Length)
			{
				activeFeedback = 0;
			}
			Feedbacks[activeFeedback].SetLocalPosition(3f, 0f);
			if (accuracy == 1f)
			{
				Feedbacks[activeFeedback].CrossIn("perfect");
			}
			else if (accuracy == 0.332f)
			{
				Feedbacks[activeFeedback].CrossIn("early");
			}
			else if (accuracy == 0.333f)
			{
				Feedbacks[activeFeedback].CrossIn("late");
			}
			activeFeedback++;
			if (activeFeedback >= Feedbacks.Length)
			{
				activeFeedback = 0;
			}
		}
		else
		{
			switch (hitType)
			{
			case 0:
				if (isAltFeedback)
				{
					speakers[2].TriggerSoundStack(6);
				}
				else
				{
					speakers[2].TriggerSoundStack(1);
				}
				Feedbacks[activeFeedback].SetLocalPosition(0f, 0f);
				break;
			case 1:
				speakers[2].TriggerSoundStack(2);
				Feedbacks[activeFeedback].SetLocalPosition(-3f, 0f);
				break;
			case 2:
				speakers[2].TriggerSoundStack(3);
				Feedbacks[activeFeedback].SetLocalPosition(3f, 0f);
				break;
			}
			if (accuracy == 1f)
			{
				Feedbacks[activeFeedback].CrossIn("perfect");
			}
			else if (accuracy == 0.332f)
			{
				Feedbacks[activeFeedback].CrossIn("early");
			}
			else if (accuracy == 0.333f)
			{
				Feedbacks[activeFeedback].CrossIn("late");
			}
			activeFeedback++;
			if (activeFeedback >= Feedbacks.Length)
			{
				activeFeedback = 0;
			}
		}
		Cockpit.Radar.FlashAccuracy(accuracy);
		Targets.HitTargets();
		Lasers.SetLaserColor(accuracy);
	}

	public void Strike()
	{
		speakers[2].TriggerSoundStack(0);
		Interface.env.Cam.Breeze(0.5f);
		Lasers.SetLaserColor(0f);
		Cockpit.Radar.FlashAccuracy(0f);
	}

	public void FakeOutDelayed(float timeStarted)
	{
		StartCoroutine(FakingOutDelayed(timeStarted));
	}

	private IEnumerator FakingOutDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Targets.FakeHide();
		fakeTargets.TriggerAnim(Random.Range(0, 3).ToString() ?? "", GetSpeed());
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.25f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Targets.FakeShow();
		fakeTargets.TriggerAnim("hidden");
	}

	public void ToggleIsAltFeedback(bool toggle)
	{
		isAltFeedback = toggle;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 60f;
	}
}
