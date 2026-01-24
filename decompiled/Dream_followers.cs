using System.Collections;
using UnityEngine;

public class Dream_followers : Dream
{
	private bool isFux;

	private bool isFuxAlt;

	private int fuxState;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		if (gameMode == 0)
		{
			InfluencerLand.env.Show(10, "k", isRemix: false);
		}
		else
		{
			InfluencerLand.env.Show(150, "k", isRemix: false);
		}
		InfluencerLand.env.Bobble();
		InfluencerLand.env.BobbleDelayed(0f);
		InfluencerLand.env.BobbleNotifications();
		InfluencerLand.env.PlayMiscSoundDelayed(timeBeatStarted, 2);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		InfluencerLand.env.Bobble();
		InfluencerLand.env.BobbleDelayed(0f);
		InfluencerLand.env.BobbleNotifications();
		InfluencerLand.env.PlayMiscSoundDelayed(timeBeatStarted, 2);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		InfluencerLand.env.Bobble();
		InfluencerLand.env.BobbleDelayed(0f);
		InfluencerLand.env.BobbleNotifications();
		InfluencerLand.env.PlayMiscSoundDelayed(timeBeatStarted, 2);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		InfluencerLand.env.Bobble();
		InfluencerLand.env.BobbleDelayed(0f);
		InfluencerLand.env.BobbleNotifications();
		InfluencerLand.env.PlayMiscSoundDelayed(timeBeatStarted, 1);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		if (gameMode == 0)
		{
			if (SaveManager.mgr.GetScore("Dream_food") <= 1 && SaveManager.mgr.GetScore("Dream_shopping") <= 1 && SaveManager.mgr.GetScore("Dream_followers") <= 1 && SaveManager.mgr.GetScore("Dream_tech") <= 1)
			{
				isFux = true;
			}
			else
			{
				isFuxAlt = true;
			}
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			while (isFux || isFuxAlt)
			{
				timeStarted = Technician.mgr.GetDspTime();
				InfluencerLand.env.Bobble();
				InfluencerLand.env.BobbleDelayed(0f);
				InfluencerLand.env.BobbleNotifications();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
	}

	protected override void OnUpdate()
	{
		if (isFux)
		{
			if (!ControlHandler.mgr.CheckIsActionPressed() || !(Time.timeScale > 0f))
			{
				return;
			}
			fuxState++;
			if (fuxState == 1)
			{
				DreamWorld.env.DialogBox.ChangeToGraphic(1, 0, 0);
			}
			else if (fuxState == 2)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(1);
			}
			else if (fuxState == 3)
			{
				if (SaveManager.GetLang() >= 5)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(2, 4.2f, 1);
				}
				else
				{
					DreamWorld.env.DialogBox.ChangeDialogState(2);
				}
			}
			else if (fuxState >= 4)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
		else if (isFuxAlt && ControlHandler.mgr.CheckIsActionPressed() && Time.timeScale > 0f)
		{
			fuxState++;
			if (fuxState == 1)
			{
				DreamWorld.env.DialogBox.ChangeToGraphic(1, 0, 0);
			}
			else if (fuxState == 2)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(1);
			}
			else if (fuxState >= 3)
			{
				isFuxAlt = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
	}

	protected override void OnBobble()
	{
		if (!isHitWindow && InfluencerLand.env.McJumper.CheckIsAwaiting())
		{
			InfluencerLand.env.Bobble();
			InfluencerLand.env.BobbleDelayed(beatDelta);
		}
		if (!InfluencerLand.env.McJumper.CheckIsEscaped())
		{
			InfluencerLand.env.RepositionNotification();
		}
	}

	protected override void OnBeat()
	{
		if (gameMode == 0)
		{
			if (phrase == 1)
			{
				if (bar == 8 && beat == 4)
				{
					InfluencerLand.env.EscapeDelayed(timeBeatStarted);
				}
			}
			else if (phrase == 2)
			{
				if (bar == 1)
				{
					if (beat == 1)
					{
						Interface.env.Cam.MoveDistance(new Vector3(0f, 30f, 0f), 3.75f * InfluencerLand.env.GetSpeed());
					}
					else if (beat == 2)
					{
						InfluencerLand.env.Reframe(30, "k", isRemix: false);
					}
				}
				else if (bar == 8 && beat == 4)
				{
					InfluencerLand.env.EscapeDelayed(timeBeatStarted);
				}
			}
			else
			{
				if (phrase != 3)
				{
					return;
				}
				if (bar == 1)
				{
					if (beat == 1)
					{
						Interface.env.Cam.MoveDistance(new Vector3(0f, 30f, 0f), 3.75f * InfluencerLand.env.GetSpeed());
					}
					else if (beat == 2)
					{
						InfluencerLand.env.Reframe(40, "k", isRemix: false);
					}
				}
				else if (bar == 8 && beat == 4)
				{
					InfluencerLand.env.EscapeDelayed(timeBeatStarted);
				}
			}
		}
		else if (gameMode == 1)
		{
			if (phrase == 1)
			{
				if (bar == 6 && beat == 4)
				{
					InfluencerLand.env.EscapeDelayed(timeBeatStarted);
				}
				else if (bar == 7)
				{
					if (beat == 1)
					{
						Interface.env.Cam.MoveDistance(new Vector3(0f, 30f, 0f), 3.75f * InfluencerLand.env.GetSpeed());
					}
					else if (beat == 2)
					{
						InfluencerLand.env.Reframe(10, "m", isRemix: false);
					}
				}
			}
			else if (phrase == 3)
			{
				if (bar == 6 && beat == 4)
				{
					InfluencerLand.env.EscapeDelayed(timeBeatStarted);
				}
				else if (bar == 7)
				{
					if (beat == 1)
					{
						Interface.env.Cam.MoveDistance(new Vector3(0f, 30f, 0f), 3.75f * InfluencerLand.env.GetSpeed());
					}
					else if (beat == 2)
					{
						InfluencerLand.env.Reframe(5, "b", isRemix: false);
					}
				}
			}
			else if (phrase == 4 && bar == 8 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
		}
		else if (gameMode == 2 && phrase == 5 && bar == 8 && beat == 4)
		{
			InfluencerLand.env.EscapeDelayed(timeBeatStarted);
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueHitWindow(1);
			InfluencerLand.env.Launch0Delayed(timeBeatStarted);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHitWindow(2);
				InfluencerLand.env.Launch1Delayed(timeBeatStarted);
				InfluencerLand.env.Launch1SoundDelayed(timeBeatStarted, beat);
				sequences[1] = 0f;
			}
			else
			{
				if (sequences[1] == 1f)
				{
					InfluencerLand.env.McJumper.CancelMovement();
					InfluencerLand.env.Launch1Delayed(timeBeatStarted);
					InfluencerLand.env.Launch1SoundDelayed(timeBeatStarted, beat);
				}
				else if (sequences[1] == 2f)
				{
					QueueHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[1] = sequences[1] + 0.5f;
				}
				else
				{
					sequences[1] += 1f;
				}
				if (sequences[1] > 2f)
				{
					sequences[1] = 0f;
				}
			}
		}
		if (sequences[2] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHitWindow(3);
				InfluencerLand.env.Launch2Delayed(timeBeatStarted);
				InfluencerLand.env.Launch2SoundDelayed(timeBeatStarted, beat);
				sequences[2] = 0f;
			}
			else
			{
				if (sequences[2] == 1f)
				{
					InfluencerLand.env.McJumper.CancelMovement();
					InfluencerLand.env.Launch2Delayed(timeBeatStarted);
					InfluencerLand.env.Launch2SoundDelayed(timeBeatStarted, beat);
				}
				else if (sequences[2] == 3f)
				{
					QueueHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[2] = sequences[2] + 0.5f;
				}
				else
				{
					sequences[2] += 1f;
				}
				if (sequences[2] > 3f)
				{
					sequences[2] = 0f;
				}
			}
		}
		if (sequences[3] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHitWindow(4);
				InfluencerLand.env.Launch3Delayed(timeBeatStarted);
				InfluencerLand.env.Launch3SoundDelayed(timeBeatStarted, beat);
				sequences[3] = 0f;
			}
			else
			{
				if (sequences[3] == 1f)
				{
					InfluencerLand.env.McJumper.CancelMovement();
					InfluencerLand.env.Launch3Delayed(timeBeatStarted);
					InfluencerLand.env.Launch3SoundDelayed(timeBeatStarted, beat);
				}
				else if (sequences[3] == 4f)
				{
					QueueHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[3] = sequences[3] + 0.5f;
				}
				else
				{
					sequences[3] += 1f;
				}
				if (sequences[3] > 4f)
				{
					sequences[3] = 0f;
				}
			}
		}
		if (sequences[4] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHitWindow(1);
				QueueHitWindow(1, isHalfBeatAdded: true);
				QueueHitWindow(2);
				InfluencerLand.env.LaunchIntroTripleDelayed(timeBeatStarted);
				InfluencerLand.env.LaunchIntroTripleSoundDelayed(timeBeatStarted);
				sequences[4] = 0f;
			}
			else
			{
				if (sequences[4] == 1f)
				{
					QueueHitWindow(1);
					QueueHitWindow(1, isHalfBeatAdded: true);
					InfluencerLand.env.LaunchIntroTripleDelayed(timeBeatStarted);
					InfluencerLand.env.LaunchIntroTripleSoundDelayed(timeBeatStarted);
				}
				else if (sequences[4] == 2f)
				{
					QueueHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[4] = sequences[4] + 0.5f;
				}
				else
				{
					sequences[4] += 1f;
				}
				if (sequences[4] > 2f)
				{
					sequences[4] = 0f;
				}
			}
		}
		if (sequences[5] > 0f)
		{
			QueueHitWindow(4);
			InfluencerLand.env.Launch5Delayed(timeBeatStarted);
			sequences[5] = 0f;
		}
	}

	protected override void OnHitWindow()
	{
		InfluencerLand.env.SetActivePlatformNum(InfluencerLand.env.McJumper.GetPositionNum());
	}

	protected override void OnHit()
	{
		InfluencerLand.env.McJumper.Jump();
		InfluencerLand.env.McJumper.Ping(bar, beat);
		InfluencerLand.env.Platforms[InfluencerLand.env.GetActivePlatformNum()].LightUp(isFeedbackShown: true, accuracy);
	}

	protected override void OnStrike()
	{
		InfluencerLand.env.McJumper.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		InfluencerLand.env.McJumper.Stumble();
		if (gameMode < 6)
		{
			InfluencerLand.env.McJumper.Sweat.CrossIn();
		}
		InfluencerLand.env.Platforms[InfluencerLand.env.GetActivePlatformNum()].LightUp(isFeedbackShown: false, 0f);
	}

	protected override void OnExit()
	{
		DreamWorld.env.RecenterZoomer(InfluencerLand.env);
	}
}
