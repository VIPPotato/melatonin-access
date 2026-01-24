using System.Collections;
using UnityEngine;

public class Results : Wrapper
{
	public StageEndMenu StageEndMenu;

	public ScoreMessage ScoreMessage;

	public ScoreStars ScoreStars;

	public PlanetRings PlanetRings;

	public WavesBox WavesBox;

	[Header("Fragments")]
	public textboxFragment[] numberboxes;

	public textboxFragment[] labels;

	private bool isEnabled;

	private int gameMode;

	private int score;

	private int oldScore;

	private Coroutine deactivating;

	protected override void Awake()
	{
		textboxFragment[] array = numberboxes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		RenderChildren(toggle: false, 1);
	}

	public void Activate(int newGameMode, int newScore, int newOldScore)
	{
		StartCoroutine(Activating(newGameMode, newScore, newOldScore));
	}

	private IEnumerator Activating(int newGameMode, int newScore, int newOldScore)
	{
		gameMode = newGameMode;
		score = newScore;
		oldScore = newOldScore;
		RenderChildren(toggle: true, 1);
		SetParentAndReposition(Interface.env.Cam.GetInnerTransform());
		gears[0].TriggerAnim("activate");
		speakers[0].TriggerSound(0);
		speakers[0].TriggerSound(2);
		speakers[0].TriggerSound(3);
		sprites[0].TriggerAnim("activate");
		for (int i = 0; i < numberboxes.Length; i++)
		{
			numberboxes[i].SetText(Dream.dir.GetCounter(i).ToString() ?? "");
		}
		if (SaveManager.GetLang() == 3)
		{
			for (int j = 0; j < labels.Length; j++)
			{
				labels[j].SetFontSize(3f);
				labels[j].SetLetterSpacing(-8f);
			}
		}
		else if (SaveManager.GetLang() == 6)
		{
			for (int k = 0; k < labels.Length; k++)
			{
				labels[k].SetFontSize(3f);
				labels[k].SetLetterSpacing(-2f);
			}
		}
		else if (SaveManager.GetLang() == 7)
		{
			for (int l = 0; l < labels.Length; l++)
			{
				labels[l].SetFontSize(3.2f);
				labels[l].SetLetterSpacing(-2f);
			}
		}
		else if (SaveManager.GetLang() == 8)
		{
			for (int m = 0; m < labels.Length; m++)
			{
				labels[m].SetFontSize(3.2f);
				labels[m].SetLetterSpacing(-2f);
			}
		}
		else if (SaveManager.GetLang() == 9)
		{
			for (int n = 0; n < labels.Length; n++)
			{
				labels[n].SetFontSize(3f);
				labels[n].SetLetterSpacing(-3.2f);
			}
		}
		ScoreMessage.Show(gameMode, score);
		if (gameMode == 2 || gameMode == 4)
		{
			PlanetRings.Show(score);
		}
		else
		{
			ScoreStars.Show(gameMode, score);
		}
		WavesBox.Show(gameMode, score, oldScore);
		StageEndMenu.Show(gameMode);
		yield return new WaitForSeconds(0.25f);
		isEnabled = true;
	}

	private void Deactivate()
	{
		isEnabled = false;
		speakers[0].TriggerSound(1);
		speakers[1].TriggerSound(1);
		speakers[1].TriggerSound(3);
		gears[0].TriggerAnim("deactivate");
		sprites[0].TriggerAnim("deactivate");
		StageEndMenu.Disable();
	}

	private void Update()
	{
		if (!isEnabled)
		{
			return;
		}
		if (Interface.env.CommunityMenu.CheckIsActivated())
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
		else if (ControlHandler.mgr.CheckIsDownPressed())
		{
			StageEndMenu.Next();
			speakers[1].TriggerSound(0);
		}
		else if (ControlHandler.mgr.CheckIsUpPressed())
		{
			StageEndMenu.Prev();
			speakers[1].TriggerSound(0);
		}
		else
		{
			if (!ControlHandler.mgr.CheckIsActionPressed())
			{
				return;
			}
			if (StageEndMenu.GetHighlightPosition() == 0)
			{
				if (gameMode == 0 || gameMode == 2)
				{
					Dream.SetGameMode(1);
					Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
					Deactivate();
				}
				else if (gameMode == 1)
				{
					if (SaveManager.mgr.GetScore(SceneMonitor.mgr.GetActiveSceneName()) >= 2)
					{
						Dream.SetGameMode(2);
						Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
						Deactivate();
					}
					else
					{
						speakers[1].TriggerSound(2);
					}
				}
				else if (gameMode == 3)
				{
					if (SaveManager.mgr.GetScore(SceneMonitor.mgr.GetActiveSceneName()) >= 2)
					{
						Dream.SetGameMode(4);
						Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
						Deactivate();
					}
					else
					{
						speakers[1].TriggerSound(2);
					}
				}
				else if (gameMode == 4)
				{
					Dream.SetGameMode(3);
					Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
					Deactivate();
				}
				else if (gameMode == 6)
				{
					Dream.dir.ExitTo(LvlEditor.GetEditorName());
					Deactivate();
				}
				else if (gameMode == 7)
				{
					Interface.env.CommunityMenu.Activate();
				}
			}
			else if (StageEndMenu.GetHighlightPosition() == 1)
			{
				if (gameMode == 0 || gameMode == 2 || gameMode == 3 || gameMode == 4 || gameMode == 6 || gameMode == 7)
				{
					Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
					Deactivate();
				}
				else if (gameMode == 1)
				{
					Dream.SetGameMode(0);
					Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
					Deactivate();
				}
			}
			else if (StageEndMenu.GetHighlightPosition() == 2)
			{
				if (gameMode == 0 || gameMode == 2 || gameMode == 6)
				{
					Dream.dir.ExitTo("Chapter_" + Chapter.GetActiveChapterNum());
					Deactivate();
				}
				else if (gameMode == 1)
				{
					Dream.dir.ExitTo(SceneMonitor.mgr.GetActiveSceneName());
					Deactivate();
				}
				else if (gameMode == 3 || gameMode == 4)
				{
					if (!Builder.mgr.CheckIsFullGame())
					{
						if (SaveManager.mgr.GetScore("Dream_indulgence") >= 2 && !Teaser.CheckIsTeaserSeen())
						{
							Dream.dir.ExitTo("Teaser");
						}
						else
						{
							Dream.dir.ExitTo("Chapter_1");
						}
					}
					else
					{
						Dream.dir.ExitTo("Chapter_" + Chapter.GetActiveChapterNum());
					}
					Deactivate();
				}
				else if (gameMode == 7)
				{
					Dream.dir.ExitTo("TitleScreen");
					Deactivate();
				}
			}
			else if (StageEndMenu.GetHighlightPosition() == 3 && gameMode == 1)
			{
				Dream.dir.ExitTo("Chapter_" + Chapter.GetActiveChapterNum());
				Deactivate();
			}
		}
	}

	public void PlayToggleSfx()
	{
		speakers[1].SetSoundPitch(0, 1f);
		speakers[1].TriggerSound(0);
	}

	public void PlayToggleSfxAlt()
	{
		speakers[1].TriggerSound(0);
		speakers[1].SetSoundPitch(0, 1.48f);
	}
}
