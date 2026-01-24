using System.Collections;
using System.Text;
using UnityEngine;

public class LvlEditor : Custom
{
	public static LvlEditor dir;

	protected string dreamName;

	public bool isRemix;

	public char initiatedDataType;

	public int barsLength;

	public string key;

	private int barsLengthOriginal;

	private int barsSkipped;

	private bool isEnabled;

	private bool isTimerMaxed;

	private float timer;

	private static string editorName;

	private static string downloadFilePath = "";

	private void Awake()
	{
		dir = this;
	}

	protected virtual void Start()
	{
		editorName = SceneMonitor.mgr.GetActiveSceneName();
		barsLengthOriginal = barsLength;
		Technician.mgr.SetAudioListener(1f);
		Technician.mgr.ToggleVsync(toggle: false);
		SaveManager.mgr.LoadEditorData(downloadFilePath);
		EditorUI.env.Activate(initiatedDataType);
		EditorUI.env.AdvancedMenu.Initiate();
		if (downloadFilePath != "")
		{
			EditorUI.env.AdvancedMenu.ToggleIsUpdateDisabled(toggle: true);
			Interface.env.Fader.SetParentAndReposition(Interface.env.Cam.GetSliderTransform());
			Interface.env.Fader.Show();
			ExitToDownloadedDream();
		}
		else
		{
			isEnabled = true;
			Interface.env.Letterbox.Show();
			Interface.env.Letterbox.DeactivateDelayed();
			Interface.env.FeatherBorder.Show();
			Interface.env.FeatherBorder.Deactivate();
			DreamWorld.env.Show(0);
		}
	}

	private void Update()
	{
		if (!isEnabled || !(Time.timeScale > 0f))
		{
			return;
		}
		if (EditorUI.env.GetActiveCustomizeMenu().CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsLeftPressed())
			{
				if (EditorUI.env.GetActiveCustomizeMenu().CheckIsUsableMenu() || EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
				{
					EditorUI.env.PlayToggleSfx();
					EditorUI.env.GetActiveCustomizeMenu().HighlightPrevColumn();
				}
				else
				{
					EditorUI.env.PlayBlockedSfx();
				}
			}
			else if (ControlHandler.mgr.CheckIsRightPressed())
			{
				if (EditorUI.env.GetActiveCustomizeMenu().CheckIsUsableMenu() || EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
				{
					EditorUI.env.PlayToggleSfx();
					EditorUI.env.GetActiveCustomizeMenu().HighlightNextColumn();
				}
				else
				{
					EditorUI.env.PlayBlockedSfx();
				}
			}
			else if (ControlHandler.mgr.CheckIsUpPressed())
			{
				if (EditorUI.env.GetActiveCustomizeMenu().CheckIsUsableMenu() || EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
				{
					EditorUI.env.PlayToggleSfx();
					EditorUI.env.GetActiveCustomizeMenu().HighlightPrevRow();
				}
				else
				{
					EditorUI.env.PlayBlockedSfx();
				}
			}
			else if (ControlHandler.mgr.CheckIsDownPressed())
			{
				if (EditorUI.env.GetActiveCustomizeMenu().CheckIsUsableMenu() || EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
				{
					EditorUI.env.PlayToggleSfx();
					EditorUI.env.GetActiveCustomizeMenu().HighlightNextRow();
				}
				else
				{
					EditorUI.env.PlayBlockedSfx();
				}
			}
			else if (ControlHandler.mgr.CheckIsActionPressed())
			{
				if (EditorUI.env.GetActiveCustomizeMenu().CheckIsUsableMenu())
				{
					EditorUI.env.PlaySelectSfx();
					EditorUI.env.PlayOutSfx();
					EditorUI.env.Daw.SetCodeOnBeat(EditorUI.env.GetActiveCustomizeMenu().GetHighlightedCustomzieItem().GetCode(), EditorUI.env.GetActiveCustomizeMenu().GetHighlightedCustomzieItem().GetDataType());
					EditorUI.env.GetActiveCustomizeMenu().Deactivate();
					EditorUI.env.Fader.Deactivate();
					int phraseNum = EditorUI.env.Daw.GetPhraseNum();
					int barNum = EditorUI.env.Daw.GetBarNum();
					int beatNum = EditorUI.env.Daw.GetBeatNum();
					string codeOnBeat = EditorUI.env.Daw.GetCodeOnBeat();
					SaveManager.mgr.SetEditorDataCode(phraseNum, barNum, beatNum, codeOnBeat);
				}
				else
				{
					EditorUI.env.PlayBlockedSfx();
				}
			}
			else if (ControlHandler.mgr.CheckIsCancelPressed())
			{
				EditorUI.env.PlayOutSfx();
				EditorUI.env.GetActiveCustomizeMenu().Deactivate();
				EditorUI.env.Fader.Deactivate();
			}
			else if (ControlHandler.mgr.CheckIsSwapPressed() && EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
			{
				EditorUI.env.PlaySwitchSfx();
				EditorUI.env.GetActiveCustomizeMenu().NextList();
			}
			else if (ControlHandler.mgr.CheckIsActionRightPressed() && ControlHandler.mgr.GetCtrlType() > 0 && EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
			{
				EditorUI.env.PlaySwitchSfx();
				EditorUI.env.GetActiveCustomizeMenu().NextList();
			}
			else if (ControlHandler.mgr.CheckIsActionLeftPressed() && ControlHandler.mgr.GetCtrlType() > 0 && EditorUI.env.GetActiveCustomizeMenu().CheckIsPaginatedMenu())
			{
				EditorUI.env.PlaySwitchSfx();
				EditorUI.env.GetActiveCustomizeMenu().PrevList();
			}
			return;
		}
		if (EditorUI.env.ConfirmModal.CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsActionPressed())
			{
				SaveManager.mgr.ResetCustomCodedBars();
				EditorUI.env.Daw.Activate(isReactivated: true, initiatedDataType);
				EditorUI.env.ConfirmModal.Deactivate();
				EditorUI.env.Fader.Deactivate();
				EditorUI.env.PlayRemoveSfx();
			}
			else if (ControlHandler.mgr.CheckIsCancelPressed())
			{
				EditorUI.env.ConfirmModal.Deactivate();
				EditorUI.env.Fader.Deactivate();
				EditorUI.env.PlaySelectSfx();
			}
			return;
		}
		if (EditorUI.env.AdvancedMenu.CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsActionPressed())
			{
				if ((EditorUI.env.AdvancedMenu.GetTabNum() == 0 && (EditorUI.env.AdvancedMenu.GetRowNum() == 0 || EditorUI.env.AdvancedMenu.GetRowNum() == 4)) || EditorUI.env.AdvancedMenu.GetTabNum() == 1)
				{
					EditorUI.env.PlaySelectSfx();
				}
				EditorUI.env.AdvancedMenu.Select();
			}
			else if (ControlHandler.mgr.CheckIsUpPressed())
			{
				if ((EditorUI.env.AdvancedMenu.GetTabNum() == 0 && EditorUI.env.AdvancedMenu.CheckIsCustomized()) || EditorUI.env.AdvancedMenu.GetTabNum() == 1)
				{
					EditorUI.env.PlayToggleSfx();
				}
				EditorUI.env.AdvancedMenu.PrevRow();
			}
			else if (ControlHandler.mgr.CheckIsDownPressed())
			{
				if ((EditorUI.env.AdvancedMenu.GetTabNum() == 0 && EditorUI.env.AdvancedMenu.CheckIsCustomized()) || EditorUI.env.AdvancedMenu.GetTabNum() == 1)
				{
					EditorUI.env.PlayToggleSfx();
				}
				EditorUI.env.AdvancedMenu.NextRow();
			}
			else if (ControlHandler.mgr.CheckIsLeftPressed())
			{
				if (EditorUI.env.AdvancedMenu.GetTabNum() == 0 && (EditorUI.env.AdvancedMenu.GetRowNum() == 1 || EditorUI.env.AdvancedMenu.GetRowNum() == 2 || EditorUI.env.AdvancedMenu.GetRowNum() == 3))
				{
					EditorUI.env.PlayToggleSfx();
				}
				if (ControlHandler.mgr.GetKeyboard().shiftKey.isPressed)
				{
					EditorUI.env.AdvancedMenu.Diminish();
				}
				else
				{
					EditorUI.env.AdvancedMenu.Decrease();
				}
			}
			else if (ControlHandler.mgr.CheckIsRightPressed())
			{
				if (EditorUI.env.AdvancedMenu.GetTabNum() == 0 && (EditorUI.env.AdvancedMenu.GetRowNum() == 1 || EditorUI.env.AdvancedMenu.GetRowNum() == 2 || EditorUI.env.AdvancedMenu.GetRowNum() == 3))
				{
					EditorUI.env.PlayToggleSfx();
				}
				if (ControlHandler.mgr.GetKeyboard().shiftKey.isPressed)
				{
					EditorUI.env.AdvancedMenu.Increment();
				}
				else
				{
					EditorUI.env.AdvancedMenu.Increase();
				}
			}
			else if (ControlHandler.mgr.CheckIsCancelPressed())
			{
				EditorUI.env.PlayOutSfx();
				EditorUI.env.AdvancedMenu.Deactivate();
				EditorUI.env.Fader.Deactivate();
			}
			else if (ControlHandler.mgr.CheckIsSwapPressed() || ((ControlHandler.mgr.CheckIsActionLeftPressed() || ControlHandler.mgr.CheckIsActionRightPressed()) && ControlHandler.mgr.GetCtrlType() > 0))
			{
				EditorUI.env.PlayToggleSfx();
				EditorUI.env.AdvancedMenu.SwapTab();
			}
			return;
		}
		if (ControlHandler.mgr.CheckIsLeftPressed())
		{
			EditorUI.env.PlayToggleSfx();
			EditorUI.env.Daw.DecreaseBeat();
		}
		else if (ControlHandler.mgr.CheckIsRightPressed())
		{
			EditorUI.env.PlayToggleSfx();
			EditorUI.env.Daw.IncreaseBeat();
		}
		if (ControlHandler.mgr.CheckIsUpPressed())
		{
			EditorUI.env.PlayToggleSfx();
			EditorUI.env.Daw.IncreaseBar();
		}
		else if (ControlHandler.mgr.CheckIsDownPressed())
		{
			EditorUI.env.PlayToggleSfx();
			EditorUI.env.Daw.DecreaseBar();
		}
		else if (ControlHandler.mgr.CheckIsActionPressed())
		{
			if (EditorUI.env.Daw.CheckIsActiveBeatDenied() && EditorUI.env.Daw.TimelineTabs.GetCharType() != 't')
			{
				EditorUI.env.PlayBlockedSfx();
			}
			else
			{
				EditorUI.env.PlaySelectSfx();
				EditorUI.env.PlayInSfx();
				EditorUI.env.GetActiveCustomizeMenu().Activate();
				EditorUI.env.Fader.Activate();
			}
		}
		else if (ControlHandler.mgr.CheckIsRemovePressed())
		{
			EditorUI.env.PlayRemoveSfx();
			EditorUI.env.Daw.RemoveCodeOnBeat(EditorUI.env.Daw.TimelineTabs.GetCharType());
			int phraseNum2 = EditorUI.env.Daw.GetPhraseNum();
			int barNum2 = EditorUI.env.Daw.GetBarNum();
			int beatNum2 = EditorUI.env.Daw.GetBeatNum();
			string codeOnBeat2 = EditorUI.env.Daw.GetCodeOnBeat();
			SaveManager.mgr.SetEditorDataCode(phraseNum2, barNum2, beatNum2, codeOnBeat2);
		}
		else if (ControlHandler.mgr.CheckIsPlayReleased())
		{
			MusicBox.SetSkippedTime(0f);
			ExitToCustomizedDream();
		}
		else if (ControlHandler.mgr.CheckIsSwapPressed())
		{
			EditorUI.env.PlaySwitchSfx();
			EditorUI.env.Daw.TimelineTabs.NextTab();
			BarSlot[] barSlots = EditorUI.env.Daw.BarSlots;
			for (int i = 0; i < barSlots.Length; i++)
			{
				barSlots[i].SetThumbnails(EditorUI.env.Daw.TimelineTabs.GetCharType());
			}
		}
		else if (ControlHandler.mgr.CheckIsActionLeftPressed() && ControlHandler.mgr.GetCtrlType() > 0)
		{
			EditorUI.env.PlaySwitchSfx();
			EditorUI.env.Daw.TimelineTabs.PrevTab();
			BarSlot[] barSlots = EditorUI.env.Daw.BarSlots;
			for (int i = 0; i < barSlots.Length; i++)
			{
				barSlots[i].SetThumbnails(EditorUI.env.Daw.TimelineTabs.GetCharType());
			}
		}
		else if (ControlHandler.mgr.CheckIsActionRightPressed() && ControlHandler.mgr.GetCtrlType() > 0)
		{
			EditorUI.env.PlaySwitchSfx();
			EditorUI.env.Daw.TimelineTabs.NextTab();
			BarSlot[] barSlots = EditorUI.env.Daw.BarSlots;
			for (int i = 0; i < barSlots.Length; i++)
			{
				barSlots[i].SetThumbnails(EditorUI.env.Daw.TimelineTabs.GetCharType());
			}
		}
		else if (ControlHandler.mgr.CheckIsMorePressed())
		{
			EditorUI.env.PlaySelectSfx();
			EditorUI.env.PlayInSfx();
			EditorUI.env.Fader.Activate();
			EditorUI.env.AdvancedMenu.Activate();
		}
		if (ControlHandler.mgr.CheckIsRemovePressing())
		{
			if (!isTimerMaxed)
			{
				timer += Time.deltaTime;
				if (timer >= 1.5f)
				{
					isTimerMaxed = true;
					EditorUI.env.ConfirmModal.Activate();
					EditorUI.env.Fader.Activate();
					EditorUI.env.PlaySwitchSfx();
				}
			}
		}
		else if (ControlHandler.mgr.CheckIsPlayPressing())
		{
			if (isTimerMaxed)
			{
				return;
			}
			timer += Time.deltaTime;
			if (timer >= 1.5f)
			{
				isTimerMaxed = true;
				if (EditorUI.env.AdvancedMenu.CheckIsCustomized())
				{
					MusicBox.SetSkippedTime((float)EditorUI.env.Daw.GetActiveBarSlotNum() * (4f * (60f / SaveManager.mgr.GetEditorData().customSongTempo)));
				}
				else
				{
					MusicBox.SetSkippedTime((float)EditorUI.env.Daw.GetActiveBarSlotNum() * (4f * MusicBox.env.GetSecsPerBeat()));
				}
				barsSkipped = EditorUI.env.Daw.GetActiveBarSlotNum();
				ExitToCustomizedDream();
			}
		}
		else
		{
			isTimerMaxed = false;
			timer = 0f;
		}
	}

	private void StringifyEditorData()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 1 + barsSkipped; i <= barsLength; i++)
		{
			string[] editorDataBar = SaveManager.mgr.GetEditorDataBar(i);
			foreach (string text in editorDataBar)
			{
				stringBuilder.Append(text + " ");
			}
		}
		Dream.SetEditorDataString(stringBuilder.ToString());
	}

	private void ExitToCustomizedDream()
	{
		StartCoroutine(ExitingToCustomizedDream());
	}

	private IEnumerator ExitingToCustomizedDream()
	{
		isEnabled = false;
		EditorUI.env.PlaySelectSfx();
		Interface.env.Disable();
		Interface.env.Letterbox.Activate();
		Interface.env.FeatherBorder.Activate();
		SceneMonitor.mgr.PreloadScene(dreamName);
		DreamWorld.env.PlayTransitionSound();
		Dream.SetGameMode(6);
		StringifyEditorData();
		yield return new WaitForSeconds(0.72f);
		DreamWorld.env.TransitionToChapter();
		yield return new WaitForSecondsRealtime(0.75f);
		yield return new WaitUntil(() => !EditorUI.env.AdvancedMenu.CheckIsImporting());
		SceneMonitor.mgr.LoadScene();
	}

	private void ExitToDownloadedDream()
	{
		StartCoroutine(ExitingToDownloadedDream());
	}

	private IEnumerator ExitingToDownloadedDream()
	{
		isEnabled = false;
		Interface.env.Disable();
		SceneMonitor.mgr.PreloadScene(dreamName);
		Dream.SetGameMode(7);
		StringifyEditorData();
		yield return new WaitForSecondsRealtime(1f);
		yield return new WaitUntil(() => !EditorUI.env.AdvancedMenu.CheckIsImporting());
		downloadFilePath = "";
		SceneMonitor.mgr.LoadScene();
	}

	public void ToggleIsEnabled(bool toggle)
	{
		isEnabled = toggle;
	}

	public void SetBarsLength(int newBarsLength)
	{
		barsLength = newBarsLength;
		EditorUI.env.Daw.RefreshBars();
	}

	public void RevertBarsLength()
	{
		barsLength = barsLengthOriginal;
		EditorUI.env.Daw.RefreshBars();
	}

	public static void SetDownloadFilePath(string newDownloadedFilePath)
	{
		downloadFilePath = newDownloadedFilePath;
	}

	public bool CheckIsRemix()
	{
		return isRemix;
	}

	public int GetBarsLength()
	{
		return barsLength;
	}

	public string GetKey()
	{
		return key;
	}

	public static string GetEditorName()
	{
		return editorName;
	}

	public static string GetDownloadFilePath()
	{
		return downloadFilePath;
	}
}
