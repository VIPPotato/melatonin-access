using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream_exercise : Dream
{
	private bool isFux;

	private int fuxState;

	private int rng;

	private List<int> rngs = new List<int> { 0, 0, 0, 0, 0 };

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Gym.env.Show();
		Interface.env.Cam.Drift(1);
		yield return new WaitForSeconds(0.1f);
		float timeStarted = Technician.mgr.GetDspTime();
		Gym.env.BobbleDelayed(0f, 0, isHitWindow: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Gym.env.BobbleDelayed(0f, 0, isHitWindow: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() == 6)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			else if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			while (isFux)
			{
				timeStarted = Technician.mgr.GetDspTime();
				Gym.env.BobbleDelayed(0f, 0, isHitWindow: false);
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
		Gym.env.BobbleDelayed(beatDelta, beat, isHitWindow);
		if (beat == 1 && (phrase != 4 || (phrase == 4 && bar % 2 == 1)))
		{
			Gym.env.PulldownDelayed(timeBeatStarted);
		}
	}

	protected override void OnBeat()
	{
		if (beat == 1)
		{
			if (phrase != 4)
			{
				if (bar % 2 == 1)
				{
					Gym.env.ShiftFocusDelayed(timeBeatStarted, 1);
				}
				else
				{
					Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
				}
			}
			else if (bar == 1 || bar == 5)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 1);
			}
			else if (bar == 3 || bar == 7)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
		}
		if (gameMode == 0)
		{
			if ((phrase == 1 && bar == 1) || bar == 2)
			{
				Gym.env.CancelShiftFocus();
			}
			else if (phrase == 2 && bar == 1 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 0);
			}
			else if (phrase == 3 && bar == 1 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 0);
			}
			else if (phrase == 3 && bar == 8 && beat == 4)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 0);
			}
			else if (phrase == 4 && bar == 1 && beat == 1)
			{
				Gym.env.CancelShiftFocus();
			}
		}
		else if (gameMode == 1)
		{
			if (phrase == 1 && bar < 5)
			{
				Gym.env.CancelShiftFocus();
			}
			if (phrase == 5 && bar == 8 && beat == 4)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 0);
			}
			else if (phrase == 6 && bar == 1 && beat == 1)
			{
				Gym.env.CancelShiftFocus();
			}
		}
		else if (gameMode < 6)
		{
			if (phrase == 5 && bar == 8 && beat == 4)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 0);
			}
			if (phrase == 6 && bar == 1 && beat == 1)
			{
				Gym.env.CancelShiftFocus();
			}
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
			{
				QueueLeftHitWindow(8);
			}
			else
			{
				QueueLeftHitWindow(4);
			}
			Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 1);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
			{
				QueueRightHitWindow(8);
			}
			else
			{
				QueueRightHitWindow(4);
			}
			Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 2);
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
			{
				QueueLeftRightHitWindow(8);
			}
			else
			{
				QueueLeftRightHitWindow(4);
			}
			Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 3);
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			rng = Random.Range(0, 2);
			if (rng == 0)
			{
				if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
				{
					QueueLeftHitWindow(8);
				}
				else
				{
					QueueLeftHitWindow(4);
				}
				Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 1);
			}
			else
			{
				if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
				{
					QueueRightHitWindow(8);
				}
				else
				{
					QueueRightHitWindow(4);
				}
				Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 2);
			}
			sequences[3] = 0f;
		}
		if (!(sequences[4] > 0f))
		{
			return;
		}
		rng = Random.Range(0, 3);
		if (rng == 0)
		{
			if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
			{
				QueueLeftHitWindow(8);
			}
			else
			{
				QueueLeftHitWindow(4);
			}
			Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 1);
		}
		else if (rng == 1)
		{
			if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
			{
				QueueRightHitWindow(8);
			}
			else
			{
				QueueRightHitWindow(4);
			}
			Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 2);
		}
		else if (rng == 2)
		{
			if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6 || gameMode == 7))
			{
				QueueLeftRightHitWindow(8);
			}
			else
			{
				QueueLeftRightHitWindow(4);
			}
			Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 3);
		}
		sequences[4] = 0f;
	}

	protected override void OnActionLeft()
	{
		Gym.env.McLifter.Lift(1);
	}

	protected override void OnActionLeftReleased()
	{
		Gym.env.McLifter.Unlift(1);
	}

	protected override void OnActionRight()
	{
		Gym.env.McLifter.Lift(2);
	}

	protected override void OnActionRightReleased()
	{
		Gym.env.McLifter.Unlift(2);
	}

	protected override void OnHit()
	{
	}

	protected override void OnMiss()
	{
		Gym.env.McLifter.Sweat.CrossIn();
	}

	protected override void OnStrike()
	{
		Gym.env.McLifter.Sweat.CrossIn();
	}
}
