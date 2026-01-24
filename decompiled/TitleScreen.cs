using System.Collections;
using UnityEngine;

public class TitleScreen : Custom
{
	public static TitleScreen dir;

	private bool isDirecting = true;

	private bool isSkippable;

	private bool isLangUiAvailable;

	private Coroutine starting;

	private void Awake()
	{
		dir = this;
	}

	private void Start()
	{
		starting = StartCoroutine(Starting());
	}

	private IEnumerator Starting()
	{
		Technician.mgr.ToggleVsync(toggle: true);
		Technician.mgr.FadeInAudioListener();
		TitleWorld.env.CompanySplash.Show();
		TitleWorld.env.Fader.Show();
		TitleWorld.env.Fader.SetSpeed(0.33f);
		TitleWorld.env.Fader.Deactivate();
		yield return new WaitForSeconds(3f);
		isSkippable = true;
		yield return new WaitForSeconds(3f);
		isSkippable = false;
		TitleWorld.env.Fader.SetSpeed(0.25f);
		TitleWorld.env.Fader.Cross();
		yield return new WaitForSeconds(2f);
		isLangUiAvailable = true;
		if (ControlHandler.mgr.GetGamepad() != null)
		{
			ControlHandler.mgr.ConfigureGamepadType();
		}
		TitleWorld.env.CompanySplash.Delete();
		TitleWorld.env.DreamOcean.Show();
		if (!Interface.env.Submenu.CheckIsActivated() && Interface.env.CheckIsEnabled())
		{
			TitleWorld.env.LangHint.Show();
			TitleWorld.env.Instruction.Show();
		}
	}

	private void Update()
	{
		if (!isDirecting)
		{
			return;
		}
		if (!Interface.env.Submenu.CheckIsActivated())
		{
			if (isSkippable && ControlHandler.mgr.CheckIsActionPressed())
			{
				SkipSplash();
			}
			else
			{
				if (!isLangUiAvailable)
				{
					return;
				}
				if (TitleWorld.env.LangHint.CheckIsActivated())
				{
					if (ControlHandler.mgr.CheckIsSwapPressed())
					{
						Interface.env.Disable();
						TitleWorld.env.LangHint.Deactivate();
						TitleWorld.env.LangMenu.Activate();
						TitleWorld.env.Instruction.Deactivate();
					}
				}
				else if (ControlHandler.mgr.CheckIsActionPressed())
				{
					TitleWorld.env.LangMenu.Select();
					CloseLangMenu();
				}
				else if (ControlHandler.mgr.CheckIsSwapPressed())
				{
					CloseLangMenu();
				}
				else if (ControlHandler.mgr.CheckIsCancelPressed())
				{
					CloseLangMenu();
				}
				else if (ControlHandler.mgr.CheckIsDownPressed())
				{
					TitleWorld.env.LangMenu.Descend();
				}
				else if (ControlHandler.mgr.CheckIsUpPressed())
				{
					TitleWorld.env.LangMenu.Ascend();
				}
			}
		}
		else if (TitleWorld.env.AchievementsMenu.CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsDownPressed())
			{
				TitleWorld.env.AchievementsMenu.Descend();
			}
			else if (ControlHandler.mgr.CheckIsUpPressed())
			{
				TitleWorld.env.AchievementsMenu.Ascend();
			}
			else if (ControlHandler.mgr.CheckIsCancelPressed())
			{
				TitleWorld.env.AchievementsMenu.Deactivate();
			}
		}
		else if (Interface.env.CommunityMenu.CheckIsActivated())
		{
			if (ControlHandler.mgr.CheckIsDownPressed())
			{
				Interface.env.CommunityMenu.Descend();
			}
			else if (ControlHandler.mgr.CheckIsUpPressed())
			{
				Interface.env.CommunityMenu.Ascend();
			}
			else if (ControlHandler.mgr.CheckIsCancelPressed())
			{
				Interface.env.CommunityMenu.Deactivate();
			}
			else if (ControlHandler.mgr.CheckIsActionPressed())
			{
				Interface.env.CommunityMenu.Select();
			}
			else if (ControlHandler.mgr.CheckIsActionLeftPressed())
			{
				Interface.env.CommunityMenu.PrevPage();
			}
			else if (ControlHandler.mgr.CheckIsActionRightPressed())
			{
				Interface.env.CommunityMenu.NextPage();
			}
		}
	}

	private void CloseLangMenu()
	{
		StartCoroutine(ClosingLangMenu());
	}

	private IEnumerator ClosingLangMenu()
	{
		TitleWorld.env.LangHint.Activate();
		TitleWorld.env.LangMenu.Deactivate();
		TitleWorld.env.Instruction.Activate();
		yield return null;
		Interface.env.Enable();
	}

	public void SkipSplash()
	{
		StartCoroutine(SkippingSplash());
	}

	private IEnumerator SkippingSplash()
	{
		isSkippable = false;
		CancelCoroutine(starting);
		TitleWorld.env.Fader.SetSpeed(0.5f);
		TitleWorld.env.Fader.Cross();
		yield return new WaitForSeconds(1f);
		isLangUiAvailable = true;
		TitleWorld.env.CompanySplash.Delete();
		TitleWorld.env.DreamOcean.Show();
		if (!Interface.env.Submenu.CheckIsActivated() && Interface.env.CheckIsEnabled())
		{
			TitleWorld.env.LangHint.Show();
			TitleWorld.env.Instruction.Show();
		}
	}

	public void ToggleIsDirecting(bool toggle)
	{
		isDirecting = toggle;
	}
}
