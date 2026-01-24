public class ScoreStars : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show(int gameMode, int score)
	{
		RenderChildren(toggle: true);
		if (gameMode == 0 || gameMode == 6 || gameMode == 7)
		{
			sprites[0].TriggerAnim("1+");
		}
		else
		{
			sprites[0].TriggerAnim(score.ToString() ?? "");
		}
	}
}
