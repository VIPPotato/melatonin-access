using System.Collections;
using UnityEngine;

public class Dream_future : Dream
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
		NeoCity.env.Show();
		NeoCity.env.Bobble(beatDelta, 1, 1);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		NeoCity.env.Bobble(beatDelta, 1, 2);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		NeoCity.env.Bobble(beatDelta, 1, 3);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		NeoCity.env.Bobble(beatDelta, 1, 4);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() == 5)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			else if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			int tempBeat = 0;
			while (isFux)
			{
				tempBeat++;
				if (tempBeat > 4)
				{
					tempBeat = 1;
				}
				NeoCity.env.Bobble(0f, 1, tempBeat);
				if (tempBeat == 1 && NeoCity.env.Targets.GetActiveLocalZ() >= 179)
				{
					NeoCity.env.Targets.ResetActiveLocalZ();
				}
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: true);
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
		NeoCity.env.Bobble(beatDelta, bar, beat);
		if (beat == 1)
		{
			if (Interface.env.Circle.CheckIsActivated())
			{
				Interface.env.Circle.ResetRingNum();
			}
			if (bar == 1 && NeoCity.env.Targets.GetActiveLocalZ() >= 179)
			{
				NeoCity.env.Targets.ResetActiveLocalZ();
			}
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueLeftHitWindow(4);
			NeoCity.env.Targets.SendLeft(timeBeatStarted);
			NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 0);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			QueueHitWindow(4);
			NeoCity.env.Targets.SendCenter(timeBeatStarted);
			NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 1);
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			QueueRightHitWindow(4);
			NeoCity.env.Targets.SendRight(timeBeatStarted);
			NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 2);
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			QueueLeftRightHitWindow(4);
			NeoCity.env.Targets.SendLeft(timeBeatStarted);
			NeoCity.env.Targets.SendRight(timeBeatStarted);
			NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 3);
			sequences[3] = 0f;
		}
		if (sequences[4] > 0f)
		{
			rng = Random.Range(0, 4);
			if (rng == 0)
			{
				QueueLeftHitWindow(4);
				NeoCity.env.Targets.SendLeft(timeBeatStarted);
				NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 0);
			}
			else if (rng == 1)
			{
				QueueHitWindow(4);
				NeoCity.env.Targets.SendCenter(timeBeatStarted);
				NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 1);
			}
			else if (rng == 2)
			{
				QueueRightHitWindow(4);
				NeoCity.env.Targets.SendRight(timeBeatStarted);
				NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 2);
			}
			else
			{
				QueueLeftRightHitWindow(4);
				NeoCity.env.Targets.SendLeft(timeBeatStarted);
				NeoCity.env.Targets.SendRight(timeBeatStarted);
				NeoCity.env.TriggerCueDelayed(timeBeatStarted, isFullBeat, 3);
			}
			sequences[4] = 0f;
		}
	}

	protected override void OnEvent()
	{
		NeoCity.env.FakeOutDelayed(timeBeatStarted);
	}

	protected override void OnAction()
	{
		NeoCity.env.ShootCenter();
	}

	protected override void OnActionLeft()
	{
		NeoCity.env.ShootLeft();
	}

	protected override void OnActionRight()
	{
		NeoCity.env.ShootRight();
	}

	protected override void OnHit()
	{
		NeoCity.env.Hit(accuracy, hitType);
	}

	protected override void OnStrike()
	{
		NeoCity.env.Strike();
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			NeoCity.env.Cockpit.Radar.FlashAccuracy(0f);
			NeoCity.env.Sweat.CrossIn();
		}
	}
}
