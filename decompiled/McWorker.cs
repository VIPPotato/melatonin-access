using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McWorker : Wrapper
{
	public WorkMessage[] WorkMessages;

	private List<WorkMessage> WorkMessages_active = new List<WorkMessage>();

	[Header("Fragments")]
	public Fragment worker;

	public spriteFragment feedback;

	public spriteFragment flash;

	public spriteFragment screen;

	private bool isAsleep;

	private int activeHand;

	private int spawnNum;

	private float typeDuration;

	private Coroutine typing;

	private Coroutine feedbacking;

	protected override void Awake()
	{
		worker.Awake();
		feedback.Initiate();
		flash.Initiate();
		screen.Initiate();
		typeDuration = worker.GetAnimDuration("left") / 2f;
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		feedback.ToggleSpriteRenderer(toggle: false);
		flash.ToggleSpriteRenderer(toggle: false);
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		activeHand = 0;
		CancelCoroutine(typing);
		CancelCoroutine(feedbacking);
		WorkMessages_active.Clear();
		WorkMessage[] workMessages = WorkMessages;
		for (int i = 0; i < workMessages.Length; i++)
		{
			workMessages[i].Hide();
		}
		RenderChildren(toggle: false);
	}

	public void BobbleDelayed(float delta, int screenNum)
	{
		screen.SetStateDelayedDelta(delta, screenNum - 1);
	}

	public void SpawnMessageDelayed(float timeStarted, int content, int beatsTilHit)
	{
		if (WorkMessages.Length != 0)
		{
			WorkMessages[spawnNum].ActivateDelayed(timeStarted, content, beatsTilHit);
			WorkMessages_active.Add(WorkMessages[spawnNum]);
			spawnNum++;
			if (spawnNum == WorkMessages.Length)
			{
				spawnNum = 0;
			}
		}
	}

	public void Sleep(float delta, float speed)
	{
		isAsleep = true;
		worker.TriggerAnimDelayedDelta(delta, "sleep", speed);
	}

	public void Slept()
	{
		isAsleep = true;
		worker.TriggerAnim("sleep", 1f, 1f);
	}

	public void Wake(float delta)
	{
		if (isAsleep)
		{
			isAsleep = false;
			worker.TriggerAnimDelayedDelta(delta, "wake");
		}
	}

	public void Type(int side)
	{
		CancelCoroutine(typing);
		typing = StartCoroutine(Typing(side));
	}

	private IEnumerator Typing(int side)
	{
		if ((activeHand == 0 || activeHand == 1) && side == 1)
		{
			activeHand = 1;
			worker.TriggerAnim("left");
		}
		else if ((activeHand == 0 || activeHand == 2) && side == 2)
		{
			activeHand = 2;
			worker.TriggerAnim("right");
		}
		else if (activeHand == 1 && side == 2)
		{
			activeHand = 0;
			worker.TriggerAnim("both");
		}
		else if (activeHand == 2 && side == 1)
		{
			activeHand = 0;
			worker.TriggerAnim("both");
		}
		yield return new WaitForSeconds(typeDuration);
		activeHand = 0;
	}

	public void HitActiveWorkMessage(float accuracy)
	{
		CancelCoroutine(feedbacking);
		feedbacking = StartCoroutine(HittingActiveWorkMessage(accuracy));
	}

	private IEnumerator HittingActiveWorkMessage(float accuracy)
	{
		if (WorkMessages.Length != 0)
		{
			WorkMessages_active[0].ScaleOut(accuracy);
			WorkMessages_active.RemoveAt(0);
		}
		feedback.ToggleSpriteRenderer(toggle: true);
		flash.ToggleSpriteRenderer(toggle: true);
		if (accuracy == 1f)
		{
			feedback.SetState(SaveManager.GetLang());
			flash.SetSpriteColor(new Color(0.91f, 1f, 1f));
		}
		else if (accuracy == 0.332f)
		{
			feedback.SetState(SaveManager.GetLang() + 10);
			flash.SetSpriteColor(new Color(1f, 0.969f, 0.933f));
		}
		else if (accuracy == 0.333f)
		{
			feedback.SetState(SaveManager.GetLang() + 20);
			flash.SetSpriteColor(new Color(1f, 0.875f, 0.973f));
		}
		yield return new WaitForSeconds(0.25f);
		feedback.ToggleSpriteRenderer(toggle: false);
		flash.ToggleSpriteRenderer(toggle: false);
	}

	public void StrikeActiveWorkMessage()
	{
		CancelCoroutine(feedbacking);
		feedbacking = StartCoroutine(StrikingActiveWorkMessage());
	}

	private IEnumerator StrikingActiveWorkMessage()
	{
		feedback.ToggleSpriteRenderer(toggle: false);
		flash.ToggleSpriteRenderer(toggle: true);
		flash.SetSpriteColor(new Color(1f, 0.875f, 0.973f));
		yield return new WaitForSeconds(0.25f);
		flash.ToggleSpriteRenderer(toggle: false);
	}

	public void MissActiveWorkMessage()
	{
		CancelCoroutine(feedbacking);
		feedbacking = StartCoroutine(MissingActiveWorkMessage());
	}

	private IEnumerator MissingActiveWorkMessage()
	{
		if (WorkMessages.Length != 0)
		{
			WorkMessages_active[0].SlideOut();
			WorkMessages_active.RemoveAt(0);
		}
		feedback.ToggleSpriteRenderer(toggle: false);
		flash.ToggleSpriteRenderer(toggle: true);
		flash.SetSpriteColor(new Color(1f, 0.875f, 0.973f));
		yield return new WaitForSeconds(0.25f);
		flash.ToggleSpriteRenderer(toggle: false);
	}
}
