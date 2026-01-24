public class Planet : Wrapper
{
	protected override void Awake()
	{
		SetupFragments();
		RenderChildren(toggle: false);
	}

	public void Show()
	{
		RenderChildren(toggle: true);
	}

	public void Hide()
	{
		RenderChildren(toggle: false);
	}

	public void Hit()
	{
		sprites[0].TriggerAnim("hit");
	}
}
