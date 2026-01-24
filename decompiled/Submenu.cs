using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submenu : Wrapper
{
	[Header("Children")]
	public RebindModal RebindModal;

	public ExtraMessage ExtraMessage;

	public List<Menu> Menus = new List<Menu>();

	public KeyboardDisplay KeyboardDisplay;

	public Fader Fader;

	[Header("Fragments")]
	public Fragment slider;

	public spriteFragment footer;

	private bool isActivated;

	private int menuType;

	private int menuDirection;

	private Coroutine deactivating;

	protected override void Awake()
	{
		slider.Awake();
		footer.Initiate();
		footer.ToggleIsRealTimeFader(toggle: true);
		SetupFragments();
		speakers[0].SetAllSoundIgnoreListenerPause(toggle: true);
		speakers[1].SetAllSoundIgnoreListenerPause(toggle: true);
		RenderChildren(toggle: false, 2);
	}

	public void Activate()
	{
		CancelCoroutine(deactivating);
		RenderChildren(toggle: true, 2);
		isActivated = true;
		menuType = 0;
		menuDirection = 0;
		SetMainMenu();
		menuDirection = 1;
		slider.TriggerAnim("enter");
		ExtraMessage.Hide();
		RebindModal.Hide();
		Menus[0].ResetActiveOption();
		Fader.Activate();
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
	}

	public void Deactivate()
	{
		CancelCoroutine(deactivating);
		deactivating = StartCoroutine(Deactivating());
	}

	private IEnumerator Deactivating()
	{
		slider.TriggerAnim("exit");
		RebindModal.Hide();
		foreach (Menu menu in Menus)
		{
			if (menu.CheckIsActivated())
			{
				menu.Deactivate();
			}
		}
		if (ExtraMessage.CheckIsActivated())
		{
			ExtraMessage.Deactivate();
		}
		Fader.Deactivate();
		isActivated = false;
		speakers[0].TriggerSound(1);
		yield return new WaitForSecondsRealtime(0.5f);
		SetParentAndReposition(Interface.env.transform);
		RenderChildren(toggle: false, 2);
	}

	public void PlaySfx(int sfxNum)
	{
		speakers[1].SetSoundPitch(sfxNum, 1f);
		speakers[1].TriggerSound(sfxNum);
	}

	public void PlaySfxAlt(int sfxNum)
	{
		speakers[1].TriggerSound(sfxNum);
		speakers[1].SetSoundPitch(sfxNum, 1.48f);
	}

	public void SetMainMenu()
	{
		if (Menus[1].CheckIsActivated())
		{
			if (menuType == 1)
			{
				ExtraMessage.Deactivate();
			}
			Menus[1].Deactivate();
			Menus[1].ResetActiveOption();
			speakers[0].TriggerSound(1);
		}
		else
		{
			speakers[0].TriggerSound(0);
		}
		menuType = 0;
		if ((bool)TitleScreen.dir)
		{
			Menus[0].MenuTitle.SetTitle("melatonin");
			Menus[0].Options[0].SetFunction(2, 0);
			Menus[0].Options[1].SetFunction(2, 4);
			Menus[0].Options[2].SetFunction(2, 5);
			if (Builder.mgr.GetOperatingSystemNum() <= 1 && SteamManager.mgr != null)
			{
				Menus[0].Options[3].SetFunction(2, 48);
				Menus[0].Options[4].SetFunction(2, 15);
				Menus[0].Options[5].SetFunction(0, 0);
			}
			else
			{
				Menus[0].Options[3].SetFunction(2, 43);
				Menus[0].Options[4].SetFunction(2, 11);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[5].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[5].SetFunction(2, 15);
				}
			}
			Menus[0].Options[6].SetFunction(0, 0);
		}
		else if ((bool)Chapter.dir)
		{
			Menus[0].MenuTitle.SetTitle(Chapter.GetActiveChapterNum().ToString() ?? "");
			Menus[0].Options[0].SetFunction(2, 1);
			if (Chapter.dir.CheckIsCutsceneIntro() || Chapter.dir.CheckIsCutsceneOutro())
			{
				Menus[0].Options[1].SetFunction(2, 28);
				Menus[0].Options[2].SetFunction(2, 4);
				Menus[0].Options[3].SetFunction(2, 5);
				Menus[0].Options[4].SetFunction(2, 10);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[5].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[5].SetFunction(2, 15);
				}
			}
			else
			{
				Menus[0].Options[1].SetFunction(2, 4);
				Menus[0].Options[2].SetFunction(2, 5);
				Menus[0].Options[3].SetFunction(2, 10);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[4].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[4].SetFunction(2, 15);
				}
				Menus[0].Options[5].SetFunction(0, 0);
			}
			Menus[0].Options[6].SetFunction(0, 0);
		}
		else if ((bool)Creditor.dir)
		{
			Menus[0].MenuTitle.SetTitle("credits");
			Menus[0].Options[0].SetFunction(2, 1);
			Menus[0].Options[1].SetFunction(2, 4);
			Menus[0].Options[2].SetFunction(2, 5);
			Menus[0].Options[3].SetFunction(2, 10);
			if (Builder.mgr.GetOperatingSystemNum() == 2)
			{
				Menus[0].Options[4].SetFunction(0, 0);
			}
			else
			{
				Menus[0].Options[4].SetFunction(2, 15);
			}
			Menus[0].Options[5].SetFunction(0, 0);
			Menus[0].Options[6].SetFunction(0, 0);
		}
		else if ((bool)Dream.dir)
		{
			if (Dream.dir.GetGameMode() < 5)
			{
				Menus[0].MenuTitle.SetTitle("dream");
				Menus[0].Options[0].SetFunction(2, 1);
				Menus[0].Options[1].SetFunction(2, 7);
				Menus[0].Options[2].SetFunction(2, 4);
				Menus[0].Options[3].SetFunction(2, 5);
				Menus[0].Options[4].SetFunction(2, 8);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[5].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[5].SetFunction(2, 15);
				}
				Menus[0].Options[6].SetFunction(0, 0);
			}
			else if (Dream.dir.GetGameMode() == 5)
			{
				Menus[0].MenuTitle.SetTitle("tutorial");
				Menus[0].Options[0].SetFunction(2, 1);
				Menus[0].Options[1].SetFunction(2, 7);
				Menus[0].Options[2].SetFunction(2, 29);
				Menus[0].Options[3].SetFunction(2, 4);
				Menus[0].Options[4].SetFunction(2, 5);
				Menus[0].Options[5].SetFunction(2, 10);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[6].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[6].SetFunction(2, 15);
				}
			}
			else if (Dream.dir.GetGameMode() == 6)
			{
				Menus[0].MenuTitle.SetTitle("dream");
				Menus[0].Options[0].SetFunction(2, 1);
				Menus[0].Options[1].SetFunction(2, 7);
				Menus[0].Options[2].SetFunction(2, 38);
				Menus[0].Options[3].SetFunction(2, 4);
				Menus[0].Options[4].SetFunction(2, 5);
				Menus[0].Options[5].SetFunction(2, 8);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[6].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[6].SetFunction(2, 15);
				}
			}
			else if (Dream.dir.GetGameMode() == 7)
			{
				Menus[0].MenuTitle.SetTitle("dream");
				Menus[0].Options[0].SetFunction(2, 1);
				Menus[0].Options[1].SetFunction(2, 7);
				Menus[0].Options[2].SetFunction(2, 4);
				Menus[0].Options[3].SetFunction(2, 5);
				Menus[0].Options[4].SetFunction(0, 0);
				Menus[0].Options[5].SetFunction(2, 10);
				if (Builder.mgr.GetOperatingSystemNum() == 2)
				{
					Menus[0].Options[6].SetFunction(0, 0);
				}
				else
				{
					Menus[0].Options[6].SetFunction(2, 15);
				}
			}
		}
		else if ((bool)LvlEditor.dir)
		{
			Menus[0].MenuTitle.SetTitle("editor");
			Menus[0].Options[0].SetFunction(2, 1);
			Menus[0].Options[1].SetFunction(2, 4);
			Menus[0].Options[2].SetFunction(2, 5);
			Menus[0].Options[3].SetFunction(2, 8);
			if (Builder.mgr.GetOperatingSystemNum() == 2)
			{
				Menus[0].Options[4].SetFunction(0, 0);
			}
			else
			{
				Menus[0].Options[4].SetFunction(2, 15);
			}
			Menus[0].Options[5].SetFunction(0, 0);
			Menus[0].Options[6].SetFunction(0, 0);
		}
		Menus[0].Activate(menuDirection);
	}

	public void SetChaptersMenu()
	{
		footer.FadeOutSprite(1f, 0.33f);
		if (Menus[2].CheckIsActivated())
		{
			Menus[2].Deactivate();
			Menus[2].ResetActiveOption();
			speakers[0].TriggerSound(1);
		}
		else
		{
			speakers[0].TriggerSound(0);
		}
		menuType = 2;
		Menus[0].Deactivate();
		Menus[1].MenuTitle.SetTitle("chapters");
		Menus[1].Options[0].SetFunction(2, 27);
		Menus[1].Options[1].SetFunction(2, 9);
		Menus[1].Options[2].SetFunction(2, 9);
		Menus[1].Options[3].SetFunction(2, 9);
		Menus[1].Options[4].SetFunction(2, 9);
		Menus[1].Options[5].SetFunction(2, 31);
		Menus[1].Options[6].SetFunction(2, 3);
		Menus[1].Activate(menuDirection);
	}

	public void SetQuitMenu()
	{
		footer.FadeOutSprite(1f, 0.33f);
		speakers[0].TriggerSound(0);
		menuType = 2;
		Menus[0].Deactivate();
		Menus[1].MenuTitle.SetTitle("quit");
		Menus[1].Options[0].SetFunction(2, 40);
		Menus[1].Options[1].SetFunction(2, 3);
		Menus[1].Options[2].SetFunction(0, 0);
		Menus[1].Options[3].SetFunction(0, 0);
		Menus[1].Options[4].SetFunction(0, 0);
		Menus[1].Options[5].SetFunction(0, 0);
		Menus[1].Options[6].SetFunction(0, 0);
		Menus[1].Activate(menuDirection);
	}

	public void SetExtrasMenu()
	{
		footer.FadeOutSprite(1f, 0.33f);
		speakers[0].TriggerSound(0);
		menuType = 2;
		Menus[0].Deactivate();
		Menus[1].MenuTitle.SetTitle("extras");
		Menus[1].Options[0].SetFunction(2, 49);
		Menus[1].Options[1].SetFunction(2, 43);
		Menus[1].Options[2].SetFunction(2, 51);
		Menus[1].Options[3].SetFunction(2, 52);
		Menus[1].Options[4].SetFunction(2, 11);
		Menus[1].Options[5].SetFunction(0, 0);
		Menus[1].Options[6].SetFunction(0, 0);
		Menus[1].Activate(menuDirection);
	}

	public void SetSettingsMenu()
	{
		footer.FadeOutSprite(1f, 0.33f);
		if (Menus[2].CheckIsActivated())
		{
			if (menuType == 4)
			{
				ExtraMessage.Deactivate();
			}
			Menus[2].Deactivate();
			Menus[2].ResetActiveOption();
			speakers[0].TriggerSound(1);
		}
		else
		{
			speakers[0].TriggerSound(0);
		}
		menuType = 3;
		Menus[0].Deactivate();
		Menus[1].MenuTitle.SetTitle("settings");
		Menus[1].Options[0].SetFunction(2, 22);
		Menus[1].Options[1].SetFunction(2, 23);
		Menus[1].Options[2].SetFunction(2, 46);
		Menus[1].Options[3].SetFunction(2, 17);
		Menus[1].Options[4].SetFunction(2, 20);
		if (Builder.mgr.GetOperatingSystemNum() == 2)
		{
			Menus[1].Options[5].SetFunction(0, 0);
		}
		else
		{
			Menus[1].Options[5].SetFunction(2, 35);
		}
		Menus[1].Options[6].SetFunction(2, 3);
		Menus[1].Activate(menuDirection);
	}

	public void SetWarningMenu()
	{
		menuType = 1;
		ExtraMessage.Activate(0);
		footer.FadeOutSprite(1f, 0.33f);
		speakers[0].TriggerSound(0);
		Menus[0].Deactivate();
		Menus[1].MenuTitle.SetTitle("new");
		Menus[1].Options[0].SetFunction(1, 0);
		Menus[1].Options[1].SetFunction(1, 0);
		Menus[1].Options[2].SetFunction(1, 0);
		Menus[1].Options[3].SetFunction(2, 16);
		Menus[1].Options[4].SetFunction(2, 3);
		Menus[1].Options[5].SetFunction(0, 0);
		Menus[1].Options[6].SetFunction(0, 0);
		Menus[1].Activate(menuDirection);
	}

	public void SetAccessibilityMenu()
	{
		menuType = 5;
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle("accessibility");
		Menus[2].Options[0].SetFunction(2, 18);
		Menus[2].Options[1].SetFunction(2, 19);
		Menus[2].Options[2].SetFunction(2, 42);
		Menus[2].Options[3].SetFunction(2, 45);
		Menus[2].Options[4].SetFunction(0, 0);
		Menus[2].Options[5].SetFunction(2, 3);
		Menus[2].Activate(menuDirection);
	}

	public void SetCalibrationMenu()
	{
		menuType = 4;
		ExtraMessage.Activate(1);
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle("calibration");
		Menus[2].Options[0].SetFunction(1, 0);
		Menus[2].Options[1].SetFunction(1, 0);
		Menus[2].Options[2].SetFunction(1, 0);
		Menus[2].Options[3].SetFunction(2, 26);
		if (Builder.mgr.GetOperatingSystemNum() == 2)
		{
			Menus[2].Options[4].SetFunction(2, 3);
			Menus[2].Options[5].SetFunction(1, 0);
		}
		else
		{
			Menus[2].Options[4].SetFunction(2, 30);
			Menus[2].Options[5].SetFunction(2, 3);
		}
		Menus[2].Activate(menuDirection);
	}

	public void SetDisplayMenu()
	{
		menuType = 5;
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle("display");
		if (Builder.mgr.GetOperatingSystemNum() == 2)
		{
			Menus[2].Options[0].SetFunction(0, 0);
			Menus[2].Options[1].SetFunction(0, 0);
			Menus[2].Options[2].SetFunction(2, 21);
			Menus[2].Options[3].SetFunction(2, 39);
			Menus[2].Options[4].SetFunction(0, 0);
			Menus[2].Options[5].SetFunction(2, 3);
		}
		else
		{
			Menus[2].Options[0].SetFunction(2, 12);
			Menus[2].Options[1].SetFunction(2, 13);
			Menus[2].Options[2].SetFunction(2, 21);
			Menus[2].Options[3].SetFunction(2, 39);
			Menus[2].Options[4].SetFunction(2, 50);
			Menus[2].Options[5].SetFunction(2, 3);
		}
		Menus[2].Activate(menuDirection);
		Fader.Deactivate();
	}

	public void SetKeyboardMenu()
	{
		menuType = 5;
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle("keyboard");
		Menus[2].Options[0].SetFunction(1, 0);
		Menus[2].Options[1].SetFunction(1, 0);
		Menus[2].Options[2].SetFunction(2, 36);
		Menus[2].Options[3].SetFunction(2, 37);
		Menus[2].Options[4].SetFunction(2, 3);
		Menus[2].Options[5].SetFunction(0, 0);
		Menus[2].Activate(menuDirection);
	}

	public void SetAudioMenu()
	{
		menuType = 5;
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle("audio");
		Menus[2].Options[0].SetFunction(2, 14);
		Menus[2].Options[1].SetFunction(2, 24);
		Menus[2].Options[2].SetFunction(2, 25);
		Menus[2].Options[3].SetFunction(2, 6);
		Menus[2].Options[4].SetFunction(2, 3);
		Menus[2].Options[5].SetFunction(0, 0);
		Menus[2].Activate(menuDirection);
	}

	public void SetGameplayMenu()
	{
		menuType = 5;
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle("gameplay");
		Menus[2].Options[0].SetFunction(2, 47);
		Menus[2].Options[1].SetFunction(2, 41);
		Menus[2].Options[2].SetFunction(0, 0);
		Menus[2].Options[3].SetFunction(0, 0);
		Menus[2].Options[4].SetFunction(0, 0);
		Menus[2].Options[5].SetFunction(2, 3);
		Menus[2].Activate(menuDirection);
	}

	public void SetChapterMenu(int chNum)
	{
		menuType = 6;
		speakers[0].TriggerSound(0);
		Menus[1].Deactivate();
		Menus[2].MenuTitle.SetTitle(chNum.ToString() ?? "");
		Menus[2].Options[0].SetChapterNum(chNum);
		Menus[2].Options[1].SetChapterNum(chNum);
		Menus[2].Options[2].SetChapterNum(chNum);
		Menus[2].Options[0].SetFunction(2, 32);
		Menus[2].Options[1].SetFunction(2, 33);
		Menus[2].Options[2].SetFunction(2, 34);
		Menus[2].Options[3].SetFunction(0, 0);
		Menus[2].Options[4].SetFunction(2, 3);
		Menus[2].Options[5].SetFunction(0, 0);
		Menus[2].Activate(menuDirection);
	}

	public void SetPreviousMenu()
	{
		menuDirection = 0;
		if (menuType == 1 || menuType == 2 || menuType == 3)
		{
			footer.FadeInSprite(1f, 0.67f);
			SetMainMenu();
		}
		else if (menuType == 4 || menuType == 5)
		{
			SetSettingsMenu();
			if (KeyboardDisplay.CheckIsActivated())
			{
				KeyboardDisplay.Deactivate();
			}
			if (!Fader.CheckIsActivated())
			{
				Fader.Activate();
			}
		}
		else if (menuType == 6)
		{
			SetChaptersMenu();
		}
		menuDirection = 1;
	}

	public void Disable()
	{
		foreach (Menu menu in Menus)
		{
			if (menu.CheckIsActivated())
			{
				menu.Disable();
			}
		}
	}

	public void Enable()
	{
		foreach (Menu menu in Menus)
		{
			if (menu.CheckIsActivated())
			{
				menu.Enable();
			}
		}
	}

	public int GetMenuType()
	{
		return menuType;
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
