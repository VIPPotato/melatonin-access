using UnityEngine;

public class SideLabel : Wrapper
{
	[Header("Fragments")]
	public Fragment labelFader;

	public Fragment prompt;

	public textboxFragment practiceText;

	public textboxFragment skipText;

	public Fragment speaker;

	private bool isActivated;

	private bool isEnabled;

	private float timer;

	protected override void Awake()
	{
		labelFader.Awake();
		prompt.Awake();
		practiceText.Initiate();
		skipText.Initiate();
		speaker.Awake();
		RenderChildren(toggle: false);
	}

	public void ShowAsPractice()
	{
		isActivated = true;
		isEnabled = true;
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		labelFader.TriggerAnim("fadedIn");
		labelFader.ToggleAnimator(toggle: false);
		practiceText.SetState(0);
		ConfigurePrompt();
		if (SaveManager.GetLang() == 1 || SaveManager.GetLang() == 2 || SaveManager.GetLang() == 4)
		{
			prompt.SetLocalY(-0.318f);
			skipText.SetLocalY(-0.97f);
		}
		else if (SaveManager.GetLang() == 3)
		{
			prompt.SetLocalY(-0.318f);
			skipText.SetLocalY(-0.97f);
			skipText.SetFontSize(2.6f);
		}
		else if (SaveManager.GetLang() == 5)
		{
			prompt.SetLocalY(-1.818f);
			skipText.SetLocalY(-2.47f);
		}
		else if (SaveManager.GetLang() == 6)
		{
			prompt.SetLocalY(-2.198f);
			skipText.SetLocalY(-2.85f);
		}
		else if (SaveManager.GetLang() == 7)
		{
			prompt.SetLocalY(-1.998f);
			skipText.SetLocalY(-2.65f);
			skipText.SetFontSize(2.4f);
			skipText.SetLetterSpacing(-3f);
		}
	}

	public void ShowAsEdited()
	{
		isActivated = true;
		isEnabled = true;
		RenderChildren(toggle: true);
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		ConfigurePrompt();
		labelFader.TriggerAnim("hidden");
		prompt.SetLocalY(0.4f);
		prompt.SetSpriteAlpha(0.5f);
		skipText.SetLocalY(-0.252f);
		skipText.SetState(1);
		skipText.SetFontAlpha(0.5f);
	}

	public void ActivateAsTutorial()
	{
		isActivated = true;
		RenderChildren(toggle: true);
		labelFader.TriggerAnim("fadeIn");
		practiceText.SetState(1);
		if (skipText != null)
		{
			skipText.Delete();
		}
		if (prompt != null)
		{
			prompt.Delete();
		}
	}

	public void Deactivate()
	{
		isActivated = false;
		labelFader.TriggerAnim("fadeOut");
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	private void Update()
	{
		if (!isEnabled)
		{
			return;
		}
		if (ControlHandler.mgr.CheckIsSwapPressed() && Time.timeScale > 0f)
		{
			if (Dream.dir.GetGameMode() == 0)
			{
				Disable();
				Dream.SetGameMode(1);
				Dream.dir.ToggleIsPlaying(toggle: false);
				Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
				Interface.env.Submenu.PlaySfx(1);
			}
			else if (Dream.dir.GetGameMode() == 6)
			{
				Dream.dir.ToggleIsAutoHit();
				if (Dream.dir.CheckIsAutoHit())
				{
					prompt.SetSpriteAlpha(1f);
					skipText.SetFontAlpha(1f);
				}
				else
				{
					prompt.SetSpriteAlpha(0.5f);
					skipText.SetFontAlpha(0.5f);
				}
			}
		}
		timer += Time.deltaTime;
		if (timer > 2f)
		{
			ConfigurePrompt();
			timer = 0f;
		}
	}

	public void Disable()
	{
		isEnabled = false;
		skipText.FadeOutText(1f, 0.25f);
		prompt.FadeOutSprite(1f, 0.25f);
		speaker.TriggerSound(0);
		MoveDistance(new Vector3(0f, -0.5f, 0f), 2f);
	}

	private void ConfigurePrompt()
	{
		if (ControlHandler.mgr.GetCtrlType() == 1)
		{
			prompt.TriggerAnim("gamepadY");
		}
		else if (ControlHandler.mgr.GetCtrlType() == 2)
		{
			prompt.TriggerAnim("gamepadTRIANGLE");
		}
		else
		{
			prompt.TriggerAnim("keyTAB");
		}
	}

	public bool CheckIsActivated()
	{
		return isActivated;
	}
}
