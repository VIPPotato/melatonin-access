using UnityEngine;

public class StageEndMenu : Wrapper
{
	[Header("Children")]
	public Wave[] Waves;

	[Header("Fragments")]
	public spriteFragment[] icons;

	public textboxFragment[] labels;

	public spriteFragment bg;

	private int gameMode;

	private int highlightPosition;

	private int activeOptionsCount;

	private int activeWavesGroup;

	protected override void Awake()
	{
		spriteFragment[] array = icons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initiate();
		}
		textboxFragment[] array2 = labels;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Initiate();
		}
		bg.Initiate();
		RenderChildren(toggle: false);
	}

	public void Show(int newGameMode)
	{
		RenderChildren(toggle: true);
		gameMode = newGameMode;
		if (gameMode == 0)
		{
			icons[0].SetStateByName("forward");
			icons[1].SetStateByName("restart");
			icons[2].SetStateByName("exit");
			labels[0].SetStateByName("scored");
			if (SaveManager.GetLang() == 5 || SaveManager.GetLang() == 9)
			{
				labels[0].SetFontSize(2.8f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				labels[0].SetFontSize(2.8f);
				labels[0].SetLetterSpacing(-3f);
			}
			labels[1].SetStateByName("restart");
			if (SaveManager.GetLang() == 6)
			{
				labels[1].SetFontSize(3f);
			}
			labels[2].SetStateByName("exit");
			if (SaveManager.GetLang() == 5)
			{
				labels[2].SetFontSize(2.7f);
				labels[2].SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				labels[2].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[2].SetFontSize(3f);
			}
		}
		else if (gameMode == 1)
		{
			if (SaveManager.mgr.GetScore(SceneMonitor.mgr.GetActiveSceneName()) >= 2)
			{
				icons[0].SetStateByName("forward");
			}
			else
			{
				icons[0].SetStateByName("locked");
			}
			icons[1].SetStateByName("backward");
			icons[2].SetStateByName("restart");
			icons[3].ToggleSpriteRenderer(toggle: true);
			icons[3].SetStateByName("exit");
			icons[0].SetLocalY(2.05f);
			icons[1].SetLocalY(0.72f);
			icons[2].SetLocalY(-0.61f);
			icons[3].SetLocalY(-1.94f);
			labels[0].SetStateByName("hard");
			if (SaveManager.GetLang() == 7)
			{
				labels[0].SetFontSize(2.8f);
			}
			labels[1].SetStateByName("practice");
			if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7)
			{
				labels[1].SetFontSize(3.2f);
			}
			labels[2].SetStateByName("restart");
			if (SaveManager.GetLang() == 6)
			{
				labels[2].SetFontSize(3f);
			}
			labels[3].ToggleMeshRenderer(toggle: true);
			labels[3].SetStateByName("exit");
			if (SaveManager.GetLang() == 5)
			{
				labels[3].SetFontSize(2.7f);
				labels[3].SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				labels[3].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[3].SetFontSize(3f);
			}
			labels[0].SetLocalY(2.06f);
			labels[1].SetLocalY(0.73f);
			labels[2].SetLocalY(-0.6f);
			labels[3].SetLocalY(-1.93f);
			bg.SetState(1);
		}
		else if (gameMode == 2)
		{
			icons[0].SetStateByName("backward");
			icons[1].SetStateByName("restart");
			icons[2].SetStateByName("exit");
			labels[0].SetStateByName("scored");
			if (SaveManager.GetLang() == 5 || SaveManager.GetLang() == 9)
			{
				labels[0].SetFontSize(2.8f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				labels[0].SetFontSize(2.8f);
				labels[0].SetLetterSpacing(-3f);
			}
			labels[1].SetStateByName("restart");
			if (SaveManager.GetLang() == 6)
			{
				labels[1].SetFontSize(3f);
			}
			labels[2].SetStateByName("exit");
			if (SaveManager.GetLang() == 5)
			{
				labels[2].SetFontSize(2.7f);
				labels[2].SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				labels[2].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[2].SetFontSize(3f);
			}
		}
		else if (gameMode == 3)
		{
			if (SaveManager.mgr.GetScore(SceneMonitor.mgr.GetActiveSceneName()) >= 2)
			{
				icons[0].SetStateByName("forward");
			}
			else
			{
				icons[0].SetStateByName("locked");
			}
			icons[1].SetStateByName("restart");
			icons[2].SetStateByName("exit");
			labels[0].SetStateByName("hard");
			if (SaveManager.GetLang() == 7)
			{
				labels[0].SetFontSize(2.8f);
			}
			labels[1].SetStateByName("restart");
			if (SaveManager.GetLang() == 6)
			{
				labels[1].SetFontSize(3f);
			}
			labels[2].SetStateByName("exit");
			if (SaveManager.GetLang() == 5)
			{
				labels[2].SetFontSize(2.7f);
				labels[2].SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				labels[2].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[2].SetFontSize(3f);
			}
		}
		else if (gameMode == 4)
		{
			icons[0].SetStateByName("backward");
			icons[1].SetStateByName("restart");
			icons[2].SetStateByName("exit");
			labels[0].SetStateByName("scored");
			if (SaveManager.GetLang() == 5 || SaveManager.GetLang() == 9)
			{
				labels[0].SetFontSize(2.8f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				labels[0].SetFontSize(2.8f);
				labels[0].SetLetterSpacing(-3f);
			}
			labels[1].SetStateByName("restart");
			labels[2].SetStateByName("exit");
			if (SaveManager.GetLang() == 5)
			{
				labels[2].SetFontSize(2.7f);
				labels[2].SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				labels[2].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[2].SetFontSize(3f);
			}
		}
		else if (gameMode == 6)
		{
			icons[0].SetStateByName("backward");
			icons[1].SetStateByName("restart");
			icons[2].SetStateByName("exit");
			labels[0].SetStateByName("reedit");
			if (SaveManager.GetLang() == 3)
			{
				labels[0].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 7)
			{
				labels[0].SetFontSize(2.8f);
			}
			labels[1].SetStateByName("restart");
			if (SaveManager.GetLang() == 6)
			{
				labels[1].SetFontSize(3f);
			}
			labels[2].SetStateByName("exit");
			if (SaveManager.GetLang() == 5)
			{
				labels[2].SetFontSize(2.7f);
				labels[2].SetLetterSpacing(-2f);
			}
			else if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7 || SaveManager.GetLang() == 9)
			{
				labels[2].SetFontSize(2.7f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[2].SetFontSize(3f);
			}
		}
		else if (gameMode == 7)
		{
			icons[0].SetStateByName("backward");
			icons[1].SetStateByName("restart");
			icons[2].SetStateByName("exit");
			labels[0].SetStateByName("more");
			labels[1].SetStateByName("restart");
			if (SaveManager.GetLang() == 6)
			{
				labels[1].SetFontSize(3f);
			}
			labels[2].SetStateByName("titleScreen");
			if (SaveManager.GetLang() == 5)
			{
				labels[0].SetFontSize(3f);
			}
			else if (SaveManager.GetLang() == 8)
			{
				labels[0].SetFontSize(3f);
			}
		}
		Waves[0].Activate();
		Waves[1].Activate();
		Waves[0].SetLocalY(icons[highlightPosition].GetLocalY() - 0.48f);
		Waves[1].SetLocalY(icons[highlightPosition].GetLocalY() - 0.51f);
		spriteFragment[] array = icons;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].CheckIsSpriteRendered())
			{
				activeOptionsCount++;
			}
		}
	}

	public void Next()
	{
		Disable();
		highlightPosition++;
		if (highlightPosition >= activeOptionsCount)
		{
			highlightPosition = 0;
			Interface.env.Results.PlayToggleSfxAlt();
		}
		else
		{
			Interface.env.Results.PlayToggleSfx();
		}
		icons[highlightPosition].SetSpriteAlpha(1f);
		labels[highlightPosition].SetFontAlpha(1f);
		if (activeWavesGroup == 0)
		{
			Waves[0].Activate();
			Waves[1].Activate();
			Waves[0].SetLocalY(icons[highlightPosition].GetLocalY() - 0.48f);
			Waves[1].SetLocalY(icons[highlightPosition].GetLocalY() - 0.51f);
		}
		else
		{
			Waves[2].Activate();
			Waves[3].Activate();
			Waves[2].SetLocalY(icons[highlightPosition].GetLocalY() - 0.48f);
			Waves[3].SetLocalY(icons[highlightPosition].GetLocalY() - 0.51f);
		}
	}

	public void Prev()
	{
		Disable();
		highlightPosition--;
		if (highlightPosition < 0)
		{
			highlightPosition = activeOptionsCount - 1;
			Interface.env.Results.PlayToggleSfxAlt();
		}
		else
		{
			Interface.env.Results.PlayToggleSfx();
		}
		icons[highlightPosition].SetSpriteAlpha(1f);
		labels[highlightPosition].SetFontAlpha(1f);
		if (activeWavesGroup == 0)
		{
			Waves[0].Activate();
			Waves[1].Activate();
			Waves[0].SetLocalY(icons[highlightPosition].GetLocalY() - 0.492f);
			Waves[1].SetLocalY(icons[highlightPosition].GetLocalY() - 0.519f);
		}
		else
		{
			Waves[2].Activate();
			Waves[3].Activate();
			Waves[2].SetLocalY(icons[highlightPosition].GetLocalY() - 0.492f);
			Waves[3].SetLocalY(icons[highlightPosition].GetLocalY() - 0.519f);
		}
	}

	public void Disable()
	{
		spriteFragment[] array = icons;
		foreach (spriteFragment spriteFragment2 in array)
		{
			if (spriteFragment2.CheckIsSpriteRendered())
			{
				spriteFragment2.SetSpriteAlpha(0.67f);
			}
		}
		textboxFragment[] array2 = labels;
		foreach (textboxFragment textboxFragment2 in array2)
		{
			if (textboxFragment2.CheckIsMeshRendered())
			{
				textboxFragment2.SetFontAlpha(0.67f);
			}
		}
		Wave[] waves = Waves;
		foreach (Wave wave in waves)
		{
			if (wave.CheckIsActivated())
			{
				wave.Deactivate();
			}
		}
		activeWavesGroup++;
		if (activeWavesGroup > 1)
		{
			activeWavesGroup = 0;
		}
	}

	public int GetHighlightPosition()
	{
		return highlightPosition;
	}
}
