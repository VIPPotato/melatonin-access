using System.Collections;
using UnityEngine;

public class LoveLand : Wrapper
{
	public static LoveLand env;

	[Header("Children")]
	public Phone Phone;

	public Cloud[] Clouds;

	public Sweat Sweat;

	public Feedback[] Feedbacks;

	[Header("Fragments")]
	public Fragment parallaxer;

	public Fragment[] flowers;

	public Fragment[] petals;

	private bool isActivated;

	private int activeSpeakerNum;

	private float feedbackX;

	private float feedbackY;

	private Coroutine countingDown;

	private const float animTempo = 120f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		parallaxer.Awake();
		Fragment[] array = flowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		array = petals;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		RenderChildren(toggle: false, 2);
	}

	public void Show(int state)
	{
		isActivated = true;
		RenderChildren(toggle: true, 2);
		Phone.Show(state);
		Cloud[] clouds = Clouds;
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].Show();
		}
		DreamWorld.env.SetFeedbacks(Feedbacks);
		switch (state)
		{
		case 0:
			Interface.env.Cam.SetPosition(0f, 1f);
			break;
		case 1:
			Interface.env.Cam.SetPosition(0f, 0f);
			break;
		default:
			parallaxer.TriggerAnim("zoomedOut");
			Interface.env.Cam.SetPosition(0f, 0f);
			break;
		}
		Fragment[] array = flowers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("swaying", Random.Range(0.5f, 0.9f));
		}
	}

	public void Hide()
	{
		isActivated = false;
		CancelCoroutine(countingDown);
		RenderChildren(toggle: false, 2);
	}

	public void ZoomOutDelayed(float timeStarted)
	{
		parallaxer.TriggerAnimDelayedTimeStarted(timeStarted, "zoomOut", GetSpeed());
	}

	public void SetToZoomedOutDelayed(float timeStarted)
	{
		parallaxer.TriggerAnimDelayedTimeStarted(timeStarted, "zoomedOut");
	}

	public void ZoomInDelayed(float timeStarted)
	{
		parallaxer.TriggerAnimDelayedTimeStarted(timeStarted, "zoomIn", GetSpeed());
	}

	public void CountdownLeftDelayed(float timeStarted, float speed)
	{
		CancelCoroutine(countingDown);
		countingDown = StartCoroutine(CountingDownLeft(timeStarted, speed));
	}

	private IEnumerator CountingDownLeft(float timeStarted, float speed)
	{
		if (!Phone.DatingApp.GetDateCard().CheckIsLastCue())
		{
			Phone.DatingApp.GetDateCard().Hide();
		}
		Phone.DatingApp.IncreaseDateCardNumber();
		speakers[activeSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, 4);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Phone.DatingApp.GetDateCard().SetHotOrNotNum(1);
		Phone.DatingApp.GetDateCard().Activate1(Phone.DatingApp.GetBioNum());
		Phone.DatingApp.ToggleIsSliding(toggle: false);
		Phone.DatingApp.ToggleIsTopCardHidden(toggle: true);
		Phone.DatingApp.IncreaseBioNum();
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / speed - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[activeSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 5);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (Phone.CheckIsSmall())
		{
			feedbackX = -6.84f;
			feedbackY = 0.135f;
		}
		else
		{
			feedbackX = -7.82f;
			feedbackY = -1.22f;
		}
		if (DreamWorld.env.GetActiveFeedback() != null)
		{
			DreamWorld.env.GetActiveFeedback().SetLocalPosition(feedbackX, feedbackY);
		}
		Sweat.SetLocalPosition(feedbackX, feedbackY);
		Phone.DatingApp.GetDateCard().Activate2();
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / speed - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[activeSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 6);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Phone.DatingApp.GetDateCard().Activate3();
		Phone.DatingApp.RotateInReject();
	}

	public void CountdownRightDelayed(float timeStarted, float speed)
	{
		CancelCoroutine(countingDown);
		countingDown = StartCoroutine(CountingDownRight(timeStarted, speed));
	}

	private IEnumerator CountingDownRight(float timeStarted, float speed)
	{
		if (!Phone.DatingApp.GetDateCard().CheckIsLastCue())
		{
			Phone.DatingApp.GetDateCard().Hide();
		}
		Phone.DatingApp.IncreaseDateCardNumber();
		speakers[activeSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Phone.DatingApp.GetDateCard().SetHotOrNotNum(2);
		Phone.DatingApp.GetDateCard().Activate1(1);
		Phone.DatingApp.ToggleIsSliding(toggle: false);
		Phone.DatingApp.ToggleIsTopCardHidden(toggle: true);
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / speed - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[activeSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 1);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (Phone.CheckIsSmall())
		{
			feedbackX = 6.84f;
			feedbackY = 0.135f;
		}
		else
		{
			feedbackX = 7.82f;
			feedbackY = -1.22f;
		}
		if (DreamWorld.env.GetActiveFeedback() != null)
		{
			DreamWorld.env.GetActiveFeedback().SetLocalPosition(feedbackX, feedbackY);
		}
		Sweat.SetLocalPosition(feedbackX, feedbackY);
		Phone.DatingApp.GetDateCard().Activate2();
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / speed - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[activeSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Phone.DatingApp.GetDateCard().Activate3();
		Phone.DatingApp.RotateInAccept();
	}

	public void React(int soundType)
	{
		if (soundType == 1)
		{
			speakers[activeSpeakerNum].TriggerSoundStack(7);
		}
		else
		{
			speakers[activeSpeakerNum].TriggerSoundStack(3);
		}
	}

	public void CancelAllSounds()
	{
		speakers[0].CancelAllSounds();
		speakers[1].CancelAllSounds();
	}

	public void BlowPetals1(float delta)
	{
		for (int i = 0; i < 4; i++)
		{
			petals[i].TriggerAnimDelayedDelta(delta, "petal_" + (i + 1), Random.Range(0.95f, 1.4f) * GetSpeed());
			petals[i].SetLocalY(Random.Range(-5f, -1.5f));
			if (Random.Range(0, 3) == 0)
			{
				petals[i].SetLocalZ(50f);
			}
			else
			{
				petals[i].SetLocalZ(0f);
			}
		}
	}

	public void BlowPetals2(float delta)
	{
		for (int i = 4; i < petals.Length; i++)
		{
			petals[i].TriggerAnimDelayedDelta(delta, "petal_" + (i + 1), Random.Range(0.95f, 1.4f) * GetSpeed());
			petals[i].SetLocalY(Random.Range(-5f, -1.5f));
			if (Random.Range(0, 3) == 0)
			{
				petals[i].SetLocalZ(50f);
			}
			else
			{
				petals[i].SetLocalZ(0f);
			}
		}
	}

	public void SetActiveSpeakerNum(int value)
	{
		activeSpeakerNum = value;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 120f;
	}
}
