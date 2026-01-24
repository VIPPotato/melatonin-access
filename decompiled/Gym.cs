using System.Collections;
using UnityEngine;

public class Gym : Wrapper
{
	public static Gym env;

	[Header("Children")]
	public McLifter McLifter;

	public Trainer Trainer;

	[Header("Fragments")]
	public Fragment parallaxer;

	public Fragment hoopBobbler;

	public Fragment bagBobber;

	public Fragment woofers;

	public Fragment pulldown;

	public Fragment chairFace;

	private bool isActivated;

	private bool isParallaxing;

	private float timer;

	private Coroutine bobbling;

	private Coroutine shiftingFocus;

	private const float animTempo = 90f;

	protected override void Awake()
	{
		env = this;
		parallaxer.Awake();
		hoopBobbler.Awake();
		bagBobber.Awake();
		woofers.Awake();
		pulldown.Awake();
		chairFace.Awake();
		RenderChildren(toggle: false, 1);
	}

	public void Show(bool isTrainerFocused = false)
	{
		timer = 0f;
		isActivated = true;
		RenderChildren(toggle: true, 1);
		Trainer.Show();
		McLifter.Show();
		DreamWorld.env.SetFeedbacks(McLifter.Feedbacks);
		if (isTrainerFocused)
		{
			Interface.env.Cam.SetPosition(Trainer.GetPosition().x, 0f);
		}
		else
		{
			Interface.env.Cam.SetPosition(0f, 0f);
		}
		isParallaxing = true;
		Parallax();
	}

	public void Hide()
	{
		isParallaxing = false;
		isActivated = false;
		Trainer.Hide();
		McLifter.Hide();
		CancelCoroutine(bobbling);
		CancelCoroutine(shiftingFocus);
		RenderChildren(toggle: false, 1);
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 0.033f)
		{
			Parallax();
			timer = 0f;
		}
	}

	private void Parallax()
	{
		if (isParallaxing)
		{
			float x = Interface.env.Cam.GetX();
			float newX = x / 8f * -1f;
			if (x != 0f)
			{
				parallaxer.SetPosition(newX, parallaxer.GetY());
			}
			else
			{
				parallaxer.SetPosition(x, parallaxer.GetY());
			}
		}
	}

	public void BobbleDelayed(float delta, int beatNum, bool isHitWindow)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta, beatNum, isHitWindow));
	}

	private IEnumerator BobblingDelayed(float delta, int beatNum, bool isHitWindow)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		if (beatNum > 0)
		{
			hoopBobbler.TriggerAnim(beatNum.ToString() ?? "");
		}
		switch (beatNum)
		{
		case 2:
			bagBobber.TriggerAnim("1");
			break;
		case 4:
			bagBobber.TriggerAnim("2");
			break;
		}
		woofers.TriggerAnim("woof");
		Trainer.Bobble();
		if (!isHitWindow)
		{
			McLifter.Bobble();
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		Trainer.Idle();
		if (!isHitWindow)
		{
			McLifter.Idle();
		}
	}

	public void PulldownDelayed(float timeStarted)
	{
		StartCoroutine(PullingDownDelayed(timeStarted));
	}

	private IEnumerator PullingDownDelayed(float timeStarted)
	{
		bool isPulled = pulldown.CheckIsAnimPlaying("pull");
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		float checkpoint = timeStarted + 0.11667f - 1f / 24f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Interface.env.Cam.Sway();
		if (isPulled)
		{
			pulldown.TriggerAnim("reset");
			chairFace.TriggerAnim("show");
		}
		else
		{
			pulldown.TriggerAnim("pull");
			chairFace.TriggerAnim("hide");
		}
	}

	public void ShiftFocusDelayed(float timeStarted, int focusNum)
	{
		CancelCoroutine(shiftingFocus);
		shiftingFocus = StartCoroutine(ShiftingFocusDelayed(timeStarted, focusNum));
	}

	private IEnumerator ShiftingFocusDelayed(float timeStarted, int focusNum)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		switch (focusNum)
		{
		case 0:
			Interface.env.Cam.MoveToTarget(new Vector3(0f, 0f, 0f), 3.5f * GetSpeed());
			break;
		case 1:
			Interface.env.Cam.MoveToTarget(Trainer.GetPosition(), 3.5f * GetSpeed());
			break;
		case 2:
			Interface.env.Cam.MoveToTarget(McLifter.GetPosition(), 3.5f * GetSpeed());
			break;
		}
	}

	public void CancelShiftFocus()
	{
		CancelCoroutine(shiftingFocus);
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
