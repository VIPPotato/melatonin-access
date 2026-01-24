using System.Collections;
using UnityEngine;

public class HypnoLair : Wrapper
{
	public static HypnoLair env;

	[Header("Children")]
	public Eye Eye;

	public PocketWatch[] PocketWatches;

	public Feedback[] Feedbacks;

	public Spiral[] Spirals;

	public Roller Roller;

	public Sweat Sweat;

	[Header("Fragments")]
	public Fragment[] pulsers;

	public Fragment[] eyeShapes;

	public Fragment pover;

	private bool isActivated;

	private int queuedEyeSilhouette;

	private float leaveDuration;

	private Color goodColor = new Color(1f, 1f, 1f);

	private Color earlyColor = new Color(0.99607843f, 1f, 0.88235295f);

	private Color lateColor = new Color(49f / 51f, 0.827451f, 76f / 85f);

	private Coroutine queueing;

	private Coroutine bobbling;

	private Coroutine halfBobbling;

	private Coroutine leavingPov;

	private const float animTempo = 60f;

	protected override void Awake()
	{
		env = this;
		Fragment[] array = pulsers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		array = eyeShapes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		pover.Awake();
		SetupFragments();
		leaveDuration = pover.GetAnimDuration("leave");
		RenderChildren(toggle: false, 1);
	}

	public void Show(bool isRemix)
	{
		isActivated = true;
		queuedEyeSilhouette = 0;
		RenderChildren(toggle: true, 1);
		Eye.Show(isRemix);
		PocketWatch[] pocketWatches = PocketWatches;
		for (int i = 0; i < pocketWatches.Length; i++)
		{
			pocketWatches[i].Hide();
		}
		Spiral[] spirals = Spirals;
		for (int i = 0; i < spirals.Length; i++)
		{
			spirals[i].Show(isRemix);
		}
		Roller.Show();
		Interface.env.Cam.SetPosition(0f, 0f);
		DreamWorld.env.SetFeedbacks(Feedbacks);
	}

	public void Hide()
	{
		isActivated = false;
		PocketWatch[] pocketWatches = PocketWatches;
		for (int i = 0; i < pocketWatches.Length; i++)
		{
			pocketWatches[i].Hide();
		}
		Spiral[] spirals = Spirals;
		for (int i = 0; i < spirals.Length; i++)
		{
			spirals[i].Hide();
		}
		Eye.Hide();
		Roller.Hide();
		CancelCoroutine(queueing);
		CancelCoroutine(bobbling);
		CancelCoroutine(halfBobbling);
		CancelCoroutine(leavingPov);
		RenderChildren(toggle: false, 1);
		Interface.env.Cam.ToggleIsGlowable(toggle: true);
	}

	public void Bobble(float delta, int beat)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(Bobbling(delta, beat));
	}

	private IEnumerator Bobbling(float delta, int beat)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		if (beat == 1)
		{
			Roller.Rotate();
		}
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		if (beat == 1 || beat == 3)
		{
			Eye.LookLeft();
		}
		else
		{
			Eye.LookRight();
		}
	}

	public void HalfBobble(float delta, int beat, int gameMode)
	{
		CancelCoroutine(halfBobbling);
		halfBobbling = StartCoroutine(HalfBobbling(delta, beat, gameMode));
	}

	private IEnumerator HalfBobbling(float delta, int beat, int gameMode)
	{
		if (PocketWatches[0].CheckIsSwinging())
		{
			float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
			switch (beat)
			{
			case 1:
			case 3:
				PocketWatches[0].SwingRight();
				break;
			case 2:
			case 4:
				PocketWatches[0].SwingLeft();
				break;
			}
		}
	}

	public void QueueShutEyeDelayed(float timeStarted)
	{
		CancelCoroutine(queueing);
		queueing = StartCoroutine(QueueingShutEyeeDelayed(timeStarted));
	}

	private IEnumerator QueueingShutEyeeDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Eye.CloseDelayed(checkpoint);
	}

	public void QueueDoubleShutEye(float timeStarted, int beat)
	{
		CancelCoroutine(queueing);
		queueing = StartCoroutine(QueueingDoubleShutEye(timeStarted, beat));
	}

	private IEnumerator QueueingDoubleShutEye(float timeStarted, int beat)
	{
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayWooshDelayed(checkpoint, isFullBeat: false);
		Eye.CloseDelayed(checkpoint);
		Eye.ToggleIsDoubled(toggle: true);
		Spiral[] spirals = Spirals;
		for (int num = 0; num < spirals.Length; num++)
		{
			spirals[num].ToggleIsDoubled(toggle: true);
		}
		GetDeactivatedPocketWatch().CrossInDelayed(checkpoint, beat);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Eye.CloseDelayed(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Eye.CloseDelayed(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.25f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Eye.ToggleIsDoubled(toggle: false);
		spirals = Spirals;
		for (int num = 0; num < spirals.Length; num++)
		{
			spirals[num].ToggleIsDoubled(toggle: false);
		}
	}

	public void Pulse(float accuracy)
	{
		Spiral[] spirals = Spirals;
		for (int i = 0; i < spirals.Length; i++)
		{
			spirals[i].Spin();
		}
		Eye.Pulse(accuracy);
		pulsers[queuedEyeSilhouette].TriggerAnim("pulse", GetSpeed() / 2f);
		if (accuracy != 1f)
		{
			if (accuracy == 0.332f)
			{
				eyeShapes[queuedEyeSilhouette].SetSpriteColor(earlyColor);
			}
			else
			{
				eyeShapes[queuedEyeSilhouette].SetSpriteColor(lateColor);
			}
		}
		else
		{
			eyeShapes[queuedEyeSilhouette].SetSpriteColor(goodColor);
		}
		queuedEyeSilhouette++;
		if (queuedEyeSilhouette >= pulsers.Length)
		{
			queuedEyeSilhouette = 0;
		}
	}

	public void PlayClick()
	{
		speakers[0].TriggerSound(0);
	}

	public void PlayBlip()
	{
		speakers[0].TriggerSound(1);
	}

	public void PlayWooshDelayed(float timeStarted, bool isFullBeat)
	{
		if (isFullBeat)
		{
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 2);
		}
		else
		{
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 3);
		}
	}

	public void RefreshSpeed(float delta, int beat)
	{
		StartCoroutine(RefreshingSpeed(delta, beat));
	}

	private IEnumerator RefreshingSpeed(float delta, int beat)
	{
		CancelCoroutine(bobbling);
		Eye.SetSpeed(GetSpeed());
		Roller.SetSpeed(GetSpeed());
		Bobble(delta, beat);
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		PocketWatches[0].SetSpeed(GetSpeed());
	}

	public void EnterPov()
	{
		CancelCoroutine(leavingPov);
		pover.TriggerAnim("enter", GetSpeed() * 0.25f);
		Interface.env.Cam.ToggleIsGlowable(toggle: false);
	}

	public void DroopPovEye()
	{
		CancelCoroutine(leavingPov);
		pover.TriggerAnim("droop", GetSpeed() * 0.25f);
		Interface.env.Cam.ToggleIsGlowable(toggle: false);
	}

	public void ShutPovEye()
	{
		CancelCoroutine(leavingPov);
		pover.TriggerAnim("shut", GetSpeed() * 0.25f);
		Interface.env.Cam.ToggleIsGlowable(toggle: false);
	}

	public void UnshutPovEye()
	{
		CancelCoroutine(leavingPov);
		pover.TriggerAnim("unshut", GetSpeed() * 0.25f);
		Interface.env.Cam.ToggleIsGlowable(toggle: false);
	}

	public void UndroopPovEye()
	{
		CancelCoroutine(leavingPov);
		pover.TriggerAnim("undroop", GetSpeed() * 0.25f);
		Interface.env.Cam.ToggleIsGlowable(toggle: false);
	}

	public void LeavePov()
	{
		CancelCoroutine(leavingPov);
		leavingPov = StartCoroutine(LeavingPov());
	}

	private IEnumerator LeavingPov()
	{
		pover.TriggerAnim("leave", GetSpeed() * 0.25f);
		Interface.env.Cam.ToggleIsGlowable(toggle: false);
		yield return new WaitForSeconds(leaveDuration / (GetSpeed() * 0.25f));
		Interface.env.Cam.ToggleIsGlowable(toggle: true);
	}

	public PocketWatch GetDeactivatedPocketWatch()
	{
		PocketWatch[] pocketWatches = PocketWatches;
		foreach (PocketWatch pocketWatch in pocketWatches)
		{
			if (!pocketWatch.CheckIsActivated())
			{
				return pocketWatch;
			}
		}
		return null;
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
