using UnityEngine;

public class ScoreMessage : Wrapper
{
	[Header("Fragments")]
	public textboxFragment title;

	public textboxFragment subtitle;

	protected override void Awake()
	{
		title.Initiate();
		subtitle.Initiate();
		RenderChildren(toggle: false);
	}

	public void Show(int gameMode, int score)
	{
		RenderChildren(toggle: true);
		switch (gameMode)
		{
		case 0:
			title.SetLocalY(-0.337f);
			title.SetFontSize(10.5f);
			title.SetStateByName("finished");
			if (SaveManager.GetLang() == 5)
			{
				subtitle.SetLocalY(0.56f);
			}
			subtitle.ToggleMeshRenderer(toggle: true);
			subtitle.SetState(0);
			return;
		case 6:
		case 7:
			title.SetFontSize(10.5f);
			title.SetStateByName("finished");
			if (SaveManager.GetLang() == 0)
			{
				title.SetLocalY(-0.337f);
				subtitle.ToggleMeshRenderer(toggle: true);
				subtitle.SetState(1);
			}
			return;
		}
		title.SetStateByName(score.ToString() ?? "");
		switch (score)
		{
		case 0:
			title.SetFontSize(10.5f);
			return;
		case 1:
			if (SaveManager.GetLang() == 8)
			{
				title.SetFontSize(10.5f);
				return;
			}
			break;
		}
		if (score == 3 && (SaveManager.GetLang() == 5 || SaveManager.GetLang() == 6 || SaveManager.GetLang() == 8))
		{
			title.SetFontSize(10f);
			title.SetLetterSpacing(-1f);
		}
		else if (score == 3 && SaveManager.GetLang() == 9)
		{
			title.SetFontSize(8f);
			title.SetLetterSpacing(-5f);
		}
		else if (score == 4 && (SaveManager.GetLang() == 3 || SaveManager.GetLang() == 5 || SaveManager.GetLang() == 8))
		{
			title.SetFontSize(10.5f);
		}
		else
		{
			title.SetFontSize(11.8f);
		}
	}
}
