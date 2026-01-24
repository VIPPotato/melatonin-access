using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCloud : Wrapper
{
	[Header("Children")]
	public FallingMoney[] FallingMoneys;

	private bool isAltFeedbackQueued;

	private int cueSpeakerNum;

	private int spawnNum;

	private List<int> activeQueue = new List<int>();

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		spawnNum = 0;
		activeQueue.Clear();
		FallingMoney[] fallingMoneys = FallingMoneys;
		for (int i = 0; i < fallingMoneys.Length; i++)
		{
			fallingMoneys[i].Hide();
		}
		RenderChildren(toggle: false);
	}

	public void SpawnLeftMoneyDelayed(float timeStarted, bool isDrift)
	{
		StartCoroutine(SpawningLeftMoneyDelayed(timeStarted, isDrift));
	}

	private IEnumerator SpawningLeftMoneyDelayed(float timeStarted, bool isDrift)
	{
		speakers[cueSpeakerNum].SetSoundVolume(0, 1f);
		speakers[cueSpeakerNum].SetSoundPanning(0, -0.67f);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		if (isDrift)
		{
			speakers[2].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		}
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDrift)
		{
			FallingMoneys[spawnNum].CrossIn("dropRightLeft");
			activeQueue.Add(spawnNum);
			IncreaseSpawnNum();
		}
		else
		{
			FallingMoneys[spawnNum].CrossIn("dropLeft");
			activeQueue.Add(spawnNum);
			IncreaseSpawnNum();
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / 2f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[cueSpeakerNum].SetSoundPanning(2, -0.67f);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		if (isDrift)
		{
			speakers[2].SetSoundPanning(0, -0.67f);
			speakers[2].TriggerSoundDelayedTimeStarted(checkpoint, 0);
		}
		cueSpeakerNum++;
		if (cueSpeakerNum > 1)
		{
			cueSpeakerNum = 0;
		}
	}

	public void SpawnRightMoneyDelayed(float timeStarted, bool isDrift)
	{
		StartCoroutine(SpawningRightMoneyDelayed(timeStarted, isDrift));
	}

	private IEnumerator SpawningRightMoneyDelayed(float timeStarted, bool isDrift)
	{
		speakers[cueSpeakerNum].SetSoundVolume(1, 1f);
		speakers[cueSpeakerNum].SetSoundPanning(1, 0.67f);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		if (isDrift)
		{
			speakers[2].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		}
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (isDrift)
		{
			FallingMoneys[spawnNum].CrossIn("dropLeftRight");
			activeQueue.Add(spawnNum);
			IncreaseSpawnNum();
		}
		else
		{
			FallingMoneys[spawnNum].CrossIn("dropRight");
			activeQueue.Add(spawnNum);
			IncreaseSpawnNum();
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / 2f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[cueSpeakerNum].SetSoundPanning(2, 0.67f);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		if (isDrift)
		{
			speakers[2].SetSoundPanning(0, 0.67f);
			speakers[2].TriggerSoundDelayedTimeStarted(checkpoint, 0);
		}
		cueSpeakerNum++;
		if (cueSpeakerNum > 1)
		{
			cueSpeakerNum = 0;
		}
	}

	public void SpawnBothMoneyDelayed(float timeStarted)
	{
		StartCoroutine(SpawningBothMoneyDelayed(timeStarted));
	}

	private IEnumerator SpawningBothMoneyDelayed(float timeStarted)
	{
		speakers[cueSpeakerNum].SetSoundVolume(0, 0.75f);
		speakers[cueSpeakerNum].SetSoundPanning(0, 0f);
		speakers[cueSpeakerNum].SetSoundVolume(1, 0.75f);
		speakers[cueSpeakerNum].SetSoundPanning(1, 0f);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, 0);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(timeStarted, 1);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		FallingMoneys[spawnNum].CrossIn("dropLeft");
		activeQueue.Add(spawnNum);
		IncreaseSpawnNum();
		FallingMoneys[spawnNum].CrossIn("dropRight");
		activeQueue.Add(spawnNum);
		IncreaseSpawnNum();
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() / 2f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		speakers[cueSpeakerNum].SetSoundPanning(2, 0f);
		speakers[cueSpeakerNum].TriggerSoundDelayedTimeStarted(checkpoint, 2);
		cueSpeakerNum++;
		if (cueSpeakerNum > 1)
		{
			cueSpeakerNum = 0;
		}
	}

	public void CatchActiveMoney()
	{
		FallingMoneys[activeQueue[0]].Hide();
		activeQueue.RemoveAt(0);
	}

	public void MissActiveMoney()
	{
		if (activeQueue.Count > 0)
		{
			activeQueue.RemoveAt(0);
		}
	}

	public void PlayFeedback()
	{
		if (isAltFeedbackQueued)
		{
			isAltFeedbackQueued = false;
			speakers[3].TriggerSoundStack(1);
		}
		speakers[3].TriggerSoundStack(0);
	}

	private void IncreaseSpawnNum()
	{
		spawnNum = ((spawnNum + 1 < FallingMoneys.Length) ? (spawnNum + 1) : 0);
	}

	public void ToggleIsAltFeedbackQueued(bool toggle)
	{
		isAltFeedbackQueued = toggle;
	}
}
