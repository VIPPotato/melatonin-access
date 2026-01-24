using System.Collections;
using UnityEngine;

public class Trainer : Wrapper
{
	private bool isLifting;

	private Coroutine lifting;

	protected override void Awake()
	{
		SetupFragments();
	}

	public void Show()
	{
		sprites[0].TriggerAnim("idled");
		sprites[1].TriggerAnim("idled");
	}

	public void Hide()
	{
		isLifting = false;
	}

	public void Bobble()
	{
		if (!isLifting)
		{
			sprites[1].TriggerAnim("beat");
		}
	}

	public void Idle()
	{
		if (!isLifting)
		{
			sprites[1].TriggerAnim("idle");
		}
	}

	public void LiftDelayed(float timeStarted, bool isFullBeat, int liftNum)
	{
		CancelCoroutine(lifting);
		lifting = StartCoroutine(LiftingDelayed(timeStarted, isFullBeat, liftNum));
	}

	private IEnumerator LiftingDelayed(float timeStarted, bool isFullBeat, int liftNum)
	{
		isLifting = true;
		sprites[0].TriggerAnim("idled");
		sprites[1].TriggerAnim("idled");
		speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 3);
		if (isFullBeat)
		{
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, liftNum - 1);
		}
		else
		{
			speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, liftNum - 1);
		}
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		sprites[0].TriggerAnim("lift" + liftNum);
		sprites[1].TriggerAnim("lift");
		checkpoint += MusicBox.env.GetSecsPerBeat() / 2f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		isLifting = false;
		sprites[0].TriggerAnim("idled");
		sprites[1].TriggerAnim("idle");
	}

	public void LookAhead()
	{
		sprites[0].TriggerAnim("idled");
		sprites[1].TriggerAnim("idled");
	}

	public void LookLeft()
	{
		sprites[0].TriggerAnim("idled");
		sprites[1].TriggerAnim("look");
	}
}
