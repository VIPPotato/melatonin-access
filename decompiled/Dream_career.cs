using System.Collections;
using UnityEngine;

public class Dream_career : Dream
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
		OfficeSpace.env.Show();
		float timeStarted = Technician.mgr.GetDspTime();
		OfficeSpace.env.BobbleDelayed(0f, 1);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		OfficeSpace.env.BobbleDelayed(0f, 2);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		OfficeSpace.env.BobbleDelayed(0f, 3);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		OfficeSpace.env.BobbleDelayed(0f, 4);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
			if (SaveManager.GetLang() == 3)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			else if (SaveManager.GetLang() >= 6)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			int tempBeat = 1;
			while (isFux)
			{
				tempBeat++;
				if (tempBeat > 4)
				{
					tempBeat = 2;
				}
				OfficeSpace.env.BobbleDelayed(0f, tempBeat);
				timeStarted = Technician.mgr.GetDspTime();
				yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat());
				yield return null;
			}
		}
		TriggerSong();
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
		OfficeSpace.env.BobbleDelayed(beatDelta, beat);
	}

	protected override void OnBeat()
	{
		if (gameMode == 0)
		{
			if (phrase == 1 && bar == 1 && beat == 1)
			{
				OfficeSpace.env.FirstLoop(isReverse: false);
			}
			else if (beat == 1 && (bar == 1 || bar == 3 || bar == 5 || bar == 7))
			{
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
		}
		else if (phrase == 1 || phrase == 3)
		{
			if (beat == 1 && bar == 1)
			{
				OfficeSpace.env.FirstLoop(isReverse: false);
				OfficeSpace.env.SetCueStyle(1);
			}
			else if (beat == 1 && (bar == 3 || bar == 5))
			{
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (beat == 1 && bar == 7)
			{
				OfficeSpace.env.LastLoop(isReverse: false);
			}
		}
		else if (phrase == 2 || phrase == 4)
		{
			if (beat == 1 && bar == 1)
			{
				OfficeSpace.env.FirstLoop(isReverse: true);
				OfficeSpace.env.SetCueStyle(0);
			}
			else if (beat == 1 && (bar == 3 || bar == 5))
			{
				OfficeSpace.env.Loop(1f, isReverse: true);
			}
			else if (beat == 1 && bar == 7)
			{
				OfficeSpace.env.LastLoop(isReverse: true);
			}
		}
		else if (phrase == 5)
		{
			if (beat == 1 && bar == 1)
			{
				OfficeSpace.env.FirstLoop(isReverse: false);
				OfficeSpace.env.SetCueStyle(2);
			}
			else if (beat == 1 && (bar == 3 || bar == 5 || bar == 7))
			{
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueLeftHitWindow(1);
			OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(0, 2), 1);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			QueueRightHitWindow(1);
			OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(2, 4), 1);
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			QueueLeftRightHitWindow(1);
			OfficeSpace.env.SendMessageDelayed(timeBeatStarted, 4, 1);
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			QueueLeftHitWindow(2);
			OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(0, 2), 2);
			sequences[3] = 0f;
		}
		if (sequences[4] > 0f)
		{
			QueueRightHitWindow(2);
			OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(2, 4), 2);
			sequences[4] = 0f;
		}
		if (sequences[5] > 0f)
		{
			QueueLeftRightHitWindow(2);
			OfficeSpace.env.SendMessageDelayed(timeBeatStarted, 4, 2);
			sequences[5] = 0f;
		}
		if (sequences[6] > 0f)
		{
			rng = Random.Range(0, 2);
			if (rng == 0)
			{
				QueueLeftHitWindow(1);
				OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(0, 2), 1);
			}
			else
			{
				QueueRightHitWindow(1);
				OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(2, 4), 1);
			}
			sequences[6] = 0f;
		}
		if (sequences[7] > 0f)
		{
			rng = Random.Range(0, 3);
			if (rng == 0)
			{
				QueueLeftHitWindow(1);
				OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(0, 2), 1);
			}
			else if (rng == 1)
			{
				QueueRightHitWindow(1);
				OfficeSpace.env.SendMessageDelayed(timeBeatStarted, Random.Range(2, 4), 1);
			}
			else
			{
				QueueLeftRightHitWindow(1);
				OfficeSpace.env.SendMessageDelayed(timeBeatStarted, 4, 1);
			}
			sequences[7] = 0f;
		}
	}

	protected override void OnActionLeft()
	{
		OfficeSpace.env.Type(1);
	}

	protected override void OnActionRight()
	{
		OfficeSpace.env.Type(2);
	}

	protected override void OnEvent()
	{
		if (eventNum == 1)
		{
			OfficeSpace.env.SleepWorkers(beatDelta);
		}
	}

	protected override void OnHit()
	{
		OfficeSpace.env.SendFeedback(hitType);
		McWorker[] mcWorkers = OfficeSpace.env.McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].HitActiveWorkMessage(accuracy);
		}
	}

	protected override void OnStrike()
	{
		OfficeSpace.env.SendMistake();
		McWorker[] mcWorkers = OfficeSpace.env.McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].StrikeActiveWorkMessage();
		}
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			OfficeSpace.env.SendMistake();
		}
		McWorker[] mcWorkers = OfficeSpace.env.McWorkers;
		for (int i = 0; i < mcWorkers.Length; i++)
		{
			mcWorkers[i].MissActiveWorkMessage();
		}
	}
}
