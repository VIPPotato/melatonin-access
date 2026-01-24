using System.Collections;
using UnityEngine;

public class Option : Wrapper
{
	[Header("Fragments")]
	public textboxFragment label;

	public textboxFragment num;

	public textboxFragment tip;

	public spriteFragment lightSwitch;

	public spriteFragment tooltip;

	[Header("Props")]
	public int chapterNum;

	private bool isActivated;

	private bool isEnabled;

	private int functionType;

	private int functionNum;

	private float timeTilDeactivate;

	private Coroutine deactivating;

	private Coroutine changingOffset;

	protected override void Awake()
	{
		label.Initiate();
		label.ToggleIsRealTimeFader(toggle: true);
		num.Initiate();
		num.ToggleIsRealTimeFader(toggle: true);
		tip.Initiate();
		tip.ToggleIsRealTimeFader(toggle: true);
		lightSwitch.Initiate();
		lightSwitch.ToggleIsRealTimeFader(toggle: true);
		tooltip.Initiate();
		tooltip.ToggleIsRealTimeFader(toggle: true);
		RenderChildren(toggle: false);
	}

	public void Activate()
	{
		if (isActivated)
		{
			return;
		}
		CancelCoroutine(deactivating);
		CancelCoroutine(changingOffset);
		isActivated = true;
		isEnabled = false;
		RenderChildren(toggle: true);
		label.FadeInText(0.6f, 0.33f);
		label.SetHorizontalAlignment(1);
		label.SetFontSize(4.8f);
		label.SetLetterSpacing(0.7f);
		num.ToggleMeshRenderer(toggle: false);
		tip.ToggleMeshRenderer(toggle: false);
		tip.SetFontSize(3.2f);
		tip.SetLineSpacing(12f);
		lightSwitch.ToggleSpriteRenderer(toggle: false);
		tooltip.ToggleSpriteRenderer(toggle: false);
		switch (functionNum)
		{
		case 0:
			if (!Builder.mgr.CheckIsFullGame())
			{
				label.SetStateByName("demo");
			}
			else if (SaveManager.mgr.GetChapterNum() == -1)
			{
				label.SetStateByName("newGame");
			}
			else
			{
				label.SetStateByName("continue");
			}
			break;
		case 1:
			label.SetStateByName("resume");
			break;
		case 2:
			label.SetStateByName("practice");
			break;
		case 3:
			label.SetStateByName("back");
			break;
		case 4:
			label.SetStateByName("chapterSelect");
			break;
		case 5:
			label.SetStateByName("settings");
			break;
		case 6:
			label.SetStateByName("metronome");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 3)
			{
				label.SetFontSize(4.6f);
			}
			else if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4.4f);
			}
			num.FadeInText(0.6f, 0.33f);
			num.SetText("< " + SaveManager.mgr.GetMetronome() + " >");
			break;
		case 7:
			label.SetStateByName("restart");
			break;
		case 8:
			label.SetStateByName("exitDream");
			break;
		case 9:
			if (SaveManager.mgr.GetChapterNum() >= chapterNum)
			{
				label.SetStateByName("chapter" + chapterNum);
			}
			else
			{
				label.SetStateByName("redacted");
			}
			break;
		case 10:
			label.SetStateByName("mainMenu");
			break;
		case 11:
			label.SetStateByName("credits");
			break;
		case 12:
			label.SetStateByName("fullscreen");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 3)
			{
				label.SetFontSize(4.4f);
			}
			else if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(3.6f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			break;
		case 13:
			label.SetStateByName("resolution");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			num.FadeInText(0.6f, 0.33f);
			break;
		case 14:
			label.SetStateByName("volume");
			label.SetHorizontalAlignment(0);
			num.FadeInText(0.6f, 0.33f);
			num.SetText("< " + SaveManager.mgr.GetMaster() + " >");
			break;
		case 15:
			label.SetStateByName("quit");
			break;
		case 16:
			label.SetStateByName("proceed");
			break;
		case 17:
			label.SetStateByName("assistance");
			break;
		case 18:
			label.SetStateByName("visualAssist");
			label.SetHorizontalAlignment(0);
			tip.FadeInText(0.6f, 0.33f);
			tip.SetStateByName("visualAssist");
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				label.SetFontSize(3.8f);
				label.SetLetterSpacing(-0.7f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.5f);
				label.SetLetterSpacing(-1.2f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (!SaveManager.mgr.CheckIsVisualAssisting())
			{
				lightSwitch.SetState(0);
			}
			else
			{
				lightSwitch.SetState(1);
			}
			tooltip.FadeInSprite(1f, 0.33f);
			break;
		case 19:
			label.SetStateByName("audioAssist");
			label.SetHorizontalAlignment(0);
			tip.FadeInText(0.6f, 0.33f);
			tip.SetStateByName("audioAssist");
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				label.SetFontSize(3.8f);
				label.SetLetterSpacing(-0.7f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(4.6f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.5f);
				label.SetLetterSpacing(-1.2f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (!SaveManager.mgr.CheckIsAudioAssisting())
			{
				lightSwitch.SetState(0);
			}
			else
			{
				lightSwitch.SetState(1);
			}
			tooltip.FadeInSprite(1f, 0.33f);
			break;
		case 20:
			label.SetStateByName("calibration");
			break;
		case 21:
		{
			int screenshake = SaveManager.mgr.GetScreenshake();
			label.SetStateByName("screenShake");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				label.SetFontSize(3.6f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				label.SetFontSize(3.6f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(3.3f);
				label.SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(4.2f);
			}
			num.FadeInText(0.6f, 0.33f);
			switch (screenshake)
			{
			case 0:
				num.SetText("0%");
				break;
			case 1:
				num.SetText("50%");
				break;
			case 2:
				num.SetText("100%");
				break;
			}
			break;
		}
		case 22:
			label.SetStateByName("display");
			break;
		case 23:
			label.SetStateByName("audio");
			break;
		case 24:
			label.SetStateByName("music");
			label.SetHorizontalAlignment(0);
			num.FadeInText(0.6f, 0.33f);
			num.SetText("< " + SaveManager.mgr.GetMusic() + " >");
			break;
		case 25:
			label.SetStateByName("sfx");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(3.4f);
				label.SetLetterSpacing(-1f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(3.3f);
				label.SetLetterSpacing(-1f);
			}
			num.FadeInText(0.6f, 0.33f);
			num.SetText("< " + SaveManager.mgr.GetSfx() + " >");
			break;
		case 26:
			label.SetStateByName("offset");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(4.4f);
			}
			num.FadeInText(0.6f, 0.33f);
			break;
		case 27:
			if (SaveManager.mgr.GetChapterNum() == -1)
			{
				label.SetStateByName("redacted");
			}
			else
			{
				label.SetStateByName("tutorial");
			}
			break;
		case 28:
			label.SetStateByName("skipCutscene");
			if (SaveManager.GetLang() == 7)
			{
				label.SetFontSize(4f);
			}
			break;
		case 29:
			label.SetStateByName("skipTutorial");
			break;
		case 30:
			label.SetStateByName("calibrationTool");
			if (TitleScreen.dir == null)
			{
				tip.FadeInText(0.6f, 0.33f);
				tip.SetStateByName("calibrationTool");
				if (SaveManager.GetLang() == 5)
				{
					label.SetFontSize(4f);
				}
				else if (SaveManager.GetLang() == 7)
				{
					label.SetFontSize(3.6f);
				}
				else if (SaveManager.GetLang() == 8)
				{
					label.SetFontSize(3.2f);
				}
				else if (SaveManager.GetLang() == 9)
				{
					label.SetFontSize(3.4f);
				}
				tooltip.FadeInSprite(1f, 0.33f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(4f);
			}
			break;
		case 31:
			if (SaveManager.mgr.GetChapterNum() < 5)
			{
				label.SetStateByName("redacted");
			}
			else
			{
				label.SetStateByName("epilogue");
			}
			break;
		case 32:
			label.SetStateByName("start");
			break;
		case 33:
			label.SetStateByName("rewatchIntro");
			if (SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.6f);
				label.SetLetterSpacing(0f);
			}
			break;
		case 34:
			if (chapterNum == 5 && SaveManager.mgr.CheckIsGameComplete())
			{
				label.SetStateByName("rewatchOutro");
			}
			else if (SaveManager.mgr.GetChapterNum() > chapterNum)
			{
				label.SetStateByName("rewatchOutro");
			}
			else
			{
				label.SetStateByName("redacted");
			}
			if (SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.6f);
				label.SetLetterSpacing(0f);
			}
			break;
		case 35:
			label.SetStateByName("keyboard");
			break;
		case 36:
			label.SetStateByName("actionKey");
			break;
		case 37:
			label.SetStateByName("wasd");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 2)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 3)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4.4f);
				label.SetLetterSpacing(0f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				label.SetFontSize(3.5f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (!SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				lightSwitch.SetState(0);
			}
			else
			{
				lightSwitch.SetState(1);
			}
			break;
		case 38:
			label.SetStateByName("re-edit");
			break;
		case 39:
			label.SetStateByName("contrast");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			num.FadeInText(0.6f, 0.33f);
			num.SetText("< " + SaveManager.mgr.GetContrast() + " >");
			break;
		case 40:
			label.SetStateByName("confirm");
			break;
		case 41:
			label.SetStateByName("vibration");
			label.SetHorizontalAlignment(0);
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (!SaveManager.mgr.CheckIsVibrationDisabled())
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			break;
		case 42:
			label.SetStateByName("bigHitWindows");
			label.SetHorizontalAlignment(0);
			tip.FadeInText(0.6f, 0.33f);
			tip.SetStateByName("bigHitWindows");
			if (SaveManager.GetLang() == 3)
			{
				label.SetFontSize(4.4f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				label.SetFontSize(3.6f);
				label.SetLetterSpacing(-0.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(4.2f);
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.3f);
				label.SetLetterSpacing(-1.2f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (SaveManager.mgr.CheckIsBiggerHitWindows())
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			tooltip.FadeInSprite(1f, 0.33f);
			break;
		case 43:
			label.SetStateByName("achievements");
			break;
		case 44:
			label.SetStateByName("warmth");
			label.SetHorizontalAlignment(0);
			tip.FadeInText(0.6f, 0.33f);
			tip.SetStateByName("warmth");
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (SaveManager.mgr.CheckIsWarmth())
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			tooltip.FadeInSprite(1f, 0.33f);
			break;
		case 45:
			label.SetStateByName("mild");
			label.SetHorizontalAlignment(0);
			tip.FadeInText(0.6f, 0.33f);
			tip.SetStateByName("mild");
			if (SaveManager.GetLang() == 3)
			{
				label.SetFontSize(4.4f);
			}
			else if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				label.SetFontSize(3.8f);
				label.SetLetterSpacing(-0.7f);
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				label.SetFontSize(3.6f);
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.5f);
				label.SetLetterSpacing(-1.2f);
				tip.SetFontSize(2.8f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (SaveManager.mgr.CheckIsEasyScoring())
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			tooltip.FadeInSprite(1f, 0.33f);
			break;
		case 46:
			label.SetStateByName("gameplay");
			break;
		case 47:
			label.SetStateByName("perfectsOnly");
			label.SetHorizontalAlignment(0);
			tip.FadeInText(0.6f, 0.33f);
			tip.SetStateByName("perfectsOnly");
			if (SaveManager.GetLang() == 3)
			{
				label.SetFontSize(4f);
				tip.SetFontSize(2.8f);
			}
			else if (SaveManager.GetLang() == 5)
			{
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 6)
			{
				label.SetFontSize(3.5f);
				label.SetLetterSpacing(-0.7f);
				tip.SetFontSize(2.8f);
				tip.SetLineSpacing(-11f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				label.SetFontSize(3.4f);
				tip.SetFontSize(2.8f);
				tip.SetLineSpacing(-11f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				label.SetFontSize(4.6f);
				tip.SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 9)
			{
				label.SetFontSize(3.8f);
				label.SetLetterSpacing(-0.7f);
				tip.SetFontSize(3f);
				tip.SetLineSpacing(-11f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (SaveManager.mgr.CheckIsPerfectsOnly())
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			tooltip.FadeInSprite(1f, 0.33f);
			break;
		case 48:
			label.SetStateByName("extras");
			break;
		case 49:
			label.SetStateByName("community");
			break;
		case 50:
			label.SetStateByName("vsync");
			label.SetHorizontalAlignment(0);
			if (SaveManager.GetLang() == 5)
			{
				label.SetFontSize(4f);
			}
			lightSwitch.FadeInSprite(0.6f, 0.33f);
			if (SaveManager.mgr.CheckIsVsynced())
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			break;
		case 51:
			label.SetStateByName("discord");
			break;
		case 52:
			label.SetStateByName("bluesky");
			break;
		}
	}

	public void Deactivate()
	{
		if (isActivated)
		{
			CancelCoroutine(deactivating);
			CancelCoroutine(changingOffset);
			deactivating = StartCoroutine(Deactivating());
		}
	}

	private IEnumerator Deactivating()
	{
		isActivated = false;
		if (isEnabled)
		{
			Disable();
		}
		label.FadeOutText(0.6f, 0.33f);
		if (num.CheckIsMeshRendered())
		{
			num.FadeOutText(0.6f, 0.33f);
		}
		if (tip.CheckIsMeshRendered())
		{
			tip.FadeOutText(0.6f, 0.33f);
		}
		if (lightSwitch.CheckIsSpriteRendered())
		{
			lightSwitch.FadeOutSprite(0.6f, 0.33f);
		}
		if (tooltip.CheckIsSpriteRendered())
		{
			tooltip.FadeOutSprite(1f, 0.33f);
		}
		yield return new WaitForSeconds(0.33f);
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (isEnabled && CheckIsDirectional())
		{
			if (ControlHandler.mgr.CheckIsRightPressed() || ControlHandler.mgr.CheckIsActionRightPressed())
			{
				Select();
			}
			else if (ControlHandler.mgr.CheckIsLeftPressed() || ControlHandler.mgr.CheckIsActionLeftPressed())
			{
				Reverse();
			}
		}
		if (isActivated && CheckIsRefreshable())
		{
			Refresh();
		}
	}

	public void Enable()
	{
		isEnabled = true;
		label.SetFontAlpha(1f);
		if (num.CheckIsMeshRendered())
		{
			num.SetFontAlpha(1f);
		}
		if (tip.CheckIsMeshRendered())
		{
			tip.SetFontAlpha(1f);
		}
		if (lightSwitch.CheckIsSpriteRendered())
		{
			lightSwitch.SetSpriteAlpha(1f);
		}
	}

	public void Disable()
	{
		isEnabled = false;
		label.SetFontAlpha(0.6f);
		if (num.CheckIsMeshRendered())
		{
			num.SetFontAlpha(0.6f);
		}
		if (tip.CheckIsMeshRendered())
		{
			tip.SetFontAlpha(0.6f);
		}
		if (lightSwitch.CheckIsSpriteRendered())
		{
			lightSwitch.SetSpriteAlpha(0.6f);
		}
	}

	public void Select()
	{
		switch (functionNum)
		{
		case 0:
			if (SaveManager.mgr.GetChapterNum() == -1)
			{
				Interface.env.Submenu.SetWarningMenu();
			}
			else if (SaveManager.mgr.GetChapterNum() == 0)
			{
				Interface.env.ExitTo("Chapter_1");
			}
			else if (!Builder.mgr.CheckIsFullGame())
			{
				Interface.env.ExitTo("Chapter_1");
			}
			else if (SaveManager.mgr.GetChapterNum() >= 1)
			{
				Interface.env.ExitTo("Chapter_" + SaveManager.mgr.GetChapterNum());
			}
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 1:
			Interface.env.CloseSubmenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 2:
			Dream.SetGameMode(0);
			Interface.env.Letterbox.Activate();
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.Fader.SetColor(1);
			Interface.env.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
			break;
		case 3:
			Interface.env.Submenu.SetPreviousMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 4:
			Interface.env.Submenu.SetChaptersMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 5:
			Interface.env.Submenu.SetSettingsMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 6:
			Technician.mgr.IncreaseVolume(3);
			this.num.SetText("< " + SaveManager.mgr.GetMetronome() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 7:
			Interface.env.Letterbox.Activate();
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
			if (Dream.dir.GetGameMode() != 5)
			{
				Interface.env.Fader.SetColor(1);
			}
			break;
		case 8:
			Interface.env.Letterbox.Activate();
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("Chapter_" + Chapter.GetActiveChapterNum());
			Interface.env.Fader.SetColor(1);
			break;
		case 9:
			if (SaveManager.mgr.GetChapterNum() >= chapterNum)
			{
				Interface.env.Submenu.SetChapterMenu(chapterNum);
				Interface.env.Submenu.PlaySfx(1);
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 10:
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("TitleScreen");
			break;
		case 11:
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("Credits");
			break;
		case 12:
			Technician.mgr.SwitchFullscreenMode();
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 13:
			Technician.mgr.IncreaseResolution();
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 14:
			Technician.mgr.IncreaseVolume(0);
			this.num.SetText("< " + SaveManager.mgr.GetMaster() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 15:
			Interface.env.Submenu.SetQuitMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 16:
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("Dream_tutorial");
			Dream.SetGameMode(5);
			break;
		case 17:
			Interface.env.Submenu.SetAccessibilityMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 18:
			if (!SaveManager.mgr.CheckIsVisualAssisting())
			{
				SaveManager.mgr.ToggleIsVisualAssisting(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsVisualAssisting(toggle: true);
					if (!Interface.env.Circle.CheckIsActivated() && Dream.dir.GetGameMode() != 5)
					{
						Interface.env.Circle.Activate();
					}
				}
				break;
			}
			SaveManager.mgr.ToggleIsVisualAssisting(toggle: false);
			lightSwitch.SetState(0);
			Interface.env.Submenu.PlaySfx(0);
			if (Dream.dir != null)
			{
				Dream.dir.ToggleIsVisualAssisting(toggle: false);
				if (Interface.env.Circle.CheckIsActivated() && Dream.dir.GetGameMode() != 5 && Dream.dir.GetGameMode() != 0)
				{
					Interface.env.Circle.Deactivate();
				}
			}
			break;
		case 19:
			if (!SaveManager.mgr.CheckIsAudioAssisting())
			{
				SaveManager.mgr.ToggleAudioAssisting(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsAudioAssisting(toggle: true);
				}
			}
			else
			{
				SaveManager.mgr.ToggleAudioAssisting(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsAudioAssisting(toggle: false);
				}
			}
			break;
		case 20:
			Interface.env.Submenu.SetCalibrationMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 21:
		{
			int num = SaveManager.mgr.GetScreenshake() + 1;
			if (num > 2)
			{
				num = 0;
				this.num.SetText("0%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
			}
			switch (num)
			{
			case 0:
				this.num.SetText("0%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
				break;
			case 1:
				this.num.SetText("50%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
				break;
			case 2:
				this.num.SetText("100%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
				break;
			}
			break;
		}
		case 22:
			Interface.env.Submenu.SetDisplayMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 23:
			Interface.env.Submenu.SetAudioMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 24:
			Technician.mgr.IncreaseVolume(1);
			this.num.SetText("< " + SaveManager.mgr.GetMusic() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 25:
			Technician.mgr.IncreaseVolume(2);
			this.num.SetText("< " + SaveManager.mgr.GetSfx() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 26:
			if (SaveManager.mgr.GetCalibrationOffsetMs() < 50)
			{
				IncreaseOffset();
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 27:
			if (SaveManager.mgr.GetChapterNum() >= 0)
			{
				Interface.env.Submenu.PlaySfx(1);
				Interface.env.ExitTo("Dream_tutorial");
				Dream.SetGameMode(5);
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 28:
			if ((bool)Chapter.dir)
			{
				if (Chapter.dir.CheckIsCutsceneIntro())
				{
					Interface.env.ExitTo("Chapter_" + Chapter.GetActiveChapterNum());
					Chapter.dir.SaveChapter();
				}
				else if (Chapter.dir.CheckIsCutsceneOutro())
				{
					if (Chapter.GetActiveChapterNum() == 1 || Chapter.GetActiveChapterNum() == 2 || Chapter.GetActiveChapterNum() == 3 || Chapter.GetActiveChapterNum() == 4)
					{
						Interface.env.ExitTo("Chapter_" + (Chapter.GetActiveChapterNum() + 1));
					}
					else if (Chapter.GetActiveChapterNum() == 5)
					{
						SaveManager.mgr.ToggleIsGameComplete(toggle: true);
						Interface.env.ExitTo("Credits");
					}
				}
			}
			else if ((bool)Creditor.dir)
			{
				Interface.env.ExitTo("TitleScreen");
			}
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 29:
			Chapter.ToggleIsEnteringWithIntro(toggle: true);
			if (SaveManager.mgr.GetChapterNum() == -1)
			{
				SaveManager.mgr.SetChapterNum(0);
			}
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("Chapter_1");
			break;
		case 30:
			if (TitleScreen.dir != null)
			{
				CalibrationTool.env.Activate();
				Interface.env.Submenu.PlaySfx(1);
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 31:
			if (SaveManager.mgr.GetChapterNum() >= chapterNum)
			{
				Interface.env.Submenu.SetChapterMenu(chapterNum);
				Interface.env.Submenu.PlaySfx(1);
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 32:
			Chapter.ClearAllCache();
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("Chapter_" + chapterNum);
			break;
		case 33:
			Chapter.ToggleIsEnteringWithIntro(toggle: true);
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo("Chapter_" + chapterNum);
			break;
		case 34:
			if (chapterNum == 5 && SaveManager.mgr.CheckIsGameComplete())
			{
				Chapter.ToggleIsEnteringWithOutro(toggle: true);
				Interface.env.Submenu.PlaySfx(1);
				Interface.env.ExitTo("Chapter_" + chapterNum);
			}
			else if (SaveManager.mgr.GetChapterNum() > chapterNum)
			{
				Chapter.ToggleIsEnteringWithOutro(toggle: true);
				Interface.env.Submenu.PlaySfx(1);
				Interface.env.ExitTo("Chapter_" + chapterNum);
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 35:
			Interface.env.Submenu.SetKeyboardMenu();
			Interface.env.Submenu.KeyboardDisplay.Activate();
			break;
		case 36:
			ControlHandler.mgr.StartRebind();
			break;
		case 37:
			Interface.env.Submenu.PlaySfx(0);
			if (SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				Interface.env.Submenu.KeyboardDisplay.ToggleIsDirectionKeysAlt(toggle: false);
				ControlHandler.mgr.ToggleDirectionKeysAlt(toggle: false);
				SaveManager.mgr.ToggleIsDirectionKeysAlt(toggle: false);
				lightSwitch.SetState(0);
			}
			else
			{
				Interface.env.Submenu.KeyboardDisplay.ToggleIsDirectionKeysAlt(toggle: true);
				ControlHandler.mgr.ToggleDirectionKeysAlt(toggle: true);
				SaveManager.mgr.ToggleIsDirectionKeysAlt(toggle: true);
				lightSwitch.SetState(1);
			}
			break;
		case 38:
			Interface.env.Letterbox.Activate();
			Interface.env.Submenu.PlaySfx(1);
			Interface.env.ExitTo(LvlEditor.GetEditorName());
			break;
		case 39:
			Interface.env.IncreaseContrast();
			this.num.SetText("< " + SaveManager.mgr.GetContrast() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 40:
			Interface.env.ExitGame();
			break;
		case 41:
			if (!SaveManager.mgr.CheckIsVibrationDisabled())
			{
				SaveManager.mgr.ToggleIsVibrationDisabled(toggle: true);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
			}
			else
			{
				SaveManager.mgr.ToggleIsVibrationDisabled(toggle: false);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
			}
			break;
		case 42:
			if (SaveManager.mgr.CheckIsBiggerHitWindows())
			{
				SaveManager.mgr.ToggleIsBiggerHitWindows(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsBiggerHitWindows(toggle: false);
				}
			}
			else
			{
				SaveManager.mgr.ToggleIsBiggerHitWindows(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsBiggerHitWindows(toggle: true);
				}
			}
			break;
		case 43:
			TitleWorld.env.AchievementsMenu.Activate();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 44:
			if (SaveManager.mgr.CheckIsWarmth())
			{
				SaveManager.mgr.ToggleIsWarmth(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				Interface.env.ToggleWarmth(toggle: false);
			}
			else
			{
				SaveManager.mgr.ToggleIsWarmth(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				Interface.env.ToggleWarmth(toggle: true);
			}
			break;
		case 45:
			if (SaveManager.mgr.CheckIsEasyScoring())
			{
				SaveManager.mgr.ToggleIsEasyScoring(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsEasyScoring(toggle: false);
				}
			}
			else
			{
				SaveManager.mgr.ToggleIsEasyScoring(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsEasyScoring(toggle: true);
				}
			}
			break;
		case 46:
			Interface.env.Submenu.SetGameplayMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 47:
			if (SaveManager.mgr.CheckIsPerfectsOnly())
			{
				SaveManager.mgr.ToggleIsPerfectsOnly(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsPerfectsOnly(toggle: false);
				}
			}
			else
			{
				SaveManager.mgr.ToggleIsPerfectsOnly(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsPerfectsOnly(toggle: true);
				}
			}
			break;
		case 48:
			Interface.env.Submenu.SetExtrasMenu();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 49:
			Interface.env.CommunityMenu.Activate();
			Interface.env.Submenu.PlaySfx(1);
			break;
		case 50:
			if (SaveManager.mgr.CheckIsVsynced())
			{
				SaveManager.mgr.ToggleIsVsynced(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
			}
			else
			{
				SaveManager.mgr.ToggleIsVsynced(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
			}
			break;
		case 51:
			SteamWorkshop.mgr.OpenDiscord();
			break;
		case 52:
			SteamWorkshop.mgr.OpenBluesky();
			break;
		}
	}

	public void Reverse()
	{
		switch (functionNum)
		{
		case 6:
			Technician.mgr.DecreaseVolume(3);
			this.num.SetText("< " + SaveManager.mgr.GetMetronome() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 12:
			Technician.mgr.SwitchFullscreenMode();
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 13:
			Technician.mgr.DecreaseResolution();
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 14:
			Technician.mgr.DecreaseVolume(0);
			this.num.SetText("< " + SaveManager.mgr.GetMaster() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 18:
			if (!SaveManager.mgr.CheckIsVisualAssisting())
			{
				SaveManager.mgr.ToggleIsVisualAssisting(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsVisualAssisting(toggle: true);
					if (!Interface.env.Circle.CheckIsActivated() && Dream.dir.GetGameMode() != 5)
					{
						Interface.env.Circle.Activate();
					}
				}
				break;
			}
			SaveManager.mgr.ToggleIsVisualAssisting(toggle: false);
			lightSwitch.SetState(0);
			Interface.env.Submenu.PlaySfx(0);
			if (Dream.dir != null)
			{
				Dream.dir.ToggleIsVisualAssisting(toggle: false);
				if (Interface.env.Circle.CheckIsActivated() && Dream.dir.GetGameMode() != 5 && Dream.dir.GetGameMode() != 0)
				{
					Interface.env.Circle.Deactivate();
				}
			}
			break;
		case 19:
			if (!SaveManager.mgr.CheckIsAudioAssisting())
			{
				SaveManager.mgr.ToggleAudioAssisting(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsAudioAssisting(toggle: true);
				}
			}
			else
			{
				SaveManager.mgr.ToggleAudioAssisting(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsAudioAssisting(toggle: false);
				}
			}
			break;
		case 21:
		{
			int num = SaveManager.mgr.GetScreenshake() - 1;
			if (num < 0)
			{
				num = 2;
				this.num.SetText("100%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
			}
			switch (num)
			{
			case 0:
				this.num.SetText("0%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
				break;
			case 1:
				this.num.SetText("50%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
				break;
			case 2:
				this.num.SetText("100%");
				Interface.env.Submenu.PlaySfx(0);
				SaveManager.mgr.SetScreenshake(num);
				break;
			}
			break;
		}
		case 24:
			Technician.mgr.DecreaseVolume(1);
			this.num.SetText("< " + SaveManager.mgr.GetMusic() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 25:
			Technician.mgr.DecreaseVolume(2);
			this.num.SetText("< " + SaveManager.mgr.GetSfx() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 26:
			if ((float)SaveManager.mgr.GetCalibrationOffsetMs() > -50f)
			{
				DecreaseOffset();
			}
			else
			{
				Interface.env.Submenu.PlaySfx(2);
			}
			break;
		case 37:
			Interface.env.Submenu.PlaySfx(0);
			if (SaveManager.mgr.CheckIsDirectionKeysAlt())
			{
				Interface.env.Submenu.KeyboardDisplay.ToggleIsDirectionKeysAlt(toggle: false);
				ControlHandler.mgr.ToggleDirectionKeysAlt(toggle: false);
				SaveManager.mgr.ToggleIsDirectionKeysAlt(toggle: false);
				lightSwitch.SetState(0);
			}
			else
			{
				Interface.env.Submenu.KeyboardDisplay.ToggleIsDirectionKeysAlt(toggle: true);
				ControlHandler.mgr.ToggleDirectionKeysAlt(toggle: true);
				SaveManager.mgr.ToggleIsDirectionKeysAlt(toggle: true);
				lightSwitch.SetState(1);
			}
			break;
		case 39:
			Interface.env.DecreaseContrast();
			this.num.SetText("< " + SaveManager.mgr.GetContrast() + " >");
			Interface.env.Submenu.PlaySfx(0);
			break;
		case 41:
			if (!SaveManager.mgr.CheckIsVibrationDisabled())
			{
				SaveManager.mgr.ToggleIsVibrationDisabled(toggle: true);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
			}
			else
			{
				SaveManager.mgr.ToggleIsVibrationDisabled(toggle: false);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
			}
			break;
		case 42:
			if (SaveManager.mgr.CheckIsBiggerHitWindows())
			{
				SaveManager.mgr.ToggleIsBiggerHitWindows(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsBiggerHitWindows(toggle: false);
				}
			}
			else
			{
				SaveManager.mgr.ToggleIsBiggerHitWindows(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsBiggerHitWindows(toggle: true);
				}
			}
			break;
		case 44:
			if (SaveManager.mgr.CheckIsWarmth())
			{
				SaveManager.mgr.ToggleIsWarmth(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				Interface.env.ToggleWarmth(toggle: false);
			}
			else
			{
				SaveManager.mgr.ToggleIsWarmth(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				Interface.env.ToggleWarmth(toggle: true);
			}
			break;
		case 45:
			if (SaveManager.mgr.CheckIsEasyScoring())
			{
				SaveManager.mgr.ToggleIsEasyScoring(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsEasyScoring(toggle: false);
				}
			}
			else
			{
				SaveManager.mgr.ToggleIsEasyScoring(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsEasyScoring(toggle: true);
				}
			}
			break;
		case 47:
			if (SaveManager.mgr.CheckIsPerfectsOnly())
			{
				SaveManager.mgr.ToggleIsPerfectsOnly(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsPerfectsOnly(toggle: false);
				}
			}
			else
			{
				SaveManager.mgr.ToggleIsPerfectsOnly(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
				if (Dream.dir != null)
				{
					Dream.dir.ToggleIsPerfectsOnly(toggle: true);
				}
			}
			break;
		case 50:
			if (SaveManager.mgr.CheckIsVsynced())
			{
				SaveManager.mgr.ToggleIsVsynced(toggle: false);
				lightSwitch.SetState(0);
				Interface.env.Submenu.PlaySfx(0);
			}
			else
			{
				SaveManager.mgr.ToggleIsVsynced(toggle: true);
				lightSwitch.SetState(1);
				Interface.env.Submenu.PlaySfx(0);
			}
			break;
		}
	}

	public void Refresh()
	{
		switch (functionNum)
		{
		case 12:
			if (Screen.fullScreen)
			{
				lightSwitch.SetState(1);
			}
			else
			{
				lightSwitch.SetState(0);
			}
			break;
		case 13:
			num.SetText("< " + Screen.height + " >");
			break;
		case 26:
			num.SetText("< " + SaveManager.mgr.GetCalibrationOffsetMs() + " >");
			break;
		}
	}

	public void IncreaseOffset()
	{
		CancelCoroutine(changingOffset);
		changingOffset = StartCoroutine(IncreasingOffset());
	}

	private IEnumerator IncreasingOffset()
	{
		int num = SaveManager.mgr.GetCalibrationOffsetMs() + 1;
		SaveManager.mgr.SetCalibrationOffsetMs(num);
		if (Dream.dir != null)
		{
			Dream.dir.SetOffsetSeconds(num);
		}
		Interface.env.Submenu.PlaySfx(0);
		yield return new WaitForSeconds(0.5f);
		while (ControlHandler.mgr.CheckIsRightPressing() && SaveManager.mgr.GetCalibrationOffsetMs() < 50)
		{
			num = SaveManager.mgr.GetCalibrationOffsetMs() + 1;
			SaveManager.mgr.SetCalibrationOffsetMs(num);
			if (Dream.dir != null)
			{
				Dream.dir.SetOffsetSeconds(num);
			}
			Interface.env.Submenu.PlaySfx(0);
			yield return new WaitForSeconds(0.08f);
			yield return null;
		}
	}

	public void DecreaseOffset()
	{
		CancelCoroutine(changingOffset);
		changingOffset = StartCoroutine(DecreasingOffset());
	}

	private IEnumerator DecreasingOffset()
	{
		int num = SaveManager.mgr.GetCalibrationOffsetMs() - 1;
		SaveManager.mgr.SetCalibrationOffsetMs(num);
		if (Dream.dir != null)
		{
			Dream.dir.SetOffsetSeconds(num);
		}
		Interface.env.Submenu.PlaySfx(0);
		yield return new WaitForSeconds(0.5f);
		while (ControlHandler.mgr.CheckIsLeftPressing() && SaveManager.mgr.GetCalibrationOffsetMs() > -50)
		{
			num = SaveManager.mgr.GetCalibrationOffsetMs() - 1;
			SaveManager.mgr.SetCalibrationOffsetMs(num);
			if (Dream.dir != null)
			{
				Dream.dir.SetOffsetSeconds(num);
			}
			Interface.env.Submenu.PlaySfx(0);
			yield return new WaitForSeconds(0.08f);
			yield return null;
		}
	}

	public void SetFunction(int newFunctionType, int newFunctionNum)
	{
		functionType = newFunctionType;
		functionNum = newFunctionNum;
	}

	public void SetChapterNum(int newChapterNum)
	{
		chapterNum = newChapterNum;
	}

	private bool CheckIsDirectional()
	{
		if (functionNum == 6 || functionNum == 12 || functionNum == 13 || functionNum == 14 || functionNum == 18 || functionNum == 19 || functionNum == 21 || functionNum == 24 || functionNum == 25 || functionNum == 26 || functionNum == 37 || functionNum == 39 || functionNum == 41 || functionNum == 42 || functionNum == 44 || functionNum == 45 || functionNum == 47 || functionNum == 50)
		{
			return true;
		}
		return false;
	}

	private bool CheckIsRefreshable()
	{
		if (functionNum == 12 || functionNum == 13 || functionNum == 26)
		{
			return true;
		}
		return false;
	}

	public int GetFunctionType()
	{
		return functionType;
	}

	public bool CheckIsEnabled()
	{
		return isEnabled;
	}
}
