using System.Collections;
using UnityEngine;

public class Dream_tutorial : Dream
{
	private bool isMissed;

	private bool isCooldowningState;

	private int state;

	private string tempString1;

	private string tempString2;

	private Coroutine cooldowningState;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		DreamWorld.env.Show(1);
		TutorialWorld.env.Show();
		DreamWorld.env.DialogBox.Show();
		if (SaveManager.GetLang() == 6)
		{
			DreamWorld.env.DialogBox.SetDialogState(0, 4f, 1);
		}
		else if (SaveManager.GetLang() == 7)
		{
			DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
		}
		else if (SaveManager.GetLang() == 8)
		{
			DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
		}
		else if (SaveManager.GetLang() == 9)
		{
			DreamWorld.env.DialogBox.SetDialogState(0, 4.2f, 1);
		}
		Interface.env.Circle.ToggleIsOffCamera(toggle: true);
		Interface.env.Circle.SetParentAndReposition(TutorialWorld.env.transform);
		Interface.env.Circle.SetLocalZ(15f);
		Interface.env.Letterbox.Hide();
		Interface.env.FeatherBorder.Hide();
		Interface.env.ScoreMeter.Delete();
		yield return null;
	}

	protected override void OnUpdate()
	{
		if (ControlHandler.mgr.CheckIsActionPressed())
		{
			if (!isCooldowningState && Time.timeScale > 0f && !isPlaying && !isExiting)
			{
				state++;
				TriggerState();
			}
		}
		else if (ControlHandler.mgr.CheckIsExtraPressed() && !isCooldowningState && Time.timeScale > 0f && !isPlaying && !isExiting && DreamWorld.env.DialogBox.CheckIsSubInfoOpen())
		{
			state -= 2;
			if (state == 1)
			{
				Restart();
			}
			else
			{
				Rewind(4);
			}
		}
	}

	private void PauseTutorial()
	{
		isPlaying = false;
		isActionPressing = false;
		isPrepped = false;
		MusicBox.env.PauseSong();
	}

	private void ResumeTutorial()
	{
		isPlaying = true;
		MusicBox.env.UnpauseSong();
	}

	private void Rewind(int barsToRewind)
	{
		StartCoroutine(Rewinding(barsToRewind));
	}

	private IEnumerator Rewinding(int barsToRewind)
	{
		CooldownState(1.25f);
		float num = MusicBox.env.GetSecsPerBeat() * (float)(4 * barsToRewind);
		MusicBox.env.RewindSong(num);
		nextFullBeatMarker -= num;
		bar -= barsToRewind;
		barTotal -= barsToRewind;
		if (bar <= 0)
		{
			bar += 8;
			phrase--;
		}
		beatTotal -= 4 * barsToRewind;
		DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
		TutorialWorld.env.Scratch();
		yield return new WaitForSeconds(1f);
		TutorialWorld.env.Rewind();
		Interface.env.Circle.SetProgress(GetSongProgress());
		yield return new WaitForSeconds(0.25f);
		TriggerState();
	}

	private void Restart()
	{
		StartCoroutine(Restarting());
	}

	private IEnumerator Restarting()
	{
		CooldownState(1.25f);
		MusicBox.env.ResetSong();
		DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
		TutorialWorld.env.Scratch();
		yield return new WaitForSeconds(1f);
		TutorialWorld.env.Rewind();
		Interface.env.Circle.SetProgress(0f);
		yield return new WaitForSeconds(0.25f);
		phrase = 0;
		bar = 0;
		barTotal = 0;
		beat = 0;
		beatTotal = 0;
		nextFullBeatMarker = 0f;
		TriggerState();
	}

	private void TriggerState()
	{
		CooldownState(0.5f);
		switch (state)
		{
		case 1:
			isTempMetronome = true;
			if (DreamWorld.env.DialogBox.CheckIsActivated())
			{
				if (SaveManager.GetLang() == 6)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f);
				}
				else if (SaveManager.GetLang() == 7)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f, 1);
				}
				else if (SaveManager.GetLang() == 8)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f, 1);
				}
				else if (SaveManager.GetLang() == 9)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1, 4.2f, 1);
				}
				else
				{
					DreamWorld.env.DialogBox.ChangeDialogState(1);
				}
			}
			else
			{
				if (SaveManager.GetLang() == 6)
				{
					DreamWorld.env.DialogBox.SetDialogState(1, 4.2f);
				}
				else if (SaveManager.GetLang() == 7)
				{
					DreamWorld.env.DialogBox.SetDialogState(1, 4.2f, 1);
				}
				else if (SaveManager.GetLang() == 8)
				{
					DreamWorld.env.DialogBox.SetDialogState(1, 4.2f, 1);
				}
				else if (SaveManager.GetLang() == 9)
				{
					DreamWorld.env.DialogBox.SetDialogState(1, 4.2f, 1);
				}
				else
				{
					DreamWorld.env.DialogBox.SetDialogState(1);
				}
				DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			}
			tempString1 = DreamWorld.env.DialogBox.GetText();
			if (ControlHandler.mgr.GetCtrlType() == 1)
			{
				DreamWorld.env.DialogBox.SetText(tempString1.Replace("[]", "A"));
			}
			else if (ControlHandler.mgr.GetCtrlType() == 2)
			{
				DreamWorld.env.DialogBox.SetText(tempString1.Replace("[]", "X"));
			}
			else
			{
				DreamWorld.env.DialogBox.SetText(tempString1.Replace("[]", "SPACE"));
			}
			break;
		case 2:
			TriggerSong();
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: false);
			if (!Interface.env.SideLabel.CheckIsActivated())
			{
				Interface.env.SideLabel.ActivateAsTutorial();
			}
			break;
		case 3:
			isTempMetronome = false;
			PauseTutorial();
			DreamWorld.env.DialogBox.SetDialogState(2);
			DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: true);
			break;
		case 4:
			DreamWorld.env.DialogBox.ChangeDialogState(3);
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: false);
			break;
		case 5:
			DreamWorld.env.DialogBox.ChangeDialogState(4);
			break;
		case 6:
			if (DreamWorld.env.DialogBox.CheckIsActivated())
			{
				if (SaveManager.GetLang() == 6)
				{
					DreamWorld.env.DialogBox.ChangeDialogState(5, 4.2f, 1);
				}
				else
				{
					DreamWorld.env.DialogBox.ChangeDialogState(5);
				}
				break;
			}
			if (SaveManager.GetLang() == 6)
			{
				DreamWorld.env.DialogBox.SetDialogState(5, 4.2f, 1);
			}
			else
			{
				DreamWorld.env.DialogBox.SetDialogState(5);
			}
			DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			break;
		case 7:
			MusicBox.env.SetTrack(1);
			ResumeTutorial();
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			break;
		case 8:
			PauseTutorial();
			DreamWorld.env.DialogBox.SetDialogState(6);
			DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: true);
			break;
		case 9:
			if (DreamWorld.env.DialogBox.CheckIsActivated())
			{
				DreamWorld.env.DialogBox.ChangeDialogState(7);
				DreamWorld.env.DialogBox.ToggleSubInfo(toggle: false);
			}
			else
			{
				DreamWorld.env.DialogBox.SetDialogState(7);
				DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			}
			break;
		case 10:
			MusicBox.env.SetTrack(2);
			ResumeTutorial();
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			break;
		case 11:
			PauseTutorial();
			if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(8, 4.2f, 1);
			}
			else
			{
				DreamWorld.env.DialogBox.SetDialogState(8);
			}
			tempString1 = DreamWorld.env.DialogBox.GetText();
			tempString2 = "";
			if (ControlHandler.mgr.GetCtrlType() == 1 || ControlHandler.mgr.GetCtrlType() == 2)
			{
				tempString2 = tempString1.Replace("[1]", "L");
				DreamWorld.env.DialogBox.SetText(tempString2.Replace("[2]", "R"));
			}
			else if (SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				tempString2 = tempString1.Replace("[1]", "A");
				DreamWorld.env.DialogBox.SetText(tempString2.Replace("[2]", "D"));
			}
			else
			{
				tempString2 = tempString1.Replace("[1]", "LEFT");
				DreamWorld.env.DialogBox.SetText(tempString2.Replace("[2]", "RIGHT"));
			}
			DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: true);
			break;
		case 12:
			MusicBox.env.SetTrack(3);
			ResumeTutorial();
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: false);
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			break;
		case 13:
			PauseTutorial();
			if (SaveManager.GetLang() == 3)
			{
				DreamWorld.env.DialogBox.SetDialogState(9, 4.2f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				DreamWorld.env.DialogBox.SetDialogState(9, 4.2f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.SetDialogState(9, 4.2f, 1);
			}
			else if (SaveManager.GetLang() == 8)
			{
				DreamWorld.env.DialogBox.SetDialogState(9, 4.2f, 1);
			}
			else
			{
				DreamWorld.env.DialogBox.SetDialogState(9);
			}
			tempString1 = DreamWorld.env.DialogBox.GetText();
			if (ControlHandler.mgr.GetCtrlType() == 1)
			{
				DreamWorld.env.DialogBox.SetText(tempString1.Replace("[]", "A"));
			}
			else if (ControlHandler.mgr.GetCtrlType() == 2)
			{
				DreamWorld.env.DialogBox.SetText(tempString1.Replace("[]", "X"));
			}
			else
			{
				DreamWorld.env.DialogBox.SetText(tempString1.Replace("[]", "SPACE"));
			}
			DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: true);
			break;
		case 14:
			MusicBox.env.SetTrack(4);
			ResumeTutorial();
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: false);
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			break;
		case 15:
			if (!isMissed)
			{
				if (!SaveManager.mgr.CheckIsTp())
				{
					SaveManager.mgr.ToggleIsTp(toggle: true);
				}
				if (SteamManager.mgr != null)
				{
					SteamManager.mgr.RewardAchievement("honor_roll");
				}
			}
			PauseTutorial();
			DreamWorld.env.DialogBox.SetDialogState(10);
			DreamWorld.env.DialogBox.Activate(isSoundTriggered: false);
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: true);
			break;
		case 16:
			if (SaveManager.GetLang() == 6)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(11, 4.2f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(11, 4.2f, 1);
			}
			else
			{
				DreamWorld.env.DialogBox.ChangeDialogState(11);
			}
			DreamWorld.env.DialogBox.ToggleSubInfo(toggle: false);
			break;
		case 17:
			if (SaveManager.GetLang() == 7)
			{
				DreamWorld.env.DialogBox.ChangeDialogState(12, 4.2f, 1);
			}
			else
			{
				DreamWorld.env.DialogBox.ChangeDialogState(12);
			}
			break;
		case 18:
			DreamWorld.env.DialogBox.ChangeDialogState(13);
			if (SaveManager.mgr.GetChapterNum() == -1)
			{
				SaveManager.mgr.SetChapterNum(0);
			}
			break;
		case 19:
			Chapter.ToggleIsEnteringWithIntro(toggle: true);
			DreamWorld.env.DialogBox.Deactivate(isSoundTriggered: true);
			Interface.env.SideLabel.Deactivate();
			Interface.env.Circle.Deactivate();
			Interface.env.ExitTo("Chapter_1");
			break;
		}
	}

	private void CooldownState(float waitTime)
	{
		CancelCoroutine(cooldowningState);
		cooldowningState = StartCoroutine(CooldowningState(waitTime));
	}

	private IEnumerator CooldowningState(float waitTime)
	{
		isCooldowningState = true;
		yield return new WaitForSeconds(waitTime);
		isCooldowningState = false;
	}

	protected override void OnWindowEnd()
	{
		if (phrase == 2 && bar == 1 && beat == 4)
		{
			state++;
			TriggerState();
		}
		else if (phrase == 2 && bar == 5 && beat == 4)
		{
			state++;
			TriggerState();
		}
		else if (phrase == 3 && bar == 1 && beat == 4)
		{
			state++;
			TriggerState();
		}
		else if (phrase == 3 && bar == 5 && beat == 4)
		{
			state++;
			TriggerState();
		}
		else if (phrase == 4 && bar == 1 && beat == 4)
		{
			state++;
			TriggerState();
		}
	}

	protected override void OnSequence()
	{
		if (sequences[0] > 0f)
		{
			float num = sequences[0];
			if (num != 2f)
			{
				if (num == 4f)
				{
					QueueHitWindow(4);
				}
			}
			else
			{
				Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: true);
				Interface.env.Circle.SetSpawnEaseType(0);
				QueueHitWindow(4);
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
		if (sequences[1] > 0f)
		{
			if (sequences[1] == 1f)
			{
				Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: true);
				Interface.env.Circle.SetSpawnEaseType(1);
				SetCtrlMode(0);
				QueueHitWindow(3);
			}
			if (isHalfBeatEnabled)
			{
				sequences[1] = sequences[1] + 0.5f;
			}
			else
			{
				sequences[1] += 1f;
			}
			if (sequences[1] > 1f)
			{
				sequences[1] = 0f;
			}
		}
		if (sequences[2] > 0f)
		{
			float num = sequences[2];
			if (num != 1f)
			{
				if (num == 3f)
				{
					Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: false);
					QueueRightHitWindow(1);
				}
			}
			else
			{
				Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: false);
				Interface.env.Circle.SetSpawnEaseType(2);
				SetCtrlMode(2);
				QueueLeftHitWindow(1);
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
		if (sequences[3] > 0f)
		{
			if (sequences[3] == 1f)
			{
				Interface.env.Circle.ToggleisSpawnHalfDistance(toggle: false);
				Interface.env.Circle.SetSpawnEaseType(0);
				SetCtrlMode(1);
				QueueHoldReleaseWindow(2, 3);
			}
			if (isHalfBeatEnabled)
			{
				sequences[3] = sequences[3] + 0.5f;
			}
			else
			{
				sequences[3] += 1f;
			}
			if (sequences[3] > 1f)
			{
				sequences[3] = 0f;
			}
		}
	}

	protected override void OnEvent()
	{
		switch (eventNum)
		{
		case 0:
			Interface.env.Circle.ToggleisSpawnBlind(toggle: false);
			break;
		case 1:
			Interface.env.Circle.ToggleisSpawnBlind(toggle: true);
			break;
		}
	}

	protected override void OnPrep()
	{
		Interface.env.Cam.Breeze();
		TutorialWorld.env.PlayActionSound(3);
		if (accuracy != 1f)
		{
			isMissed = true;
		}
	}

	protected override void OnHit()
	{
		if (state == 2)
		{
			TutorialWorld.env.PlayActionSound(5);
		}
		else if (ctrlMode == 0)
		{
			TutorialWorld.env.PlayActionSound(0);
		}
		else if (ctrlMode == 1)
		{
			TutorialWorld.env.PlayActionSound(4);
		}
		else if (beat == 2)
		{
			TutorialWorld.env.PlayActionSound(1);
		}
		else
		{
			TutorialWorld.env.PlayActionSound(2);
		}
		Interface.env.Cam.Breeze();
		if (accuracy != 1f)
		{
			isMissed = true;
		}
	}

	protected override void OnMiss()
	{
		TutorialWorld.env.Sweat.CrossIn();
		if (accuracy != 1f)
		{
			isMissed = true;
		}
	}
}
