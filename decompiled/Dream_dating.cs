using System.Collections;
using UnityEngine;

public class Dream_dating : Dream
{
	private bool isFux;

	private int fuxState;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		LoveLand.env.Show(0);
		Interface.env.Cam.Drift(1);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitForSeconds(1.25f);
		LoveLand.env.Phone.LaunchApp();
		yield return new WaitForSeconds(2f);
		Interface.env.Cam.MoveToTarget(new Vector3(0f, 0f, 0f), 2f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() >= 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			while (isFux)
			{
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
		Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: true);
		Interface.env.Circle.SetSpawnEaseType(1);
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
		if (isHitWindow)
		{
			LoveLand.env.Phone.DatingApp.SlideWithDateCard();
		}
		else
		{
			LoveLand.env.Phone.DatingApp.Slide();
		}
		if (gameMode != 0)
		{
			if (bar % 2 == 1 && beat == 1)
			{
				LoveLand.env.BlowPetals1(beatDelta);
			}
			else if (bar % 2 == 0 && beat == 1)
			{
				LoveLand.env.BlowPetals2(beatDelta);
			}
		}
	}

	protected override void OnBeat()
	{
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			if (gameMode < 6)
			{
				QueueLeftHitWindow(3);
				LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 1f);
				sequences[0] = 0f;
			}
			else
			{
				if (sequences[0] == 1f)
				{
					LoveLand.env.CancelAllSounds();
					LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 1f);
				}
				else if (sequences[0] == 3f)
				{
					QueueLeftHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[0] = sequences[0] + 0.5f;
				}
				else
				{
					sequences[0] += 1f;
				}
				if (sequences[0] > 3f)
				{
					sequences[0] = 0f;
				}
			}
		}
		if (sequences[1] > 0f)
		{
			if (gameMode < 6)
			{
				QueueLeftHitWindow(6);
				LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 0.5f);
				sequences[1] = 0f;
			}
			else
			{
				if (sequences[1] == 1f)
				{
					LoveLand.env.CancelAllSounds();
					LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 0.5f);
				}
				else if (sequences[1] == 6f)
				{
					QueueLeftHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[1] = sequences[1] + 0.5f;
				}
				else
				{
					sequences[1] += 1f;
				}
				if (sequences[1] > 6f)
				{
					sequences[1] = 0f;
				}
			}
		}
		if (sequences[2] > 0f)
		{
			if (gameMode < 6)
			{
				QueueRightHitWindow(3);
				LoveLand.env.CountdownRightDelayed(timeBeatStarted, 1f);
				sequences[2] = 0f;
			}
			else
			{
				if (sequences[2] == 1f)
				{
					LoveLand.env.CancelAllSounds();
					LoveLand.env.CountdownRightDelayed(timeBeatStarted, 1f);
				}
				else if (sequences[2] == 3f)
				{
					QueueRightHitWindow(1);
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
		if (!(sequences[3] > 0f))
		{
			return;
		}
		if (gameMode < 6)
		{
			QueueRightHitWindow(6);
			LoveLand.env.CountdownRightDelayed(timeBeatStarted, 0.5f);
			sequences[3] = 0f;
			return;
		}
		if (sequences[3] == 1f)
		{
			LoveLand.env.CancelAllSounds();
			LoveLand.env.CountdownRightDelayed(timeBeatStarted, 0.5f);
		}
		else if (sequences[3] == 6f)
		{
			QueueRightHitWindow(1);
		}
		if (isHalfBeatEnabled)
		{
			sequences[3] = sequences[3] + 0.5f;
		}
		else
		{
			sequences[3] += 1f;
		}
		if (sequences[3] > 6f)
		{
			sequences[3] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 0)
		{
			LoveLand.env.ZoomInDelayed(timeBeatStarted);
			LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, 0);
		}
		else if (eventNum == 1)
		{
			if (LoveLand.env.Phone.CheckIsSmall())
			{
				LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
				LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, 1);
			}
			else
			{
				LoveLand.env.ZoomOutDelayed(timeBeatStarted);
				LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, 1);
			}
		}
		else if (eventNum <= 5)
		{
			LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
			LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, eventNum);
		}
		else if (eventNum == 6)
		{
			LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
			LoveLand.env.Phone.SmallRotateDelayed(timeBeatStarted, 1);
		}
		else if (eventNum == 7)
		{
			LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
			LoveLand.env.Phone.SmallRotateDelayed(timeBeatStarted, 0);
		}
	}

	protected override void OnActionLeft()
	{
		LoveLand.env.Phone.ThumbLeft();
		if (!isHitWindow)
		{
			LoveLand.env.Phone.DatingApp.GetDateCard().NudgeLeft();
		}
	}

	protected override void OnActionRight()
	{
		LoveLand.env.Phone.ThumbRight();
		if (!isHitWindow)
		{
			LoveLand.env.Phone.DatingApp.GetDateCard().NudgeRight();
		}
	}

	protected override void OnHitWindow()
	{
		LoveLand.env.Phone.DatingApp.ToggleIsSliding(toggle: true);
	}

	protected override void OnHit()
	{
		if (hitType == 1)
		{
			if (LoveLand.env.Phone.DatingApp.DateCards[0].CheckIsLastCue())
			{
				LoveLand.env.Phone.DatingApp.DateCards[0].Deactivate("swipeLeft", accuracy);
			}
			else if (LoveLand.env.Phone.DatingApp.DateCards[1].CheckIsLastCue())
			{
				LoveLand.env.Phone.DatingApp.DateCards[1].Deactivate("swipeLeft", accuracy);
			}
		}
		else if (hitType == 2)
		{
			if (LoveLand.env.Phone.DatingApp.DateCards[0].CheckIsLastCue())
			{
				LoveLand.env.Phone.DatingApp.DateCards[0].Deactivate("swipeRight", accuracy);
			}
			else if (LoveLand.env.Phone.DatingApp.DateCards[1].CheckIsLastCue())
			{
				LoveLand.env.Phone.DatingApp.DateCards[1].Deactivate("swipeRight", accuracy);
			}
		}
		LoveLand.env.Phone.DatingApp.RotateOut();
		LoveLand.env.React(hitType);
	}

	protected override void OnStrike()
	{
		if (ControlHandler.mgr.CheckIsActionLeftPressed())
		{
			LoveLand.env.Sweat.SetLocalX(-7.1f);
		}
		else if (ControlHandler.mgr.CheckIsActionRightPressed())
		{
			LoveLand.env.Sweat.SetLocalX(7.1f);
		}
		LoveLand.env.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			LoveLand.env.Sweat.CrossIn();
		}
		LoveLand.env.Phone.DatingApp.RotateOut();
		if (LoveLand.env.Phone.DatingApp.DateCards[0].CheckIsLastCue())
		{
			LoveLand.env.Phone.DatingApp.DateCards[0].Deactivate("swipeDown", 0f);
		}
		else if (LoveLand.env.Phone.DatingApp.DateCards[1].CheckIsLastCue())
		{
			LoveLand.env.Phone.DatingApp.DateCards[1].Deactivate("swipeDown", 0f);
		}
	}
}
