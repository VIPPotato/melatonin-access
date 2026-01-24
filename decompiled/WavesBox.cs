using UnityEngine;

public class WavesBox : Wrapper
{
	[Header("Children")]
	public Wave[] Waves;

	[Header("Fragments")]
	public textboxFragment message;

	protected override void Awake()
	{
		message.Initiate();
	}

	public void Show(int gameMode, int score, int oldScore)
	{
		switch (gameMode)
		{
		case 0:
			if (score <= 1)
			{
				message.SetStateByName("practice0to1");
				if (SaveManager.GetLang() == 6 || SaveManager.GetLang() == 7)
				{
					message.SetFontSize(3f);
				}
				else if (SaveManager.GetLang() == 9)
				{
					message.SetFontSize(3.2f);
				}
			}
			else
			{
				message.SetStateByName("practice2to4");
			}
			break;
		case 1:
			if (score == 0)
			{
				message.SetFontSize(3f);
			}
			message.SetStateByName("scored" + score);
			break;
		case 2:
		case 4:
			if (score <= 1)
			{
				message.SetStateByName("alt0to1");
			}
			else
			{
				message.SetStateByName("scored" + score);
			}
			break;
		case 3:
			if (!Builder.mgr.CheckIsFullGame())
			{
				if (Teaser.CheckIsTeaserSeen())
				{
					if (score <= 1)
					{
						message.SetStateByName("scored0to1Remix");
					}
					else
					{
						message.SetStateByName("scored" + score);
					}
				}
				else if (score <= 1)
				{
					message.SetStateByName("scored0to1Unlock");
				}
				else
				{
					message.SetStateByName("scored2to4Unlock");
				}
			}
			else if (SaveManager.mgr.GetChapterNum() == Chapter.GetActiveChapterNum() && !SaveManager.mgr.CheckIsGameComplete())
			{
				if (score <= 1)
				{
					message.SetStateByName("scored0to1Unlock");
				}
				else
				{
					message.SetStateByName("scored2to4Unlock");
				}
			}
			else if (score <= 1)
			{
				message.SetStateByName("scored0to1Remix");
			}
			else
			{
				message.SetStateByName("scored" + score);
			}
			break;
		case 6:
		case 7:
			message.SetStateByName("custom");
			break;
		}
		if (gameMode != 6 && gameMode != 7 && score >= 4)
		{
			Waves[1].SetColor(new Color(73f / 85f, 1f, 0.9843137f));
		}
		else
		{
			Waves[1].SetColor(new Color(50f / 51f, 0.9490196f, 50f / 51f));
		}
		Waves[0].Show();
		Waves[1].Show();
	}
}
