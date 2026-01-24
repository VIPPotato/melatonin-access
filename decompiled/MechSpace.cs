using System.Collections;
using UnityEngine;

public class MechSpace : Wrapper
{
	public static MechSpace env;

	[Header("Children")]
	public Crosshair Crosshair;

	public SpaceOppWrapper[] SpaceOppWrappers;

	public McVirtual McVirtual;

	public Feedback[] Feedbacks;

	public RoomHolos RoomHolos;

	private bool isActivated;

	private bool isStoppedAiming;

	private Coroutine triggeringEnemy;

	private Coroutine bobbling;

	private Coroutine blinding;

	private const float animTempo = 90f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false, 3);
	}

	public void Show()
	{
		isActivated = true;
		isStoppedAiming = false;
		RenderChildren(toggle: true, 3);
		sprites[0].TriggerAnim("hidden");
		sprites[1].TriggerAnim("hidden");
		sprites[2].TriggerAnim("idled");
		Interface.env.Cam.SetPosition(0f, 0f);
		SpaceOppWrapper[] spaceOppWrappers = SpaceOppWrappers;
		for (int i = 0; i < spaceOppWrappers.Length; i++)
		{
			spaceOppWrappers[i].Hide();
		}
		McVirtual.Show();
		Crosshair.Activate();
		DreamWorld.env.SetFeedbacks(Feedbacks);
		RoomHolos.Show();
	}

	public void Hide()
	{
		isActivated = false;
		McVirtual.Hide();
		RoomHolos.Hide();
		CancelCoroutine(triggeringEnemy);
		CancelCoroutine(bobbling);
		CancelCoroutine(blinding);
		RenderChildren(toggle: false, 3);
	}

	public void TriggerEnemyDelayed(float timeStarted, int phrase, int beat, bool isFullBeat, bool isAltSound, int monsterNum = 0)
	{
		triggeringEnemy = StartCoroutine(TriggeringEnemyDelayed(timeStarted, phrase, beat, isFullBeat, isAltSound, monsterNum));
	}

	private IEnumerator TriggeringEnemyDelayed(float timeStarted, int phrase, int beat, bool isFullBeat, bool isAltSound, int monsterNum = 0)
	{
		int num = beat - 1;
		if (phrase >= 4)
		{
			num = ((num - 1 < 0) ? 3 : (num - 1));
			num = ((num - 1 < 0) ? 3 : (num - 1));
		}
		int oppNum = 0;
		if (isFullBeat)
		{
			speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, num);
			switch (beat)
			{
			case 1:
				oppNum = 0;
				break;
			case 2:
				oppNum = 2;
				break;
			case 3:
				oppNum = 4;
				break;
			}
		}
		else
		{
			speakers[2].TriggerSoundDelayedTimeStarted(timeStarted, num);
			switch (beat)
			{
			case 1:
				oppNum = 1;
				break;
			case 2:
				oppNum = 3;
				break;
			case 3:
				oppNum = 5;
				break;
			}
		}
		if (isAltSound)
		{
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		}
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		SpaceOppWrappers[oppNum].Activate(monsterNum);
		checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		SpaceOppWrappers[oppNum].AttractFeedback();
	}

	public void Flash()
	{
		sprites[0].TriggerAnim("flash");
		sprites[1].TriggerAnim("flash");
		speakers[0].TriggerSoundStack(2);
	}

	public void BobbleDelayed(float delta, bool isHitWindow)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta, isHitWindow));
	}

	private IEnumerator BobblingDelayed(float delta, bool isHitWindow)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		if (!isHitWindow)
		{
			sprites[2].TriggerAnim("travel");
		}
		McVirtual.Bobble();
		Crosshair.Spin();
		RoomHolos.Scroll(delta);
	}

	public void Blind(float timeStarted)
	{
		CancelCoroutine(blinding);
		blinding = StartCoroutine(Blinding(timeStarted));
	}

	private IEnumerator Blinding(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		sprites[3].TriggerAnim("in");
		Crosshair.Alert();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Crosshair.Alert();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Crosshair.Alert();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Crosshair.Alert();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isStoppedAiming = true;
		sprites[3].TriggerAnim("out");
		Crosshair.Hide();
		McVirtual.Idle();
	}

	public void Unblind(float delta, int beat)
	{
		CancelCoroutine(blinding);
		StartCoroutine(Unblinding(delta, beat));
	}

	private IEnumerator Unblinding(float delta, int beat)
	{
		isStoppedAiming = false;
		CancelCoroutine(blinding);
		sprites[3].TriggerAnim("hidden");
		if (beat == 1)
		{
			Crosshair.AimDelayed(delta);
			McVirtual.AimDelayed(delta);
		}
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		Crosshair.Activate();
	}

	public void ToggleIsStoppedAiming(float delta, bool toggle)
	{
		isStoppedAiming = toggle;
		if (isStoppedAiming)
		{
			Crosshair.Idle();
			McVirtual.Idle();
		}
		else
		{
			Crosshair.AimDelayed(delta);
			McVirtual.AimDelayed(delta);
		}
	}

	public void ToggleIsStoppedAimingDelayed(float delta, bool toggle)
	{
		StartCoroutine(TogglingIsStoppedAimingDelayed(delta, toggle));
	}

	private IEnumerator TogglingIsStoppedAimingDelayed(float delta, bool toggle)
	{
		float checkpoint = Technician.mgr.GetDspTime() + MusicBox.env.GetSecsPerBeat() / 2f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		isStoppedAiming = toggle;
		if (isStoppedAiming)
		{
			Crosshair.Idle();
			McVirtual.Idle();
		}
		else
		{
			Crosshair.AimDelayed(delta);
			McVirtual.AimDelayed(delta);
		}
	}

	public void ShootSound()
	{
		speakers[0].TriggerSoundStack(1);
	}

	public void StartSoundDelayed(int soundNum)
	{
		speakers[0].TriggerSoundDelayedDelta(0f, 2 + soundNum);
	}

	public bool CheckIsStoppedAiming()
	{
		return isStoppedAiming;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 90f;
	}
}
