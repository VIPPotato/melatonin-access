using UnityEngine;

public class Landmark : Wrapper
{
	public DreamTitle DreamTitle;

	public Fragment[] reactSprites;

	[Header("Children")]
	public ScoreBubble ScoreBubble;

	[Header("Props")]
	public bool isBig;

	public string dreamName;

	private bool isRemix;

	private bool isDisabled;

	private bool isTriggered;

	private int starScore;

	private int ringScore;

	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
		starScore = SaveManager.mgr.GetScore("Dream_" + dreamName);
		ringScore = SaveManager.mgr.GetScore("Dream_" + dreamName + "Alt");
		if (starScore > 0 || ringScore > 0)
		{
			ScoreBubble.SetNumStars(starScore);
			ScoreBubble.SetNumRings(ringScore);
			ScoreBubble.Show();
		}
		if (this == Map.env.Neighbourhood.GetRemixLandmark())
		{
			isRemix = true;
			if (dreamName != "final")
			{
				isDisabled = true;
				gears[0].TriggerAnim("disabled");
				sprites[0].TriggerAnim("greyscaled");
				sprites[1].TriggerAnim("greyscaled");
				ScoreBubble.Hide();
			}
		}
	}

	private void Update()
	{
		if (!isTriggered || !(Time.timeScale > 0f))
		{
			return;
		}
		if (ControlHandler.mgr.CheckIsActionPressed() && Map.env.Neighbourhood.McMap.ModeMenu.CheckIsActivated())
		{
			if (Map.env.Neighbourhood.McMap.ModeMenu.CheckIsTranstioned())
			{
				if (Map.env.Neighbourhood.McMap.ModeMenu.GetActiveItemNum() == 0)
				{
					Descend(0);
				}
				else if (Map.env.Neighbourhood.McMap.ModeMenu.GetActiveItemNum() == 1 && (starScore > 0 || isRemix))
				{
					Descend(1);
				}
				else if (Map.env.Neighbourhood.McMap.ModeMenu.GetActiveItemNum() == 2 && starScore >= 2)
				{
					Descend(2);
				}
				else if (Map.env.Neighbourhood.McMap.ModeMenu.GetActiveItemNum() == 3 && starScore >= 2 && Builder.mgr.CheckIsFullGame())
				{
					Descend(3);
				}
				else
				{
					Map.env.PlayNopeSound();
				}
			}
			else
			{
				Map.env.PlayNextSound();
				Map.env.Neighbourhood.McMap.Disable();
				Map.env.Neighbourhood.McMap.ModeMenu.Transition(dreamName, starScore, ringScore);
			}
		}
		else if (ControlHandler.mgr.CheckIsCancelPressed() && Map.env.Neighbourhood.McMap.ModeMenu.CheckIsTranstioned())
		{
			Map.env.PlayCancelSound();
			Map.env.Neighbourhood.McMap.ModeMenu.Cancel();
			Map.env.Neighbourhood.McMap.Enable();
		}
		else if (ControlHandler.mgr.CheckIsDownPressed() && Map.env.Neighbourhood.McMap.ModeMenu.CheckIsTranstioned())
		{
			Map.env.Neighbourhood.McMap.ModeMenu.NavigateDown();
		}
		else if (ControlHandler.mgr.CheckIsUpPressed() && Map.env.Neighbourhood.McMap.ModeMenu.CheckIsTranstioned())
		{
			Map.env.Neighbourhood.McMap.ModeMenu.NavigateUp();
		}
	}

	private void OnTriggerEnter2D()
	{
		if (isDisabled)
		{
			Map.env.PlayNopeSound();
			Map.env.RequiredBox.Activate();
			return;
		}
		isTriggered = true;
		gears[0].TriggerAnim("lower");
		Map.env.PlayToggleSound();
		if (DreamTitle != null)
		{
			DreamTitle.React();
			DreamTitle.Morph();
		}
		if (reactSprites.Length != 0)
		{
			Fragment[] array = reactSprites;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TriggerAnim("react");
			}
		}
		if (starScore > 0 || ringScore > 0)
		{
			ScoreBubble.Deactivate();
		}
		Map.env.Neighbourhood.McMap.ReadyUp();
		Map.env.Neighbourhood.McMap.ModeMenu.Activate(isRemix);
	}

	private void OnTriggerExit2D()
	{
		if (isDisabled)
		{
			Map.env.RequiredBox.Deactivate();
			return;
		}
		isTriggered = false;
		gears[0].TriggerAnim("rise");
		Map.env.PlayToggleSound();
		if (DreamTitle != null)
		{
			DreamTitle.Reset();
			DreamTitle.Unmorph();
		}
		if (reactSprites.Length != 0)
		{
			Fragment[] array = reactSprites;
			foreach (Fragment fragment in array)
			{
				if (fragment.CheckIsAnimExists("reset"))
				{
					fragment.TriggerAnim("reset");
				}
			}
		}
		if (starScore > 0 || ringScore > 0)
		{
			ScoreBubble.Activate();
		}
		Map.env.Neighbourhood.McMap.Unready();
		Map.env.Neighbourhood.McMap.ModeMenu.Deactivate(0);
	}

	private void Descend(int activeItemNum)
	{
		gears[0].TriggerAnim("descend");
		Map.env.PlayNextSound();
		Map.env.Neighbourhood.McMap.Descend();
		Map.env.Neighbourhood.McMap.ModeMenu.Deactivate(1);
		if (isBig)
		{
			Map.env.Neighbourhood.McMap.SetPosition(GetX(), GetY() + 1f);
		}
		if (dreamName == "final")
		{
			Map.env.Neighbourhood.McMap.SetPosition(GetX(), GetY() + 1f);
		}
		else
		{
			Map.env.Neighbourhood.McMap.SetPosition(GetX(), GetY() + 1.2f);
		}
		Chapter.dir.SetActiveDreamName(dreamName);
		switch (activeItemNum)
		{
		case 0:
			Dream.SetGameMode(0);
			Chapter.dir.ExitToDream("Dream_" + dreamName);
			break;
		case 1:
			if (isRemix)
			{
				Dream.SetGameMode(3);
			}
			else
			{
				Dream.SetGameMode(1);
			}
			Chapter.dir.ExitToDream("Dream_" + dreamName);
			break;
		case 2:
			if (isRemix)
			{
				Dream.SetGameMode(4);
			}
			else
			{
				Dream.SetGameMode(2);
			}
			Chapter.dir.ExitToDream("Dream_" + dreamName);
			break;
		case 3:
			Dream.SetGameMode(6);
			Chapter.dir.ExitToDream("LvlEditor_" + dreamName);
			Daw.ResetActiveBeatAndBar();
			break;
		}
		isDisabled = true;
		isTriggered = false;
	}

	public void Enable(bool isPlaySound)
	{
		isDisabled = false;
		if (isPlaySound)
		{
			Map.env.PlayToggleSound();
			if (starScore > 0 || ringScore > 0)
			{
				ScoreBubble.Activate();
			}
		}
		else if (starScore > 0 || ringScore > 0)
		{
			ScoreBubble.Show();
		}
		gears[0].TriggerAnim("enabled");
		sprites[0].TriggerAnim("colored");
		sprites[1].TriggerAnim("colored");
	}

	public string GetDreamName()
	{
		return dreamName;
	}
}
