using System.Collections;
using UnityEngine;

public class Dream_tech : Dream
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
		MechSpace.env.Show();
		Interface.env.Circle.SetSpawnEaseType(2);
		float timeStarted = Technician.mgr.GetDspTime();
		MechSpace.env.BobbleDelayed(0f, isHitWindow: false);
		MechSpace.env.StartSoundDelayed(1);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		MechSpace.env.BobbleDelayed(0f, isHitWindow: false);
		MechSpace.env.StartSoundDelayed(1);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		MechSpace.env.BobbleDelayed(0f, isHitWindow: false);
		MechSpace.env.StartSoundDelayed(1);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		MechSpace.env.BobbleDelayed(0f, isHitWindow: false);
		MechSpace.env.StartSoundDelayed(2);
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
			if (SaveManager.GetLang() >= 6)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			else
			{
				DreamWorld.env.DialogBox.SetDialogState(0);
			}
			while (isFux || isFuxAlt)
			{
				timeStarted = Technician.mgr.GetDspTime();
				MechSpace.env.BobbleDelayed(0f, isHitWindow: false);
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
		if (!MechSpace.env.CheckIsStoppedAiming() && beat == 1)
		{
			MechSpace.env.Crosshair.AimDelayed(beatDelta);
			MechSpace.env.McVirtual.AimDelayed(beatDelta);
		}
		MechSpace.env.BobbleDelayed(beatDelta, isHitWindow);
	}

	protected override void OnBeat()
	{
		if (gameMode == 0)
		{
			if (phrase == 4 && bar == 1 && beat == 1)
			{
				MechSpace.env.ToggleIsStoppedAiming(beatDelta, toggle: true);
			}
		}
		else if (gameMode != 6 && gameMode != 7 && phrase == 6 && bar == 1 && beat == 1)
		{
			MechSpace.env.ToggleIsStoppedAiming(beatDelta, toggle: true);
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: false);
			}
			sequences[0] = 0f;
		}
		else if (sequences[1] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: true);
			}
			sequences[1] = 0f;
		}
		else if (sequences[2] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: false, 1);
			}
			sequences[2] = 0f;
		}
		else if (sequences[3] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: true, 1);
			}
			sequences[3] = 0f;
		}
		else if (sequences[4] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: false, 2);
			}
			sequences[4] = 0f;
		}
		else if (sequences[5] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: true, 2);
			}
			sequences[5] = 0f;
		}
		else if (sequences[6] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: false, 3);
			}
			sequences[6] = 0f;
		}
		else if (sequences[7] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: true, 3);
			}
			sequences[7] = 0f;
		}
		else if (sequences[8] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: false, 4);
			}
			sequences[8] = 0f;
		}
		else if (sequences[9] > 0f)
		{
			if (beat != 4)
			{
				QueueHitWindow(1);
				MechSpace.env.TriggerEnemyDelayed(timeBeatStarted, phrase, beat, isFullBeat, isAltSound: true, 4);
			}
			sequences[9] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 0)
		{
			MechSpace.env.Unblind(beatDelta, beat);
		}
		else if (eventNum == 1)
		{
			MechSpace.env.Blind(timeBeatStarted);
		}
		else if (eventNum == 2)
		{
			MechSpace.env.ToggleIsStoppedAiming(beatDelta, toggle: true);
		}
		else if (eventNum == 3)
		{
			MechSpace.env.ToggleIsStoppedAiming(beatDelta, toggle: false);
		}
		else if (eventNum == 4)
		{
			MechSpace.env.ToggleIsStoppedAimingDelayed(beatDelta, toggle: true);
		}
	}

	protected override void OnAction()
	{
		MechSpace.env.ShootSound();
		MechSpace.env.McVirtual.Shoot();
		MechSpace.env.Crosshair.Shoot();
	}

	protected override void OnHitWindow()
	{
		if (isFullBeat)
		{
			if (beat == 2)
			{
				MechSpace.env.SpaceOppWrappers[0].AttackDelayed(timeBeatStarted);
			}
			else if (beat == 3)
			{
				MechSpace.env.SpaceOppWrappers[2].AttackDelayed(timeBeatStarted);
			}
			else if (beat == 4)
			{
				MechSpace.env.SpaceOppWrappers[4].AttackDelayed(timeBeatStarted);
			}
		}
		else if (halfBeat == 2)
		{
			MechSpace.env.SpaceOppWrappers[1].AttackDelayed(timeBeatStarted);
		}
		else if (halfBeat == 3)
		{
			MechSpace.env.SpaceOppWrappers[3].AttackDelayed(timeBeatStarted);
		}
		else if (halfBeat == 4)
		{
			MechSpace.env.SpaceOppWrappers[5].AttackDelayed(timeBeatStarted);
		}
	}

	protected override void OnHit()
	{
		MechSpace.env.Flash();
		if (isFullBeat)
		{
			if (beat == 2)
			{
				MechSpace.env.SpaceOppWrappers[0].Die();
			}
			else if (beat == 3)
			{
				MechSpace.env.SpaceOppWrappers[2].Die();
			}
			else if (beat == 4)
			{
				MechSpace.env.SpaceOppWrappers[4].Die();
			}
		}
		else if (halfBeat == 2)
		{
			MechSpace.env.SpaceOppWrappers[1].Die();
		}
		else if (halfBeat == 3)
		{
			MechSpace.env.SpaceOppWrappers[3].Die();
		}
		else if (halfBeat == 4)
		{
			MechSpace.env.SpaceOppWrappers[5].Die();
		}
		Interface.env.Cam.Shake();
	}

	protected override void OnStrike()
	{
		MechSpace.env.McVirtual.Sweat.CrossIn();
		Interface.env.Cam.Breeze();
	}

	protected override void OnMiss()
	{
		MechSpace.env.McVirtual.Sweat.CrossIn();
	}
}
