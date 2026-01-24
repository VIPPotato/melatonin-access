using System.Collections;
using UnityEngine;

public class Dream_setbacks : Dream
{
	private int rng;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Underworld.env.Show(isCuedUp: false);
		Underworld.env.LavaPool.SetToLowered(0.8518f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
		Underworld.env.LavaPool.PopBubble();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		TriggerSong();
	}

	protected override void OnUpdate()
	{
		if (!Darkroom.env.CheckIsActivated() || !Darkroom.env.CheckIsPlayable())
		{
			return;
		}
		if (isActionPressing)
		{
			foreach (MemoryPhoto burnableMemoryPhoto in Darkroom.env.PhotoPulley.GetBurnableMemoryPhotos())
			{
				burnableMemoryPhoto.StartBurn();
			}
			return;
		}
		foreach (MemoryPhoto burnableMemoryPhoto2 in Darkroom.env.PhotoPulley.GetBurnableMemoryPhotos())
		{
			burnableMemoryPhoto2.StopBurn();
		}
	}

	protected override void OnBobble()
	{
		if (Underworld.env.CheckIsActivated())
		{
			if (!isHitWindow)
			{
				Underworld.env.Climb(0.8518f);
				Underworld.env.LavaPool.LinearRise(beatDelta, 0.8518f);
			}
			if (beat == 1 || beat == 3)
			{
				Underworld.env.LavaPool.PopBubble();
			}
		}
		else if (Espot.env.CheckIsActivated())
		{
			Espot.env.UfoMachine.FlashSlotsDelayed(beatDelta, beat);
			Espot.env.Arcade.RefreshScreensDelayed(beatDelta);
		}
		else if (NeoCity.env.CheckIsActivated())
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
		if (Darkroom.env.CheckIsActivated())
		{
			Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
		}
	}

	protected override void OnEvent()
	{
		if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable())
		{
			Darkroom.env.PhotoPulley.SetDragType(eventNum);
		}
		else if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.FakeOutDelayed(timeBeatStarted);
		}
	}

	protected override void OnBeat()
	{
		if (gameMode >= 6)
		{
			return;
		}
		if (phrase == 1)
		{
			if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				ctrlMode = 1;
				Underworld.env.Hide();
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
			}
			else if (bar == 8 && beat == 1)
			{
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
			}
		}
		else if (phrase == 2)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				Espot.env.Hide();
				Darkroom.env.MakePlayable();
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				Darkroom.env.Hide();
				NeoCity.env.Show();
			}
		}
		else if (phrase == 3)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				NeoCity.env.Hide();
				Underworld.env.Show(isCuedUp: true);
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Underworld.env.Hide();
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				Espot.env.Hide();
				Underworld.env.Show(isCuedUp: true);
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Underworld.env.Hide();
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
			}
			else if (bar == 8 && beat == 1)
			{
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
			}
		}
		else if (phrase == 4)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Espot.env.Hide();
				Darkroom.env.MakePlayable();
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				Darkroom.env.Hide();
				NeoCity.env.Show();
			}
			else if (bar == 4 && beat == 1)
			{
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				NeoCity.env.Hide();
				Darkroom.env.MakePlayable();
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				Darkroom.env.Hide();
				NeoCity.env.Show();
			}
		}
		else if (phrase == 5)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				NeoCity.env.Hide();
				Underworld.env.Show(isCuedUp: true);
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Underworld.env.Hide();
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
			}
			else if (bar == 4 && beat == 1)
			{
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Espot.env.Hide();
				Darkroom.env.MakePlayable();
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				Darkroom.env.Hide();
				NeoCity.env.Show();
			}
		}
	}

	protected override void OnSequence()
	{
		if (Underworld.env.CheckIsActivated())
		{
			if (sequences[0] > 0f)
			{
				QueueLeftHitWindow(1);
				Underworld.env.CueLeftDelayed(timeBeatStarted);
				sequences[0] = 0f;
			}
			if (sequences[1] > 0f)
			{
				QueueRightHitWindow(1);
				Underworld.env.CueRightDelayed(timeBeatStarted);
				sequences[1] = 0f;
			}
			if (sequences[2] > 0f)
			{
				QueueHitWindow(1);
				Underworld.env.CueCenterDelayed(timeBeatStarted);
				sequences[2] = 0f;
			}
			if (sequences[3] > 0f)
			{
				rng = Random.Range(0, 2);
				if (rng == 0)
				{
					QueueLeftHitWindow(1);
					Underworld.env.CueLeftDelayed(timeBeatStarted);
				}
				else
				{
					QueueRightHitWindow(1);
					Underworld.env.CueRightDelayed(timeBeatStarted);
				}
				sequences[3] = 0f;
			}
		}
		else if (Espot.env.CheckIsActivated())
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
		else if (NeoCity.env.CheckIsActivated())
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
		if (!Darkroom.env.CheckIsActivated())
		{
			return;
		}
		if (gameMode >= 6)
		{
			if (sequences[0] > 0f)
			{
				QueueHoldReleaseWindow(4, 5);
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 0);
				sequences[0] = 0f;
			}
			else if (sequences[1] > 0f)
			{
				QueueHoldReleaseWindow(4, 4, isHalfBeatAddedToHold: false, isHalfBeatAddedToRelease: true);
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 1);
				sequences[1] = 0f;
			}
			else if (sequences[2] > 0f)
			{
				QueueHoldReleaseWindow(4, 6);
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 3);
				sequences[2] = 0f;
			}
			else if (sequences[3] > 0f)
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
				sequences[3] = 0f;
			}
			else if (sequences[4] > 0f)
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 1);
				sequences[4] = 0f;
			}
			else if (sequences[5] > 0f)
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 3);
				sequences[5] = 0f;
			}
			return;
		}
		float num;
		if (sequences[6] > 0f)
		{
			num = sequences[6];
			if (num != 1f)
			{
				if (num != 2f)
				{
					if (num != 3f)
					{
						if (num == 4f)
						{
							Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
						}
					}
					else
					{
						QueueHoldReleaseWindow(4, 5);
						Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 0);
					}
				}
				else
				{
					Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
				}
			}
			else if (gameMode == 4)
			{
				QueueHoldReleaseWindow(4, 5);
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 0);
			}
			else
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
			}
			if (isHalfBeatEnabled)
			{
				sequences[6] = sequences[6] + 0.5f;
			}
			else
			{
				sequences[6] += 1f;
			}
			if (sequences[6] > 4f)
			{
				sequences[6] = 0f;
			}
		}
		if (sequences[7] > 0f)
		{
			num = sequences[7];
			if (num != 1f)
			{
				if (num != 2f)
				{
					if (num != 3f)
					{
						if (num == 4f)
						{
							Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
						}
					}
					else
					{
						QueueHoldReleaseWindow(4, 4, isHalfBeatAddedToHold: false, isHalfBeatAddedToRelease: true);
						Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 1);
					}
				}
				else
				{
					Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
				}
			}
			else if (gameMode == 4)
			{
				QueueHoldReleaseWindow(4, 5);
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 0);
			}
			else
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
			}
			if (isHalfBeatEnabled)
			{
				sequences[7] = sequences[7] + 0.5f;
			}
			else
			{
				sequences[7] += 1f;
			}
			if (sequences[7] > 4f)
			{
				sequences[7] = 0f;
			}
		}
		if (sequences[8] > 0f)
		{
			num = sequences[8];
			if (num != 1f)
			{
				if (num != 2f)
				{
					if (num == 4f)
					{
						Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
					}
				}
				else
				{
					QueueHoldReleaseWindow(4, 6);
					Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 3);
				}
			}
			else
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
			}
			if (isHalfBeatEnabled)
			{
				sequences[8] = sequences[8] + 0.5f;
			}
			else
			{
				sequences[8] += 1f;
			}
			if (sequences[8] > 4f)
			{
				sequences[8] = 0f;
			}
		}
		if (!(sequences[9] > 0f))
		{
			return;
		}
		num = sequences[9];
		if (num != 1f)
		{
			if (num != 2f)
			{
				if (num != 3f)
				{
					if (num == 4f)
					{
						Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
					}
				}
				else
				{
					QueueHoldReleaseWindow(4, 5);
					Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: false, 0);
				}
			}
			else
			{
				Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
			}
		}
		else
		{
			Darkroom.env.PhotoPulley.QueuePhoto(newIsQueuedGood: true, 0);
		}
		if (isHalfBeatEnabled)
		{
			sequences[9] = sequences[9] + 0.5f;
		}
		else
		{
			sequences[9] += 1f;
		}
		if (sequences[9] > 4f)
		{
			sequences[9] = 0f;
		}
	}

	protected override void OnAction()
	{
		if (!Underworld.env.CheckIsActivated())
		{
			if (Espot.env.CheckIsActivated())
			{
				Espot.env.PlayGrabSfx();
				Espot.env.UfoMachine.Grab();
			}
			else if (NeoCity.env.CheckIsActivated())
			{
				NeoCity.env.ShootCenter();
			}
			else if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable())
			{
				Darkroom.env.McLighter.Light();
			}
		}
	}

	protected override void OnActionReleased()
	{
		if (Espot.env.CheckIsActivated())
		{
			Espot.env.PlayReleaseSfx();
			Espot.env.UfoMachine.Release();
		}
		else if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable())
		{
			Darkroom.env.McLighter.Release();
		}
	}

	protected override void OnActionLeft()
	{
		if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.ShootLeft();
		}
	}

	protected override void OnActionRight()
	{
		if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.ShootRight();
		}
	}

	protected override void OnPrep()
	{
		if (Espot.env.CheckIsActivated())
		{
			Espot.env.UfoMachine.Claw.React(accuracy);
			Espot.env.UfoMachine.PickUpCapsule();
			Espot.env.Feedbacks[0].SetLocalPosition(3.49f, -0.7f);
		}
		else if (Darkroom.env.CheckIsActivated())
		{
			Darkroom.env.CheckIsPlayable();
		}
	}

	protected override void OnHitWindow()
	{
		if (Underworld.env.CheckIsActivated())
		{
			if (hitType == 1)
			{
				Underworld.env.LavaPool.LinearRise(beatDelta, 3.4072f);
			}
			else if (hitType == 2)
			{
				Underworld.env.LavaPool.LinearRise(beatDelta, 3.4072f);
			}
			else
			{
				Underworld.env.LavaPool.LinearRise(beatDelta, 5.1108003f);
			}
			Underworld.env.LavaPool.LungeDelayed(timeBeatStarted);
		}
	}

	protected override void OnHit()
	{
		if (Underworld.env.CheckIsActivated())
		{
			Underworld.env.PlayJump(hitType);
			if (accuracy == 0.333f)
			{
				if (hitType == 1)
				{
					Underworld.env.McClimber.Move("burnLeft", -5.5f, 3.4072f);
				}
				else if (hitType == 2)
				{
					Underworld.env.McClimber.Move("burnRight", 5.5f, 3.4072f);
				}
				else
				{
					Underworld.env.McClimber.Move("burnUp", 0f, 5.1108003f);
				}
				Underworld.env.BurnMc();
			}
			else if (hitType == 1)
			{
				Underworld.env.McClimber.Move("jumpLeft", -5.5f, 3.4072f);
			}
			else if (hitType == 2)
			{
				Underworld.env.McClimber.Move("jumpRight", 5.5f, 3.4072f);
			}
			else
			{
				Underworld.env.McClimber.Move("jumpUp", 0f, 5.1108003f);
			}
			if (hitType == 1)
			{
				Underworld.env.Feedbacks[0].SetLocalPosition(2.7f, -2.67f);
			}
			else if (hitType == 2)
			{
				Underworld.env.Feedbacks[0].SetLocalPosition(-2.7f, -2.67f);
			}
			else
			{
				Underworld.env.Feedbacks[0].SetLocalPosition(0f, -0.42f);
			}
		}
		else if (Espot.env.CheckIsActivated())
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
		else if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.Hit(accuracy, hitType);
		}
		else if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable() && accuracy >= 1f)
		{
			Darkroom.env.PlayBurnFeedback();
			Darkroom.env.PhotoPulley.GetDissolvableMemoryPhoto().Dissolve();
		}
	}

	protected override void OnStrike()
	{
		if (Underworld.env.CheckIsActivated())
		{
			Underworld.env.Sweat.SetPosition(Underworld.env.McClimber.GetHeadPosition().x, Underworld.env.McClimber.GetHeadPosition().y);
			Underworld.env.Sweat.CrossIn();
		}
		else if (Espot.env.CheckIsActivated())
		{
			Espot.env.UfoMachine.Claw.React(0f);
			if (Espot.env.UfoMachine.CheckIsPickedUp())
			{
				Espot.env.UfoMachine.StickCapsule();
			}
			Espot.env.Sweat.CrossIn();
		}
		else if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.Strike();
		}
		else if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable())
		{
			Darkroom.env.McLighter.Sweat.CrossIn();
		}
	}

	protected override void OnMiss()
	{
		if (Underworld.env.CheckIsActivated())
		{
			if (hitType == 1)
			{
				Underworld.env.McClimber.Move("burnLeft", -5.5f, 3.4072f);
			}
			else if (hitType == 2)
			{
				Underworld.env.McClimber.Move("burnRight", 5.5f, 3.4072f);
			}
			else
			{
				Underworld.env.McClimber.Move("burnUp", 0f, 5.1108003f);
			}
			Underworld.env.BurnMc();
		}
		else if (Espot.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				Espot.env.Sweat.CrossIn();
			}
		}
		else if (NeoCity.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				NeoCity.env.Cockpit.Radar.FlashAccuracy(0f);
				NeoCity.env.Sweat.CrossIn();
			}
		}
		else if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable() && gameMode < 6)
		{
			Darkroom.env.McLighter.Sweat.CrossIn();
		}
	}
}
