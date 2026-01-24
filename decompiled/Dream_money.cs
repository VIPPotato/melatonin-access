using System.Collections;
using UnityEngine;

public class Dream_money : Dream
{
	private bool isFux;

	private int fuxState;

	private int rng;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Interface.env.Cam.Drift(1);
		TropicalBank.env.Show();
		float timeStarted = Technician.mgr.GetDspTime();
		TropicalBank.env.BobbleDelayed(0f, 1, isCameraMoving: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 0.5f);
		TropicalBank.env.SplashDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		TropicalBank.env.BobbleDelayed(0f, 2, isCameraMoving: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1.5f);
		TropicalBank.env.SplashDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		TropicalBank.env.BobbleDelayed(0f, 3, isCameraMoving: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2.5f);
		TropicalBank.env.SplashDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		TropicalBank.env.BobbleDelayed(0f, 4, isCameraMoving: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3.5f);
		TropicalBank.env.SplashDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			int tempBeat = 0;
			while (isFux || tempBeat != 4)
			{
				tempBeat++;
				if (tempBeat > 4)
				{
					tempBeat = 1;
				}
				TropicalBank.env.BobbleDelayed(0f, tempBeat, isCameraMoving: false);
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
	}

	protected override void OnUpdate()
	{
		if (isFux && ControlHandler.mgr.CheckIsActionPressed() && DreamWorld.env.DialogBox.CheckIsActivated() && Time.timeScale > 0f)
		{
			fuxState++;
			if (fuxState >= 1)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
	}

	protected override void OnBobble()
	{
		TropicalBank.env.BobbleDelayed(beatDelta, beat, isCameraMoving: true);
	}

	protected override void OnHalfBobble()
	{
		TropicalBank.env.SplashDelayed(beatDelta);
	}

	protected override void OnBeat()
	{
		if (gameMode != 0 && phrase >= 2 && bar == 8 && beat == 4)
		{
			TropicalBank.env.MoneyCloud.ToggleIsAltFeedbackQueued(toggle: true);
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueLeftHitWindow(1);
			TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: false);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			QueueRightHitWindow(1);
			TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: false);
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			QueueLeftRightHitWindow(1);
			TropicalBank.env.MoneyCloud.SpawnBothMoneyDelayed(timeBeatStarted);
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			QueueLeftHitWindow(1);
			TropicalBank.env.ThunderDelayed(timeBeatStarted, "left");
			TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: true);
			sequences[3] = 0f;
		}
		if (sequences[4] > 0f)
		{
			QueueRightHitWindow(1);
			TropicalBank.env.ThunderDelayed(timeBeatStarted, "right");
			TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: true);
			sequences[4] = 0f;
		}
		if (sequences[5] > 0f)
		{
			rng = Random.Range(0, 5);
			if (rng == 0)
			{
				QueueLeftHitWindow(1);
				TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: false);
			}
			else if (rng == 1)
			{
				QueueRightHitWindow(1);
				TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: false);
			}
			else if (rng == 2)
			{
				QueueLeftRightHitWindow(1);
				TropicalBank.env.MoneyCloud.SpawnBothMoneyDelayed(timeBeatStarted);
			}
			else if (rng == 3)
			{
				QueueLeftHitWindow(1);
				TropicalBank.env.ThunderDelayed(timeBeatStarted, "left");
				TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: true);
			}
			else
			{
				QueueRightHitWindow(1);
				TropicalBank.env.ThunderDelayed(timeBeatStarted, "right");
				TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: true);
			}
			sequences[5] = 0f;
		}
	}

	protected override void OnActionLeft()
	{
		if (isHitWindow && hitType == 1)
		{
			TropicalBank.env.McCatcher.Pocket(1);
		}
		else
		{
			TropicalBank.env.McCatcher.Grab(1);
		}
	}

	protected override void OnActionRight()
	{
		if (isHitWindow && hitType == 2)
		{
			TropicalBank.env.McCatcher.Pocket(2);
		}
		else
		{
			TropicalBank.env.McCatcher.Grab(2);
		}
	}

	protected override void OnHit()
	{
		TropicalBank.env.MoneyCloud.PlayFeedback();
		if (hitType == 1)
		{
			TropicalBank.env.MoneyCloud.CatchActiveMoney();
		}
		else if (hitType == 2)
		{
			TropicalBank.env.MoneyCloud.CatchActiveMoney();
		}
		else if (hitType == 3)
		{
			TropicalBank.env.McCatcher.Pocket(1);
			TropicalBank.env.McCatcher.Pocket(2);
			TropicalBank.env.MoneyCloud.CatchActiveMoney();
			TropicalBank.env.MoneyCloud.CatchActiveMoney();
		}
	}

	protected override void OnMiss()
	{
		TropicalBank.env.McCatcher.Sweat.CrossIn();
		if (hitType == 1)
		{
			TropicalBank.env.MoneyCloud.MissActiveMoney();
		}
		else if (hitType == 2)
		{
			TropicalBank.env.MoneyCloud.MissActiveMoney();
		}
		else if (hitType == 3)
		{
			TropicalBank.env.MoneyCloud.MissActiveMoney();
			TropicalBank.env.MoneyCloud.MissActiveMoney();
		}
	}

	protected override void OnStrike()
	{
		TropicalBank.env.McCatcher.Sweat.CrossIn();
	}
}
