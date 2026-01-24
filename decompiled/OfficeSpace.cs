using System.Collections;
using UnityEngine;

public class OfficeSpace : Wrapper
{
	public static OfficeSpace env;

	[Header("Children")]
	public McWorker[] McWorkers;

	public Sweat[] Sweats;

	private bool isActivated;

	private int cueStyle = 1;

	private const float animTempo = 120f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		RenderChildren(toggle: false, 2);
	}

	public void Show()
	{
		isActivated = true;
		RenderChildren(toggle: true, 2);
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].Show();
		}
		DreamWorld.env.ClearFeedbacks();
		Interface.env.Cam.SetPosition(0f, 0f);
	}

	public void Hide()
	{
		isActivated = false;
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].Hide();
		}
		RenderChildren(toggle: false, 2);
	}

	public void BobbleDelayed(float delta, int screenNum)
	{
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].BobbleDelayed(delta, screenNum);
		}
	}

	public void FirstLoop(bool isReverse)
	{
		if (isReverse)
		{
			gears[0].TriggerAnim("firstLoopReverse", GetSpeed());
		}
		else
		{
			gears[0].TriggerAnim("firstLoop", GetSpeed());
		}
	}

	public void Loop(float multiplier, bool isReverse)
	{
		if (isReverse)
		{
			gears[0].TriggerAnim("loopReverse", GetSpeed() * multiplier);
		}
		else
		{
			gears[0].TriggerAnim("loop", GetSpeed() * multiplier);
		}
	}

	public void LastLoop(bool isReverse)
	{
		if (isReverse)
		{
			gears[0].TriggerAnim("lastLoopReverse", GetSpeed());
		}
		else
		{
			gears[0].TriggerAnim("lastLoop", GetSpeed());
		}
	}

	public void TransitionLoop()
	{
		gears[0].TriggerAnim("transition");
	}

	public void SleepWorkers(float delta)
	{
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].Sleep(delta, GetSpeed() / 2f);
		}
	}

	public void SleptWorkers(float delta)
	{
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].Slept();
		}
	}

	public void WakeWorkers(float delta)
	{
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].Wake(delta);
		}
	}

	public void Type(int side)
	{
		speakers[1].TriggerSound(0);
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].Type(side);
		}
	}

	public void SendMessageDelayed(float timeStarted, int content, int beatsTilHit)
	{
		StartCoroutine(SendingMessageDelayed(timeStarted, content, beatsTilHit));
	}

	private IEnumerator SendingMessageDelayed(float timeStarted, int content, int beatsTilHit)
	{
		McWorker[] mcWorkers = McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].SpawnMessageDelayed(timeStarted, content, beatsTilHit);
		}
		switch (beatsTilHit)
		{
		case 1:
			PlayCueDelayed(timeStarted, content);
			break;
		case 2:
		{
			float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
			yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
			PlayCueDelayed(checkpoint, content);
			break;
		}
		}
	}

	public void SendMistake()
	{
		Sweat[] sweats = Sweats;
		for (int i = 0; i < sweats.Length; i++)
		{
			sweats[i].CrossIn();
		}
	}

	public void SendFeedback(int hitType)
	{
		if (hitType == 3)
		{
			speakers[1].TriggerSound(1);
		}
	}

	private void PlayCueDelayed(float timeStarted, int content)
	{
		switch (content)
		{
		case 0:
		case 1:
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 3 * cueStyle);
			break;
		case 2:
		case 3:
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 1 + 3 * cueStyle);
			break;
		default:
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, 2 + 3 * cueStyle);
			break;
		}
	}

	public void SetCueStyle(int value)
	{
		cueStyle = value;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 120f;
	}
}
