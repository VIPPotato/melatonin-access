using System.Collections;
using UnityEngine;

public class Dream_food : Dream
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
		Interface.env.Cam.Drift(1);
		FoodySkies.env.Show();
		FoodySkies.env.GetActivePizzaBox().Hide();
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
		FoodySkies.env.GetActivePizzaBox().Activate(0);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
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
			if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			else if (SaveManager.GetLang() == 9)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			while (isFux || isFuxAlt)
			{
				timeStarted = Technician.mgr.GetDspTime();
				FoodySkies.env.McChomper.BobbleDelayed(0f);
				FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
		Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: true);
	}

	protected override void OnUpdate()
	{
		if (isFux)
		{
			if (!ControlHandler.mgr.CheckIsActionPressed() || !DreamWorld.env.DialogBox.CheckIsActivated() || !(Time.timeScale > 0f))
			{
				return;
			}
			fuxState++;
			if (fuxState == 1)
			{
				if (SaveManager.GetLang() >= 5)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f, 1);
				}
				else
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1);
				}
			}
			else if (fuxState >= 2)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
		else if (isFuxAlt && ControlHandler.mgr.CheckIsActionPressed() && DreamWorld.env.DialogBox.CheckIsActivated() && Time.timeScale > 0f)
		{
			fuxState++;
			if (fuxState >= 1)
			{
				isFuxAlt = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
	}

	protected override void OnBobble()
	{
		if (FoodySkies.env.GetActivePizzaBox().CheckIsActivated())
		{
			FoodySkies.env.GetActivePizzaBox().BobbleDelayed(beatDelta);
		}
		FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			if (gameMode < 6)
			{
				sequences[0] = 0f;
				QueueHitWindow(2);
				FoodySkies.env.GetActivePizzaBox().ThrowNormalDelayed(timeBeatStarted);
				FoodySkies.env.GetActivePizzaBox().ThrowSoundNormalDelayed(timeBeatStarted);
			}
			else
			{
				if (sequences[0] == 1f)
				{
					FoodySkies.env.CancelAllSounds();
					FoodySkies.env.GetActivePizzaBox().ThrowNormalDelayed(timeBeatStarted);
					FoodySkies.env.GetActivePizzaBox().ThrowSoundNormalDelayed(timeBeatStarted);
				}
				else if (sequences[0] == 2f)
				{
					QueueHitWindow(1);
				}
				if (isHalfBeatEnabled)
				{
					sequences[0] = sequences[0] + 0.5f;
				}
				else
				{
					sequences[0] += 1f;
				}
				if (sequences[0] > 2f)
				{
					sequences[0] = 0f;
				}
			}
		}
		if (sequences[1] > 0f)
		{
			if (gameMode < 6)
			{
				sequences[1] = 0f;
				QueueHitWindow(4);
				FoodySkies.env.GetActivePizzaBox().ThrowSlowDelayed(timeBeatStarted);
				FoodySkies.env.GetActivePizzaBox().ThrowSoundSlowDelayed(timeBeatStarted);
			}
			else
			{
				if (sequences[1] == 1f)
				{
					FoodySkies.env.CancelAllSounds();
					FoodySkies.env.GetActivePizzaBox().ThrowSlowDelayed(timeBeatStarted);
					FoodySkies.env.GetActivePizzaBox().ThrowSoundSlowDelayed(timeBeatStarted);
				}
				else if (sequences[1] == 3f)
				{
					QueueHitWindow(2);
				}
				if (isHalfBeatEnabled)
				{
					sequences[1] = sequences[1] + 0.5f;
				}
				else
				{
					sequences[1] += 1f;
				}
				if (sequences[1] > 3f)
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
				FoodySkies.env.GetActivePizzaBox().ThrowFast(timeBeatStarted, 1f);
				FoodySkies.env.GetActivePizzaBox().ThrowSoundFast(timeBeatStarted, 1f);
				sequences[2] = 0f;
			}
			else
			{
				if (sequences[2] == 1f)
				{
					FoodySkies.env.CancelAllSounds();
					FoodySkies.env.GetActivePizzaBox().ThrowFast(timeBeatStarted, 1f);
					FoodySkies.env.GetActivePizzaBox().ThrowSoundFast(timeBeatStarted, 1f);
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
		if (!(sequences[3] > 0f))
		{
			return;
		}
		if (gameMode < 6)
		{
			QueueHitWindow(6);
			FoodySkies.env.GetActivePizzaBox().ThrowFast(timeBeatStarted, 2f);
			FoodySkies.env.GetActivePizzaBox().ThrowSoundFast(timeBeatStarted, 2f);
			sequences[3] = 0f;
			return;
		}
		if (sequences[3] == 1f)
		{
			FoodySkies.env.CancelAllSounds();
			FoodySkies.env.GetActivePizzaBox().ThrowFast(timeBeatStarted, 2f);
			FoodySkies.env.GetActivePizzaBox().ThrowSoundFast(timeBeatStarted, 2f);
		}
		else if (sequences[3] == 6f)
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
		if (sequences[3] > 6f)
		{
			sequences[3] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 0)
		{
			FoodySkies.env.Unblind();
		}
		else if (eventNum == 1)
		{
			FoodySkies.env.Blind();
		}
		else if (eventNum == 2)
		{
			FoodySkies.env.GetActivePizzaBox().Swap(timeBeatStarted, 0);
		}
		else if (eventNum == 3)
		{
			FoodySkies.env.GetActivePizzaBox().Swap(timeBeatStarted, 1);
		}
		else if (eventNum == 4)
		{
			FoodySkies.env.GetActivePizzaBox().Swap(timeBeatStarted, 2);
		}
	}

	protected override void OnHit()
	{
		if (accuracy >= 1f)
		{
			FoodySkies.env.Hit(timeBeatStarted);
			return;
		}
		FoodySkies.env.PlayMissFeedbackSfx();
		FoodySkies.env.McChomper.Strike();
	}

	protected override void OnStrike()
	{
		FoodySkies.env.McChomper.Strike();
		FoodySkies.env.McChomper.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			FoodySkies.env.McChomper.Sweat.CrossIn();
		}
	}

	protected override void OnResults()
	{
		FoodySkies.env.ZoomEnd();
	}
}
