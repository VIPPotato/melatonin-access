using System.Collections;
using UnityEngine;

public class AngrySkies : Wrapper
{
	public static AngrySkies env;

	[Header("Children")]
	public NasaTv NasaTv;

	public Sparkle Sparkle;

	public Planet Planet;

	public McMarchers McMarchers;

	public Shuttle Shuttle;

	public Sweat Sweat;

	[Header("Fragments")]
	public Fragment parallaxer;

	public Fragment zoomer;

	public Fragment shootingStar;

	private bool isPrepped;

	private bool isActivated;

	private Coroutine countdowning;

	private Coroutine countdowningSound;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		env = this;
		parallaxer.Awake();
		zoomer.Awake();
		shootingStar.Awake();
		SetupFragments();
		RenderChildren(toggle: false, 4);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true, 4);
		parallaxer.TriggerAnim("awaiting");
		zoomer.TriggerAnim("idled");
		speakers[3].ToggleSoundMute(1, toggle: true);
		Planet.Show();
		NasaTv.Show();
		McMarchers.Show();
		Shuttle.Show();
		Interface.env.Cam.SetPosition(0f, 0f);
		DreamWorld.env.ClearFeedbacks();
	}

	public void Hide()
	{
		isActivated = false;
		Planet.Hide();
		NasaTv.Hide();
		McMarchers.Hide();
		Shuttle.Hide();
		CancelCoroutine(countdowning);
		CancelCoroutine(countdowningSound);
		RenderChildren(toggle: false, 4);
	}

	public void ShootStarDelayed(float delta)
	{
		shootingStar.SetLocalX(Random.Range(-10.5f, 10.5f));
		shootingStar.SetLocalY(Random.Range(-3.5f, 6f));
		shootingStar.TriggerAnimDelayedDelta(delta, "shoot");
	}

	public void SparkleStarDelayed(float delta)
	{
		Sparkle.SetLocalX(Random.Range(-10.5f, 10.5f));
		Sparkle.SetLocalY(Random.Range(-3.5f, 6f));
		Sparkle.CrossInDelayed(delta, 0);
	}

	public void Countdown0Delayed(float timeStarted)
	{
		CancelCoroutine(countdowning);
		countdowning = StartCoroutine(Countdowning0Delayed(timeStarted));
	}

	private IEnumerator Countdowning0Delayed(float timeStarted)
	{
		Shuttle.SetPrepDuration(1f);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (Shuttle.CheckIsLifted())
		{
			Shuttle.Reset(4);
		}
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Increase(0.5f, 2);
			NasaTv.SpaceMeters[1].Increase(0.5f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Increase(0.5f, 2);
			NasaTv.SpaceMeters[1].Increase(0.5f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McMarchers.ToggleIsMarching(toggle: false);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].SetAmount(3);
			NasaTv.SpaceMeters[0].Decrease(1f, 3);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped && NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].Decrease(1f, 3);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped && NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].Decrease(1f, 3);
		}
	}

	public void Countdown0SoundDelayed(float timeStarted)
	{
		CancelCoroutine(countdowningSound);
		countdowningSound = StartCoroutine(Countdowning0SoundDelayed(timeStarted));
	}

	private IEnumerator Countdowning0SoundDelayed(float timeStarted)
	{
		PlayFillDelayed(timeStarted, 0);
		PlayRiserDelayed(timeStarted, 1f);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 0);
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 2);
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayLowerDelayed(checkpoint, 1f);
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped)
		{
			PlayDrainDelayed(checkpoint);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped)
		{
			PlayDrainDelayed(checkpoint);
		}
	}

	public void Countdown1Delayed(float timeStarted)
	{
		CancelCoroutine(countdowning);
		countdowning = StartCoroutine(Countdowning1Delayed(timeStarted));
	}

	private IEnumerator Countdowning1Delayed(float timeStarted)
	{
		Shuttle.SetPrepDuration(0.5f);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (Shuttle.CheckIsLifted())
		{
			Shuttle.Reset(2);
		}
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Increase(1f, 2);
			NasaTv.SpaceMeters[1].Increase(1f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Increase(1f, 2);
			NasaTv.SpaceMeters[1].Increase(1f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McMarchers.ToggleIsMarching(toggle: false);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].Decrease(2f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped && NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].Decrease(2f, 2);
		}
	}

	public void Countdown1SoundDelayed(float timeStarted)
	{
		CancelCoroutine(countdowningSound);
		countdowningSound = StartCoroutine(Countdowning1SoundDelayed(timeStarted));
	}

	private IEnumerator Countdowning1SoundDelayed(float timeStarted)
	{
		PlayFillDelayed(timeStarted, 0);
		PlayRiserDelayed(timeStarted, 2f);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 2);
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayLowerDelayed(checkpoint, 2f);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped)
		{
			PlayDrainDelayed(checkpoint);
		}
	}

	public void Countdown2Delayed(float timeStarted)
	{
		CancelCoroutine(countdowning);
		countdowning = StartCoroutine(Countdowning2Delayed(timeStarted));
	}

	private IEnumerator Countdowning2Delayed(float timeStarted)
	{
		Shuttle.SetPrepDuration(2f);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		timeStarted = Technician.mgr.GetDspTime();
		if (Shuttle.CheckIsLifted())
		{
			Shuttle.Reset(8);
		}
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Increase(0.25f, 2);
			NasaTv.SpaceMeters[1].Increase(0.25f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 4f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Increase(0.25f, 2);
			NasaTv.SpaceMeters[1].Increase(0.25f, 2);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 3f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McMarchers.ToggleIsMarching(toggle: false);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 1f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].SetAmount(3);
			NasaTv.SpaceMeters[0].Decrease(0.5f, 3);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped && NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].Decrease(0.5f, 3);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped && NasaTv.CheckIsActivated())
		{
			NasaTv.Flash();
			NasaTv.SpaceMeters[0].Decrease(0.5f, 3);
		}
	}

	public void Countdown2SoundDelayed(float timeStarted)
	{
		CancelCoroutine(countdowningSound);
		countdowningSound = StartCoroutine(Countdowning2SoundDelayed(timeStarted));
	}

	private IEnumerator Countdowning2SoundDelayed(float timeStarted)
	{
		PlayFillDelayed(timeStarted, 0);
		PlayRiserDelayed(timeStarted, 0.5f);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 0);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayFillDelayed(checkpoint, 2);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayLowerDelayed(checkpoint, 0.5f);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped)
		{
			PlayDrainDelayed(checkpoint);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isPrepped)
		{
			PlayDrainDelayed(checkpoint);
		}
	}

	public void Blind(float duration)
	{
		parallaxer.TriggerAnim("lookLeft", GetSpeed() / duration);
		zoomer.TriggerAnim("zoomOut", GetSpeed() / duration);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.MoveRight(duration);
		}
	}

	public void Unblind(float duration)
	{
		parallaxer.TriggerAnim("lookRight", GetSpeed() / duration);
		zoomer.TriggerAnim("zoomIn", GetSpeed() / duration);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.MoveLeft(duration);
		}
	}

	public void TriggerBlinded()
	{
		parallaxer.TriggerAnim("lookLeft", 1f, 1f);
		zoomer.TriggerAnim("zoomOut", 1f, 1f);
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.Hide();
		}
	}

	private void PlayFillDelayed(float timeStarted, int soundNum)
	{
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, soundNum);
	}

	private void PlayRiserDelayed(float timeStarted, float pitch)
	{
		speakers[3].SetSoundPitch(0, pitch * GetSpeed());
		speakers[3].TriggerSoundDelayedTimeStarted(timeStarted, 0);
	}

	private void PlayLowerDelayed(float timeStarted, float pitch)
	{
		speakers[3].SetSoundPitch(1, pitch * GetSpeed());
		speakers[3].TriggerSoundDelayedTimeStarted(timeStarted, 1);
	}

	private void PlayDrainDelayed(float timeStarted)
	{
		speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, 1);
	}

	public void PlayDrain()
	{
		speakers[1].TriggerSoundStack(0);
		speakers[1].TriggerSoundStack(1);
	}

	public void PlayLaunch()
	{
		speakers[2].TriggerSound(0);
	}

	public void Reset()
	{
		CancelCoroutine(countdowning);
		isPrepped = false;
		if (NasaTv.CheckIsActivated())
		{
			NasaTv.SpaceMeters[0].Reset();
			NasaTv.SpaceMeters[1].Reset();
			NasaTv.SpaceMeters[0].ToggleIsVisible(toggle: false);
			NasaTv.SpaceMeters[1].ToggleIsVisible(toggle: true);
		}
		speakers[0].CancelAllSounds();
		speakers[3].CancelAllSounds();
	}

	public void ToggleIsPrepped(bool toggle)
	{
		isPrepped = toggle;
	}

	public void ToggleLowerMute(bool toggle)
	{
		speakers[3].ToggleSoundMute(1, toggle);
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
