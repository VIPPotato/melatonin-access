using System.Collections;
using UnityEngine;

public class PocketWatch : Wrapper
{
	public Fragment swinger;

	public Fragment rotator;

	public Fragment spiral;

	private float speed;

	private bool isActivated;

	private bool isCamTracked;

	private bool isSwinging;

	private Coroutine crossing;

	protected override void Awake()
	{
		swinger.Awake();
		rotator.Awake();
		spiral.Awake();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		isActivated = true;
		isCamTracked = true;
		isSwinging = true;
		speed = HypnoLair.env.GetSpeed();
		RenderChildren(toggle: true);
		rotator.TriggerAnim("spiraling", speed);
	}

	public void CrossInDelayed(float timeStarted, int beat)
	{
		CancelCoroutine(crossing);
		crossing = StartCoroutine(CrossingInDelayed(timeStarted, beat));
	}

	private IEnumerator CrossingInDelayed(float timeStarted, int beat)
	{
		isActivated = true;
		isCamTracked = false;
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isSwinging = true;
		speed = HypnoLair.env.GetSpeed();
		RenderChildren(toggle: true);
		rotator.TriggerAnim("spiraling", speed);
		if (beat == 1 || beat == 3)
		{
			swinger.TriggerAnim("cross", speed / 2f);
		}
		else
		{
			swinger.TriggerAnim("crossReversed", speed / 2f);
		}
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isActivated = false;
		isSwinging = false;
		isCamTracked = false;
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		isActivated = false;
		isCamTracked = false;
		isSwinging = false;
		CancelCoroutine(crossing);
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (isCamTracked)
		{
			Interface.env.Cam.SetPosition(spiral.GetX() / 35f * -1f, spiral.GetY() / 22.5f * -1f);
		}
	}

	public void StartSwing()
	{
		swinger.TriggerAnim("start", speed);
	}

	public void SwingRight()
	{
		swinger.TriggerAnim("swingRight", speed);
	}

	public void SwingLeft()
	{
		swinger.TriggerAnim("swingLeft", speed);
	}

	public void EndSwingDelayed(float timeStarted)
	{
		StartCoroutine(EndingSwingDelayed(timeStarted));
	}

	private IEnumerator EndingSwingDelayed(float timeStarted)
	{
		isSwinging = false;
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 0.5f + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		swinger.TriggerAnim("startReversed", speed);
	}

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
		swinger.SetCurrentAnimSpeed(speed);
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public bool CheckIsSwinging()
	{
		return isSwinging;
	}
}
