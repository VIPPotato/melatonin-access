public class PlanetRings : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show(int score)
	{
		RenderChildren(toggle: true);
		sprites[0].TriggerAnim(score.ToString() ?? "");
	}
}
