using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream_pressure : Dream
{
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
		Gym.env.BobbleDelayed(0f, 1, isHitWindow: false);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		Gym.env.BobbleDelayed(0f, 2, isHitWindow: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		Gym.env.BobbleDelayed(0f, 3, isHitWindow: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		Gym.env.BobbleDelayed(0f, 4, isHitWindow: false);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		TriggerSong();
	}

	protected override void OnBobble()
	{
		if (OfficeSpace.env.CheckIsActivated())
		{
			OfficeSpace.env.BobbleDelayed(beatDelta, beat);
		}
		else if (LoveLand.env.CheckIsActivated())
		{
			if (bar % 2 == 1 && beat == 1)
			{
				LoveLand.env.BlowPetals1(beatDelta);
			}
			else if (bar % 2 == 0 && beat == 1)
			{
				LoveLand.env.BlowPetals2(beatDelta);
			}
		}
		else if (Gym.env.CheckIsActivated())
		{
			Gym.env.BobbleDelayed(beatDelta, beat, isHitWindow);
		}
		else if (TropicalBank.env.CheckIsActivated())
		{
			TropicalBank.env.BobbleDelayed(beatDelta, beat, isCameraMoving: false);
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
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				Gym.env.Hide();
				OfficeSpace.env.Show();
			}
			else if (bar == 4 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				LoveLand.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				TropicalBank.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.TransitionLoop();
			}
			else if (bar == 8 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
				LoveLand.env.Phone.DatingApp.Slide();
			}
			else if (bar == 8 && beat == 2)
			{
				LoveLand.env.Phone.DatingApp.Slide();
			}
			else if (bar == 8 && beat == 3)
			{
				LoveLand.env.Phone.DatingApp.Slide();
			}
			else if (bar == 8 && beat == 4)
			{
				LoveLand.env.Phone.DatingApp.Slide();
			}
		}
		else if (phrase == 2)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				LoveLand.env.Hide();
				Gym.env.Show(isTrainerFocused: true);
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				Gym.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 4 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
				LoveLand.env.SetActiveSpeakerNum(1);
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				LoveLand.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				TropicalBank.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 8 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
			}
		}
		else if (phrase == 3)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				LoveLand.env.Hide();
				Gym.env.Show(isTrainerFocused: true);
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
			else if (bar == 3 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 1);
			}
			else if (bar == 4 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				Gym.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: true);
			}
			else if (bar == 7 && beat == 1)
			{
				OfficeSpace.env.Loop(1f, isReverse: true);
			}
		}
		else if (phrase == 4)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				LoveLand.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 4 && beat == 1)
			{
				CancelAllSequences();
				TropicalBank.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				LoveLand.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				TropicalBank.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
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
				CancelAllSequences();
				LoveLand.env.Hide();
				Gym.env.Show(isTrainerFocused: true);
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				Gym.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 4 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				TropicalBank.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 6 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				TropicalBank.env.Hide();
				if (gameMode == 3)
				{
					LoveLand.env.Show(1);
				}
				else if (gameMode == 4)
				{
					LoveLand.env.Show(2);
				}
			}
		}
	}

	protected override void OnSequence()
	{
		if (OfficeSpace.env.CheckIsActivated())
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
		else if (LoveLand.env.CheckIsActivated())
		{
			if (sequences[0] > 0f)
			{
				if (gameMode < 6)
				{
					QueueLeftHitWindow(3);
					LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 1f);
					sequences[0] = 0f;
				}
				else
				{
					if (sequences[0] == 1f)
					{
						LoveLand.env.CancelAllSounds();
						LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 1f);
					}
					else if (sequences[0] == 3f)
					{
						QueueLeftHitWindow(1);
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
				if (gameMode < 6)
				{
					QueueLeftHitWindow(6);
					LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 0.5f);
					sequences[1] = 0f;
				}
				else
				{
					if (sequences[1] == 1f)
					{
						LoveLand.env.CancelAllSounds();
						LoveLand.env.CountdownLeftDelayed(timeBeatStarted, 0.5f);
					}
					else if (sequences[1] == 6f)
					{
						QueueLeftHitWindow(1);
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
			}
			if (sequences[2] > 0f)
			{
				if (gameMode < 6)
				{
					QueueRightHitWindow(3);
					LoveLand.env.CountdownRightDelayed(timeBeatStarted, 1f);
					sequences[2] = 0f;
				}
				else
				{
					if (sequences[2] == 1f)
					{
						LoveLand.env.CancelAllSounds();
						LoveLand.env.CountdownRightDelayed(timeBeatStarted, 1f);
					}
					else if (sequences[2] == 3f)
					{
						QueueRightHitWindow(1);
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
				QueueRightHitWindow(6);
				LoveLand.env.CountdownRightDelayed(timeBeatStarted, 0.5f);
				sequences[3] = 0f;
				return;
			}
			if (sequences[3] == 1f)
			{
				LoveLand.env.CancelAllSounds();
				LoveLand.env.CountdownRightDelayed(timeBeatStarted, 0.5f);
			}
			else if (sequences[3] == 6f)
			{
				QueueRightHitWindow(1);
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
		else if (Gym.env.CheckIsActivated())
		{
			if (sequences[0] > 0f)
			{
				QueueLeftHitWindow(4);
				Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 1);
				sequences[0] = 0f;
			}
			if (sequences[1] > 0f)
			{
				QueueRightHitWindow(4);
				Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 2);
				sequences[1] = 0f;
			}
			if (sequences[2] > 0f)
			{
				QueueLeftRightHitWindow(4);
				Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 3);
				sequences[2] = 0f;
			}
			if (sequences[3] > 0f)
			{
				rng = Random.Range(0, 2);
				if (rng == 0)
				{
					QueueLeftHitWindow(4);
					Gym.env.Trainer.LiftDelayed(timeBeatStarted, isFullBeat, 1);
				}
				else
				{
					QueueRightHitWindow(4);
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
		else
		{
			if (!TropicalBank.env.CheckIsActivated())
			{
				return;
			}
			if (sequences[0] > 0f)
			{
				QueueLeftHitWindow(1);
				TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: false);
				sequences[0] = 0f;
			}
			if (sequences[1] > 0f)
			{
				QueueRightHitWindow(1);
				TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: false);
				sequences[1] = 0f;
			}
			if (sequences[2] > 0f)
			{
				QueueLeftRightHitWindow(1);
				TropicalBank.env.MoneyCloud.SpawnBothMoneyDelayed(timeBeatStarted);
				sequences[2] = 0f;
			}
			if (sequences[3] > 0f)
			{
				QueueLeftHitWindow(1);
				TropicalBank.env.ThunderDelayed(timeBeatStarted, "left");
				TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: true);
				sequences[3] = 0f;
			}
			if (sequences[4] > 0f)
			{
				QueueRightHitWindow(1);
				TropicalBank.env.ThunderDelayed(timeBeatStarted, "right");
				TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: true);
				sequences[4] = 0f;
			}
			if (sequences[5] > 0f)
			{
				rng = Random.Range(0, 5);
				if (rng == 0)
				{
					QueueLeftHitWindow(1);
					TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: false);
				}
				else if (rng == 1)
				{
					QueueRightHitWindow(1);
					TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: false);
				}
				else if (rng == 2)
				{
					QueueLeftRightHitWindow(1);
					TropicalBank.env.MoneyCloud.SpawnBothMoneyDelayed(timeBeatStarted);
				}
				else if (rng == 3)
				{
					QueueLeftHitWindow(1);
					TropicalBank.env.ThunderDelayed(timeBeatStarted, "left");
					TropicalBank.env.MoneyCloud.SpawnLeftMoneyDelayed(timeBeatStarted, isDrift: true);
				}
				else
				{
					QueueRightHitWindow(1);
					TropicalBank.env.ThunderDelayed(timeBeatStarted, "right");
					TropicalBank.env.MoneyCloud.SpawnRightMoneyDelayed(timeBeatStarted, isDrift: true);
				}
				sequences[5] = 0f;
			}
		}
	}

	protected override void OnEvent()
	{
		if (!LoveLand.env.CheckIsActivated())
		{
			return;
		}
		if (eventNum == 0)
		{
			LoveLand.env.ZoomInDelayed(timeBeatStarted);
			LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, 0);
		}
		else if (eventNum == 1)
		{
			if (LoveLand.env.Phone.CheckIsSmall())
			{
				LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
				LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, 1);
			}
			else
			{
				LoveLand.env.ZoomOutDelayed(timeBeatStarted);
				LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, 1);
			}
		}
		else if (eventNum <= 5)
		{
			LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
			LoveLand.env.Phone.SetSmallRotationDelayed(timeBeatStarted, eventNum);
		}
		else if (eventNum == 6)
		{
			LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
			LoveLand.env.Phone.SmallRotateDelayed(timeBeatStarted, 1);
		}
		else if (eventNum == 7)
		{
			LoveLand.env.SetToZoomedOutDelayed(timeBeatStarted);
			LoveLand.env.Phone.SmallRotateDelayed(timeBeatStarted, 0);
		}
	}

	protected override void OnActionLeft()
	{
		if (OfficeSpace.env.CheckIsActivated())
		{
			OfficeSpace.env.Type(1);
		}
		else if (LoveLand.env.CheckIsActivated())
		{
			LoveLand.env.Phone.ThumbLeft();
			if (!isHitWindow)
			{
				LoveLand.env.Phone.DatingApp.GetDateCard().NudgeLeft();
			}
		}
		else if (Gym.env.CheckIsActivated())
		{
			Gym.env.McLifter.Lift(1);
		}
		else if (TropicalBank.env.CheckIsActivated())
		{
			if (isHitWindow && hitType == 1)
			{
				TropicalBank.env.McCatcher.Pocket(1);
			}
			else
			{
				TropicalBank.env.McCatcher.Grab(1);
			}
		}
	}

	protected override void OnActionLeftReleased()
	{
		if (Gym.env.CheckIsActivated())
		{
			Gym.env.McLifter.Unlift(1);
		}
	}

	protected override void OnActionRight()
	{
		if (OfficeSpace.env.CheckIsActivated())
		{
			OfficeSpace.env.Type(2);
		}
		else if (LoveLand.env.CheckIsActivated())
		{
			LoveLand.env.Phone.ThumbRight();
			if (!isHitWindow)
			{
				LoveLand.env.Phone.DatingApp.GetDateCard().NudgeRight();
			}
		}
		else if (Gym.env.CheckIsActivated())
		{
			Gym.env.McLifter.Lift(2);
		}
		else if (TropicalBank.env.CheckIsActivated())
		{
			if (isHitWindow && hitType == 2)
			{
				TropicalBank.env.McCatcher.Pocket(2);
			}
			else
			{
				TropicalBank.env.McCatcher.Grab(2);
			}
		}
	}

	protected override void OnActionRightReleased()
	{
		if (Gym.env.CheckIsActivated())
		{
			Gym.env.McLifter.Unlift(2);
		}
	}

	protected override void OnHitWindow()
	{
		if (LoveLand.env.CheckIsActivated())
		{
			LoveLand.env.Phone.DatingApp.ToggleIsSliding(toggle: true);
		}
	}

	protected override void OnHit()
	{
		if (OfficeSpace.env.CheckIsActivated())
		{
			OfficeSpace.env.SendFeedback(hitType);
			McWorker[] mcWorkers = OfficeSpace.env.McWorkers;
			for (int i = 0; i < mcWorkers.Length; i++)
			{
				mcWorkers[i].HitActiveWorkMessage(accuracy);
			}
		}
		else if (LoveLand.env.CheckIsActivated())
		{
			if (hitType == 1)
			{
				if (LoveLand.env.Phone.DatingApp.DateCards[0].CheckIsLastCue())
				{
					LoveLand.env.Phone.DatingApp.DateCards[0].Deactivate("swipeLeft", accuracy);
				}
				else if (LoveLand.env.Phone.DatingApp.DateCards[1].CheckIsLastCue())
				{
					LoveLand.env.Phone.DatingApp.DateCards[1].Deactivate("swipeLeft", accuracy);
				}
			}
			else if (hitType == 2)
			{
				if (LoveLand.env.Phone.DatingApp.DateCards[0].CheckIsLastCue())
				{
					LoveLand.env.Phone.DatingApp.DateCards[0].Deactivate("swipeRight", accuracy);
				}
				else if (LoveLand.env.Phone.DatingApp.DateCards[1].CheckIsLastCue())
				{
					LoveLand.env.Phone.DatingApp.DateCards[1].Deactivate("swipeRight", accuracy);
				}
			}
			LoveLand.env.Phone.DatingApp.RotateOut();
			LoveLand.env.React(hitType);
		}
		else if (!Gym.env.CheckIsActivated() && TropicalBank.env.CheckIsActivated())
		{
			TropicalBank.env.MoneyCloud.PlayFeedback();
			if (hitType == 1)
			{
				TropicalBank.env.MoneyCloud.CatchActiveMoney();
			}
			else if (hitType == 2)
			{
				TropicalBank.env.MoneyCloud.CatchActiveMoney();
			}
			else if (hitType == 3)
			{
				TropicalBank.env.McCatcher.Pocket(1);
				TropicalBank.env.McCatcher.Pocket(2);
				TropicalBank.env.MoneyCloud.CatchActiveMoney();
				TropicalBank.env.MoneyCloud.CatchActiveMoney();
			}
		}
	}

	protected override void OnStrike()
	{
		if (OfficeSpace.env.CheckIsActivated())
		{
			OfficeSpace.env.SendMistake();
			McWorker[] mcWorkers = OfficeSpace.env.McWorkers;
			for (int i = 0; i < mcWorkers.Length; i++)
			{
				mcWorkers[i].StrikeActiveWorkMessage();
			}
		}
		else if (LoveLand.env.CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsActionLeftPressed())
			{
				LoveLand.env.Sweat.SetLocalX(-7.1f);
			}
			else if (ControlHandler.mgr.CheckIsActionRightPressed())
			{
				LoveLand.env.Sweat.SetLocalX(7.1f);
			}
			LoveLand.env.Sweat.CrossIn();
		}
		else if (Gym.env.CheckIsActivated())
		{
			Gym.env.McLifter.Sweat.CrossIn();
		}
		else if (TropicalBank.env.CheckIsActivated())
		{
			TropicalBank.env.McCatcher.Sweat.CrossIn();
		}
	}

	protected override void OnMiss()
	{
		if (OfficeSpace.env.CheckIsActivated())
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
		else if (LoveLand.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				LoveLand.env.Sweat.CrossIn();
			}
			LoveLand.env.Phone.DatingApp.RotateOut();
			if (LoveLand.env.Phone.DatingApp.DateCards[0].CheckIsLastCue())
			{
				LoveLand.env.Phone.DatingApp.DateCards[0].Deactivate("swipeDown", 0f);
			}
			else if (LoveLand.env.Phone.DatingApp.DateCards[1].CheckIsLastCue())
			{
				LoveLand.env.Phone.DatingApp.DateCards[1].Deactivate("swipeDown", 0f);
			}
		}
		else if (Gym.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				Gym.env.McLifter.Sweat.CrossIn();
			}
		}
		else if (TropicalBank.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				TropicalBank.env.McCatcher.Sweat.CrossIn();
			}
			if (hitType == 1)
			{
				TropicalBank.env.MoneyCloud.MissActiveMoney();
			}
			else if (hitType == 2)
			{
				TropicalBank.env.MoneyCloud.MissActiveMoney();
			}
			else if (hitType == 3)
			{
				TropicalBank.env.MoneyCloud.MissActiveMoney();
				TropicalBank.env.MoneyCloud.MissActiveMoney();
			}
		}
	}
}
