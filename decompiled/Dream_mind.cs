using System.Collections;
using UnityEngine;

public class Dream_mind : Dream
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
		HypnoLair.env.Show(isRemix: false);
		HypnoLair.env.PocketWatches[0].Show();
		float timeStarted = 0f;
		if (gameMode == 0)
		{
			HypnoLair.env.Roller.Freeze();
			timeStarted = Technician.mgr.GetDspTime();
			yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() >= 6 && SaveManager.GetLang() <= 8)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			while (isFux)
			{
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
			HypnoLair.env.Roller.Show();
		}
		else if (gameMode == 2)
		{
			HypnoLair.env.EnterPov();
		}
		timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 0.5f + 0.11667f);
		HypnoLair.env.PocketWatches[0].StartSwing();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
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
		HypnoLair.env.Bobble(beatDelta, beat);
	}

	protected override void OnHalfBobble()
	{
		HypnoLair.env.HalfBobble(beatDelta, beat, gameMode);
	}

	protected override void OnBeat()
	{
		if (gameMode == 0)
		{
			if (phrase == 2 && bar == 4)
			{
				if (beat == 3)
				{
					HypnoLair.env.PocketWatches[0].EndSwingDelayed(timeBeatStarted);
				}
				else if (beat == 4)
				{
					isPenaltyLess = true;
				}
			}
		}
		else if ((gameMode == 1 || gameMode == 2) && phrase == 6 && bar == 4)
		{
			if (beat == 3)
			{
				HypnoLair.env.PocketWatches[0].EndSwingDelayed(timeBeatStarted);
			}
			else if (beat == 4)
			{
				isPenaltyLess = true;
			}
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueHitWindow(1);
			HypnoLair.env.QueueShutEyeDelayed(timeBeatStarted);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			QueueHitWindow(1);
			QueueHitWindow(1, isHalfBeatAdded: true);
			QueueHitWindow(2);
			HypnoLair.env.QueueDoubleShutEye(timeBeatStarted, beat);
			sequences[1] = 0f;
		}
		float num;
		if (sequences[2] > 0f)
		{
			num = sequences[2];
			if (num != 1f)
			{
				if (num == 1.5f)
				{
					HypnoLair.env.Eye.CloseDelayed(timeBeatStarted);
				}
			}
			else
			{
				QueueHitWindow(1);
				HypnoLair.env.PlayWooshDelayed(timeBeatStarted, isFullBeat);
				HypnoLair.env.GetDeactivatedPocketWatch().CrossInDelayed(timeBeatStarted, beat);
				HypnoLair.env.Eye.ToggleIsDoubled(toggle: true);
			}
			if (isHalfBeatEnabled)
			{
				sequences[2] = sequences[2] + 0.5f;
			}
			else
			{
				sequences[2] += 1f;
			}
			if (sequences[2] > 1.5f)
			{
				sequences[2] = 0f;
			}
		}
		if (sequences[3] > 0f)
		{
			num = sequences[3];
			if (num != 1.5f)
			{
				if (num == 2f)
				{
					HypnoLair.env.Eye.CloseDelayed(timeBeatStarted);
				}
			}
			else
			{
				QueueHitWindow(1);
				HypnoLair.env.PlayWooshDelayed(timeBeatStarted, isFullBeat);
				HypnoLair.env.GetDeactivatedPocketWatch().CrossInDelayed(timeBeatStarted, beat);
				HypnoLair.env.Eye.ToggleIsDoubled(toggle: true);
			}
			if (isHalfBeatEnabled)
			{
				sequences[3] = sequences[3] + 0.5f;
			}
			else
			{
				sequences[3] += 1f;
			}
			if (sequences[3] > 2f)
			{
				sequences[3] = 0f;
			}
		}
		if (sequences[4] > 0f)
		{
			HypnoLair.env.PlayWooshDelayed(timeBeatStarted, isFullBeat);
			sequences[4] = 0f;
		}
		if (!(sequences[5] > 0f))
		{
			return;
		}
		num = sequences[5];
		if (num != 1f)
		{
			if (num != 1.5f)
			{
				if (num == 2f)
				{
					HypnoLair.env.Eye.CloseDelayed(timeBeatStarted);
				}
			}
			else
			{
				HypnoLair.env.Eye.CloseDelayed(timeBeatStarted);
				QueueHitWindow(1);
				HypnoLair.env.PlayWooshDelayed(timeBeatStarted, isFullBeat);
				HypnoLair.env.GetDeactivatedPocketWatch().CrossInDelayed(timeBeatStarted, beat);
				HypnoLair.env.Eye.ToggleIsDoubled(toggle: true);
			}
		}
		else
		{
			QueueHitWindow(1);
			HypnoLair.env.PlayWooshDelayed(timeBeatStarted, isFullBeat);
			HypnoLair.env.GetDeactivatedPocketWatch().CrossInDelayed(timeBeatStarted, beat);
			HypnoLair.env.Eye.ToggleIsDoubled(toggle: true);
		}
		if (isHalfBeatEnabled)
		{
			sequences[5] = sequences[5] + 0.5f;
		}
		else
		{
			sequences[5] += 1f;
		}
		if (sequences[5] > 2f)
		{
			sequences[5] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 1)
		{
			HypnoLair.env.EnterPov();
		}
		else if (eventNum == 2)
		{
			HypnoLair.env.DroopPovEye();
		}
		else if (eventNum == 3)
		{
			HypnoLair.env.ShutPovEye();
		}
		else if (eventNum == 4)
		{
			HypnoLair.env.UnshutPovEye();
		}
		else if (eventNum == 5)
		{
			HypnoLair.env.UndroopPovEye();
		}
		else if (eventNum == 6)
		{
			HypnoLair.env.LeavePov();
		}
	}

	protected override void OnTempoChange()
	{
		HypnoLair.env.RefreshSpeed(beatDelta, beat);
	}

	protected override void OnAction()
	{
		if (beat == 1 || beat == 3)
		{
			HypnoLair.env.PlayClick();
		}
		else
		{
			HypnoLair.env.PlayBlip();
		}
	}

	protected override void OnActionReleased()
	{
	}

	protected override void OnPrep()
	{
	}

	protected override void OnHit()
	{
		HypnoLair.env.Pulse(accuracy);
	}

	protected override void OnStrike()
	{
		HypnoLair.env.Pulse(0f);
		HypnoLair.env.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			HypnoLair.env.Sweat.CrossIn();
		}
	}
}
