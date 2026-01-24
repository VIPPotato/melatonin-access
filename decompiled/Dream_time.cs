using System.Collections;
using UnityEngine;

public class Dream_time : Dream
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
		Matrix.env.Show();
		Matrix.env.ParallaxIn(timeBeatStarted, newIsZoomedFarOut: false);
		Matrix.env.LilBlocks.Wave();
		if (gameMode == 2)
		{
			Matrix.env.Bobble(2f);
		}
		else
		{
			Matrix.env.Bobble(1f);
		}
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		if (gameMode != 2)
		{
			Matrix.env.Bobble(1f);
		}
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		if (gameMode == 2)
		{
			Matrix.env.Bobble(2f);
		}
		else
		{
			Matrix.env.Bobble(1f);
		}
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		if (gameMode != 2)
		{
			Matrix.env.Bobble(1f);
		}
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
					Matrix.env.LilBlocks.Wave();
				}
				Matrix.env.Bobble(1f);
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
			if (SaveManager.GetLang() == 0)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(1);
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
			else if (SaveManager.GetLang() == 6)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f, 1);
			}
			else if (SaveManager.GetLang() == 7)
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

	protected override void OnBobble()
	{
		Matrix.env.Tick();
		if (gameMode == 2 && beat % 2 == 1)
		{
			Matrix.env.Bobble(2f);
		}
		else if (gameMode != 2)
		{
			Matrix.env.Bobble(1f);
		}
		if (beat != 1)
		{
			return;
		}
		Matrix.env.LilBlocks.Wave();
		if (!Matrix.env.CheckIsZoomedFarOut())
		{
			if (bar % 2 == 1)
			{
				Matrix.env.ParallaxOut(newIsZoomedFarOut: false);
			}
			else
			{
				Matrix.env.ParallaxIn(timeBeatStarted, newIsZoomedFarOut: false);
			}
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true);
				Matrix.env.ThrowDelayed(timeBeatStarted, isExtended: false);
				Matrix.env.ThrowTossSoundDelayed(timeBeatStarted, isExtended: false);
				sequences[0] = 0f;
			}
			else
			{
				if (sequences[0] == 1f)
				{
					Matrix.env.CancelAllSounds();
					Matrix.env.ThrowDelayed(timeBeatStarted, isExtended: false);
					Matrix.env.ThrowTossSoundDelayed(timeBeatStarted, isExtended: false);
				}
				else if (sequences[0] == 3f)
				{
					QueueHoldReleaseWindow(0, 1, isHalfBeatAddedToHold: true);
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
			float num = sequences[1];
			if (num != 1f)
			{
				if (num != 4f)
				{
					if (num == 6f)
					{
						QueueHoldReleaseWindow(1, 2, isHalfBeatAddedToHold: true);
					}
				}
				else
				{
					QueueHoldReleaseWindow(1, 2, isHalfBeatAddedToHold: true);
				}
			}
			else
			{
				QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true);
				Matrix.env.ThrowDelayed(timeBeatStarted, isExtended: true);
				Matrix.env.ThrowTossSoundDelayed(timeBeatStarted, isExtended: true);
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
		if (sequences[2] > 0f)
		{
			if (gameMode < 6)
			{
				QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true);
				Matrix.env.TossDelayed(timeBeatStarted, isExtended: false);
				Matrix.env.ThrowTossSoundDelayed(timeBeatStarted, isExtended: false);
				sequences[2] = 0f;
			}
			else
			{
				if (sequences[2] == 1f)
				{
					Matrix.env.CancelAllSounds();
					Matrix.env.TossDelayed(timeBeatStarted, isExtended: false);
					Matrix.env.ThrowTossSoundDelayed(timeBeatStarted, isExtended: false);
				}
				else if (sequences[2] == 3f)
				{
					QueueHoldReleaseWindow(0, 1, isHalfBeatAddedToHold: true);
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
		if (sequences[3] > 0f)
		{
			float num = sequences[3];
			if (num != 1f)
			{
				if (num != 4f)
				{
					if (num == 6f)
					{
						QueueHoldReleaseWindow(1, 2, isHalfBeatAddedToHold: true);
					}
				}
				else
				{
					QueueHoldReleaseWindow(1, 2, isHalfBeatAddedToHold: true);
				}
			}
			else
			{
				QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true);
				Matrix.env.TossDelayed(timeBeatStarted, isExtended: true);
				Matrix.env.ThrowTossSoundDelayed(timeBeatStarted, isExtended: true);
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
		if (!(sequences[4] > 0f))
		{
			return;
		}
		if (gameMode < 6)
		{
			QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true);
			Matrix.env.ShootDelayed(timeBeatStarted);
			Matrix.env.ShootSoundDelayed(timeBeatStarted);
			sequences[4] = 0f;
			return;
		}
		if (sequences[4] == 1f)
		{
			Matrix.env.CancelAllSounds();
			Matrix.env.ShootDelayed(timeBeatStarted);
			Matrix.env.ShootSoundDelayed(timeBeatStarted);
		}
		else if (sequences[4] == 3f)
		{
			QueueHoldReleaseWindow(0, 1, isHalfBeatAddedToHold: true);
		}
		if (isHalfBeatEnabled)
		{
			sequences[4] = sequences[4] + 0.5f;
		}
		else
		{
			sequences[4] += 1f;
		}
		if (sequences[4] > 3f)
		{
			sequences[4] = 0f;
		}
	}

	protected override void OnEvent()
	{
		if (eventNum == 0)
		{
			Matrix.env.ParallaxIn(beatDelta, newIsZoomedFarOut: true);
		}
		else if (eventNum == 1)
		{
			Matrix.env.ParallaxOut(newIsZoomedFarOut: true);
		}
		else if (eventNum == 2)
		{
			Matrix.env.ToggleIsZoomedOutFar(toggle: false);
		}
		else if (eventNum == 3)
		{
			Matrix.env.ToggleIsZoomedOutFar(toggle: true);
		}
	}

	protected override void OnAction()
	{
		Matrix.env.Press();
	}

	protected override void OnHit()
	{
		Matrix.env.Hit();
		Matrix.env.Release();
		Interface.env.Cam.Shake();
	}

	protected override void OnStrike()
	{
		Matrix.env.Strike();
		Matrix.env.ResetMcSwingers();
	}

	protected override void OnMiss()
	{
		if (isPrepped)
		{
			Matrix.env.IdleMcSwingers();
		}
		if (gameMode < 6)
		{
			Matrix.env.Strike();
		}
	}
}
