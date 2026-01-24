using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Dream : Custom
{
	public static Dream dir;

	[Header("Props")]
	public int gameMode;

	public int ctrlMode;

	public bool isHalfBeatEnabled;

	public bool isCustomLevelSingleSequenced;

	public string[] codedBarsPractice;

	public string[] codedBarsScored;

	public string[] codedBarsAlt;

	protected bool isPlaying;

	protected bool isFullBeat = true;

	protected bool isHitWindow;

	protected bool isPrepWindow;

	protected bool isPrepped;

	protected bool isLeftHit;

	protected bool isRightHit;

	protected bool isTempMetronome;

	protected bool isActionPressing;

	protected bool isPenaltyLess;

	protected bool isExiting;

	protected int phrase;

	protected int bar;

	protected int barTotal;

	protected int beat;

	protected int beatTotal;

	protected int halfBeat;

	protected int halfBeatTotal;

	protected int hitType;

	protected float accuracy;

	protected float offsetSeconds;

	protected float nextFullBeatMarker;

	protected float nextHalfBeatMarker;

	protected float timeBeatStarted;

	protected float beatDelta;

	protected float[] sequences = new float[10];

	protected int eventNum;

	protected int combo;

	protected float points;

	protected float pointsTotal;

	private bool isControllable;

	private bool isAutoHit;

	private bool isVisualAssisting;

	private bool isAudioAssisting;

	private bool isBiggerHitWindows;

	private bool isEasyScoring;

	private bool isPerfectsOnly;

	private int[] counters = new int[4];

	private string[] codedBeats;

	private float songTime;

	private List<int> hitWindowQueue = new List<int>();

	private List<int> hitWindowQueueHalf = new List<int>();

	private List<int> prepWindowQueue = new List<int>();

	private List<int> prepWindowQueueHalf = new List<int>();

	private List<int> hitTypeQueue = new List<int>();

	private Coroutine triggeringBeat;

	private static int gameModeQueued = -1;

	private static string editorDataString;

	private void Awake()
	{
		dir = this;
		if (gameModeQueued >= 0)
		{
			gameMode = gameModeQueued;
		}
		if (gameMode != 6 && gameMode != 7)
		{
			isCustomLevelSingleSequenced = false;
		}
		ConfigureCodedBeats();
		ConfigurePointsTotal();
	}

	protected virtual void Start()
	{
		if (SaveManager.mgr.CheckIsVisualAssisting())
		{
			isVisualAssisting = true;
		}
		if (SaveManager.mgr.CheckIsAudioAssisting())
		{
			isAudioAssisting = true;
		}
		if (SaveManager.mgr.CheckIsBiggerHitWindows())
		{
			isBiggerHitWindows = true;
		}
		if (SaveManager.mgr.CheckIsEasyScoring())
		{
			isEasyScoring = true;
		}
		if (SaveManager.mgr.CheckIsPerfectsOnly())
		{
			isPerfectsOnly = true;
		}
		if (gameMode == 0)
		{
			Interface.env.ScoreMeter.Delete();
			Interface.env.SideLabel.ShowAsPractice();
			MusicBox.env.SetSpeakerNum(0);
			MusicBox.SetSkippedTime(0f);
		}
		else if (gameMode == 1 || gameMode == 3)
		{
			Interface.env.ScoreMeter.Show(gameMode);
			Interface.env.SideLabel.Delete();
			MusicBox.env.SetSpeakerNum(1);
			MusicBox.SetSkippedTime(0f);
		}
		else if (gameMode == 2 || gameMode == 4)
		{
			Interface.env.ScoreMeter.Show(gameMode);
			Interface.env.SideLabel.Delete();
			MusicBox.env.SetSpeakerNum(2);
			MusicBox.SetSkippedTime(0f);
		}
		else if (gameMode == 5)
		{
			Interface.env.ScoreMeter.Delete();
			MusicBox.env.SetSpeakerNum(1);
			MusicBox.SetSkippedTime(0f);
		}
		else if (gameMode == 6)
		{
			Interface.env.SideLabel.ShowAsEdited();
			MusicBox.env.SetSpeakerNum(1);
			if (MusicBox.CheckIfCustomSongClipExists())
			{
				MusicBox.env.SwapToCustomizedMusic();
			}
		}
		else if (gameMode == 7)
		{
			Interface.env.ScoreMeter.Delete();
			MusicBox.env.SetSpeakerNum(1);
			if (MusicBox.CheckIfCustomSongClipExists())
			{
				MusicBox.env.SwapToCustomizedMusic();
			}
		}
		if (isHalfBeatEnabled)
		{
			nextHalfBeatMarker = MusicBox.env.GetSecsPerBeat() / 2f;
		}
		offsetSeconds = (float)SaveManager.mgr.GetCalibrationOffsetMs() / 1000f;
		Technician.mgr.SetAudioListener(1f);
		Technician.mgr.ToggleVsync(SaveManager.mgr.CheckIsVsynced());
		Interface.env.Letterbox.Show();
		Interface.env.FeatherBorder.Show();
		Interface.env.FeatherBorder.Deactivate();
		DreamWorld.env.Show(0);
	}

	private void Update()
	{
		if (isPlaying)
		{
			if (isControllable)
			{
				DetectControls();
			}
			songTime = MusicBox.env.GetSongTime();
			if (songTime > nextFullBeatMarker)
			{
				timeBeatStarted = nextFullBeatMarker;
				beatDelta = songTime - timeBeatStarted;
				TriggerBeat(toggle: true);
			}
			if (isHalfBeatEnabled && songTime > nextHalfBeatMarker)
			{
				timeBeatStarted = nextHalfBeatMarker;
				beatDelta = songTime - timeBeatStarted;
				TriggerBeat(toggle: false);
			}
		}
		OnUpdate();
	}

	private void DetectControls()
	{
		if (ctrlMode == 0)
		{
			if (ControlHandler.mgr.CheckIsActionPressed())
			{
				TriggerAction();
			}
		}
		else if (ctrlMode == 1)
		{
			if (ControlHandler.mgr.CheckIsActionPressed())
			{
				TriggerAction();
			}
			else if (ControlHandler.mgr.CheckIsActionReleased())
			{
				TriggerActionReleased();
			}
		}
		else if (ctrlMode == 2)
		{
			if (ControlHandler.mgr.CheckIsActionLeftPressed())
			{
				TriggerActionLeft();
			}
			else if (ControlHandler.mgr.CheckIsActionLeftReleased())
			{
				TriggerActionLeftReleased();
			}
			if (ControlHandler.mgr.CheckIsActionRightPressed())
			{
				TriggerActionRight();
			}
			else if (ControlHandler.mgr.CheckIsActionRightReleased())
			{
				TriggerActionRightReleased();
			}
		}
		else if (ctrlMode == 3)
		{
			if (ControlHandler.mgr.CheckIsActionUpPressed())
			{
				TriggerAction();
			}
			else if (ControlHandler.mgr.CheckIsActionUpReleased())
			{
				TriggerActionReleased();
			}
			if (ControlHandler.mgr.CheckIsActionLeftPressed())
			{
				TriggerActionLeft();
			}
			else if (ControlHandler.mgr.CheckIsActionLeftReleased())
			{
				TriggerActionLeftReleased();
			}
			if (ControlHandler.mgr.CheckIsActionRightPressed())
			{
				TriggerActionRight();
			}
			else if (ControlHandler.mgr.CheckIsActionRightReleased())
			{
				TriggerActionRightReleased();
			}
		}
	}

	private void TriggerAction()
	{
		OnAction();
		if (ctrlMode == 0)
		{
			if (isHitWindow)
			{
				isHitWindow = false;
				Hit();
			}
			else
			{
				Strike();
			}
		}
		else if (ctrlMode == 1)
		{
			isActionPressing = true;
			if (isPrepWindow)
			{
				Prep();
				isPrepWindow = false;
			}
		}
		else if (ctrlMode == 3)
		{
			if (isHitWindow && hitType == 0)
			{
				isHitWindow = false;
				Hit();
			}
			else
			{
				Strike();
			}
		}
	}

	public void TriggerActionReleased()
	{
		if (!isActionPressing)
		{
			return;
		}
		isActionPressing = false;
		OnActionReleased();
		if (isPrepped)
		{
			if (isHitWindow)
			{
				isHitWindow = false;
				Hit();
			}
			else
			{
				Strike();
			}
			isPrepped = false;
		}
		else
		{
			Strike();
		}
	}

	private void TriggerActionLeft()
	{
		OnActionLeft();
		if (isHitWindow && hitType == 1)
		{
			isHitWindow = false;
			Hit();
			hitType = 0;
		}
		else if (isHitWindow && hitType == 3)
		{
			if (!isLeftHit)
			{
				isLeftHit = true;
			}
			if (isLeftHit && isRightHit)
			{
				isHitWindow = false;
				Hit();
				isLeftHit = false;
				isRightHit = false;
				hitType = 0;
			}
		}
		else
		{
			Strike();
		}
	}

	private void TriggerActionLeftReleased()
	{
		OnActionLeftReleased();
	}

	private void TriggerActionRight()
	{
		OnActionRight();
		if (isHitWindow && hitType == 2)
		{
			isHitWindow = false;
			Hit();
			hitType = 0;
		}
		else if (isHitWindow && hitType == 3)
		{
			if (!isRightHit)
			{
				isRightHit = true;
			}
			if (isLeftHit && isRightHit)
			{
				isHitWindow = false;
				Hit();
				isLeftHit = false;
				isRightHit = false;
				hitType = 0;
			}
		}
		else
		{
			Strike();
		}
	}

	private void TriggerActionRightReleased()
	{
		OnActionRightReleased();
	}

	public void TriggerSong()
	{
		if (gameMode != 5 && Interface.env.Letterbox.CheckIsActivated())
		{
			Interface.env.Letterbox.DeactivateDelayed();
		}
		songTime = 0f;
		timeBeatStarted = 0f;
		beatDelta = 0f;
		isPlaying = true;
		isControllable = true;
		if (gameMode == 0 || gameMode == 5 || isVisualAssisting)
		{
			Interface.env.Circle.Activate();
		}
		MusicBox.env.PlaySong();
		TriggerBeat(toggle: true);
	}

	protected void TriggerBeat(bool toggle)
	{
		CancelCoroutine(triggeringBeat);
		accuracy = 0f;
		Interface.env.AccuracyChecker.SetAccuracy(0f);
		CloseWindow();
		triggeringBeat = StartCoroutine(TriggeringBeat(toggle));
	}

	private IEnumerator TriggeringBeat(bool toggle)
	{
		isFullBeat = toggle;
		if (isFullBeat)
		{
			IncreaseBeat();
			TriggerHitWindow();
			Bobble();
			OnBeat();
			DetectCodedBeat();
		}
		else
		{
			IncreaseHalfBeat();
			TriggerHitWindowHalf();
			HalfBobble();
		}
		OnSequence();
		if (isFullBeat)
		{
			if (Time.timeScale != 1f || isTempMetronome || isAudioAssisting || gameMode == 0)
			{
				MusicBox.env.PingMetronomeDelayed(timeBeatStarted, beat);
			}
			if (Interface.env.Circle.CheckIsActivated())
			{
				PingCircleDelayed();
			}
		}
		if (isAutoHit)
		{
			AutoHit();
		}
		accuracy = 0.332f;
		bool isDebugging = Interface.env.AccuracyChecker.CheckIsActivated();
		if (isDebugging)
		{
			Interface.env.AccuracyChecker.SetAccuracy(accuracy);
			Interface.env.AccuracyChecker.Time(timeBeatStarted);
		}
		if (isBiggerHitWindows)
		{
			float checkpoint = timeBeatStarted + 0.051f + offsetSeconds;
			yield return new WaitUntil(() => songTime > checkpoint);
			accuracy = 1f;
			if (isDebugging)
			{
				Interface.env.AccuracyChecker.SetAccuracy(accuracy);
			}
			checkpoint = checkpoint + 0.12534f + offsetSeconds;
			yield return new WaitUntil(() => songTime > checkpoint);
			accuracy = 0.333f;
			if (isDebugging)
			{
				Interface.env.AccuracyChecker.SetAccuracy(accuracy);
			}
			checkpoint = checkpoint + 0.057f + offsetSeconds;
			yield return new WaitUntil(() => songTime > checkpoint);
		}
		else
		{
			float checkpoint2 = timeBeatStarted + 0.067f + offsetSeconds;
			yield return new WaitUntil(() => songTime > checkpoint2);
			accuracy = 1f;
			if (isDebugging)
			{
				Interface.env.AccuracyChecker.SetAccuracy(accuracy);
			}
			checkpoint2 = checkpoint2 + 0.09334f + offsetSeconds;
			yield return new WaitUntil(() => songTime > checkpoint2);
			accuracy = 0.333f;
			if (isDebugging)
			{
				Interface.env.AccuracyChecker.SetAccuracy(accuracy);
			}
			checkpoint2 = checkpoint2 + 0.073f + offsetSeconds;
			yield return new WaitUntil(() => songTime > checkpoint2);
		}
		accuracy = 0f;
		if (isDebugging)
		{
			Interface.env.AccuracyChecker.SetAccuracy(0f);
			Interface.env.AccuracyChecker.EndTimer();
		}
		CloseWindow();
		if (gameMode == 5)
		{
			OnWindowEnd();
		}
	}

	private void PingCircleDelayed()
	{
		StartCoroutine(PingingCircleDelayed());
	}

	private IEnumerator PingingCircleDelayed()
	{
		float checkpoint = timeBeatStarted + 0.11667f;
		yield return new WaitUntil(() => songTime > checkpoint);
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.FlashNumber(timeBeatStarted, beat, MusicBox.env.GetSecsPerBeat());
			Interface.env.Circle.Bobble();
			Interface.env.Circle.SetProgress(GetSongProgress());
		}
	}

	private void AutoHit()
	{
		StartCoroutine(AutoHitting());
	}

	private IEnumerator AutoHitting()
	{
		float checkpoint = timeBeatStarted + 0.11667f;
		yield return new WaitUntil(() => songTime > checkpoint);
		if (ctrlMode == 0)
		{
			if (isHitWindow)
			{
				TriggerAction();
			}
		}
		else if (ctrlMode == 1)
		{
			if (isPrepWindow)
			{
				TriggerAction();
			}
			else if (isHitWindow)
			{
				TriggerActionReleased();
			}
		}
		else if (ctrlMode == 2)
		{
			if (!isHitWindow)
			{
				yield break;
			}
			checkpoint += MusicBox.env.GetSecsPerBeat() / 4f;
			if (hitType == 1)
			{
				TriggerActionLeft();
				yield return new WaitUntil(() => songTime > checkpoint);
				TriggerActionLeftReleased();
				yield break;
			}
			if (hitType == 2)
			{
				TriggerActionRight();
				yield return new WaitUntil(() => songTime > checkpoint);
				TriggerActionRightReleased();
				yield break;
			}
			TriggerActionLeft();
			TriggerActionRight();
			yield return new WaitUntil(() => songTime > checkpoint);
			TriggerActionLeftReleased();
			TriggerActionRightReleased();
		}
		else
		{
			if (ctrlMode != 3 || !isHitWindow)
			{
				yield break;
			}
			if (hitType == 0)
			{
				TriggerAction();
				yield break;
			}
			if (hitType == 1 || hitType == 3)
			{
				TriggerActionLeft();
			}
			if (hitType == 2 || hitType == 3)
			{
				TriggerActionRight();
			}
		}
	}

	private void TriggerHitWindow()
	{
		for (int i = 0; i < prepWindowQueue.Count; i++)
		{
			if (prepWindowQueue[i] == beatTotal)
			{
				isPrepWindow = true;
				prepWindowQueue.RemoveAt(i);
				break;
			}
		}
		for (int j = 0; j < hitWindowQueue.Count; j++)
		{
			if (hitWindowQueue[j] != beatTotal)
			{
				continue;
			}
			isHitWindow = true;
			hitWindowQueue.RemoveAt(j);
			if (hitTypeQueue.Count > 0)
			{
				hitType = hitTypeQueue[0];
				hitTypeQueue.RemoveAt(0);
				if (hitType == 3)
				{
					isLeftHit = false;
					isRightHit = false;
				}
			}
			OnHitWindow();
			break;
		}
	}

	private void TriggerHitWindowHalf()
	{
		for (int i = 0; i < prepWindowQueueHalf.Count; i++)
		{
			if (prepWindowQueueHalf[i] == beatTotal)
			{
				isPrepWindow = true;
				prepWindowQueueHalf.RemoveAt(i);
				break;
			}
		}
		for (int j = 0; j < hitWindowQueueHalf.Count; j++)
		{
			if (hitWindowQueueHalf[j] == halfBeatTotal)
			{
				isHitWindow = true;
				hitWindowQueueHalf.RemoveAt(j);
				if (hitTypeQueue.Count > 0)
				{
					hitType = hitTypeQueue[0];
					hitTypeQueue.RemoveAt(0);
				}
				OnHitWindow();
				break;
			}
		}
	}

	private void CloseWindow()
	{
		if (isHitWindow)
		{
			Miss();
			isHitWindow = false;
		}
		else if (isPrepWindow)
		{
			Miss();
			isPrepWindow = false;
		}
	}

	private void Bobble()
	{
		OnBobble();
	}

	private void HalfBobble()
	{
		OnHalfBobble();
	}

	private void DetectCodedBeat()
	{
		if (beatTotal <= codedBeats.Length)
		{
			string text = codedBeats[beatTotal - 1];
			for (int i = 0; i < text.Length && text[i] != '.'; i++)
			{
				if (text[i] == 'd')
				{
					if (isCustomLevelSingleSequenced)
					{
						CancelAllSequences(isCancelHold: false);
					}
					char c = text[i + 1];
					if (c != 'x')
					{
						StartSequenceDown(c - 48);
					}
				}
				else if (text[i] == 'u')
				{
					if (isCustomLevelSingleSequenced)
					{
						CancelAllSequences(isCancelHold: false);
					}
					StartSequenceUp(text[i + 1] - 48);
				}
				else if (text[i] == 'e')
				{
					TriggerEvent(text[i + 1] - 48);
				}
				else if (text[i] == 't')
				{
					if (!MusicBox.CheckIfCustomSongClipExists())
					{
						int newTrack = text[i + 1] - 48;
						float secsPerBeat = MusicBox.env.GetSecsPerBeat();
						MusicBox.env.ChangeTrackLive(timeBeatStarted, newTrack, beatTotal);
						float num = MusicBox.env.GetSecsPerBeat() - secsPerBeat;
						nextFullBeatMarker += num;
						if (isHalfBeatEnabled)
						{
							nextHalfBeatMarker += num / 2f;
						}
						OnTempoChange();
					}
				}
				else if (text[i] == 's')
				{
					char c2 = text[i + 1];
					if (c2 != '0' && FoodySkies.env != null && FoodySkies.env.CheckIsActivated())
					{
						FoodySkies.env.Hide();
					}
					else if (c2 != '1' && Mall.env != null && Mall.env.CheckIsActivated())
					{
						Mall.env.Hide();
					}
					else if (c2 != '2' && MechSpace.env != null && MechSpace.env.CheckIsActivated())
					{
						MechSpace.env.Hide();
					}
					else if (c2 != '3' && InfluencerLand.env != null && InfluencerLand.env.CheckIsActivated())
					{
						InfluencerLand.env.Hide();
					}
					else if (c2 != '4' && Gym.env != null && Gym.env.CheckIsActivated())
					{
						Gym.env.Hide();
					}
					else if (c2 != '5' && OfficeSpace.env != null && OfficeSpace.env.CheckIsActivated())
					{
						OfficeSpace.env.Hide();
					}
					else if (c2 != '6' && TropicalBank.env != null && TropicalBank.env.CheckIsActivated())
					{
						TropicalBank.env.Hide();
					}
					else if (c2 != '7' && LoveLand.env != null && LoveLand.env.CheckIsActivated())
					{
						LoveLand.env.Hide();
					}
					else if (c2 != '8' && Matrix.env != null && Matrix.env.CheckIsActivated())
					{
						Matrix.env.Hide();
					}
					else if (c2 != '9' && HypnoLair.env != null && HypnoLair.env.CheckIsActivated())
					{
						HypnoLair.env.Hide();
					}
					else if (c2 != '!' && AngrySkies.env != null && AngrySkies.env.CheckIsActivated())
					{
						AngrySkies.env.Hide();
					}
					else if (c2 != '@' && Conservatory.env != null && Conservatory.env.CheckIsActivated())
					{
						Conservatory.env.Hide();
					}
					else if (c2 != '#' && Underworld.env != null && Underworld.env.CheckIsActivated())
					{
						Underworld.env.Hide();
					}
					else if (c2 != '$' && Espot.env != null && Espot.env.CheckIsActivated())
					{
						Espot.env.Hide();
					}
					else if (c2 != '%' && Darkroom.env != null && Darkroom.env.CheckIsActivated())
					{
						Darkroom.env.Hide();
					}
					else if (c2 != '^' && NeoCity.env != null && NeoCity.env.CheckIsActivated())
					{
						NeoCity.env.Hide();
					}
					if (c2 == '0' && !FoodySkies.env.CheckIsActivated())
					{
						ctrlMode = 0;
						isCustomLevelSingleSequenced = true;
						CancelAllSequences();
						CancelHitWindows();
						FoodySkies.env.Show();
					}
					else if (c2 == '1' && !Mall.env.CheckIsActivated())
					{
						ctrlMode = 0;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						Mall.env.Show();
						OnBobble();
					}
					else if (c2 == '2' && !MechSpace.env.CheckIsActivated())
					{
						ctrlMode = 0;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						MechSpace.env.Show();
						OnBobble();
					}
					else if (c2 == '3' && !InfluencerLand.env.CheckIsActivated())
					{
						ctrlMode = 0;
						isCustomLevelSingleSequenced = true;
						CancelAllSequences();
						CancelHitWindows();
						InfluencerLand.env.Show(500, "k");
					}
					else if (c2 == '4' && !Gym.env.CheckIsActivated())
					{
						ctrlMode = 2;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						Gym.env.Show();
						OnBobble();
					}
					else if (c2 == '5' && !OfficeSpace.env.CheckIsActivated())
					{
						ctrlMode = 2;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						OfficeSpace.env.Show();
						OfficeSpace.env.Loop(1f, isReverse: false);
					}
					else if (c2 == '6' && !TropicalBank.env.CheckIsActivated())
					{
						ctrlMode = 2;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						TropicalBank.env.Show();
						OnBobble();
					}
					else if (c2 == '7' && !LoveLand.env.CheckIsActivated())
					{
						ctrlMode = 2;
						isCustomLevelSingleSequenced = true;
						CancelAllSequences();
						CancelHitWindows();
						LoveLand.env.Show(1);
					}
					else if (c2 == '8' && !Matrix.env.CheckIsActivated())
					{
						ctrlMode = 1;
						isCustomLevelSingleSequenced = true;
						CancelAllSequences();
						CancelHitWindows();
						Matrix.env.Show();
						OnBobble();
					}
					else if (c2 == '9' && !HypnoLair.env.CheckIsActivated())
					{
						ctrlMode = 0;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						HypnoLair.env.Show(isRemix: true);
					}
					else if (c2 == '!' && !AngrySkies.env.CheckIsActivated())
					{
						ctrlMode = 1;
						isCustomLevelSingleSequenced = true;
						CancelAllSequences();
						CancelHitWindows();
						AngrySkies.env.Show();
					}
					else if (c2 == '@' && !Conservatory.env.CheckIsActivated())
					{
						ctrlMode = 1;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						Conservatory.env.Show();
						OnBobble();
					}
					else if (c2 == '#' && !Underworld.env.CheckIsActivated())
					{
						ctrlMode = 3;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						Underworld.env.Show(isCuedUp: true);
					}
					else if (c2 == '$' && !Espot.env.CheckIsActivated())
					{
						ctrlMode = 1;
						isCustomLevelSingleSequenced = true;
						CancelAllSequences();
						CancelHitWindows();
						Espot.env.Show();
					}
					else if (c2 == '%' && !Darkroom.env.CheckIsActivated())
					{
						ctrlMode = 1;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						Darkroom.env.Show();
						Darkroom.env.MakePlayable();
						Darkroom.env.PhotoPulley.DragDelayed(beatDelta);
					}
					else if (c2 == '^' && !NeoCity.env.CheckIsActivated())
					{
						ctrlMode = 3;
						isCustomLevelSingleSequenced = false;
						CancelAllSequences();
						CancelHitWindows();
						NeoCity.env.Show();
					}
				}
			}
		}
		else if (gameMode != 5)
		{
			TriggerResults();
		}
	}

	private void StartSequenceDown(int codeNum)
	{
		sequences[codeNum] = 1f;
	}

	private void StartSequenceUp(int codeNum)
	{
		StartCoroutine(StartingSequenceUp(codeNum));
	}

	private IEnumerator StartingSequenceUp(int codeNum)
	{
		if (isHalfBeatEnabled)
		{
			yield return new WaitUntil(() => accuracy == 1f);
			sequences[codeNum] = 1f;
		}
	}

	private void TriggerEvent(int newEventNum)
	{
		eventNum = newEventNum;
		OnEvent();
	}

	private void Prep()
	{
		OnPrep();
		AddToPoints(accuracy);
		isPrepped = true;
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.Hit(accuracy);
		}
		if (accuracy < 1f)
		{
			if (isPerfectsOnly && isPlaying)
			{
				AutoRestart();
			}
			ControlHandler.mgr.RumbleLight();
		}
		else
		{
			ControlHandler.mgr.RumbleHard();
		}
		if (DreamWorld.env.CheckIsThereFeedbacks())
		{
			if (accuracy == 0.332f)
			{
				DreamWorld.env.GetActiveFeedback().CrossIn("early");
			}
			else if (accuracy == 0.333f)
			{
				DreamWorld.env.GetActiveFeedback().CrossIn("late");
			}
			else if (accuracy == 1f)
			{
				DreamWorld.env.GetActiveFeedback().CrossIn("perfect");
			}
		}
		DreamWorld.env.IncreaseActiveFeedback();
	}

	private void Hit()
	{
		OnHit();
		AddToPoints(accuracy);
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.Hit(accuracy);
		}
		if (accuracy < 1f)
		{
			if (isPerfectsOnly && isPlaying)
			{
				AutoRestart();
			}
			ControlHandler.mgr.RumbleLight();
		}
		else
		{
			ControlHandler.mgr.RumbleHard();
		}
		if (DreamWorld.env.CheckIsThereFeedbacks())
		{
			if (accuracy == 0.332f)
			{
				DreamWorld.env.GetActiveFeedback().CrossIn("early");
			}
			else if (accuracy == 0.333f)
			{
				DreamWorld.env.GetActiveFeedback().CrossIn("late");
			}
			else if (accuracy == 1f)
			{
				DreamWorld.env.GetActiveFeedback().CrossIn("perfect");
			}
		}
		DreamWorld.env.IncreaseActiveFeedback();
	}

	private void Miss()
	{
		if (!isPenaltyLess)
		{
			if (isPerfectsOnly && isPlaying)
			{
				AutoRestart();
			}
			OnMiss();
			AddToPoints(-0.167f);
			if (Interface.env.Circle.CheckIsActivated())
			{
				Interface.env.Circle.Strike();
			}
		}
		isPrepped = false;
	}

	private void Strike()
	{
		if (!isPenaltyLess)
		{
			if (isPerfectsOnly && isPlaying)
			{
				AutoRestart();
			}
			OnStrike();
			AddToPoints(-0.166f);
			if (Interface.env.Circle.CheckIsActivated())
			{
				Interface.env.Circle.Strike();
			}
		}
	}

	private void TriggerCombo(float addedPoints)
	{
		if (addedPoints == 1f)
		{
			if (combo < 5)
			{
				combo++;
				if (combo == 5 && !Interface.env.ScoreMeter.CheckIsOnFire())
				{
					Interface.env.ScoreMeter.FireItUp();
				}
			}
		}
		else if (combo == 5)
		{
			combo = 0;
			if (Interface.env.ScoreMeter.CheckIsOnFire())
			{
				Interface.env.ScoreMeter.Extinguish();
			}
		}
	}

	private void DetectAchievements()
	{
		if (gameMode > 0 && gameMode < 6)
		{
			if (!(SteamManager.mgr != null))
			{
				return;
			}
			if (GetScore() >= 3)
			{
				if (gameMode == 1 || gameMode == 3)
				{
					SteamManager.mgr.RewardAchievement("star_precision");
				}
				else if (gameMode == 2 || gameMode == 4)
				{
					SteamManager.mgr.RewardAchievement("ring_precision");
				}
			}
			if (GetScore() == 4)
			{
				if (gameMode == 1 || gameMode == 3)
				{
					SteamManager.mgr.RewardAchievement("star_perfectionist");
				}
				else if (gameMode == 2 || gameMode == 4)
				{
					SteamManager.mgr.RewardAchievement("ring_perfectionist");
				}
				if (SaveManager.mgr.GetTotalPerfects() >= 42)
				{
					SteamManager.mgr.RewardAchievement("go_to_bed");
				}
			}
			if (SaveManager.mgr.CheckStargazerAchievement())
			{
				SteamManager.mgr.RewardAchievement("stargazer");
			}
			if (SaveManager.mgr.CheckRingCollectorAchievement())
			{
				SteamManager.mgr.RewardAchievement("ring_collector");
			}
		}
		else if (gameMode >= 6)
		{
			if (SteamManager.mgr != null)
			{
				SteamManager.mgr.RewardAchievement("creator");
			}
			if (!SaveManager.mgr.CheckIsCreator())
			{
				SaveManager.mgr.ToggleIsCreator(toggle: true);
			}
		}
	}

	public void TriggerResults()
	{
		isPlaying = false;
		isControllable = false;
		isPrepped = false;
		TriggerActionReleased();
		CancelCoroutine(triggeringBeat);
		CancelAllSequences();
		OnResults();
		string text = SceneMonitor.mgr.GetActiveSceneName();
		if (gameMode == 2 || gameMode == 4)
		{
			text += "Alt";
		}
		int score = GetScore();
		int score2 = SaveManager.mgr.GetScore(text);
		if (gameMode == 0)
		{
			Interface.env.SideLabel.Disable();
		}
		else if (gameMode < 6 && score > score2)
		{
			SaveManager.mgr.SetScore(text, score);
		}
		DetectAchievements();
		Interface.env.Results.Activate(gameMode, score, score2);
		Interface.env.Disable();
	}

	private void AutoRestart()
	{
		isPlaying = false;
		Interface.env.Submenu.PlaySfx(1);
		dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
		if (gameMode == 0)
		{
			Interface.env.SideLabel.Disable();
		}
	}

	public void ExitTo(string sceneName)
	{
		StartCoroutine(ExitingTo(sceneName));
	}

	private IEnumerator ExitingTo(string sceneName)
	{
		isExiting = true;
		OnExit();
		Interface.env.Letterbox.Activate();
		Interface.env.FeatherBorder.Activate();
		SceneMonitor.mgr.PreloadScene(sceneName);
		DreamWorld.env.PlayTransitionSound();
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.Deactivate();
		}
		yield return new WaitForSeconds(0.72f);
		DreamWorld.env.TransitionToChapter();
		yield return new WaitForSecondsRealtime(0.75f);
		SceneMonitor.mgr.LoadScene();
	}

	private void IncreaseBeat()
	{
		if (beat == 4 || beat == 0)
		{
			beat = 1;
			if (bar == 8 || bar == 0)
			{
				bar = 1;
				phrase++;
			}
			else
			{
				bar++;
			}
			barTotal++;
		}
		else
		{
			beat++;
		}
		beatTotal++;
		IncreaseNextFullBeatMarker();
	}

	private void IncreaseHalfBeat()
	{
		if (halfBeat == 4 || halfBeat == 0)
		{
			halfBeat = 1;
		}
		else
		{
			halfBeat++;
		}
		halfBeatTotal++;
		IncreaseNextHalfBeatMarker();
	}

	private void ConfigureCodedBeats()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (gameMode == 0)
		{
			string[] array = codedBarsPractice;
			foreach (string value in array)
			{
				stringBuilder.Append(value);
			}
		}
		else if (gameMode == 1 || gameMode == 3 || gameMode == 5)
		{
			string[] array = codedBarsScored;
			foreach (string value2 in array)
			{
				stringBuilder.Append(value2);
			}
		}
		else if (gameMode == 2 || gameMode == 4)
		{
			string[] array = codedBarsAlt;
			foreach (string value3 in array)
			{
				stringBuilder.Append(value3);
			}
		}
		else if (gameMode == 6 || gameMode == 7)
		{
			stringBuilder.Append(editorDataString);
		}
		string text = stringBuilder.ToString();
		codedBeats = text.Split(new char[4] { ' ', '/', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
	}

	private void ConfigurePointsTotal()
	{
		for (int i = 0; i < codedBeats.Length; i++)
		{
			string text = codedBeats[i];
			for (int j = 0; j < text.Length; j++)
			{
				if (text[j] == '!' || text[j] == '?' || text[j] == '<' || text[j] == '>' || text[j] == '^' || text[j] == '\'')
				{
					pointsTotal += 1f;
				}
			}
		}
	}

	private void IncreaseNextFullBeatMarker()
	{
		nextFullBeatMarker += MusicBox.env.GetSecsPerBeat();
	}

	private void IncreaseNextHalfBeatMarker()
	{
		nextHalfBeatMarker += MusicBox.env.GetSecsPerBeat();
	}

	private void CancelHitWindows()
	{
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.Hide();
			Interface.env.Circle.Show();
		}
		hitWindowQueue.Clear();
		hitWindowQueueHalf.Clear();
		hitTypeQueue.Clear();
		prepWindowQueue.Clear();
		isHitWindow = false;
		isPrepWindow = false;
	}

	protected void SetCtrlMode(int newCtrlMode)
	{
		ctrlMode = newCtrlMode;
	}

	protected void CancelAllSequences(bool isCancelHold = true)
	{
		if (isCancelHold)
		{
			isActionPressing = false;
		}
		for (int i = 0; i < sequences.Length; i++)
		{
			sequences[i] = 0f;
		}
	}

	public void AddToPoints(float addedPoints)
	{
		if (addedPoints == 1f)
		{
			counters[0]++;
			Interface.env.Cam.Glow(1f);
		}
		else if (addedPoints == 0.333f)
		{
			counters[1]++;
			Interface.env.Cam.Glow(0.333f);
			if (isEasyScoring)
			{
				addedPoints = 0.6f;
			}
		}
		else if (addedPoints == 0.332f)
		{
			counters[2]++;
			Interface.env.Cam.Glow(0.332f);
			addedPoints = ((!isEasyScoring) ? 0.333f : 0.6f);
		}
		else if (addedPoints == -0.167f)
		{
			counters[3]++;
		}
		else if (addedPoints == -0.166f && points > 0f)
		{
			counters[3]++;
		}
		points += addedPoints;
		points = ((points < 0f) ? 0f : points);
		if (Interface.env.ScoreMeter.CheckIsActivated())
		{
			TriggerCombo(addedPoints);
			Interface.env.ScoreMeter.TriggerScoreUpdate();
		}
	}

	public void SetDebugScore(float addedPoints, float addedPointsTotal)
	{
		points = addedPoints;
		pointsTotal = addedPointsTotal;
	}

	public void SetOffsetSeconds(int ms)
	{
		offsetSeconds = (float)ms / 1000f;
	}

	public void ToggleIsPlaying(bool toggle)
	{
		isPlaying = toggle;
	}

	public void ToggleIsControllable(bool toggle)
	{
		isControllable = toggle;
	}

	public void ToggleIsAutoHit()
	{
		isAutoHit = !isAutoHit;
	}

	public void ToggleIsVisualAssisting(bool toggle)
	{
		isVisualAssisting = toggle;
	}

	public void ToggleIsAudioAssisting(bool toggle)
	{
		isAudioAssisting = toggle;
	}

	public void ToggleIsBiggerHitWindows(bool toggle)
	{
		isBiggerHitWindows = toggle;
	}

	public void ToggleIsEasyScoring(bool toggle)
	{
		isEasyScoring = toggle;
	}

	public void ToggleIsPerfectsOnly(bool toggle)
	{
		isPerfectsOnly = toggle;
	}

	public void QueueHitWindow(int numBeatsTilHit, bool isHalfBeatAdded = false)
	{
		if (!isFullBeat)
		{
			if (isHalfBeatAdded)
			{
				hitWindowQueue.Add(beatTotal + numBeatsTilHit + 1);
			}
			else
			{
				hitWindowQueueHalf.Add(halfBeatTotal + numBeatsTilHit);
			}
		}
		else if (isHalfBeatAdded)
		{
			hitWindowQueueHalf.Add(beatTotal + numBeatsTilHit);
		}
		else
		{
			hitWindowQueue.Add(beatTotal + numBeatsTilHit);
		}
		if (ctrlMode == 3)
		{
			hitTypeQueue.Add(0);
			if (Interface.env.Circle.CheckIsActivated())
			{
				Interface.env.Circle.GetRing().CrossInDelayed(timeBeatStarted, numBeatsTilHit, 3, isHalfBeatAdded);
			}
		}
		else if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.GetRing().CrossInDelayed(timeBeatStarted, numBeatsTilHit, 0, isHalfBeatAdded);
		}
	}

	public void QueueLeftHitWindow(int numBeatsTilHit, bool isHalfBeatAdded = false)
	{
		if (!isFullBeat)
		{
			if (isHalfBeatAdded)
			{
				hitWindowQueue.Add(beatTotal + numBeatsTilHit + 1);
			}
			else
			{
				hitWindowQueueHalf.Add(halfBeatTotal + numBeatsTilHit);
			}
		}
		else if (isHalfBeatAdded)
		{
			hitWindowQueueHalf.Add(beatTotal + numBeatsTilHit);
		}
		else
		{
			hitWindowQueue.Add(beatTotal + numBeatsTilHit);
		}
		hitTypeQueue.Add(1);
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.GetRing().CrossInDelayed(timeBeatStarted, numBeatsTilHit, 1, isHalfBeatAdded);
		}
	}

	public void QueueRightHitWindow(int numBeatsTilHit, bool isHalfBeatAdded = false)
	{
		if (!isFullBeat)
		{
			if (isHalfBeatAdded)
			{
				hitWindowQueue.Add(beatTotal + numBeatsTilHit + 1);
			}
			else
			{
				hitWindowQueueHalf.Add(halfBeatTotal + numBeatsTilHit);
			}
		}
		else if (isHalfBeatAdded)
		{
			hitWindowQueueHalf.Add(beatTotal + numBeatsTilHit);
		}
		else
		{
			hitWindowQueue.Add(beatTotal + numBeatsTilHit);
		}
		hitTypeQueue.Add(2);
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.GetRing().CrossInDelayed(timeBeatStarted, numBeatsTilHit, 2, isHalfBeatAdded);
		}
	}

	public void QueueLeftRightHitWindow(int numBeatsTilHit, bool isHalfBeatAdded = false)
	{
		if (!isFullBeat)
		{
			if (isHalfBeatAdded)
			{
				hitWindowQueue.Add(beatTotal + numBeatsTilHit + 1);
			}
			else
			{
				hitWindowQueueHalf.Add(halfBeatTotal + numBeatsTilHit);
			}
		}
		else if (isHalfBeatAdded)
		{
			hitWindowQueueHalf.Add(beatTotal + numBeatsTilHit);
		}
		else
		{
			hitWindowQueue.Add(beatTotal + numBeatsTilHit);
		}
		hitTypeQueue.Add(3);
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.GetRing().CrossInDelayed(timeBeatStarted, numBeatsTilHit, 1, isHalfBeatAdded);
			Interface.env.Circle.GetRing().CrossInDelayed(timeBeatStarted, numBeatsTilHit, 2, isHalfBeatAdded);
		}
	}

	public void QueueHoldReleaseWindow(int numBeatsTilHold, int numBeatsTilRelease, bool isHalfBeatAddedToHold = false, bool isHalfBeatAddedToRelease = false)
	{
		if (!isFullBeat)
		{
			if (isHalfBeatAddedToHold)
			{
				prepWindowQueue.Add(beatTotal + numBeatsTilHold + 1);
			}
			else
			{
				prepWindowQueueHalf.Add(halfBeatTotal + numBeatsTilHold);
			}
			if (isHalfBeatAddedToRelease)
			{
				hitWindowQueue.Add(beatTotal + numBeatsTilRelease + 1);
			}
			else
			{
				hitWindowQueueHalf.Add(halfBeatTotal + numBeatsTilRelease);
			}
		}
		else
		{
			if (isHalfBeatAddedToHold)
			{
				prepWindowQueueHalf.Add(beatTotal + numBeatsTilHold);
			}
			else
			{
				prepWindowQueue.Add(beatTotal + numBeatsTilHold);
			}
			if (isHalfBeatAddedToRelease)
			{
				hitWindowQueueHalf.Add(beatTotal + numBeatsTilRelease);
			}
			else
			{
				hitWindowQueue.Add(beatTotal + numBeatsTilRelease);
			}
		}
		if (Interface.env.Circle.CheckIsActivated())
		{
			Interface.env.Circle.GetRadial().CrossInDelayed(timeBeatStarted, numBeatsTilHold, numBeatsTilRelease, isHalfBeatAddedToHold, isHalfBeatAddedToRelease);
		}
	}

	public static void SetGameMode(int value)
	{
		gameModeQueued = value;
	}

	public static void SetEditorDataString(string newEditorDataString)
	{
		editorDataString = newEditorDataString;
	}

	protected float GetSongProgress()
	{
		return (float)beatTotal * 1f / (float)codedBeats.Length;
	}

	public bool CheckIsFullBeat()
	{
		return isFullBeat;
	}

	public bool CheckIsHitWindow()
	{
		return isHitWindow;
	}

	public bool CheckIsAutoHit()
	{
		return isAutoHit;
	}

	public int GetGameMode()
	{
		return gameMode;
	}

	public int GetPhrase()
	{
		return phrase;
	}

	public int GetBar()
	{
		return bar;
	}

	public int GetBeat()
	{
		return beat;
	}

	public int GetBeatTotal()
	{
		return beatTotal;
	}

	public int GetHalfBeatTotal()
	{
		return halfBeatTotal;
	}

	public int GetScore()
	{
		float percentScore = GetPercentScore();
		if (percentScore < 0.5f)
		{
			return 0;
		}
		if (percentScore >= 0.5f && percentScore < 0.725f)
		{
			return 1;
		}
		if (percentScore >= 0.725f && percentScore < 0.935f)
		{
			return 2;
		}
		if (percentScore >= 0.935f && percentScore < 1f)
		{
			return 3;
		}
		return 4;
	}

	public int GetCounter(int index)
	{
		return counters[index];
	}

	public float GetPercentScore()
	{
		if (pointsTotal > 0f)
		{
			if (!(points / pointsTotal > 1f))
			{
				return points / pointsTotal;
			}
			return 1f;
		}
		return 0f;
	}

	protected virtual void OnBobble()
	{
	}

	protected virtual void OnHalfBobble()
	{
	}

	protected virtual void OnBeat()
	{
	}

	protected virtual void OnWindowEnd()
	{
	}

	protected virtual void OnSequence()
	{
	}

	protected virtual void OnEvent()
	{
	}

	protected virtual void OnTempoChange()
	{
	}

	protected virtual void OnAction()
	{
	}

	protected virtual void OnActionReleased()
	{
	}

	protected virtual void OnActionLeft()
	{
	}

	protected virtual void OnActionLeftReleased()
	{
	}

	protected virtual void OnActionRight()
	{
	}

	protected virtual void OnActionRightReleased()
	{
	}

	protected virtual void OnHitWindow()
	{
	}

	protected virtual void OnPrep()
	{
	}

	protected virtual void OnHit()
	{
	}

	protected virtual void OnStrike()
	{
	}

	protected virtual void OnMiss()
	{
	}

	protected virtual void OnResults()
	{
	}

	protected virtual void OnUpdate()
	{
	}

	protected virtual void OnExit()
	{
	}
}
