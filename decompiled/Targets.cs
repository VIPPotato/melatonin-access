using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : Wrapper
{
	[Header("Fragments")]
	public Fragment[] senders;

	public Fragment[] targets;

	public List<Fragment> targets_sent = new List<Fragment>();

	private int activeNum;

	private int activeLocalZ;

	protected override void Awake()
	{
		SetupFragments();
		Fragment[] array = senders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
		array = targets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Awake();
		}
	}

	public void Show()
	{
		activeNum = 0;
		activeLocalZ = 0;
		Fragment[] array = senders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].TriggerAnim("hidden");
		}
	}

	public void Hide()
	{
		targets_sent = new List<Fragment>();
	}

	public void SendCenter(float timeStarted)
	{
		StartCoroutine(SendingCenter(timeStarted));
	}

	private IEnumerator SendingCenter(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Fragment target_sent = targets[activeNum];
		senders[activeNum].TriggerAnim("sendCenter", NeoCity.env.GetSpeed() / 4.15f);
		targets[activeNum].TriggerAnim("idling");
		targets[activeNum].SetLocalZ(activeLocalZ);
		activeNum++;
		activeLocalZ++;
		if (activeLocalZ >= 150)
		{
			activeLocalZ = 0;
		}
		if (activeNum >= senders.Length)
		{
			activeNum = 0;
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * 4f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		targets_sent.Add(target_sent);
		checkpoint += 0.23334f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		targets_sent.Remove(target_sent);
	}

	public void SendLeft(float timeStarted)
	{
		StartCoroutine(SendingLeft(timeStarted));
	}

	private IEnumerator SendingLeft(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Fragment target_sent = targets[activeNum];
		senders[activeNum].TriggerAnim("sendLeft", NeoCity.env.GetSpeed() / 4.15f);
		targets[activeNum].TriggerAnim("idling");
		targets[activeNum].SetLocalZ(activeLocalZ);
		activeNum++;
		activeLocalZ++;
		if (activeLocalZ >= 150)
		{
			activeLocalZ = 0;
		}
		if (activeNum >= senders.Length)
		{
			activeNum = 0;
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * 4f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		targets_sent.Add(target_sent);
		checkpoint += 0.23334f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		targets_sent.Remove(target_sent);
	}

	public void SendRight(float timeStarted)
	{
		StartCoroutine(SendingRight(timeStarted));
	}

	private IEnumerator SendingRight(float timeStarted)
	{
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Fragment target_sent = targets[activeNum];
		senders[activeNum].TriggerAnim("sendRight", NeoCity.env.GetSpeed() / 4.15f);
		targets[activeNum].TriggerAnim("idling");
		targets[activeNum].SetLocalZ(activeLocalZ);
		activeNum++;
		activeLocalZ++;
		if (activeLocalZ >= 150)
		{
			activeLocalZ = 0;
		}
		if (activeNum >= senders.Length)
		{
			activeNum = 0;
		}
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * 4f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		targets_sent.Add(target_sent);
		checkpoint += 0.23334f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		targets_sent.Remove(target_sent);
	}

	public void HitTargets()
	{
		foreach (Fragment item in targets_sent)
		{
			if (Random.Range(0, 2) == 0)
			{
				item.ToggleSpriteFlip(toggle: false);
			}
			else
			{
				item.ToggleSpriteFlip(toggle: true);
			}
			item.TriggerAnim("hit");
		}
	}

	public void FakeHide()
	{
		Fragment[] array = targets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ToggleSpriteRenderer(toggle: false);
		}
	}

	public void FakeShow()
	{
		Fragment[] array = targets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ToggleSpriteRenderer(toggle: true);
		}
	}

	public void ResetActiveLocalZ()
	{
		activeLocalZ = 0;
	}

	public int GetActiveLocalZ()
	{
		return activeLocalZ;
	}
}
