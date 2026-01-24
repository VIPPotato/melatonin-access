using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluencerLand : Wrapper
{
	public static InfluencerLand env;

	[Header("Children")]
	public McJumper McJumper;

	public InfluenceItem[] InfluenceItems;

	public Platform[] Platforms;

	public List<Notification> Notifications = new List<Notification>();

	public FollowerBg FollowerBg;

	[Header("Props")]
	public int[] notificationBar1;

	public int[] notificationBar2;

	public int[] vibrateBar1;

	public int[] vibrateBar2;

	private bool isTripleLaunching;

	private bool isActivated;

	private int activePlatform;

	private int bottomPlatform;

	private int nextNotification;

	private float assemblePosX;

	private float assemblePosY;

	private float spaceBetweenX = 3.35f;

	private float spaceBetweenY = 3.15f;

	private Coroutine launching;

	private Coroutine launchingSound;

	private Coroutine bobbling;

	private const float animTempo = 80f;

	protected override void Awake()
	{
		env = this;
		SetupFragments();
		assemblePosX = Platforms[4].GetLocalX();
		assemblePosY = Platforms[4].GetLocalY();
		RenderChildren(toggle: false, 3);
	}

	public void Show(int newFollowerNum, string newFollowLetter, bool isRemix = true)
	{
		isActivated = true;
		RenderChildren(toggle: true, 3);
		bottomPlatform = 0;
		for (int i = 0; i < Platforms.Length; i++)
		{
			Platforms[i].Show();
			if (i == 4)
			{
				Platforms[i].AwaitSpring();
				Platforms[i].SetLocalPosition(assemblePosX, assemblePosY);
				Platforms[i].SetLocalZ(11f);
				Platforms[i].Number.Show();
			}
			else
			{
				if (i < 4)
				{
					Platforms[i].Hide();
				}
				Platforms[i].SetLocalPosition(assemblePosX + spaceBetweenX * (float)(i - 4), assemblePosY + spaceBetweenY * (float)(i - 4));
				Platforms[i].SetLocalZ(11 + (i - 4));
			}
			if (InfluenceItems[i] != null)
			{
				InfluenceItems[i].Show();
				InfluenceItems[i].SetPosition(Platforms[i].GetX(), Platforms[i].GetY());
				InfluenceItems[i].ShiftPosition();
			}
		}
		if (!isRemix)
		{
			for (int j = 0; j < Notifications.Count; j++)
			{
				if (j != nextNotification)
				{
					Notifications[j].Show();
				}
			}
		}
		McJumper.Show();
		FollowerBg.SetParent(Interface.env.Cam.GetOuterTransform());
		Interface.env.Cam.SetPosition(0f, 0f);
		DreamWorld.env.ClearFeedbacks();
		SetFollowers(newFollowerNum, newFollowLetter);
	}

	public void Hide()
	{
		Interface.env.Cam.SetPosition(0f, 0f);
		CancelCoroutine(launching);
		CancelCoroutine(launchingSound);
		CancelCoroutine(bobbling);
		FollowerBg.SetParent(base.transform);
		isActivated = false;
		McJumper.Hide();
		for (int i = 1; i < Notifications.Count; i++)
		{
			Notifications[i].Hide();
		}
		RenderChildren(toggle: false, 3);
	}

	public void Bobble()
	{
		McJumper.Bobble();
		Platforms[env.McJumper.GetPositionNum()].Bobble();
	}

	public void BobbleDelayed(float delta)
	{
		CancelCoroutine(bobbling);
		bobbling = StartCoroutine(BobblingDelayed(delta));
	}

	private IEnumerator BobblingDelayed(float delta)
	{
		float checkpoint = Technician.mgr.GetDspTime() + 0.11667f - delta;
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() > checkpoint);
		Platform[] platforms = Platforms;
		for (int num = 0; num < platforms.Length; num++)
		{
			platforms[num].Number.IncreaseNumber();
		}
	}

	public void BobbleNotifications()
	{
		for (int i = 0; i < Notifications.Count; i++)
		{
			if (Notifications[i].CheckIsActivated())
			{
				Notifications[i].Bobble();
			}
		}
	}

	public void Launch0Delayed(float timeStarted)
	{
		if (Dream.dir.GetGameMode() >= 6 && isTripleLaunching)
		{
			isTripleLaunching = false;
			McJumper.IncreasePositionNum(1);
			RepositionPlatforms(1);
			activePlatform = McJumper.GetPositionNum();
		}
		CancelCoroutine(launching);
		launching = StartCoroutine(Launching0Delayed(timeStarted));
	}

	private IEnumerator Launching0Delayed(float timeStarted)
	{
		if (McJumper.CheckIsAwaiting())
		{
			PlayMiscSoundDelayed(timeStarted, 0);
		}
		McJumper.SetJumpNum(1);
		McJumper.MoveDelayed(timeStarted, 1);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(1);
		Platforms[McJumper.GetNextPositionNum()].Number.Activate(1, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(1);
		RepositionPlatforms(1);
		Platforms[McJumper.GetPositionNum()].Dip(checkpoint, isShort: false);
	}

	public void Launch1Delayed(float timeStarted)
	{
		if (Dream.dir.GetGameMode() >= 6 && isTripleLaunching)
		{
			isTripleLaunching = false;
			McJumper.IncreasePositionNum(1);
			RepositionPlatforms(1);
			activePlatform = McJumper.GetPositionNum();
		}
		CancelCoroutine(launching);
		launching = StartCoroutine(Launching1Delayed(timeStarted));
	}

	private IEnumerator Launching1Delayed(float timeStarted)
	{
		if (McJumper.CheckIsAwaiting())
		{
			PlayMiscSoundDelayed(timeStarted, 0);
		}
		McJumper.SetJumpNum(1);
		McJumper.MoveDelayed(timeStarted, 2);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(2);
		Platforms[McJumper.GetNextNextPositionNum()].Number.Activate(2, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(2);
		RepositionPlatforms(2);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Buzz(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Dip(checkpoint, isShort: false);
	}

	public void Launch1SoundDelayed(float timeStarted, int beatStarted)
	{
		CancelCoroutine(launchingSound);
		launchingSound = StartCoroutine(Launching1SoundDelayed(timeStarted, beatStarted));
	}

	private IEnumerator Launching1SoundDelayed(float timeStarted, int beatStarted)
	{
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Dream.dir.GetBeat() != beatStarted);
		ChimeSoundDelayed(checkpoint);
		VibrateSoundDelayed(checkpoint);
	}

	public void Launch2Delayed(float timeStarted)
	{
		if (Dream.dir.GetGameMode() >= 6 && isTripleLaunching)
		{
			isTripleLaunching = false;
			McJumper.IncreasePositionNum(1);
			RepositionPlatforms(1);
			activePlatform = McJumper.GetPositionNum();
		}
		CancelCoroutine(launching);
		launching = StartCoroutine(Launching2Delayed(timeStarted));
	}

	private IEnumerator Launching2Delayed(float timeStarted)
	{
		if (McJumper.CheckIsAwaiting())
		{
			PlayMiscSoundDelayed(timeStarted, 0);
		}
		McJumper.SetJumpNum(1);
		McJumper.MoveDelayed(timeStarted, 3);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(3);
		Platforms[McJumper.GetNextNextNextPositionNum()].Number.Activate(3, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(3);
		RepositionPlatforms(3);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 1.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Buzz(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Dip(checkpoint, isShort: false);
	}

	public void Launch2SoundDelayed(float timeStarted, int beatStarted)
	{
		CancelCoroutine(launchingSound);
		launchingSound = StartCoroutine(Launching2SoundDelayed(timeStarted, beatStarted));
	}

	private IEnumerator Launching2SoundDelayed(float timeStarted, int beatStarted)
	{
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Dream.dir.GetBeat() != beatStarted);
		ChimeSoundDelayed(checkpoint);
		int beat = Dream.dir.GetBeat();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Dream.dir.GetBeat() != beat);
		ChimeSoundDelayed(checkpoint);
		VibrateSoundDelayed(checkpoint);
	}

	public void Launch3Delayed(float timeStarted)
	{
		if (Dream.dir.GetGameMode() >= 6 && isTripleLaunching)
		{
			isTripleLaunching = false;
			McJumper.IncreasePositionNum(1);
			RepositionPlatforms(1);
			activePlatform = McJumper.GetPositionNum();
		}
		CancelCoroutine(launching);
		launching = StartCoroutine(Launching3Delayed(timeStarted));
	}

	private IEnumerator Launching3Delayed(float timeStarted)
	{
		if (McJumper.CheckIsAwaiting())
		{
			PlayMiscSoundDelayed(timeStarted, 0);
		}
		McJumper.SetJumpNum(1);
		McJumper.MoveDelayed(timeStarted, 4);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(4);
		Platforms[McJumper.GetNextNextNextNextPositionNum()].Number.Activate(4, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(4);
		RepositionPlatforms(4);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 2.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Buzz(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Dip(checkpoint, isShort: false);
	}

	public void Launch3SoundDelayed(float timeStarted, int beatStarted)
	{
		CancelCoroutine(launchingSound);
		launchingSound = StartCoroutine(Launching3SoundDelayed(timeStarted, beatStarted));
	}

	private IEnumerator Launching3SoundDelayed(float timeStarted, int beatStarted)
	{
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Dream.dir.GetBeat() != beatStarted);
		ChimeSoundDelayed(checkpoint);
		int beat = Dream.dir.GetBeat();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Dream.dir.GetBeat() != beat);
		ChimeSoundDelayed(checkpoint);
		beat = Dream.dir.GetBeat();
		checkpoint += MusicBox.env.GetSecsPerBeat();
		yield return new WaitUntil(() => Dream.dir.GetBeat() != beat);
		ChimeSoundDelayed(checkpoint);
		VibrateSoundDelayed(checkpoint);
	}

	public void LaunchIntroTripleDelayed(float timeStarted)
	{
		if (Dream.dir.GetGameMode() >= 6 && isTripleLaunching)
		{
			isTripleLaunching = false;
			McJumper.IncreasePositionNum(1);
			RepositionPlatforms(1);
			activePlatform = McJumper.GetPositionNum();
		}
		CancelCoroutine(launching);
		launching = StartCoroutine(LaunchingIntroTripleDelayed(timeStarted));
	}

	private IEnumerator LaunchingIntroTripleDelayed(float timeStarted)
	{
		if (McJumper.CheckIsAwaiting())
		{
			PlayMiscSoundDelayed(timeStarted, 0);
		}
		McJumper.SetJumpNum(1);
		McJumper.MoveDelayed(timeStarted, 1);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(1);
		Platforms[McJumper.GetNextPositionNum()].Buzz(checkpoint);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(1);
		RepositionPlatforms(1);
		Platforms[McJumper.GetPositionNum()].Dip(checkpoint, isShort: false);
		Platforms[McJumper.GetPositionNum()].Number.Activate(1, 2);
		Platforms[McJumper.GetNextPositionNum()].Buzz(checkpoint);
		Platforms[McJumper.GetNextNextPositionNum()].Buzz(checkpoint);
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * 0.5f - 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.SetJumpNum(2);
		McJumper.MoveDelayed(checkpoint, 1, isShort: true);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(1, isShort: true);
		Platforms[McJumper.GetNextPositionNum()].Dip(checkpoint, isShort: true);
		Platforms[McJumper.GetNextPositionNum()].Number.Activate(1, 2);
		checkpoint += 0.11667f;
		int halfBeatTotal = Dream.dir.GetHalfBeatTotal();
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(1);
		if (Dream.dir.GetGameMode() >= 6 && Dream.dir.GetHalfBeatTotal() != halfBeatTotal)
		{
			activePlatform = McJumper.GetPositionNum();
		}
		RepositionPlatforms(1);
		checkpoint = checkpoint + MusicBox.env.GetSecsPerBeat() * 0.5f - 0.23334f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.MoveDelayed(checkpoint, 1, isShort: true);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (Dream.dir.GetGameMode() >= 6)
		{
			isTripleLaunching = true;
		}
		Platforms[McJumper.GetPositionNum()].Throw(1, isShort: true);
		Platforms[McJumper.GetNextPositionNum()].Dip(checkpoint, isShort: true);
		Platforms[McJumper.GetNextPositionNum()].Number.Activate(1, 2);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		if (Dream.dir.GetGameMode() >= 6)
		{
			isTripleLaunching = false;
		}
		McJumper.IncreasePositionNum(1);
		RepositionPlatforms(1);
	}

	public void LaunchIntroTripleSoundDelayed(float timeStarted)
	{
		CancelCoroutine(launchingSound);
		launchingSound = StartCoroutine(LaunchingIntroTripleSoundDelayed(timeStarted));
	}

	private IEnumerator LaunchingIntroTripleSoundDelayed(float timeStarted)
	{
		VibrateSoundDelayed(timeStarted);
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		ChimeSoundDelayed(checkpoint);
		VibrateSoundDelayed(checkpoint);
	}

	public void Launch5Delayed(float timeStarted)
	{
		if (Dream.dir.GetGameMode() >= 6 && isTripleLaunching)
		{
			isTripleLaunching = false;
			McJumper.IncreasePositionNum(1);
			RepositionPlatforms(1);
			activePlatform = McJumper.GetPositionNum();
		}
		CancelCoroutine(launching);
		launching = StartCoroutine(Launching5Delayed(timeStarted));
	}

	private IEnumerator Launching5Delayed(float timeStarted)
	{
		float checkpoint = timeStarted + MusicBox.env.GetSecsPerBeat() * 3f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		PlayMiscSoundDelayed(checkpoint, 0);
		McJumper.SetJumpNum(1);
		McJumper.MoveDelayed(checkpoint, 1);
		checkpoint += 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		Platforms[McJumper.GetPositionNum()].Throw(1);
		Platforms[McJumper.GetNextPositionNum()].Number.Activate(1, 1);
		checkpoint += MusicBox.env.GetSecsPerBeat() * 0.5f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.IncreasePositionNum(1);
		RepositionPlatforms(1);
		Platforms[McJumper.GetPositionNum()].Dip(checkpoint, isShort: false);
	}

	public void Reframe(int newFollowerNum, string newFollowLetter, bool isRemix)
	{
		float x = Interface.env.Cam.GetX();
		float num = Interface.env.Cam.GetY() + 30f;
		bottomPlatform = 0;
		for (int i = 0; i < Platforms.Length; i++)
		{
			Platforms[i].Show();
			if (i == 4)
			{
				Platforms[i].AwaitSpring();
				Platforms[i].SetLocalPosition(assemblePosX + x, assemblePosY + num);
				Platforms[i].SetLocalZ(11f);
				Platforms[i].Number.Show();
			}
			else
			{
				if (i < 4)
				{
					Platforms[i].Hide();
				}
				Platforms[i].SetLocalPosition(assemblePosX + spaceBetweenX * (float)(i - 4) + x, assemblePosY + spaceBetweenY * (float)(i - 4) + num);
				Platforms[i].SetLocalZ(11 + (i - 4));
			}
			if (InfluenceItems[i] != null)
			{
				InfluenceItems[i].Show();
				InfluenceItems[i].SetPosition(Platforms[i].GetX(), Platforms[i].GetY());
				InfluenceItems[i].ShiftPosition();
			}
		}
		if (!isRemix)
		{
			for (int j = 0; j < Notifications.Count; j++)
			{
				if (j != nextNotification)
				{
					Notifications[j].Show();
				}
			}
		}
		SetFollowers(newFollowerNum, newFollowLetter);
		McJumper.Show();
		McJumper.SetDistance(x, num);
		Interface.env.Cam.MoveDistance(new Vector3(0f, 30f, 0f), 3.5f * env.GetSpeed());
	}

	public void RepositionPlatforms(int amount)
	{
		StartCoroutine(RespositioningPlatforms(amount));
	}

	private IEnumerator RespositioningPlatforms(int amount)
	{
		while (amount > 0)
		{
			Platforms[bottomPlatform].Show();
			Platforms[bottomPlatform].SetPosition(Platforms[GetTopPlatformNum()].GetLocalX() + spaceBetweenX, Platforms[GetTopPlatformNum()].GetLocalY() + spaceBetweenY);
			Platforms[bottomPlatform].SetLocalZ(Platforms[GetTopPlatformNum()].GetLocalZ() + 1f);
			Platforms[bottomPlatform].Number.SetNumber(Platforms[GetTopPlatformNum()].Number.GetNum() + 1);
			Platforms[bottomPlatform].Number.SetLetter(Platforms[GetTopPlatformNum()].Number.GetLetter());
			if (InfluenceItems[bottomPlatform] != null)
			{
				InfluenceItems[bottomPlatform].SetPosition(Platforms[bottomPlatform].GetX(), Platforms[bottomPlatform].GetY());
				InfluenceItems[bottomPlatform].ShiftPosition();
			}
			IncreaseBottomPlatform();
			amount--;
			yield return null;
		}
	}

	public void RepositionNotification()
	{
		Notifications[nextNotification].Activate();
		nextNotification = ((nextNotification + 1 < Notifications.Count) ? (nextNotification + 1) : 0);
		Notifications[nextNotification].Deactivate();
	}

	private void ChimeSoundDelayed(float timeStarted)
	{
		if (Dream.dir.GetBar() % 2 == 1)
		{
			int index = notificationBar1[Dream.dir.GetBeat() - 1];
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, index);
		}
		else
		{
			int index2 = notificationBar2[Dream.dir.GetBeat() - 1];
			speakers[0].TriggerSoundDelayedTimeStarted(timeStarted, index2);
		}
	}

	private void VibrateSoundDelayed(float timeStarted)
	{
		if (Dream.dir.GetBar() % 2 == 1)
		{
			int index = vibrateBar1[Dream.dir.GetBeat() - 1];
			speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, index);
		}
		else
		{
			int index2 = vibrateBar2[Dream.dir.GetBeat() - 1];
			speakers[1].TriggerSoundDelayedTimeStarted(timeStarted, index2);
		}
	}

	public void PlayMiscSoundDelayed(float timeStarted, int soundNum)
	{
		speakers[2].TriggerSoundDelayedTimeStarted(timeStarted, soundNum);
	}

	public void EscapeDelayed(float timeStarted)
	{
		StartCoroutine(EscapingDelayed(timeStarted));
	}

	private IEnumerator EscapingDelayed(float timeStarted)
	{
		PlayMiscSoundDelayed(timeStarted, 0);
		float checkpoint = timeStarted + 0.11667f;
		yield return new WaitUntil(() => MusicBox.env.GetSongTime() > checkpoint);
		McJumper.Escape();
		Platforms[env.McJumper.GetPositionNum()].Throw(4);
	}

	private void IncreaseBottomPlatform()
	{
		bottomPlatform++;
		bottomPlatform = ((bottomPlatform > 8) ? (bottomPlatform - 9) : bottomPlatform);
	}

	public void SetFollowers(int followNum, string followLetter)
	{
		for (int i = 0; i < Platforms.Length; i++)
		{
			Platforms[i].Number.SetNumber(i + followNum - 4);
			Platforms[i].Number.SetLetter(followLetter);
		}
	}

	public void SetActivePlatformNum(int newActivePlatformNum)
	{
		activePlatform = newActivePlatformNum;
	}

	private int GetTopPlatformNum()
	{
		if (bottomPlatform + 8 <= 8)
		{
			return bottomPlatform + 8;
		}
		return bottomPlatform + 8 - 9;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}

	public int GetActivePlatformNum()
	{
		return activePlatform;
	}

	public float GetSpeed()
	{
		return MusicBox.env.GetActiveTempo() / 80f;
	}

	public float GetSpaceBetweenX()
	{
		return spaceBetweenX;
	}

	public float GetSpaceBetweenY()
	{
		return spaceBetweenY;
	}
}
