using System.Collections;
using UnityEngine;

public class DateCard : Wrapper
{
	private int hotOrNotNum;

	private bool isLastCue;

	private bool isActivated;

	private float timeTilOut;

	private Coroutine deactivating;

	protected override void Awake()
	{
		SetupFragments();
		timeTilOut = gears[1].GetAnimDuration("out") * 2f;
		RenderChildren(toggle: false);
	}

	public void Activate1(int bioNum)
	{
		isActivated = true;
		isLastCue = false;
		RenderChildren(toggle: true);
		gears[1].TriggerAnim("shown");
		gears[2].TriggerAnim("awaiting");
		gears[3].TriggerAnim("awaiting");
		sprites[0].TriggerAnim(bioNum.ToString() ?? "");
		sprites[0].SetSpriteColor(new Color(1f, 1f, 1f));
		sprites[1].TriggerAnim("load1");
		SetLocalZ(3f);
	}

	public void Activate2()
	{
		sprites[1].TriggerAnim("load2");
		if (hotOrNotNum == 1)
		{
			gears[3].TriggerAnim("hintLeft");
		}
		else
		{
			gears[3].TriggerAnim("hintRight");
		}
	}

	public void Activate3()
	{
		isLastCue = true;
		sprites[1].TriggerAnim("load3");
		SetLocalZ(0f);
	}

	public void Deactivate(string motionType, float accuracy)
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating(motionType, accuracy));
	}

	private IEnumerator Deactivating(string motionType, float accuracy)
	{
		isLastCue = false;
		isActivated = false;
		gears[1].TriggerAnim("out");
		gears[2].TriggerAnim(motionType);
		switch (motionType)
		{
		case "swipeLeft":
		case "swipeRight":
			speakers[0].TriggerSound(0);
			break;
		case "nudgeLeft":
		case "nudgeRight":
			speakers[0].TriggerSound(1);
			break;
		}
		if (accuracy != 1f)
		{
			if (accuracy != 0.332f)
			{
				if (accuracy == 0.333f)
				{
					sprites[0].SetSpriteColor(new Color(1f, 0.937f, 0.945f));
				}
			}
			else
			{
				sprites[0].SetSpriteColor(new Color(0.9997f, 1f, 0.929f));
			}
		}
		else
		{
			sprites[0].SetSpriteColor(new Color(0.938f, 0.993f, 1f));
		}
		yield return new WaitForSeconds(timeTilOut);
		RenderChildren(toggle: false);
	}

	public void Hide()
	{
		CancelCoroutine(deactivating);
		isLastCue = false;
		isActivated = false;
		RenderChildren(toggle: false);
	}

	public void NudgeLeft()
	{
		if (isActivated)
		{
			gears[2].TriggerAnim("nudgeLeft");
			speakers[0].TriggerSound(1);
		}
	}

	public void NudgeRight()
	{
		if (isActivated)
		{
			gears[2].TriggerAnim("nudgeRight");
			speakers[0].TriggerSound(1);
		}
	}

	public void SetHotOrNotNum(int newHotOrNotNum)
	{
		hotOrNotNum = newHotOrNotNum;
	}

	public bool CheckIsLastCue()
	{
		return isLastCue;
	}
}
