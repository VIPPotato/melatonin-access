using UnityEngine;

public class McLifter : Wrapper
{
	[Header("Children")]
	public Sweat Sweat;

	public Feedback[] Feedbacks;

	private bool isLifting;

	private int activeLiftNum;

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

	public void Lift(int newLiftNum)
	{
		isLifting = true;
		speakers[0].CancelSound(0);
		speakers[0].CancelSound(1);
		speakers[0].CancelSound(2);
		speakers[0].TriggerSoundStack(3);
		switch (newLiftNum)
		{
		case 1:
			if (activeLiftNum == 2)
			{
				sprites[0].TriggerAnim("lift3");
				activeLiftNum = 3;
			}
			else
			{
				sprites[0].TriggerAnim("lift1");
				activeLiftNum = 1;
			}
			break;
		case 2:
			if (activeLiftNum == 1)
			{
				sprites[0].TriggerAnim("lift3");
				activeLiftNum = 3;
			}
			else
			{
				sprites[0].TriggerAnim("lift2");
				activeLiftNum = 2;
			}
			break;
		}
		sprites[1].TriggerAnim("action");
		speakers[0].TriggerSound(activeLiftNum - 1);
	}

	public void Unlift(int unliftNum)
	{
		isLifting = false;
		switch (unliftNum)
		{
		case 1:
			if (activeLiftNum == 1)
			{
				sprites[0].TriggerAnim("idled");
				activeLiftNum = 0;
			}
			else
			{
				sprites[0].TriggerAnim("lift2");
				activeLiftNum = 2;
			}
			break;
		case 2:
			if (activeLiftNum == 2)
			{
				sprites[0].TriggerAnim("idled");
				activeLiftNum = 0;
			}
			else
			{
				sprites[0].TriggerAnim("lift1");
				activeLiftNum = 1;
			}
			break;
		}
		sprites[1].TriggerAnim("idle");
	}
}
