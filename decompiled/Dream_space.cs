using System.Collections;
using UnityEngine;

public class Dream_space : Dream
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
		AngrySkies.env.Show();
		AngrySkies.env.NasaTv.Hover();
		AngrySkies.env.NasaTv.Tick(0f);
		AngrySkies.env.McMarchers.MarchDelayed(0f);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		AngrySkies.env.McMarchers.MarchDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		AngrySkies.env.NasaTv.Tick(0f);
		AngrySkies.env.McMarchers.MarchDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		AngrySkies.env.McMarchers.MarchDelayed(0f);
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
				if (tempBeat == 1)
				{
					AngrySkies.env.NasaTv.Hover();
					AngrySkies.env.NasaTv.Tick(beatDelta);
				}
				else if (tempBeat == 2)
				{
					AngrySkies.env.SparkleStarDelayed(beatDelta);
				}
				else if (tempBeat == 3 && AngrySkies.env.NasaTv.CheckIsActivated())
				{
					AngrySkies.env.NasaTv.Tick(beatDelta);
				}
				else if (tempBeat == 4)
				{
					AngrySkies.env.SparkleStarDelayed(beatDelta);
				}
				AngrySkies.env.McMarchers.MarchDelayed(0f);
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
	}

	protected override void OnUpdate()
	{
		if (!isFux || !ControlHandler.mgr.CheckIsActionPressed() || !DreamWorld.env.DialogBox.CheckIsActivated() || !(Time.timeScale > 0f))
		{
			return;
		}
		fuxState++;
		if (fuxState == 1)
		{
			DreamWorld.env.DialogBox.ChangeDialogState(1);
			if (SaveManager.GetLang() == 0)
			{
				string text = DreamWorld.env.DialogBox.GetText();
				if (ControlHandler.mgr.GetCtrlType() == 1)
				{
					DreamWorld.env.DialogBox.SetText(text.Replace("[]", "A"));
				}
				else if (ControlHandler.mgr.GetCtrlType() == 2)
				{
					DreamWorld.env.DialogBox.SetText(text.Replace("[]", "X"));
				}
				else
				{
					DreamWorld.env.DialogBox.SetText(text.Replace("[]", SaveManager.mgr.GetActionKey()));
				}
			}
		}
		else if (fuxState >= 2)
		{
			isFux = false;
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
		}
	}

	protected override void OnBobble()
	{
		AngrySkies.env.McMarchers.MarchDelayed(beatDelta);
		if (beat == 1 && AngrySkies.env.NasaTv.CheckIsActivated())
		{
			AngrySkies.env.NasaTv.Hover();
			AngrySkies.env.NasaTv.Tick(beatDelta);
		}
		else if (beat == 2)
		{
			AngrySkies.env.SparkleStarDelayed(beatDelta);
		}
		else if (beat == 3 && AngrySkies.env.NasaTv.CheckIsActivated())
		{
			AngrySkies.env.NasaTv.Tick(beatDelta);
		}
		else if (beat == 4)
		{
			AngrySkies.env.SparkleStarDelayed(beatDelta);
		}
		if (bar % 2 == 0 && beat == 3)
		{
			AngrySkies.env.ShootStarDelayed(beatDelta);
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
				QueueHoldReleaseWindow(4, 7);
				AngrySkies.env.Countdown0Delayed(timeBeatStarted);
				AngrySkies.env.Countdown0SoundDelayed(timeBeatStarted);
				sequences[0] = 0f;
			}
			else
			{
				if (sequences[0] == 1f)
				{
					AngrySkies.env.Reset();
					AngrySkies.env.Countdown0Delayed(timeBeatStarted);
					AngrySkies.env.Countdown0SoundDelayed(timeBeatStarted);
				}
				else if (sequences[0] == 4f)
				{
					QueueHoldReleaseWindow(1, 4);
				}
				if (isHalfBeatEnabled)
				{
					sequences[0] = sequences[0] + 0.5f;
				}
				else
				{
					sequences[0] += 1f;
				}
				if (sequences[0] > 4f)
				{
					sequences[0] = 0f;
				}
			}
		}
		if (sequences[1] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHoldReleaseWindow(2, 3);
				AngrySkies.env.Countdown1Delayed(timeBeatStarted);
				AngrySkies.env.Countdown1SoundDelayed(timeBeatStarted);
				sequences[1] = 0f;
			}
			else
			{
				if (sequences[1] == 1f)
				{
					AngrySkies.env.Reset();
					AngrySkies.env.Countdown1Delayed(timeBeatStarted);
					AngrySkies.env.Countdown1SoundDelayed(timeBeatStarted);
				}
				else if (sequences[1] == 2f)
				{
					QueueHoldReleaseWindow(1, 2);
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
		if (!(sequences[2] > 0f))
		{
			return;
		}
		if (gameMode < 6)
		{
			QueueHoldReleaseWindow(8, 14);
			AngrySkies.env.Countdown2Delayed(timeBeatStarted);
			AngrySkies.env.Countdown2SoundDelayed(timeBeatStarted);
			sequences[2] = 0f;
			return;
		}
		if (sequences[2] == 1f)
		{
			AngrySkies.env.Reset();
			AngrySkies.env.Countdown2Delayed(timeBeatStarted);
			AngrySkies.env.Countdown2SoundDelayed(timeBeatStarted);
		}
		else if (sequences[2] == 8f)
		{
			QueueHoldReleaseWindow(1, 7);
		}
		if (isHalfBeatEnabled)
		{
			sequences[2] = sequences[2] + 0.5f;
		}
		else
		{
			sequences[2] += 1f;
		}
		if (sequences[2] > 8f)
		{
			sequences[2] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 0)
		{
			AngrySkies.env.Unblind(4f);
		}
		else if (eventNum == 1)
		{
			AngrySkies.env.Blind(4f);
		}
		else if (eventNum == 2)
		{
			AngrySkies.env.Unblind(16f);
		}
		else if (eventNum == 3)
		{
			AngrySkies.env.Blind(16f);
		}
		else if (eventNum == 4)
		{
			AngrySkies.env.Unblind(2f);
		}
		else if (eventNum == 5)
		{
			AngrySkies.env.Blind(2f);
		}
		else if (eventNum == 6)
		{
			AngrySkies.env.TriggerBlinded();
		}
	}

	protected override void OnPrep()
	{
		AngrySkies.env.PlayDrain();
		AngrySkies.env.ToggleIsPrepped(toggle: true);
		AngrySkies.env.ToggleLowerMute(toggle: false);
		AngrySkies.env.NasaTv.Flash();
		AngrySkies.env.NasaTv.SpaceMeters[0].SetColor(accuracy);
		AngrySkies.env.NasaTv.SpaceMeters[0].ToggleIsVisible(toggle: true);
		AngrySkies.env.NasaTv.SpaceMeters[1].ToggleIsVisible(toggle: false);
		AngrySkies.env.Shuttle.Prep();
	}

	protected override void OnHit()
	{
		AngrySkies.env.Reset();
		AngrySkies.env.PlayLaunch();
		AngrySkies.env.ToggleLowerMute(toggle: true);
		AngrySkies.env.McMarchers.ToggleIsMarching(toggle: true);
		AngrySkies.env.Shuttle.Lift(accuracy);
		AngrySkies.env.NasaTv.TriggerFeedback(accuracy);
		Interface.env.Cam.Shake(1.15f);
	}

	protected override void OnStrike()
	{
		AngrySkies.env.Sweat.CrossIn();
		if (isPrepped)
		{
			AngrySkies.env.Reset();
			AngrySkies.env.Shuttle.Unprep();
		}
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			AngrySkies.env.Sweat.CrossIn();
		}
		AngrySkies.env.Reset();
		AngrySkies.env.McMarchers.ToggleIsMarching(toggle: true);
		AngrySkies.env.ToggleLowerMute(toggle: true);
		if (isPrepped)
		{
			AngrySkies.env.Shuttle.Unprep();
		}
	}
}
