using System.Collections;
using UnityEngine;

public class Dream_nature : Dream
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
		Conservatory.env.Show();
		Conservatory.env.WaterCan.Hover();
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
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
					Conservatory.env.WaterCan.Hover();
				}
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
		if (beat == 1)
		{
			Conservatory.env.WaterCan.Hover();
		}
	}

	protected override void OnBeat()
	{
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			Conservatory.env.MoveToNextSproutDelayed(beatDelta);
			sequences[0] = 0f;
		}
		else if (sequences[1] > 0f)
		{
			ctrlMode = 0;
			QueueHitWindow(2);
			QueueHitWindow(2, isHalfBeatAdded: true);
			QueueHitWindow(3);
			Conservatory.env.ThirstNextSproutDelayed(timeBeatStarted, isBubbled: true);
			sequences[1] = 0f;
		}
		else if (sequences[2] > 0f)
		{
			ctrlMode = 1;
			QueueHoldReleaseWindow(2, 3);
			Conservatory.env.DieNextSproutDelayed(timeBeatStarted, isBubbled: true);
			sequences[2] = 0f;
		}
		else if (sequences[3] > 0f)
		{
			ctrlMode = 0;
			QueueHitWindow(2);
			QueueHitWindow(2, isHalfBeatAdded: true);
			QueueHitWindow(3);
			Conservatory.env.ThirstNextSproutDelayed(timeBeatStarted, isBubbled: false);
			sequences[3] = 0f;
		}
		else if (sequences[4] > 0f)
		{
			ctrlMode = 1;
			QueueHoldReleaseWindow(2, 3);
			Conservatory.env.DieNextSproutDelayed(timeBeatStarted, isBubbled: false);
			sequences[4] = 0f;
		}
	}

	protected override void OnEvent()
	{
		switch (eventNum)
		{
		case 0:
			Conservatory.env.SetCamY(0f);
			Conservatory.env.SetCanY(Conservatory.env.GetCanY() - 0.6f);
			break;
		case 1:
			Conservatory.env.SetCamY(6.2f);
			Conservatory.env.SetCanY(Conservatory.env.GetCanY() + 0.6f);
			break;
		}
	}

	protected override void OnAction()
	{
		if (ctrlMode == 0)
		{
			Conservatory.env.SprayWater();
		}
		else
		{
			Conservatory.env.PourWater();
		}
	}

	protected override void OnActionReleased()
	{
		Conservatory.env.IdleWater();
		if (isPrepped)
		{
			Conservatory.env.Garden.GetActiveCrop().SoakCancel();
		}
	}

	protected override void OnPrep()
	{
		Conservatory.env.Garden.GetActiveCrop().Soak();
	}

	protected override void OnHit()
	{
		if (ctrlMode == 0)
		{
			Conservatory.env.Garden.GetActiveCrop().Spray(accuracy);
			return;
		}
		Conservatory.env.FinishWater();
		if (Conservatory.env.Garden.GetActiveCrop().PlantBubble.CheckIsInteractive())
		{
			if (accuracy == 1f)
			{
				Conservatory.env.Garden.GetActiveCrop().PlantBubble.WaterLevel.MaxLinearRise();
			}
			else
			{
				Conservatory.env.Garden.GetActiveCrop().PlantBubble.WaterLevel.PauseLinearRise(accuracy);
			}
		}
	}

	protected override void OnStrike()
	{
		Conservatory.env.Sweat.CrossIn();
		if (isPrepped && Conservatory.env.Garden.GetActiveCrop().PlantBubble.CheckIsInteractive())
		{
			Conservatory.env.Garden.GetActiveCrop().PlantBubble.WaterLevel.PauseLinearRise(0f);
		}
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			Conservatory.env.Sweat.CrossIn();
		}
		Conservatory.env.Garden.GetActiveCrop().SoakCancel();
		if (isPrepped && Conservatory.env.Garden.GetActiveCrop().PlantBubble.CheckIsInteractive())
		{
			Conservatory.env.Garden.GetActiveCrop().PlantBubble.WaterLevel.PauseLinearRise(0f);
		}
	}

	protected override void OnExit()
	{
		Conservatory.env.Exit();
		DreamWorld.env.RecenterZoomer(Conservatory.env);
	}
}
