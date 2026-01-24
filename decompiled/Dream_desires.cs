using System.Collections;
using UnityEngine;

public class Dream_desires : Dream
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
		Espot.env.Show();
		yield return new WaitForSeconds(0.1f);
		float timeStarted = Technician.mgr.GetDspTime();
		Espot.env.UfoMachine.FlashSlotsDelayed(0f, 1);
		Espot.env.Arcade.RefreshScreensDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Espot.env.UfoMachine.FlashSlotsDelayed(0f, 2);
		Espot.env.Arcade.RefreshScreensDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		Espot.env.UfoMachine.FlashSlotsDelayed(0f, 3);
		Espot.env.Arcade.RefreshScreensDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		Espot.env.UfoMachine.FlashSlotsDelayed(0f, 4);
		Espot.env.Arcade.RefreshScreensDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		if (gameMode == 0)
		{
			isFux = true;
			Interface.env.Letterbox.DeactivateDelayed();
			DreamWorld.env.DialogBox.ActivateDelayed(0f, isSoundTriggered: true);
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
			else if (SaveManager.GetLang() == 3)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			else if (SaveManager.GetLang() >= 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
			}
			while (isFux)
			{
				Espot.env.UfoMachine.FlashSlotsDelayed(0f, 4);
				Espot.env.Arcade.RefreshScreensDelayed(0f);
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
			if (fuxState == 1)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(1);
			}
			else if (fuxState >= 2)
			{
				isFux = false;
				DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			}
		}
	}

	protected override void OnBobble()
	{
		if (bar == 1 && beat == 1)
		{
			Espot.env.ScaleBobble(1f);
		}
		Espot.env.UfoMachine.FlashSlotsDelayed(beatDelta, beat);
		Espot.env.Arcade.RefreshScreensDelayed(beatDelta);
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			QueueHoldReleaseWindow(1, 3);
			Espot.env.CueFastDelayed(timeBeatStarted, 0);
			Espot.env.CueFastSoundDelayed(timeBeatStarted, 0);
			sequences[0] = 0f;
		}
		if (sequences[1] > 0f)
		{
			QueueHoldReleaseWindow(2, 6);
			Espot.env.CueSlowDelayed(timeBeatStarted, 0);
			Espot.env.CueSlowSoundDelayed(timeBeatStarted, 0);
			sequences[1] = 0f;
		}
		if (sequences[2] > 0f)
		{
			QueueHoldReleaseWindow(1, 9);
			Espot.env.CueFastDelayed(timeBeatStarted, 1);
			Espot.env.CueFastSoundDelayed(timeBeatStarted, 1);
			sequences[2] = 0f;
		}
		if (sequences[3] > 0f)
		{
			QueueHoldReleaseWindow(2, 10);
			Espot.env.CueSlowDelayed(timeBeatStarted, 1);
			Espot.env.CueSlowSoundDelayed(timeBeatStarted, 1);
			sequences[3] = 0f;
		}
		if (sequences[4] > 0f)
		{
			QueueHoldReleaseWindow(1, 15);
			Espot.env.CueFastDelayed(timeBeatStarted, 2);
			Espot.env.CueFastSoundDelayed(timeBeatStarted, 2);
			sequences[4] = 0f;
		}
		if (sequences[5] > 0f)
		{
			QueueHoldReleaseWindow(2, 14);
			Espot.env.CueSlowDelayed(timeBeatStarted, 2);
			Espot.env.CueSlowSoundDelayed(timeBeatStarted, 2);
			sequences[5] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 0)
		{
			Espot.env.Arcade.Light();
		}
		else if (eventNum == 1)
		{
			Espot.env.Arcade.Darken();
		}
	}

	protected override void OnAction()
	{
		Espot.env.PlayGrabSfx();
		Espot.env.UfoMachine.Grab();
	}

	protected override void OnActionReleased()
	{
		Espot.env.PlayReleaseSfx();
		Espot.env.UfoMachine.Release();
	}

	protected override void OnPrep()
	{
		Espot.env.UfoMachine.Claw.React(accuracy);
		Espot.env.UfoMachine.PickUpCapsule();
		Espot.env.Feedbacks[0].SetLocalPosition(3.49f, -0.7f);
	}

	protected override void OnHit()
	{
		Espot.env.UfoMachine.Claw.React(accuracy);
		if (accuracy >= 1f)
		{
			Espot.env.UfoMachine.DropCapsule();
		}
		else
		{
			Espot.env.UfoMachine.StickCapsule();
		}
		Espot.env.Feedbacks[0].SetLocalPosition(-5.55f, -0.7f);
	}

	protected override void OnStrike()
	{
		Espot.env.UfoMachine.Claw.React(0f);
		if (Espot.env.UfoMachine.CheckIsPickedUp())
		{
			Espot.env.UfoMachine.StickCapsule();
		}
		Espot.env.Sweat.CrossIn();
	}

	protected override void OnMiss()
	{
		if (gameMode < 6)
		{
			Espot.env.Sweat.CrossIn();
		}
	}
}
