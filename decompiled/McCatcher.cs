using System.Collections;
using UnityEngine;

public class McCatcher : Wrapper
{
	[Header("Children")]
	public Sweat Sweat;

	public Feedback[] Feedbacks;

	[Header("Fragments")]
	public Fragment eyesMover;

	public Fragment body;

	public Fragment eyes;

	private int activeArmNum;

	private bool isGrabbing;

	private bool isPocketing;

	private bool isDoubleGrabReversed;

	private float grabDuration;

	private float pocketDuration;

	private Coroutine grabbing;

	private Coroutine pocketing;

	private Coroutine hidingEyes;

	protected override void Awake()
	{
		eyesMover.Awake();
		body.Awake();
		eyes.Awake();
		grabDuration = body.GetAnimDuration("grab");
		pocketDuration = body.GetAnimDuration("pocket");
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		eyesMover.CheckIsAnimPlaying("idledLeft");
		body.TriggerAnim("idled");
		eyes.ToggleSpriteRenderer(toggle: true);
		Sweat.Hide();
	}

	public void Hide()
	{
		activeArmNum = 0;
		isGrabbing = false;
		isPocketing = false;
		CancelCoroutine(pocketing);
		CancelCoroutine(grabbing);
		CancelCoroutine(hidingEyes);
		Sweat.Hide();
		Feedback[] feedbacks = Feedbacks;
		for (int i = 0; i < feedbacks.Length; i++)
		{
			feedbacks[i].Hide();
		}
		RenderChildren(toggle: false);
	}

	public void MoveEyes()
	{
		if (eyesMover.CheckIsAnimPlaying("idledLeft"))
		{
			eyesMover.TriggerAnim("moveRight");
		}
		else
		{
			eyesMover.TriggerAnim("moveLeft");
		}
	}

	public void Grab(int newActiveArmNum)
	{
		if (isPocketing)
		{
			activeArmNum = 0;
			CancelCoroutine(pocketing);
			isPocketing = false;
		}
		CancelCoroutine(grabbing);
		grabbing = StartCoroutine(Grabbing(newActiveArmNum));
	}

	private IEnumerator Grabbing(int newActiveArmNum)
	{
		HideEyes(grabDuration);
		isGrabbing = true;
		if (activeArmNum == 0 || newActiveArmNum == activeArmNum)
		{
			activeArmNum = newActiveArmNum;
			body.TriggerAnim("grab");
			if (activeArmNum == 1)
			{
				body.ToggleSpriteFlip(toggle: false);
			}
			else if (activeArmNum == 2)
			{
				body.ToggleSpriteFlip(toggle: true);
			}
		}
		else
		{
			activeArmNum = 3;
			body.TriggerAnim("grabBoth");
		}
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat() / 2f);
		activeArmNum = 0;
		isGrabbing = false;
	}

	public void Pocket(int newActiveArmNum)
	{
		if (isGrabbing)
		{
			activeArmNum = 0;
			CancelCoroutine(grabbing);
			isGrabbing = false;
		}
		CancelCoroutine(pocketing);
		pocketing = StartCoroutine(Pocketing(newActiveArmNum));
	}

	private IEnumerator Pocketing(int newActiveArmNum)
	{
		HideEyes(pocketDuration);
		isPocketing = true;
		if (activeArmNum == 0 || newActiveArmNum == activeArmNum)
		{
			activeArmNum = newActiveArmNum;
			body.TriggerAnim("pocket");
			if (activeArmNum == 1)
			{
				body.ToggleSpriteFlip(toggle: false);
			}
			else if (activeArmNum == 2)
			{
				body.ToggleSpriteFlip(toggle: true);
			}
		}
		else
		{
			activeArmNum = 3;
			body.TriggerAnim("pocketBoth");
			if (isDoubleGrabReversed)
			{
				isDoubleGrabReversed = false;
				body.ToggleSpriteFlip(toggle: false);
			}
			else
			{
				isDoubleGrabReversed = true;
				body.ToggleSpriteFlip(toggle: true);
			}
		}
		yield return new WaitForSeconds(MusicBox.env.GetSecsPerBeat() / 2f);
		activeArmNum = 0;
		isPocketing = false;
	}

	private void HideEyes(float duration)
	{
		CancelCoroutine(hidingEyes);
		hidingEyes = StartCoroutine(HidingEyes(duration));
	}

	private IEnumerator HidingEyes(float duration)
	{
		eyes.ToggleSpriteRenderer(toggle: false);
		yield return new WaitForSeconds(duration);
		eyes.ToggleSpriteRenderer(toggle: true);
	}
}
