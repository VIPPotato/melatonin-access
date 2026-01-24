using System.Collections;
using UnityEngine;

public class WorkMessage : Wrapper
{
	[Header("Fragments")]
	public Fragment fader;

	public Fragment arrow;

	public Fragment content;

	public Fragment cursorMover;

	public spriteFragment cursor;

	private int contentNum;

	private float timeTilOut;

	protected override void Awake()
	{
		fader.Awake();
		arrow.Awake();
		content.Awake();
		cursor.Initiate();
		timeTilOut = fader.GetAnimDuration("slideOut");
		RenderChildren(toggle: false);
	}

	public void ActivateDelayed(float timeStarted, int newContentNum, int beatsTilHit)
	{
		StartCoroutine(ActivatingDelayed(timeStarted, newContentNum, beatsTilHit));
	}

	private IEnumerator ActivatingDelayed(float timeStarted, int newContentNum, int beatsTilHit)
	{
		contentNum = newContentNum;
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		RenderChildren(toggle: true);
		arrow.TriggerAnim("awaiting");
		cursorMover.TriggerAnim("awaiting");
		content.TriggerAnim(contentNum.ToString() ?? "");
		if (beatsTilHit == 2)
		{
			fader.TriggerAnim("inToHalf");
			checkpoint += MusicBox.env.GetSecsPerBeat();
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			fader.TriggerAnim("inFromHalf");
		}
		else
		{
			fader.TriggerAnim("in");
		}
		arrow.TriggerAnim("point", OfficeSpace.env.GetSpeed());
		cursorMover.TriggerAnim(contentNum.ToString() ?? "", OfficeSpace.env.GetSpeed());
		if (contentNum <= 1)
		{
			cursor.SetState(0);
		}
		else if (contentNum <= 3)
		{
			cursor.SetState(1);
		}
		else
		{
			cursor.SetState(2);
		}
	}

	public void ScaleOut(float accuracy)
	{
		StartCoroutine(ScalingOut(accuracy));
	}

	private IEnumerator ScalingOut(float accuracy)
	{
		fader.TriggerAnim("scaleOut");
		arrow.TriggerAnim("unpoint");
		cursorMover.TriggerAnim("awaiting");
		if (accuracy != 1f)
		{
			if (accuracy != 0.332f)
			{
				if (accuracy == 0.333f)
				{
					content.TriggerAnim(contentNum + "late");
				}
			}
			else
			{
				content.TriggerAnim(contentNum + "early");
			}
		}
		else
		{
			content.TriggerAnim(contentNum + "perfect");
		}
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public void SlideOut()
	{
		StartCoroutine(SlidingOut());
	}

	private IEnumerator SlidingOut()
	{
		fader.TriggerAnim("slideOut");
		arrow.TriggerAnim("unpoint");
		cursorMover.TriggerAnim("awaiting");
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}
}
