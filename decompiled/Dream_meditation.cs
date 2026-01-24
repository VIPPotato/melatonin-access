using System.Collections;
using UnityEngine;

public class Dream_meditation : Dream
{
	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Matrix.env.Show();
		Matrix.env.LilBlocks.Wave();
		Matrix.env.Bobble(1f);
		Matrix.env.ParallaxIn(timeBeatStarted, newIsZoomedFarOut: false);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Matrix.env.Bobble(1f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		Matrix.env.Bobble(1f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		Matrix.env.Bobble(1f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		Matrix.env.ParallaxOut(newIsZoomedFarOut: false);
		TriggerSong();
	}

	protected override void OnBobble()
	{
		if (Matrix.env.CheckIsActivated())
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
			if (beat == 1)
			{
				Matrix.env.LilBlocks.Wave();
			}
		}
		else if (AngrySkies.env.CheckIsActivated())
		{
			AngrySkies.env.McMarchers.MarchDelayed(beatDelta);
			if (beat == 1 && AngrySkies.env.NasaTv.CheckIsActivated())
			{
				AngrySkies.env.NasaTv.Hover();
				AngrySkies.env.NasaTv.Tick(beatDelta);
			}
			else if (beat == 2)
			{
				AngrySkies.env.SparkleStarDelayed(beatDelta);
			}
			else if (beat == 3 && AngrySkies.env.NasaTv.CheckIsActivated())
			{
				AngrySkies.env.NasaTv.Tick(beatDelta);
			}
			else if (beat == 4)
			{
				AngrySkies.env.SparkleStarDelayed(beatDelta);
			}
			if (bar % 2 == 0 && beat == 3)
			{
				AngrySkies.env.ShootStarDelayed(beatDelta);
			}
		}
		else if (HypnoLair.env.CheckIsActivated())
		{
			HypnoLair.env.Bobble(beatDelta, beat);
		}
		else if (Conservatory.env.CheckIsActivated() && beat == 1)
		{
			Conservatory.env.WaterCan.Hover();
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
			if (bar == 2 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				HypnoLair.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(0, 0.8f);
				}
				ctrlMode = 1;
				CancelAllSequences();
				Conservatory.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 6 && beat == 1)
			{
				DreamWorld.env.Await();
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 7 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(1, 0.5f);
				}
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				AngrySkies.env.Show();
			}
		}
		else if (phrase == 2)
		{
			if (bar == 1 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(0, 0.8f);
				}
				CancelAllSequences();
				AngrySkies.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				DreamWorld.env.Await();
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				HypnoLair.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(0, 0.8f);
				}
				ctrlMode = 1;
				CancelAllSequences();
				Conservatory.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 6 && beat == 1)
			{
				DreamWorld.env.Await();
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 7 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(1, 0.5f);
				}
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				AngrySkies.env.Show();
			}
		}
		else if (phrase == 3)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				AngrySkies.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Conservatory.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				HypnoLair.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Conservatory.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
		}
		else if (phrase == 4)
		{
			if (bar == 1 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(0, 0.8f);
				}
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			if (bar == 2 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(1, 0.5f);
				}
				CancelAllSequences();
				Matrix.env.Hide();
				AngrySkies.env.Show();
			}
			else if (bar == 4 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(0, 0.5f);
				}
				CancelAllSequences();
				AngrySkies.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 6 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(1, 0.5f);
				}
				CancelAllSequences();
				Matrix.env.Hide();
				AngrySkies.env.Show();
			}
			else if (bar == 8 && beat == 1)
			{
				CancelAllSequences();
				AngrySkies.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
		}
		else
		{
			if (phrase != 5)
			{
				return;
			}
			if (bar == 1 && beat == 1)
			{
				DreamWorld.env.Await();
				CancelAllSequences();
				Matrix.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Conservatory.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 5 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(0, 0.5f);
				}
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				if (gameMode == 3)
				{
					DreamWorld.env.ZoomBobble(1, 0.5f);
				}
				CancelAllSequences();
				Matrix.env.Hide();
				AngrySkies.env.Show();
			}
		}
	}

	protected override void OnSequence()
	{
		if (Matrix.env.CheckIsActivated())
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
		else if (AngrySkies.env.CheckIsActivated())
		{
			if (sequences[0] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHoldReleaseWindow(4, 7);
					AngrySkies.env.Countdown0Delayed(timeBeatStarted);
					AngrySkies.env.Countdown0SoundDelayed(timeBeatStarted);
					sequences[0] = 0f;
				}
				else
				{
					if (sequences[0] == 1f)
					{
						AngrySkies.env.Reset();
						AngrySkies.env.Countdown0Delayed(timeBeatStarted);
						AngrySkies.env.Countdown0SoundDelayed(timeBeatStarted);
					}
					else if (sequences[0] == 4f)
					{
						QueueHoldReleaseWindow(1, 4);
					}
					if (isHalfBeatEnabled)
					{
						sequences[0] = sequences[0] + 0.5f;
					}
					else
					{
						sequences[0] += 1f;
					}
					if (sequences[0] > 4f)
					{
						sequences[0] = 0f;
					}
				}
			}
			if (sequences[1] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHoldReleaseWindow(2, 3);
					AngrySkies.env.Countdown1Delayed(timeBeatStarted);
					AngrySkies.env.Countdown1SoundDelayed(timeBeatStarted);
					sequences[1] = 0f;
				}
				else
				{
					if (sequences[1] == 1f)
					{
						AngrySkies.env.Reset();
						AngrySkies.env.Countdown1Delayed(timeBeatStarted);
						AngrySkies.env.Countdown1SoundDelayed(timeBeatStarted);
					}
					else if (sequences[1] == 2f)
					{
						QueueHoldReleaseWindow(1, 2);
					}
					if (isHalfBeatEnabled)
					{
						sequences[1] = sequences[1] + 0.5f;
					}
					else
					{
						sequences[1] += 1f;
					}
					if (sequences[1] > 2f)
					{
						sequences[1] = 0f;
					}
				}
			}
			if (!(sequences[2] > 0f))
			{
				return;
			}
			if (gameMode < 6)
			{
				QueueHoldReleaseWindow(8, 14);
				AngrySkies.env.Countdown2Delayed(timeBeatStarted);
				AngrySkies.env.Countdown2SoundDelayed(timeBeatStarted);
				sequences[2] = 0f;
				return;
			}
			if (sequences[2] == 1f)
			{
				AngrySkies.env.Reset();
				AngrySkies.env.Countdown2Delayed(timeBeatStarted);
				AngrySkies.env.Countdown2SoundDelayed(timeBeatStarted);
			}
			else if (sequences[2] == 8f)
			{
				QueueHoldReleaseWindow(1, 7);
			}
			if (isHalfBeatEnabled)
			{
				sequences[2] = sequences[2] + 0.5f;
			}
			else
			{
				sequences[2] += 1f;
			}
			if (sequences[2] > 8f)
			{
				sequences[2] = 0f;
			}
		}
		else if (HypnoLair.env.CheckIsActivated())
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
		else if (Conservatory.env.CheckIsActivated())
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
	}

	protected override void OnEvent()
	{
		if (Matrix.env.CheckIsActivated())
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
		else if (AngrySkies.env.CheckIsActivated())
		{
			if (eventNum == 0)
			{
				AngrySkies.env.Unblind(4f);
			}
			else if (eventNum == 1)
			{
				AngrySkies.env.Blind(4f);
			}
			else if (eventNum == 2)
			{
				AngrySkies.env.Unblind(16f);
			}
			else if (eventNum == 3)
			{
				AngrySkies.env.Blind(16f);
			}
			else if (eventNum == 4)
			{
				AngrySkies.env.Unblind(2f);
			}
			else if (eventNum == 5)
			{
				AngrySkies.env.Blind(2f);
			}
			else if (eventNum == 6)
			{
				AngrySkies.env.TriggerBlinded();
			}
		}
		else if (HypnoLair.env.CheckIsActivated())
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
	}

	protected override void OnAction()
	{
		if (Matrix.env.CheckIsActivated())
		{
			Matrix.env.Press();
		}
		else
		{
			if (AngrySkies.env.CheckIsActivated())
			{
				return;
			}
			if (HypnoLair.env.CheckIsActivated())
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
			else if (Conservatory.env.CheckIsActivated())
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
		}
	}

	protected override void OnActionReleased()
	{
		if (!Matrix.env.CheckIsActivated() && !AngrySkies.env.CheckIsActivated() && !HypnoLair.env.CheckIsActivated() && Conservatory.env.CheckIsActivated())
		{
			Conservatory.env.IdleWater();
			if (isPrepped)
			{
				Conservatory.env.Garden.GetActiveCrop().SoakCancel();
			}
		}
	}

	protected override void OnPrep()
	{
		if (!Matrix.env.CheckIsActivated())
		{
			if (AngrySkies.env.CheckIsActivated())
			{
				AngrySkies.env.PlayDrain();
				AngrySkies.env.ToggleIsPrepped(toggle: true);
				AngrySkies.env.ToggleLowerMute(toggle: false);
				AngrySkies.env.NasaTv.Flash();
				AngrySkies.env.NasaTv.SpaceMeters[0].SetColor(accuracy);
				AngrySkies.env.NasaTv.SpaceMeters[0].ToggleIsVisible(toggle: true);
				AngrySkies.env.NasaTv.SpaceMeters[1].ToggleIsVisible(toggle: false);
				AngrySkies.env.Shuttle.Prep();
			}
			else if (!HypnoLair.env.CheckIsActivated() && Conservatory.env.CheckIsActivated())
			{
				Conservatory.env.Garden.GetActiveCrop().Soak();
			}
		}
	}

	protected override void OnHit()
	{
		if (Matrix.env.CheckIsActivated())
		{
			Matrix.env.Hit();
			Matrix.env.Release();
			Interface.env.Cam.Shake();
		}
		else if (AngrySkies.env.CheckIsActivated())
		{
			AngrySkies.env.Reset();
			AngrySkies.env.PlayLaunch();
			AngrySkies.env.ToggleLowerMute(toggle: true);
			AngrySkies.env.McMarchers.ToggleIsMarching(toggle: true);
			AngrySkies.env.Shuttle.Lift(accuracy);
			AngrySkies.env.NasaTv.TriggerFeedback(accuracy);
			Interface.env.Cam.Shake(1.15f);
		}
		else if (HypnoLair.env.CheckIsActivated())
		{
			HypnoLair.env.Pulse(accuracy);
		}
		else
		{
			if (!Conservatory.env.CheckIsActivated())
			{
				return;
			}
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
	}

	protected override void OnStrike()
	{
		if (Matrix.env.CheckIsActivated())
		{
			Matrix.env.Strike();
			Matrix.env.ResetMcSwingers();
		}
		else if (AngrySkies.env.CheckIsActivated())
		{
			AngrySkies.env.Sweat.CrossIn();
			if (isPrepped)
			{
				AngrySkies.env.Reset();
				AngrySkies.env.Shuttle.Unprep();
			}
		}
		else if (HypnoLair.env.CheckIsActivated())
		{
			HypnoLair.env.Pulse(0f);
			HypnoLair.env.Sweat.CrossIn();
		}
		else if (Conservatory.env.CheckIsActivated())
		{
			Conservatory.env.Sweat.CrossIn();
			if (isPrepped && Conservatory.env.Garden.GetActiveCrop().PlantBubble.CheckIsInteractive())
			{
				Conservatory.env.Garden.GetActiveCrop().PlantBubble.WaterLevel.PauseLinearRise(0f);
			}
		}
	}

	protected override void OnMiss()
	{
		if (Matrix.env.CheckIsActivated())
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
		else if (AngrySkies.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				AngrySkies.env.Sweat.CrossIn();
			}
			AngrySkies.env.Reset();
			AngrySkies.env.McMarchers.ToggleIsMarching(toggle: true);
			AngrySkies.env.ToggleLowerMute(toggle: true);
			if (isPrepped)
			{
				AngrySkies.env.Shuttle.Unprep();
			}
		}
		else if (HypnoLair.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				HypnoLair.env.Sweat.CrossIn();
			}
		}
		else if (Conservatory.env.CheckIsActivated())
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
	}
}
