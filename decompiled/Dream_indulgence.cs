using System.Collections;
using UnityEngine;

public class Dream_indulgence : Dream
{
	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		FoodySkies.env.Show();
		FoodySkies.env.GetActivePizzaBox().Hide();
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().Activate(2);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		float timeStarted = Technician.mgr.GetDspTime();
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		FoodySkies.env.McChomper.BobbleDelayed(0f);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 4f);
		TriggerSong();
	}

	protected override void OnBobble()
	{
		if (FoodySkies.env.CheckIsActivated())
		{
			if (FoodySkies.env.GetActivePizzaBox().CheckIsActivated())
			{
				FoodySkies.env.GetActivePizzaBox().BobbleDelayed(beatDelta);
			}
			FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
		}
		else if (InfluencerLand.env.CheckIsActivated())
		{
			if (!isHitWindow && InfluencerLand.env.McJumper.CheckIsAwaiting())
			{
				InfluencerLand.env.Bobble();
				InfluencerLand.env.BobbleDelayed(beatDelta);
			}
			if (!InfluencerLand.env.McJumper.CheckIsEscaped())
			{
				InfluencerLand.env.RepositionNotification();
			}
		}
		else if (Mall.env.CheckIsActivated())
		{
			if (!isHitWindow && (beat == 1 || beat == 3))
			{
				Mall.env.Counter.McSpender.BobbleDelayed(beatDelta);
			}
			Mall.env.Counter.CardMachine.Bobble();
		}
		else if (MechSpace.env.CheckIsActivated())
		{
			if (!MechSpace.env.CheckIsStoppedAiming() && beat == 1)
			{
				MechSpace.env.Crosshair.AimDelayed(beatDelta);
				MechSpace.env.McVirtual.AimDelayed(beatDelta);
			}
			MechSpace.env.BobbleDelayed(beatDelta, isHitWindow);
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
				CancelAllSequences();
				FoodySkies.env.Hide();
				InfluencerLand.env.Show(100, "k");
			}
			else if (bar == 3 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
			else if (bar == 4 && beat == 1)
			{
				CancelAllSequences();
				InfluencerLand.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				FoodySkies.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				MechSpace.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
		}
		else if (phrase == 2)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				Mall.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 2 && beat == 1)
			{
				CancelAllSequences();
				FoodySkies.env.Hide();
				InfluencerLand.env.Show(500, "k");
			}
			else if (bar == 3 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
			else if (bar == 4 && beat == 1)
			{
				CancelAllSequences();
				InfluencerLand.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				FoodySkies.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				MechSpace.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
		}
		else if (phrase == 3)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				Mall.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				FoodySkies.env.Hide();
				InfluencerLand.env.Show(5, "m");
			}
			else if (bar == 4 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				InfluencerLand.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				FoodySkies.env.Hide();
				InfluencerLand.env.Show(100, "m");
			}
			else if (bar == 8 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
		}
		else if (phrase == 4)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				InfluencerLand.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				MechSpace.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
		}
		else if (phrase == 5)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				Mall.env.Hide();
				InfluencerLand.env.Show(500, "b");
			}
			else if (bar == 2 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				InfluencerLand.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				MechSpace.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				Mall.env.Hide();
				FoodySkies.env.Show();
			}
		}
	}

	protected override void OnSequence()
	{
		if (FoodySkies.env.CheckIsActivated())
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
		else if (InfluencerLand.env.CheckIsActivated())
		{
			if (sequences[0] > 0f)
			{
				QueueHitWindow(1);
				InfluencerLand.env.Launch0Delayed(timeBeatStarted);
				sequences[0] = 0f;
			}
			if (sequences[1] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHitWindow(2);
					InfluencerLand.env.Launch1Delayed(timeBeatStarted);
					InfluencerLand.env.Launch1SoundDelayed(timeBeatStarted, beat);
					sequences[1] = 0f;
				}
				else
				{
					if (sequences[1] == 1f)
					{
						InfluencerLand.env.McJumper.CancelMovement();
						InfluencerLand.env.Launch1Delayed(timeBeatStarted);
						InfluencerLand.env.Launch1SoundDelayed(timeBeatStarted, beat);
					}
					else if (sequences[1] == 2f)
					{
						QueueHitWindow(1);
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
			if (sequences[2] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHitWindow(3);
					InfluencerLand.env.Launch2Delayed(timeBeatStarted);
					InfluencerLand.env.Launch2SoundDelayed(timeBeatStarted, beat);
					sequences[2] = 0f;
				}
				else
				{
					if (sequences[2] == 1f)
					{
						InfluencerLand.env.McJumper.CancelMovement();
						InfluencerLand.env.Launch2Delayed(timeBeatStarted);
						InfluencerLand.env.Launch2SoundDelayed(timeBeatStarted, beat);
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
			if (sequences[3] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHitWindow(4);
					InfluencerLand.env.Launch3Delayed(timeBeatStarted);
					InfluencerLand.env.Launch3SoundDelayed(timeBeatStarted, beat);
					sequences[3] = 0f;
				}
				else
				{
					if (sequences[3] == 1f)
					{
						InfluencerLand.env.McJumper.CancelMovement();
						InfluencerLand.env.Launch3Delayed(timeBeatStarted);
						InfluencerLand.env.Launch3SoundDelayed(timeBeatStarted, beat);
					}
					else if (sequences[3] == 4f)
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
					if (sequences[3] > 4f)
					{
						sequences[3] = 0f;
					}
				}
			}
			if (sequences[4] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHitWindow(1);
					QueueHitWindow(1, isHalfBeatAdded: true);
					QueueHitWindow(2);
					InfluencerLand.env.LaunchIntroTripleDelayed(timeBeatStarted);
					InfluencerLand.env.LaunchIntroTripleSoundDelayed(timeBeatStarted);
					sequences[4] = 0f;
				}
				else
				{
					if (sequences[4] == 1f)
					{
						QueueHitWindow(1);
						QueueHitWindow(1, isHalfBeatAdded: true);
						InfluencerLand.env.LaunchIntroTripleDelayed(timeBeatStarted);
						InfluencerLand.env.LaunchIntroTripleSoundDelayed(timeBeatStarted);
					}
					else if (sequences[4] == 2f)
					{
						QueueHitWindow(1);
					}
					if (isHalfBeatEnabled)
					{
						sequences[4] = sequences[4] + 0.5f;
					}
					else
					{
						sequences[4] += 1f;
					}
					if (sequences[4] > 2f)
					{
						sequences[4] = 0f;
					}
				}
			}
			if (sequences[5] > 0f)
			{
				QueueHitWindow(4);
				InfluencerLand.env.Launch5Delayed(timeBeatStarted);
				sequences[5] = 0f;
			}
		}
		else if (Mall.env.CheckIsActivated())
		{
			if (sequences[0] > 0f)
			{
				if (!Mall.env.StoreDisplay.CheckIsDropping())
				{
					if (Interface.env.Circle.CheckIsActivated())
					{
						Interface.env.Circle.ResetRingNum();
					}
					if (gameMode != 2 && gameMode != 4 && gameMode < 6)
					{
						if (Mall.env.StoreDisplay.CheckIsSlid())
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
						}
						else
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
						}
					}
					Mall.env.StoreDisplay.Clear();
					Mall.env.StoreDisplay.Drop(timeBeatStarted);
				}
				if (isFullBeat)
				{
					if (Mall.env.StoreDisplay.CheckIsDropping2())
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate();
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
					else
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate();
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
				}
				else if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate();
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate();
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				if (Mall.env.StoreDisplay.CheckIsSlid())
				{
					QueueHitWindow(8);
				}
				else
				{
					QueueHitWindow(4);
				}
				sequences[0] = 0f;
			}
			if (sequences[1] > 0f)
			{
				if (!Mall.env.StoreDisplay.CheckIsDropping())
				{
					if (Interface.env.Circle.CheckIsActivated())
					{
						Interface.env.Circle.ResetRingNum();
					}
					if (gameMode != 2 && gameMode != 4 && gameMode < 6)
					{
						if (Mall.env.StoreDisplay.CheckIsSlid())
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
						}
						else
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
						}
					}
					Mall.env.StoreDisplay.Clear();
					Mall.env.StoreDisplay.Drop(timeBeatStarted);
				}
				if (isFullBeat)
				{
					if (Mall.env.StoreDisplay.CheckIsDropping2())
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(1);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
					else
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(1);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
				}
				else if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(1);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(1);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				if (Mall.env.StoreDisplay.CheckIsSlid())
				{
					QueueHitWindow(8);
				}
				else
				{
					QueueHitWindow(4);
				}
				sequences[1] = 0f;
			}
			if (sequences[2] > 0f)
			{
				if (!Mall.env.StoreDisplay.CheckIsDropping())
				{
					if (Interface.env.Circle.CheckIsActivated())
					{
						Interface.env.Circle.ResetRingNum();
					}
					if (gameMode != 2 && gameMode != 4 && gameMode < 6)
					{
						if (Mall.env.StoreDisplay.CheckIsSlid())
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
						}
						else
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
						}
					}
					Mall.env.StoreDisplay.Clear();
					Mall.env.StoreDisplay.Drop(timeBeatStarted);
				}
				if (isFullBeat)
				{
					if (Mall.env.StoreDisplay.CheckIsDropping2())
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(2);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
					else
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(2);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
				}
				else if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(2);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(2);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				if (Mall.env.StoreDisplay.CheckIsSlid())
				{
					QueueHitWindow(8);
				}
				else
				{
					QueueHitWindow(4);
				}
				sequences[2] = 0f;
			}
			if (sequences[3] > 0f)
			{
				if (!Mall.env.StoreDisplay.CheckIsDropping())
				{
					if (Interface.env.Circle.CheckIsActivated())
					{
						Interface.env.Circle.ResetRingNum();
					}
					if (gameMode != 2 && gameMode != 4 && gameMode < 6)
					{
						if (Mall.env.StoreDisplay.CheckIsSlid())
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
						}
						else
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
						}
					}
					Mall.env.StoreDisplay.Clear();
					Mall.env.StoreDisplay.Drop(timeBeatStarted);
				}
				if (isFullBeat)
				{
					if (Mall.env.StoreDisplay.CheckIsDropping2())
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(3);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
					else
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(3);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
				}
				else if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(3);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(3);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				if (Mall.env.StoreDisplay.CheckIsSlid())
				{
					QueueHitWindow(8);
				}
				else
				{
					QueueHitWindow(4);
				}
				sequences[3] = 0f;
			}
			if (sequences[4] > 0f)
			{
				if (!Mall.env.StoreDisplay.CheckIsDropping())
				{
					if (Interface.env.Circle.CheckIsActivated())
					{
						Interface.env.Circle.ResetRingNum();
					}
					if (gameMode != 2 && gameMode != 4 && gameMode < 6)
					{
						if (Mall.env.StoreDisplay.CheckIsSlid())
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
						}
						else
						{
							Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
						}
					}
					Mall.env.StoreDisplay.Clear();
					Mall.env.StoreDisplay.Drop(timeBeatStarted);
				}
				if (isFullBeat)
				{
					if (Mall.env.StoreDisplay.CheckIsDropping2())
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(4);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
					else
					{
						Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(4);
						Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
						Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
					}
				}
				else if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(4);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(4);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				if (Mall.env.StoreDisplay.CheckIsSlid())
				{
					QueueHitWindow(8);
				}
				else
				{
					QueueHitWindow(4);
				}
				sequences[4] = 0f;
			}
			if (!(sequences[5] > 0f))
			{
				return;
			}
			if (!Mall.env.StoreDisplay.CheckIsDropping())
			{
				if (Interface.env.Circle.CheckIsActivated())
				{
					Interface.env.Circle.ResetRingNum();
				}
				if (gameMode != 2 && gameMode != 4 && gameMode < 6)
				{
					if (Mall.env.StoreDisplay.CheckIsSlid())
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 8);
					}
					else
					{
						Mall.env.Counter.CardMachine.IncreaseMeter(timeBeatStarted, 4);
					}
				}
				Mall.env.StoreDisplay.Clear();
				Mall.env.StoreDisplay.Drop(timeBeatStarted);
			}
			if (isFullBeat)
			{
				if (Mall.env.StoreDisplay.CheckIsDropping2())
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 8].Activate(5);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
				else
				{
					Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2].Activate(5);
					Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
					Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
				}
			}
			else if (Mall.env.StoreDisplay.CheckIsDropping2())
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 9].Activate(5);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			else
			{
				Mall.env.StoreDisplay.LuxuryItems[(beat - 1) * 2 + 1].Activate(5);
				Mall.env.StoreDisplay.IncreaseLuxuryItemNum();
				Mall.env.StoreDisplay.PresentSoundDelayed(timeBeatStarted);
			}
			if (Mall.env.StoreDisplay.CheckIsSlid())
			{
				QueueHitWindow(8);
			}
			else
			{
				QueueHitWindow(4);
			}
			sequences[5] = 0f;
		}
		else
		{
			if (!MechSpace.env.CheckIsActivated())
			{
				return;
			}
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
	}

	protected override void OnEvent()
	{
		if (FoodySkies.env.CheckIsActivated())
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
		else if (MechSpace.env.CheckIsActivated())
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
	}

	protected override void OnAction()
	{
		if (!FoodySkies.env.CheckIsActivated() && !InfluencerLand.env.CheckIsActivated())
		{
			if (Mall.env.CheckIsActivated())
			{
				Mall.env.PlayFeedback();
				Mall.env.Counter.McSpender.Purchase();
			}
			else if (MechSpace.env.CheckIsActivated())
			{
				MechSpace.env.ShootSound();
				MechSpace.env.McVirtual.Shoot();
				MechSpace.env.Crosshair.Shoot();
			}
		}
	}

	protected override void OnHitWindow()
	{
		if (MechSpace.env.CheckIsActivated())
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
		else if (InfluencerLand.env.CheckIsActivated())
		{
			InfluencerLand.env.SetActivePlatformNum(InfluencerLand.env.McJumper.GetPositionNum());
		}
	}

	protected override void OnHit()
	{
		if (FoodySkies.env.CheckIsActivated())
		{
			if (accuracy >= 1f)
			{
				FoodySkies.env.Hit(timeBeatStarted);
				return;
			}
			FoodySkies.env.PlayMissFeedbackSfx();
			FoodySkies.env.McChomper.Strike();
		}
		else if (InfluencerLand.env.CheckIsActivated())
		{
			InfluencerLand.env.McJumper.Jump();
			InfluencerLand.env.McJumper.Ping(bar, beat);
			InfluencerLand.env.Platforms[InfluencerLand.env.GetActivePlatformNum()].LightUp(isFeedbackShown: true, accuracy);
		}
		else if (Mall.env.CheckIsActivated())
		{
			Mall.env.PlayGoodFeedback();
			Mall.env.Counter.ReactGood();
			Mall.env.StoreDisplay.GetActiveItem().BagUp();
			Mall.env.StoreDisplay.GetActiveItem().AttractFeedback();
		}
		else
		{
			if (!MechSpace.env.CheckIsActivated())
			{
				return;
			}
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
	}

	protected override void OnStrike()
	{
		if (FoodySkies.env.CheckIsActivated())
		{
			FoodySkies.env.McChomper.Strike();
			FoodySkies.env.McChomper.Sweat.CrossIn();
		}
		else if (InfluencerLand.env.CheckIsActivated())
		{
			InfluencerLand.env.McJumper.Sweat.CrossIn();
		}
		else if (Mall.env.CheckIsActivated())
		{
			Mall.env.PlayBadFeedback();
			Mall.env.Counter.ReactBad();
			Mall.env.Counter.CardMachine.ResetMeter();
			Mall.env.Sweat.Hide();
			Mall.env.Sweat.SetPosition(Mall.env.Counter.CardMachine.GetX() - 2f, Mall.env.Counter.CardMachine.GetY() + 0.67f);
			Mall.env.Sweat.CrossIn();
		}
		else if (MechSpace.env.CheckIsActivated())
		{
			MechSpace.env.McVirtual.Sweat.CrossIn();
			Interface.env.Cam.Breeze();
		}
	}

	protected override void OnMiss()
	{
		if (FoodySkies.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				FoodySkies.env.McChomper.Sweat.CrossIn();
			}
		}
		else if (InfluencerLand.env.CheckIsActivated())
		{
			InfluencerLand.env.McJumper.Stumble();
			if (gameMode < 6)
			{
				InfluencerLand.env.McJumper.Sweat.CrossIn();
			}
			InfluencerLand.env.Platforms[InfluencerLand.env.GetActivePlatformNum()].LightUp(isFeedbackShown: false, accuracy);
		}
		else if (Mall.env.CheckIsActivated())
		{
			Mall.env.PlayBadFeedback();
			Mall.env.Counter.ReactBad();
			Mall.env.Counter.CardMachine.ResetMeter();
			Mall.env.StoreDisplay.GetActiveItem().AttractFeedback();
			if (gameMode < 6)
			{
				Mall.env.Sweat.CrossIn();
			}
		}
		else if (MechSpace.env.CheckIsActivated() && gameMode < 6)
		{
			MechSpace.env.McVirtual.Sweat.CrossIn();
		}
	}

	protected override void OnResults()
	{
		FoodySkies.env.ZoomEnd();
	}
}
