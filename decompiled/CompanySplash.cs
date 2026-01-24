public class CompanySplash : Wrapper
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
}
