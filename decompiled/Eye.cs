using System.Collections;
using UnityEngine;

public class Eye : Wrapper
{
	public Fragment eyeball;

	public Fragment looker;

	private float speed;

	private bool isClosed;

	private bool isDoubled;

	private Coroutine closing;

	protected override void Awake()
	{
		SetupFragments();
		eyeball.Awake();
		looker.Awake();
		RenderChildren(toggle: false);
	}

	public void Show(bool isRemix)
	{
		isClosed = false;
		speed = HypnoLair.env.GetSpeed();
		RenderChildren(toggle: true);
		if (isRemix)
		{
			gears[0].TriggerAnim("idled");
		}
		else
		{
			gears[0].TriggerAnim("open", speed / 4f);
		}
	}

	public void Hide()
	{
		CancelCoroutine(closing);
		RenderChildren(toggle: false);
	}

	public void CloseDelayed(float timeStarted)
	{
		closing = StartCoroutine(ClosingeDelayed(timeStarted));
	}

	private IEnumerator ClosingeDelayed(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDoubled)
		{
			checkpoint += MusicBox.env.GetSecsPerBeat() * 0.25f;
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			if (!isClosed)
			{
				isClosed = true;
				gears[0].TriggerAnim("close", speed * 4f);
			}
		}
		else if (!isClosed)
		{
			isClosed = true;
			gears[0].TriggerAnim("close", speed * 2f);
		}
	}

	public void Pulse(float accuracy)
	{
		isClosed = false;
		if (isDoubled)
		{
			gears[0].TriggerAnim("pulse", speed * 2f);
		}
		else
		{
			gears[0].TriggerAnim("pulse", speed);
		}
		if (accuracy != 1f)
		{
			if (accuracy == 0.332f)
			{
				eyeball.TriggerAnim("flashYellow");
			}
			else
			{
				eyeball.TriggerAnim("flashRed");
			}
		}
		else
		{
			eyeball.TriggerAnim("flashBlue");
		}
	}

	public void LookLeft()
	{
		looker.TriggerAnim("lookLeft", speed);
	}

	public void LookRight()
	{
		looker.TriggerAnim("lookRight", speed);
	}

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	public void ToggleIsDoubled(bool toggle)
	{
		isDoubled = toggle;
	}
}
