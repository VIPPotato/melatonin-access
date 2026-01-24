using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream_final : Dream
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
		FoodySkies.env.Show();
		FoodySkies.env.GetActivePizzaBox().Hide();
		float timeStarted = Technician.mgr.GetDspTime();
		FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
		FoodySkies.env.GetActivePizzaBox().Activate(2);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(0f);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 1f);
		FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(beatDelta);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 2f);
		FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(beatDelta);
		yield return new WaitUntil(() => Technician.mgr.GetDspTime() - timeStarted > MusicBox.env.GetSecsPerBeat() * 3f);
		FoodySkies.env.McChomper.BobbleDelayed(beatDelta);
		FoodySkies.env.GetActivePizzaBox().BobbleDelayed(beatDelta);
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
		else if (OfficeSpace.env.CheckIsActivated())
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
		else if (Matrix.env.CheckIsActivated())
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
				if (gameMode < 6 && !Matrix.env.CheckIsZoomedFarOut())
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
		else if (Conservatory.env.CheckIsActivated())
		{
			if (beat == 1)
			{
				Conservatory.env.WaterCan.Hover();
			}
		}
		else if (Underworld.env.CheckIsActivated())
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

	protected override void OnBeat()
	{
		if (gameMode >= 6)
		{
			return;
		}
		if (phrase == 1)
		{
			if (bar == 1 && beat == 3)
			{
				FoodySkies.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				Mall.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 2 && beat == 3)
			{
				MechSpace.env.Hide();
				InfluencerLand.env.Show(100, "k");
				OnBobble();
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 2;
				InfluencerLand.env.Hide();
				Gym.env.Show();
				OnBobble();
			}
			else if (bar == 3 && beat == 3)
			{
				Gym.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 4 && beat == 1)
			{
				OfficeSpace.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 4 && beat == 3)
			{
				TropicalBank.env.Hide();
				LoveLand.env.Show(1);
				LoveLand.env.Phone.DatingApp.Spin();
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 1;
				LoveLand.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 3)
			{
				ctrlMode = 0;
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 6 && beat == 1)
			{
				HypnoLair.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 6 && beat == 3)
			{
				ctrlMode = 1;
				Conservatory.env.Hide();
				AngrySkies.env.Show();
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 3;
				AngrySkies.env.Hide();
				Underworld.env.Show(isCuedUp: false);
				OnBobble();
			}
			else if (bar == 7 && beat == 3)
			{
				ctrlMode = 1;
				Underworld.env.Hide();
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
			}
			else if (bar == 8 && beat == 1)
			{
				Espot.env.Hide();
				Darkroom.env.MakePlayable();
			}
			else if (bar == 8 && beat == 3)
			{
				ctrlMode = 3;
				Darkroom.env.Hide();
				NeoCity.env.Show();
			}
		}
		else if (phrase == 2)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				NeoCity.env.Hide();
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
		else if (phrase == 4)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 2;
				CancelAllSequences();
				Mall.env.Hide();
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
		else if (phrase == 5)
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
		else if (phrase == 6)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				LoveLand.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 4 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				AngrySkies.env.Show();
			}
		}
		else if (phrase == 7)
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
				CancelAllSequences();
				Conservatory.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 4 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Conservatory.env.Hide();
				AngrySkies.env.Show();
			}
		}
		else if (phrase == 8)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				AngrySkies.env.Hide();
				Underworld.env.Show(isCuedUp: true);
			}
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
		else if (phrase == 9)
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
		else if (phrase == 10)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				NeoCity.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 2 && beat == 1)
			{
				CancelAllSequences();
				FoodySkies.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 3 && beat == 1)
			{
				CancelAllSequences();
				MechSpace.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
			else if (bar == 5 && beat == 1)
			{
				CancelAllSequences();
				Mall.env.Hide();
				InfluencerLand.env.Show(100, "k");
			}
			else if (bar == 5 && beat == 4)
			{
				InfluencerLand.env.EscapeDelayed(timeBeatStarted);
			}
			else if (bar == 6 && beat == 1)
			{
				CancelAllSequences();
				ctrlMode = 2;
				InfluencerLand.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 7 && beat == 1)
			{
				CancelAllSequences();
				OfficeSpace.env.Hide();
				Gym.env.Show(isTrainerFocused: true);
				OnBobble();
			}
			else if (bar == 8 && beat == 1)
			{
				Gym.env.ShiftFocusDelayed(timeBeatStarted, 2);
			}
		}
		else if (phrase == 11)
		{
			if (bar == 1 && beat == 1)
			{
				CancelAllSequences();
				Gym.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
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
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				LoveLand.env.Hide();
				AngrySkies.env.Show();
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				AngrySkies.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 6 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				HypnoLair.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				Matrix.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 8 && beat == 1)
			{
				ctrlMode = 3;
				CancelAllSequences();
				Conservatory.env.Hide();
				Underworld.env.Show(isCuedUp: true);
			}
		}
		else if (phrase == 12)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 1;
				CancelAllSequences();
				Underworld.env.Hide();
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
			}
			else if (bar == 2 && beat == 1)
			{
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
				NeoCity.env.ToggleIsAltFeedback(toggle: true);
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 0;
				CancelAllSequences();
				NeoCity.env.Hide();
				FoodySkies.env.Show();
			}
			else if (bar == 6 && beat == 1)
			{
				FoodySkies.env.Hide();
				Mall.env.Show();
				OnBobble();
			}
			else if (bar == 7 && beat == 1)
			{
				Mall.env.Hide();
				MechSpace.env.Show();
				OnBobble();
			}
			else if (bar == 8 && beat == 1)
			{
				MechSpace.env.Hide();
				InfluencerLand.env.Show(100, "k");
				OnBobble();
			}
		}
		else if (phrase == 13)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 2;
				InfluencerLand.env.Hide();
				Gym.env.Show();
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				Gym.env.Hide();
				OfficeSpace.env.Show();
				OfficeSpace.env.Loop(1f, isReverse: false);
			}
			else if (bar == 3 && beat == 1)
			{
				OfficeSpace.env.Hide();
				TropicalBank.env.Show();
				OnBobble();
			}
			else if (bar == 4 && beat == 1)
			{
				TropicalBank.env.Hide();
				LoveLand.env.Show(1);
				LoveLand.env.Phone.DatingApp.Spin();
			}
			else if (bar == 5 && beat == 1)
			{
				ctrlMode = 1;
				LoveLand.env.Hide();
				Matrix.env.Show();
				OnBobble();
			}
			else if (bar == 6 && beat == 1)
			{
				ctrlMode = 0;
				Matrix.env.Hide();
				HypnoLair.env.Show(isRemix: true);
			}
			else if (bar == 7 && beat == 1)
			{
				HypnoLair.env.Hide();
				Conservatory.env.Show();
				OnBobble();
			}
			else if (bar == 8 && beat == 1)
			{
				ctrlMode = 1;
				Conservatory.env.Hide();
				AngrySkies.env.Show();
			}
		}
		else if (phrase == 14)
		{
			if (bar == 1 && beat == 1)
			{
				ctrlMode = 3;
				AngrySkies.env.Hide();
				Darkroom.env.Show();
				Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
				Underworld.env.Show(isCuedUp: true);
				OnBobble();
			}
			else if (bar == 2 && beat == 1)
			{
				ctrlMode = 1;
				Underworld.env.Hide();
				Darkroom.env.MakePlayable();
			}
			else if (bar == 3 && beat == 1)
			{
				ctrlMode = 3;
				Darkroom.env.Hide();
				NeoCity.env.Show();
			}
			else if (bar == 4 && beat == 1)
			{
				ctrlMode = 1;
				NeoCity.env.Hide();
				Espot.env.Show();
				Espot.env.ScaleBobble(1f);
			}
		}
	}

	protected override void OnSequence()
	{
		float num;
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
			if (sequences[3] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHitWindow(6);
					FoodySkies.env.GetActivePizzaBox().ThrowFast(timeBeatStarted, 2f);
					FoodySkies.env.GetActivePizzaBox().ThrowSoundFast(timeBeatStarted, 2f);
					sequences[3] = 0f;
				}
				else
				{
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
			if (sequences[5] > 0f)
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
		}
		else if (MechSpace.env.CheckIsActivated())
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
		else if (OfficeSpace.env.CheckIsActivated())
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
			if (sequences[3] > 0f)
			{
				if (gameMode < 6)
				{
					QueueRightHitWindow(6);
					LoveLand.env.CountdownRightDelayed(timeBeatStarted, 0.5f);
					sequences[3] = 0f;
				}
				else
				{
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
			if (sequences[4] > 0f)
			{
				rng = Random.Range(0, 3);
				if (rng == 0)
				{
					if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6))
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
					if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6))
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
					if (phrase == 4 && (gameMode == 1 || gameMode == 2 || gameMode == 6))
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
		}
		else if (TropicalBank.env.CheckIsActivated())
		{
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
		else if (Matrix.env.CheckIsActivated())
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
				num = sequences[1];
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
				num = sequences[3];
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
			if (sequences[4] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHoldReleaseWindow(2, 3, isHalfBeatAddedToHold: true);
					Matrix.env.ShootDelayed(timeBeatStarted);
					Matrix.env.ShootSoundDelayed(timeBeatStarted);
					sequences[4] = 0f;
				}
				else
				{
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
			if (sequences[2] > 0f)
			{
				if (gameMode < 6)
				{
					QueueHoldReleaseWindow(8, 14);
					AngrySkies.env.Countdown2Delayed(timeBeatStarted);
					AngrySkies.env.Countdown2SoundDelayed(timeBeatStarted);
					sequences[2] = 0f;
				}
				else
				{
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
			if (sequences[5] > 0f)
			{
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
		else if (Underworld.env.CheckIsActivated())
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
		else if (LoveLand.env.CheckIsActivated())
		{
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
		else if (Matrix.env.CheckIsActivated())
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
		else if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.FakeOutDelayed(timeBeatStarted);
		}
		else if (Darkroom.env.CheckIsActivated() && Darkroom.env.CheckIsPlayable())
		{
			Darkroom.env.PhotoPulley.SetDragType(eventNum);
		}
	}

	protected override void OnAction()
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
		else if (Matrix.env.CheckIsActivated())
		{
			Matrix.env.Press();
		}
		else if (HypnoLair.env.CheckIsActivated())
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
		else if (Espot.env.CheckIsActivated())
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

	protected override void OnActionReleased()
	{
		if (Conservatory.env.CheckIsActivated())
		{
			Conservatory.env.IdleWater();
			if (isPrepped)
			{
				Conservatory.env.Garden.GetActiveCrop().SoakCancel();
			}
		}
		else if (Espot.env.CheckIsActivated())
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
		else if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.ShootLeft();
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
		else if (NeoCity.env.CheckIsActivated())
		{
			NeoCity.env.ShootRight();
		}
	}

	protected override void OnActionRightReleased()
	{
		if (Gym.env.CheckIsActivated())
		{
			Gym.env.McLifter.Unlift(2);
		}
	}

	protected override void OnPrep()
	{
		if (Conservatory.env.CheckIsActivated())
		{
			Conservatory.env.Garden.GetActiveCrop().Soak();
		}
		else if (Espot.env.CheckIsActivated())
		{
			Espot.env.UfoMachine.Claw.React(accuracy);
			Espot.env.UfoMachine.PickUpCapsule();
			Espot.env.Feedbacks[0].SetLocalPosition(3.49f, -0.7f);
		}
		else if (AngrySkies.env.CheckIsActivated())
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
		else if (Darkroom.env.CheckIsActivated())
		{
			Darkroom.env.CheckIsPlayable();
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
		else if (LoveLand.env.CheckIsActivated())
		{
			LoveLand.env.Phone.DatingApp.ToggleIsSliding(toggle: true);
		}
		else if (Underworld.env.CheckIsActivated())
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
			Mall.env.Counter.CardMachine.ResetMeter();
			Mall.env.StoreDisplay.GetActiveItem().BagUp();
			Mall.env.StoreDisplay.GetActiveItem().AttractFeedback();
		}
		else if (MechSpace.env.CheckIsActivated())
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
		else if (OfficeSpace.env.CheckIsActivated())
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
		else if (TropicalBank.env.CheckIsActivated())
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
		else if (Matrix.env.CheckIsActivated())
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
		else if (Conservatory.env.CheckIsActivated())
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
		else if (Underworld.env.CheckIsActivated())
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
			Mall.env.Sweat.Hide();
			Mall.env.Sweat.SetPosition(Mall.env.Counter.CardMachine.GetX() - 2f, Mall.env.Counter.CardMachine.GetY() + 0.67f);
			Mall.env.Sweat.CrossIn();
		}
		else if (MechSpace.env.CheckIsActivated())
		{
			MechSpace.env.McVirtual.Sweat.CrossIn();
			Interface.env.Cam.Breeze();
		}
		else if (OfficeSpace.env.CheckIsActivated())
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
		else if (Matrix.env.CheckIsActivated())
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
		else if (Underworld.env.CheckIsActivated())
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
		else if (MechSpace.env.CheckIsActivated())
		{
			if (gameMode < 6)
			{
				MechSpace.env.McVirtual.Sweat.CrossIn();
			}
		}
		else if (OfficeSpace.env.CheckIsActivated())
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
		else if (Matrix.env.CheckIsActivated())
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
		else if (Underworld.env.CheckIsActivated())
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
